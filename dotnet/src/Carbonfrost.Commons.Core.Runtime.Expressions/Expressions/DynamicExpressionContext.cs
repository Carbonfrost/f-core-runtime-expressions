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
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    public class DynamicExpressionContext : DynamicObject, IExpressionContext {

        public IExpressionContext Parent {
            get;
            private set;
        }

        private IReadOnlyDictionary<string, PropertyInfo> Properties {
            get {
                return PropertyCache.ReflectGetPropertiesCache(GetType());
            }
        }

        public DynamicExpressionContext()
            : this(null) {}

        public DynamicExpressionContext(IExpressionContext parent) {
            Parent = parent;
        }

        public DynamicExpressionContext WithParent(IExpressionContext parent) {
            if (parent == Parent) {
                return this;
            }
            var result = Clone();
            result.Parent = parent;
            return result;
        }

        public sealed override IEnumerable<string> GetDynamicMemberNames() {
            return GetDynamicMemberNamesCore();
        }

        public sealed override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result) {
            if (binder == null)
                throw new ArgumentNullException("binder");

            if (TryGetIndex(indexes, out result)) {
                return true;
            }
            result = Undefined.Value;
            return true;
        }

        public sealed override bool TryGetMember(GetMemberBinder binder, out object result) {
            if (binder == null)
                throw new ArgumentNullException("binder");

            if (TryGetMember(binder.Name, out result)) {
                return true;
            }
            result = Undefined.Value;
            return true;
        }

        public sealed override bool TrySetMember(SetMemberBinder binder, object value) {
            if (binder == null) {
                throw new ArgumentNullException("binder");
            }

            return TrySetMember(binder.Name, value);;
        }

        public bool TryGetIndex(object[] indexes, out object result) {
            if (indexes.Length > 1) {
                throw CoreRuntimeExpressionsFailure.ExpressionContextInvalidIndexCount("indexes");
            }

            if (indexes[0] == null) {
                result = Undefined.Value;
                return true;
            }

            string key = indexes[0].ToString();
            if (_TryGetCore(key, out result)) {
                return true;
            }

            if (Parent != null) {
                return Parent.TryGetIndex(indexes, out result);
            }

            result = Undefined.Value;
            return false;
        }

        public bool TryGetMember(string name, out object result) {
            if (_TryGetCore(name, out result)) {
                return true;
            }

            if (Parent != null)
                return Parent.TryGetMember(name, out result);

            result = Undefined.Value;
            return false;
        }

        protected virtual bool TrySetMemberCore(string name, object value) {
            PropertyInfo prop;
            if (Properties.TryGetValue(name, out prop)) {
                try {
                    prop.SetValue(this, value);
                    return true;
                } catch (TargetInvocationException) {
                }
            }
            return false;
        }

        protected virtual bool TryGetMemberCore(string name, out object result) {
            PropertyInfo prop;
            if (Properties.TryGetValue(name, out prop)) {
                try {
                    result = prop.GetValue(this);
                    return true;
                } catch (TargetInvocationException) {
                }
            }
            result = null;
            return false;
        }

        protected virtual IEnumerable<string> GetDynamicMemberNamesCore() {
            return Properties.Keys;
        }

        public bool TrySetMember(string name, object value) {
            return TrySetMemberCore(name, value);
        }

        bool _TryGetCore(string key, out object result) {
            return TryGetMemberCore(key, out result);
        }

        public DynamicExpressionContext CreateChildContext() {
            return CreateChildContextCore();
        }

        protected virtual DynamicExpressionContext CreateChildContextCore() {
            return new DynamicExpressionContext { Parent = this };
        }

        public virtual object GetService(Type serviceType) {
            if (serviceType == null) {
                throw new ArgumentNullException("serviceType");
            }

            if (serviceType.GetTypeInfo().IsInstanceOfType(this)) {
                return this;
            }
            if (Parent is IServiceProvider p) {
                return p.GetService(serviceType);
            }

            return null;
        }

        public DynamicExpressionContext Clone() {
            return CloneCore();
        }

        protected virtual DynamicExpressionContext CloneCore() {
            return new DynamicExpressionContext(Parent);
        }
    }
}
