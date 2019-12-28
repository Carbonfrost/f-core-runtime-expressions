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
using System.Linq;
using System.Reflection;

using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Core.Runtime.Expressions.Serialization;

[assembly: ExpressionSerializerFactory(typeof(ComponentModelExpressionSerializerFactory))]

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    class ComponentModelExpressionSerializerFactory : AdapterFactory {

        private static IDictionary<Type, Type> _argumentProducers;

        // Map from Guid ==> GuidArguments
        public static IDictionary<Type, Type> ConstructorArgumentProducers {
            get {
                return _argumentProducers ?? (_argumentProducers = CreateIntrinsics());
            }
        }

        public override Type GetAdapterType(Type adapteeType, string adapterRoleName) {
            if (adapterRoleName != AdapterRole.ExpressionSerializer) {
                return null;
            }

            if (adapteeType.GetTypeInfo().IsEnum) {
                return typeof(EnumExpressionSerializer);
            }

            var result = GetBuiltinSerializerType(adapteeType);
            if (result != null) {
                return result;
            }

            // This logic gives us a type, which is a requirement of the adapter factory.
            // Say Constructor<Guid, GuidArguments>, which
            // invokes GuidArguments.Get() in order to obtain the arguments for use when
            // serializing Guid
            var args = ConstructorArgumentProducers.GetValueOrDefault(adapteeType);
            if (args != null) {
                return typeof(ConstructorSerializer<,>).MakeGenericType(adapteeType, args);
            }

            return typeof(CompositeExpressionSerializer);
        }

        static Type GetBuiltinSerializerType(Type type) {
            switch (Type.GetTypeCode(type)) {
                case TypeCode.String:
                    return typeof(StringExpressionSerializer);

                case TypeCode.Boolean:
                    return typeof(BooleanExpressionSerializer);

                case TypeCode.Object:
                    if (typeof(Type).IsAssignableFrom(type))
                        return typeof(TypeExpressionSerializer);

                    if (type.IsArray)
                        return typeof(ArrayExpressionSerializer);

                    if (type.GetTypeInfo().IsGenericType) {
                        var def = type.GetGenericTypeDefinition();
                        if (def == typeof(Dictionary<,>)) {
                            return typeof(DictionaryExpressionSerializer);
                        }
                    }
                    break;

                case TypeCode.DateTime:
                    break;

                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return typeof(ConstantExpressionSerializer);
            }

            return null;
        }

        static IDictionary<Type, Type> CreateIntrinsics() {
            return typeof(ComponentModelExpressionSerializerFactory)
                .GetNestedTypes(BindingFlags.NonPublic)
                .Where(t => t.Name.EndsWith("Arguments"))
                .ToDictionary(t => t.GetTypeInfo().GetInterfaces()[0].GetGenericArguments()[0], t => t);
        }

        interface IConstructorArguments<T> {
            object[] Get(T t);
        }

        sealed class GuidArguments : IConstructorArguments<Guid> {

            public object[] Get(Guid guid) {
                return new [] { guid.ToString() };
            }
        }

        sealed class UriArguments : IConstructorArguments<Uri> {

            public object[] Get(Uri uri) {
                return new object[] { uri.OriginalString, uri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative };
            }
        }

        class ConstructorSerializer<T, TArguments> : IExpressionSerializer
            where TArguments : IConstructorArguments<T>, new() {

            public Expression ConvertToExpression(object value, IExpressionSerializerContext context) {
                IConstructorArguments<T> producer = new TArguments();
                object[] arguments = producer.Get((T) value);
                var ctor = typeof(T).GetConstructor(arguments.Select(t => t.GetType()).ToArray());

                return ExpressionSerializer.ConvertCore(ctor, arguments, context, typeof(T));
            }

        }

    }
}

