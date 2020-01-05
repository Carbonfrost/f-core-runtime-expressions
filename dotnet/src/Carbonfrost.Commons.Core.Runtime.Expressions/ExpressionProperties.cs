//
// Copyright 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using Carbonfrost.Commons.Core.Runtime.Expressions;

namespace Carbonfrost.Commons.Core.Runtime {

    public class ExpressionProperties : IProperties {

        private readonly IProperties _store;
        private readonly IExpressionBinding _binding;
        private readonly IExpressionBinding NullBindingImpl = new NullBindingImplClass();

        protected IProperties Properties {
            get {
                return _store;
            }
        }

        public ExpressionProperties() {
            _store = new Properties();
            _binding = NullBindingImpl;
        }

        internal ExpressionProperties(IExpressionBinding binding, IPropertyStore propertyStore) : this(propertyStore) {
            _binding = binding;
        }

        public ExpressionProperties(IPropertyStore propertyStore) {
            if (propertyStore == null) {
                throw new ArgumentNullException("propertyStore");
            }
            _binding = NullBindingImpl;
            var properties = propertyStore as IProperties;
            if (properties != null) {
                _store = properties;
            }
            else {
                _store = Runtime.Properties.ReadOnly(propertyStore);
            }
        }

        public object this[string property] {
            get {
                return this.GetProperty(property);
            }
            set {
                SetProperty(property, value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) {
            var handler = PropertyChanged;
            if (handler != null) {
                handler(this, e);
            }
        }

        public void ClearProperties() {
            _store.ClearProperties();
        }

        public void ClearProperty(string property) {
            _store.SetProperty(property, null);
        }

        public void SetProperty(string property, object value) {
            if (property == null) {
                throw new ArgumentNullException("property");
            }
            if (string.IsNullOrEmpty(property)) {
                throw Failure.EmptyString("property");
            }

            if (value is Expression && _binding.GetBindingMode(property) == BindingMode.Disabled) {
                var ec = CreateExpressionContext();
                value = ((Expression) value).Evaluate(ec);
            }
            _store.SetProperty(property, value);
        }

        public bool TrySetProperty(string property, object value) {
            SetProperty(property, value);
            return true;
        }

        public bool TryGetProperty(string property, Type propertyType, out object value) {
            if (property == null) {
                throw new ArgumentNullException("property");
            }
            if (string.IsNullOrEmpty(property)) {
                throw Failure.EmptyString("property");
            }
            propertyType = propertyType ?? typeof(object);
            if (_store.TryGetProperty(property, typeof(object), out value)) {
                var e = value as Expression;
                if (e != null && typeof(Expression) != propertyType) {
                    value = e.Evaluate(CreateExpressionContext());
                    if (propertyType.IsInstanceOfType(value)) {
                        return true;
                    }
                    // Apply type conversion from string
                    string str = Convert.ToString(value);
                    try {
                        value = Activation.FromText(propertyType, str);
                        return true;
                    }
                    catch {
                    }
                    return false;
                }
                return propertyType.IsInstanceOfType(value);
            }
            return false;
        }

        public Type GetPropertyType(string property) {
            return _store.GetPropertyType(property);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() {
            return _store.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public override string ToString() {
            return _store.ToString();
        }

        protected virtual ExpressionContext CreateExpressionContext() {
            return _binding.CreateExpressionContext();
        }

        class NullBindingImplClass : IExpressionBinding {
            public ExpressionContext CreateExpressionContext() {
                return new ExpressionContext();
            }
            public BindingMode GetBindingMode(string property) {
                return BindingMode.Disabled;
            }
        }
    }
}
