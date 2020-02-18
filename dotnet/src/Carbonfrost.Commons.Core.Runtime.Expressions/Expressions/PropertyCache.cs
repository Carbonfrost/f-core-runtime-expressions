//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    class PropertyCache : IReadOnlyDictionary<string, PropertyInfo> {

        private static IDictionary<Type, PropertyCache> _all = new Dictionary<Type, PropertyCache>();
        private static readonly IReadOnlyDictionary<string, PropertyInfo> EMPTY = new Dictionary<string, PropertyInfo>();

        private readonly PropertyCache _parent;
        private readonly IReadOnlyDictionary<string, PropertyInfo> _properties;
        private readonly string _name;

        public PropertyCache Parent {
            get {
                return _parent;
            }
        }

        public IEnumerable<PropertyCache> AncestorsAndSelf {
            get {
                var it = this;
                while (it != null) {
                    yield return it;
                    it = it.Parent;
                }
            }
        }

        public PropertyInfo this[string key] {
            get {
                PropertyInfo result;
                if (_properties.TryGetValue(key, out result)) {
                    return result;
                }
                if (Parent == null) {
                    return null;
                }
                return Parent[key];
            }
        }

        public IEnumerable<string> Keys {
            get {
                return AncestorsAndSelf.SelectMany(a => a._properties.Keys).Distinct();
            }
        }

        public IEnumerable<PropertyInfo> Values {
            get {
                return AncestorsAndSelf.SelectMany(a => a._properties.Values);
            }
        }

        public int Count {
            get {
                return Keys.Count();
            }
        }

        private PropertyCache(PropertyCache parent, Type baseType) {
            _parent = parent;
            _name = baseType.Name;
            var props = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);

            foreach (var prop in baseType.GetTypeInfo().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)) {
                if (prop.GetIndexParameters().Length == 0 && !props.ContainsKey(prop.Name)) {
                    props.Add(prop.Name, prop);
                }
            }
            _properties = props.Count == 0 ? EMPTY : props;
        }

        internal static PropertyCache ReflectGetPropertiesCache(Type sourceClrType) {
            if (sourceClrType == null) {
                return null;
            }
            PropertyCache result;
            if (_all.TryGetValue(sourceClrType, out result)) {
                return result;
            }

            var baseType = sourceClrType.GetTypeInfo().BaseType == null ? null : sourceClrType.GetTypeInfo().BaseType;
            _all[sourceClrType] = result = new PropertyCache(
                ReflectGetPropertiesCache(baseType),
                sourceClrType
            );
            return result;
        }

        public bool ContainsKey(string key) {
            return _properties.ContainsKey(key) || (Parent != null && Parent.ContainsKey(key));
        }

        public IEnumerator<KeyValuePair<string, PropertyInfo>> GetEnumerator() {
            return AncestorsAndSelf.SelectMany(a => a._properties).GetEnumerator();
        }

        public bool TryGetValue(string key, out PropertyInfo value) {
            value = this[key];
            return value != null;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
