//
// Copyright 2021 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Linq;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    public static partial class Factory {

        public static NewObjectExpression NewObject(Expression expression) {
            return Expression.NewObject(expression);
        }

        public static NewObjectExpression NewObject(Expression expression, IEnumerable<Expression> arguments) {
            return Expression.NewObject(expression, arguments);
        }

        public static NewArrayExpression NewArray(params Expression[] expressions) {
            return Expression.NewArray(expressions);
        }

        public static NewArrayExpression NewArray(IEnumerable<Expression> expressions) {
            return Expression.NewArray(expressions);
        }

        public static BinaryExpression Assign(Expression left, Expression right) {
            return Expression.Assign(left, right);
        }

        public static CallExpression Call(Expression expression, params Expression[] arguments) {
            return Expression.Call(expression, arguments);
        }

        public static CallExpression Call(Expression expression, IEnumerable<Expression> arguments) {
            return Expression.Call(expression, arguments);
        }

        public static ValueExpression Value(object value) {
            return Expression.Value(value);
        }

        public static ConstantExpression Constant(object value) {
            return Expression.Constant(value);
        }

        public static LambdaExpression Lambda(Expression body) {
            return Expression.Lambda(body);
        }

        public static LambdaExpression Lambda(Expression body, IEnumerable<LambdaParameter> parameters) {
            return Expression.Lambda(body, parameters);
        }

        public static LambdaExpression Lambda(Expression body, IExpressionContext closure, IEnumerable<LambdaParameter> parameters) {
            return Expression.Lambda(body, closure, parameters);
        }

        public static MemberAccessExpression MemberAccess(Expression expression, string name) {
            return Expression.MemberAccess(expression, name);
        }

        public static NameExpression Name(string name) {
            return Expression.Name(name);
        }

        public static BlockExpression Block(IEnumerable<Expression> expressions) {
            return Expression.Block(expressions);
        }

        public static BlockExpression Block(params Expression[] expressions) {
            return Expression.Block(expressions);
        }

        public static ConditionalExpression Conditional(Expression test, Expression ifTrue, Expression ifFalse) {
            return Expression.Conditional(test, ifTrue, ifFalse);
        }

        public static InterpolatedStringExpression InterpolatedString(params InterpolatedStringContent[] elements) {
            return Expression.InterpolatedString(elements);
        }

    }
}
