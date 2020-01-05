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
using Carbonfrost.Commons.Core.Runtime.Expressions.Serialization;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    public class ExpressionSerializerFactory : AdapterFactory<IExpressionSerializer> {

        public static readonly ExpressionSerializerFactory Default
            = new DefaultExpressionSerializerFactory();

        protected ExpressionSerializerFactory(IAdapterFactory implementation)
            : base(AdapterRole.ExpressionSerializer, implementation) {
        }

        public IExpressionSerializer GetExpressionSerializer(Type componentType) {
            if (componentType == null) {
                throw new ArgumentNullException("componentType"); // $NON-NLS-1
            }

            Type result = GetExpressionSerializerType(componentType);
            if (result == null)
                return null;

            return Activation.CreateInstance<IExpressionSerializer>(result);
        }

        public virtual Type GetExpressionSerializerType(Type componentType) {
            if (componentType == null) {
                throw new ArgumentNullException("componentType");
            }

            return GetAdapterType(componentType);
        }

        class DefaultExpressionSerializerFactory : ExpressionSerializerFactory {

            internal DefaultExpressionSerializerFactory()
                : base(AdapterFactory.Default) {
            }

            public override Type GetExpressionSerializerType(Type componentType) {
                var result = base.GetExpressionSerializerType(componentType);

                // Select a conventions-based serializer when no better one is available
                if (result == typeof(CompositeExpressionSerializer)) {
                    var method = componentType.GetToExpressionMethod();
                    if (method != null) {
                        return typeof(InvokeExpressionSerializer);
                    }
                    var parseMethod = componentType.GetParseMethod();
                    if (parseMethod != null) {
                        return typeof(InvokeParseMethodExpressionSerializer);
                    }
                }
                return result;
            }
        }
    }
}
