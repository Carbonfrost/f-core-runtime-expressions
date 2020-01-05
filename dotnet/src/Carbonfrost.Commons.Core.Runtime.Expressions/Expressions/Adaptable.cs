//
// Copyright 2006, 2008, 2010, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    public static class Adaptable {

        private static IDictionary<Type, IDictionary<string, PropertyInfo>> _propertyCache
            = new Dictionary<Type, IDictionary<string, PropertyInfo>>();

        public static BindingMode GetExpressionBindingMode(this PropertyInfo property) {
            if (property == null) {
                throw new ArgumentNullException("property");
            }
            var attr = (ExpressionAttribute) property.GetCustomAttribute(typeof(ExpressionAttribute))
                ?? (ExpressionAttribute) property.DeclaringType.GetTypeInfo().GetCustomAttribute(typeof(ExpressionAttribute))
                ?? ExpressionAttribute.Default;

            return attr.BindingMode;
        }

        public static MethodInfo GetToExpressionMethod(this Type type) {
            if (type == null) {
                throw new ArgumentNullException("type");
            }

            var method = type.GetTypeInfo().GetMethod("ToExpression", Array.Empty<Type>());
            if (method != null && method.ReturnType == typeof(Expression)) {
                return method;
            }
            return null;
        }

        public static ExpressionSerializationMode GetExpressionSerializationMode(this PropertyInfo property) {
            if (property == null) {
                throw new ArgumentNullException("property");
            }

            var attr = property.GetCustomAttribute<ExpressionSerializationModeAttribute>();
            if (attr == null) {
                return ExpressionSerializationMode.Default;
            }

            return attr.Mode;
        }

        internal static IDictionary<string, PropertyInfo> ReflectGetPropertiesCache(Type sourceClrType) {
            return _propertyCache.GetValueOrCache(sourceClrType, () => ReflectGetProperties(sourceClrType));
        }

        private static IDictionary<string, PropertyInfo> ReflectGetProperties(Type sourceClrType) {
            var baseType = sourceClrType;
            var names = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
            do {
                foreach (var prop in baseType.GetTypeInfo().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)) {
                    if (prop.GetIndexParameters().Length == 0 && !names.ContainsKey(prop.Name)) {
                        names.Add(prop.Name, prop);
                    }
                }
                baseType = baseType.GetTypeInfo().BaseType == null ? null : baseType.GetTypeInfo().BaseType;
            } while (baseType != null && baseType != typeof(object));
            return names;
        }
    }
}
