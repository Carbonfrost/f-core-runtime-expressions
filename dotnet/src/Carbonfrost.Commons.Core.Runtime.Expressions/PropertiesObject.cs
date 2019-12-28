//
// Copyright 2014, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Dynamic;
using System.Linq;
using System.Reflection;
using LinqExpression = System.Linq.Expressions.Expression;
using Carbonfrost.Commons.Core.Runtime.Expressions;

namespace Carbonfrost.Commons.Core.Runtime {

    public class PropertiesObject : IDynamicMetaObjectProvider, IExpressionBinding, IPropertiesContainer {

        private ExpressionProperties _store;

        public ExpressionProperties Properties {
            get {
                return _store;
            }
        }

        public PropertiesObject() {
            _store = new ExpressionProperties(this, new Properties());
        }

        public PropertiesObject(IPropertyStore propertyStore) {
            if (propertyStore == null)
                throw new ArgumentNullException("propertyStore");

            _store = new ExpressionProperties(this, propertyStore);
        }

        DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(LinqExpression parameter) {
            if (parameter == null)
                throw new ArgumentNullException("parameter");

            return new PropertiesObjectMetaObject(parameter, this);
        }

        IProperties IPropertiesContainer.Properties {
            get {
                return Properties;
            }
        }

        ExpressionContext IExpressionBinding.CreateExpressionContext() {
            return CreateExpressionContext();
        }

        BindingMode IExpressionBinding.GetBindingMode(string property) {
        var pi = GetType().GetTypeInfo().GetProperty(property);
            if (pi == null) {
                return BindingMode.Once;
            }
            return pi.GetExpressionBindingMode();
        }

        protected virtual ExpressionContext CreateExpressionContext() {
            var ec = new ExpressionContext();
            ec.DataProviders.AddNew("this", Properties);
            return ec;
        }
    }
}
