//
// Copyright 2015 Carbonfrost Systems, Inc. (http://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Collections.Generic;
using System.Reflection;
using Carbonfrost.Commons.Core.Runtime.Expressions.Serialization;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    public abstract class ExpressionSerializer : IExpressionSerializer {

        public static readonly IExpressionSerializer Null
            = new NullExpressionSerializer();

        public static IExpressionSerializer GetExpressionSerializer(object value) {
            if (ReferenceEquals(value, null))
                return Null;

            return GetExpressionSerializer(value.GetType());
        }

        public static IExpressionSerializer GetExpressionSerializer(Type type) {
            return ExpressionSerializerFactory.Default.GetExpressionSerializer(type);
        }

        public static Type GetExpressionSerializerType(Type type) {
            return ExpressionSerializerFactory.Default.GetExpressionSerializerType(type);
        }

        public abstract Expression ConvertToExpression(object value, IExpressionSerializerContext context);

        internal static void StoreSerializationInfo(Expression expr1, string name) {
            expr1.AddAnnotation(new SerializerInfo(name));
        }

        internal static Expression SerializeOrReference(object value, IExpressionSerializerContext context) {
            if (value == null)
                return ExpressionSerializer.GetExpressionSerializer(value).ConvertToExpression(value, context);

            string existingName = context.GetVariable(value);
            if (existingName == null)
                return ExpressionSerializer.GetExpressionSerializer(value).ConvertToExpression(value, context);
            else
                return Expression.Name(existingName);
        }

        internal static Expression CreateTypeReference(Type type) {
            // TODO: Should be qualified with global::
            Expression result = null;

            foreach (var str in type.FullName.Split('.', '+', '/')) {
                if (result == null)
                    result = Expression.Name(str);
                else
                    result = Expression.MemberAccess(result, str);
            }

            return result;
        }

        internal static Expression ConvertCore(MemberInfo member,
                                               object[] arguments,
                                               IExpressionSerializerContext context,
                                               Type requiredType) {
            var mem = member;

            switch (mem.MemberType) {
                case MemberTypes.Field:
                    return Expression.FromField((FieldInfo) member);
                case MemberTypes.Property:
                    // TODO Field or property must be static
                    return Expression.FromProperty((PropertyInfo) member);

                case MemberTypes.Method:
                    return ConvertMethod((MethodInfo) mem, arguments, context, requiredType);

                case MemberTypes.Constructor:
                    return Expression.NewObject(
                        CreateTypeReference(mem.DeclaringType),
                        ConvertArguments(arguments, context));

                default:
                    throw new NotImplementedException();
            }
        }

        private static void ApplyTypeBinding(Expression output, Type type) {
            output.AddAnnotation(new TypeBinding(type));
        }

        private static Expression ConvertMethod(MethodInfo mem,
                                                object[] arguments,
                                                IExpressionSerializerContext context,
                                                Type requiredType) {
            if (!mem.IsStatic)
                throw new NotImplementedException();

            var expr = Expression.Call(
                Expression.MemberAccess(CreateTypeReference(mem.DeclaringType), mem.Name),
                ConvertArguments(arguments, context));

            if (mem.ReturnType != requiredType) {
                ApplyTypeBinding(expr, requiredType);
            }

            return expr;
        }

        private static IEnumerable<Expression> ConvertArguments(System.Collections.ICollection arguments, IExpressionSerializerContext context) {
            foreach (object o in arguments) {
                var es = ExpressionSerializer.GetExpressionSerializer(o);
                yield return es.ConvertToExpression(o, context);
            }

        }
    }
}
