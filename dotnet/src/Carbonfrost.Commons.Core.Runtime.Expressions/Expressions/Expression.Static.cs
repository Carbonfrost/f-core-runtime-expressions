//
// Copyright 2006, 2008, 2010, 2012, 2016, 2021 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    partial class Expression {

        public static Expression True {
            get {
                return Constant(true);
            }
        }

        public static Expression False {
            get {
                return Constant(false);
            }
        }

        public static Expression Null {
            get {
                return Constant(null);
            }
        }

        public static Expression NaN {
            get {
                return Constant(double.NaN);
            }
        }

        public static Expression Undefined {
            get {
                return Constant(Runtime.Undefined.Value);
            }
        }

        public static NewObjectExpression NewObject(Expression expression) {
            return NewObject(expression, null);
        }

        public static NewObjectExpression NewObject(Expression expression, IEnumerable<Expression> arguments) {
            if (expression == null) {
                throw new ArgumentNullException(nameof(expression));
            }

            arguments = arguments ?? Enumerable.Empty<Expression>();
            return new NewObjectExpression(expression, arguments);
        }

        public static NewArrayExpression NewArray(params Expression[] expressions) {
            return NewArray((IEnumerable<Expression>) expressions);
        }

        public static NewArrayExpression NewArray(IEnumerable<Expression> expressions) {
            expressions = expressions ?? Enumerable.Empty<Expression>();
            return new NewArrayExpression(expressions);
        }

        public static BinaryExpression Assign(Expression left, Expression right) {
            if (left == null) {
                throw new ArgumentNullException(nameof(left));
            }
            if (right == null) {
                throw new ArgumentNullException(nameof(right));
            }

            return new BinaryExpression(ExpressionType.Assign, left, right);
        }

        public static CallExpression Call(Expression expression, params Expression[] arguments) {
            if (expression == null) {
                throw new ArgumentNullException(nameof(expression));
            }

            return new CallExpression(expression, arguments ?? Enumerable.Empty<Expression>());
        }

        public static CallExpression Call(Expression expression, IEnumerable<Expression> arguments) {
            if (expression == null) {
                throw new ArgumentNullException(nameof(expression));
            }
            return new CallExpression(expression, arguments ?? Enumerable.Empty<Expression>());
        }

        public static ValueExpression Value(object value) {
            return new ValueExpression(value);
        }

        public static ConstantExpression Constant(object value) {
            if (value == null) {
                return new ConstantExpression(value, typeof(object));
            }
            return new ConstantExpression(value, value.GetType());
        }

        public static LambdaExpression Lambda(Expression body) {
            return Lambda(body, null);
        }

        public static LambdaExpression Lambda(Expression body, IEnumerable<LambdaParameter> parameters) {
            return Lambda(body, null, parameters);
        }

        public static LambdaExpression Lambda(Expression body, IExpressionContext closure, IEnumerable<LambdaParameter> parameters) {
            if (body == null) {
                throw new ArgumentNullException(nameof(body));
            }
            return new LambdaExpression(body, closure, parameters);
        }

        public static MemberAccessExpression MemberAccess(Expression expression, string name) {
            if (expression == null) {
                throw new ArgumentNullException(nameof(expression));
            }
            if (name == null) {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrEmpty(name)) {
                throw Failure.EmptyString(nameof(name));
            }

            return new MemberAccessExpression(expression, name);
        }

        public static NameExpression Name(string name) {
            if (name == null) {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrEmpty(name)) {
                throw Failure.EmptyString(nameof(name));
            }

            return new NameExpression(name);
        }

        static ConstantExpression NullConstant(Type type) {
            return new ConstantExpression(null, type);
        }

        public static BlockExpression Block(IEnumerable<Expression> expressions) {
            if (expressions == null) {
                throw new ArgumentNullException(nameof(expressions));
            }

            var items = expressions.ToList().AsReadOnly();
            if (items.Count == 0) {
                throw Failure.EmptyCollection(nameof(expressions));
            }
            return new BlockExpression(items);
        }

        public static BlockExpression Block(params Expression[] expressions) {
            if (expressions == null) {
                throw new ArgumentNullException(nameof(expressions));
            }
            var items = new ReadOnlyCollection<Expression>(expressions);
            if (items.Count == 0) {
                throw Failure.EmptyCollection(nameof(expressions));
            }
            return new BlockExpression(items);
        }

        public static ConditionalExpression Conditional(Expression test, Expression ifTrue, Expression ifFalse) {
            if (test == null) {
                throw new ArgumentNullException(nameof(test));
            }
            if (ifTrue == null) {
                throw new ArgumentNullException(nameof(ifTrue));
            }
            if (ifFalse == null) {
                throw new ArgumentNullException(nameof(ifFalse));
            }

            return new ConditionalExpression(test, ifTrue, ifFalse);
        }

        public static InterpolatedStringExpression InterpolatedString(params InterpolatedStringContent[] elements) {
            return new InterpolatedStringExpression(elements);
        }

        public static bool TryParse(string text, out Expression result) {
            try {
                result = Parse(text, null);
                return true;

            } catch (Exception ex) {
                if (Failure.IsCriticalException(ex)) {
                    throw;
                }

                result = null;
                return false;
            }
        }

        public static bool TryParse(string text, IServiceProvider serviceProvider, out Expression result) {
            try {
                result = Parse(text, serviceProvider);
                return true;
            } catch (Exception ex) {
                if (Failure.IsCriticalException(ex)) {
                    throw;
                }

                result = null;
                return false;
            }
        }

        public static Expression Parse(string text) {
            return Parse(text, null);
        }

        public static Expression Parse(string text, IServiceProvider serviceProvider) {
            serviceProvider = serviceProvider ?? ServiceProvider.Root;
            var ep = serviceProvider.GetService(typeof(IExpressionParser)) as IExpressionParser ?? ExpressionParser.Default;
            var ec = serviceProvider.GetService(typeof(ExpressionContext)) as ExpressionContext;
            return ep.Parse(text, ec);
        }

        public static Expression Serialize(object graph) {
            var mgr = new ExpressionSerializationManager();
            using (mgr.CreateSession()) {
                return mgr.Serialize(graph);
            }
        }

        public static Expression FromMethod(MethodInfo method, params object[] args) {
            if (method == null) {
                throw new ArgumentNullException(nameof(method));
            }
            if (!method.IsStatic || !method.IsPublic) {
                throw new NotImplementedException();
            }

            Expression[] myArgs = args.Select(t => Expression.Serialize(t)).ToArray();
            return Expression.Call(Expression.MemberAccess(
                ExpressionSerializer.CreateTypeReference(method.DeclaringType), method.Name),
                                   myArgs);
        }

        public static Expression FromField(FieldInfo field) {
            if (field == null) {
                throw new ArgumentNullException(nameof(field));
            }
            if (!field.IsStatic || !field.IsPublic) {
                throw CoreRuntimeExpressionsFailure.RequiresStaticFieldOrProperty();
            }
            return Expression.MemberAccess(
                ExpressionSerializer.CreateTypeReference(field.DeclaringType), field.Name);
        }

        public static Expression FromProperty(PropertyInfo property) {
            if (property == null) {
                throw new ArgumentNullException(nameof(property));
            }
            return Expression.MemberAccess(
                ExpressionSerializer.CreateTypeReference(property.DeclaringType), property.Name);
        }
    }
}
