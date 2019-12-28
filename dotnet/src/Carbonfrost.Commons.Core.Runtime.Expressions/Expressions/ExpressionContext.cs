//
// Copyright 2006, 2008, 2010, 2015, 2016, 2019 Carbonfrost Systems, Inc.
// (http://carbonfrost.com)
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
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    public class ExpressionContext : DynamicObject, IExpressionContext {

        public static readonly ExpressionContext Empty = new ExpressionContext();

        private readonly IPropertyProvider _selfValues;
        private readonly IDictionary<string, object> _data = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        private readonly IProperties _self;
        private readonly PropertyProviderCollection _dataProviders = new PropertyProviderCollection();
        private INameScope _nameScope;

        // TODO Define variables using the context, plus parent contexts and binding

        public PropertyProviderCollection DataProviders {
            get {
                return _dataProviders;
            }
        }

        public INameScope NameScope {
            get {
                return _nameScope;
            }
        }

        public IDictionary<string, object> Data {
            get { return _data; }
        }

        public ExpressionContext Parent { get; private set; }

        public ExpressionContext()
            : this(null) {}

        public ExpressionContext(ExpressionContext parent)
            : this(parent, null) {}

        public ExpressionContext(ExpressionContext parent, INameScope nameScope) {
            _self = Properties.FromValue(this);
            Parent = parent;
            var dataValues = Properties.FromValue(Data);
            _selfValues = PropertyProvider.Compose(_self, dataValues, DataProviders);
            _nameScope = nameScope ?? new NameScope();
        }

        internal static IExpressionContext Combine(params IExpressionContext[] contexts) {
            return new CompositeExpressionContext(contexts);
        }

        public ExpressionContext WithNameScope(INameScope nameScope) {
            if (nameScope == NameScope) {
                return this;
            }
            var result = Clone();
            result._nameScope = nameScope;
            return result;
        }

        public ExpressionContext WithParent(ExpressionContext parent) {
            if (parent == Parent) {
                return this;
            }
            var result = Clone();
            result.Parent = parent;
            return result;
        }

        public object Evaluate(string expression) {
            return Evaluate(Expression.Parse(expression));
        }

        public object Evaluate(Expression expression) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }
            return expression.Evaluate(this);
        }

        public override IEnumerable<string> GetDynamicMemberNames() {
            return _self.Select(t => t.Key).Concat(Data.Keys);
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result) {
            if (binder == null)
                throw new ArgumentNullException("binder");

            if (TryGetIndex(indexes, out result)) {
                return true;
            }
            result = Undefined.Value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            if (binder == null)
                throw new ArgumentNullException("binder");

            if (TryGetMember(binder.Name, out result)) {
                return true;
            }
            result = Undefined.Value;
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value) {
            if (binder == null)
                throw new ArgumentNullException("binder");

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

        public bool TrySetMember(string name, object value) {
            if (_self.HasProperty(name))
                _self.SetProperty(name, value);
            else
                this.Data[name] = value;
            return true;
        }

        bool _TryGetCore(string key, out object result) {
            if (_selfValues.TryGetProperty(key, typeof(object), out result)) {
                return true;
            }
            object variable = _nameScope.FindName(key);
            if (variable != null) {
                result = variable;
                return true;
            }
            result = null;
            return false;
        }

        public ExpressionContext CreateChildContext() {
            return CreateChildContextCore();
        }

        protected virtual ExpressionContext CreateChildContextCore() {
            return new ExpressionContext { Parent = this };
        }

        public virtual object GetService(Type serviceType) {
            if (serviceType == null) {
                throw new ArgumentNullException("serviceType");
            }

            if (serviceType.GetTypeInfo().IsInstanceOfType(this)) {
                return this;
            }

            return null;
        }

        public ExpressionContext Clone() {
            return CloneCore();
        }

        protected virtual ExpressionContext CloneCore() {
            var ec = new ExpressionContext();
            ec._dataProviders.AddMany(_dataProviders);
            ec._data.AddMany(_data);
            ec._nameScope = NameScope;
            ec.Parent = Parent;
            return ec;
        }

        public void MakeReadOnly() {
            IsReadOnly = true;
        }

        public bool IsReadOnly {
            get;
            private set;
        }

        protected void ThrowIfReadOnly() {
            if (IsReadOnly) {
                throw Failure.Sealed();
            }
        }
    }
}
