//
// Copyright 2006, 2008, 2010, 2015, 2016, 2019-2020 Carbonfrost Systems, Inc.
// (https://carbonfrost.com)
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

    public partial class ExpressionContext : DynamicExpressionContext {

        private readonly IPropertyProvider _selfValues;
        private readonly IDictionary<string, object> _data = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        private readonly IProperties _self;
        private readonly PropertyProviderCollection _dataProviders = new PropertyProviderCollection();

        // TODO Define variables using the context, plus parent contexts and binding

        public PropertyProviderCollection DataProviders {
            get {
                return _dataProviders;
            }
        }

        public IDictionary<string, object> Data {
            get { return _data; }
        }

        public ExpressionContext()
            : this(null) {}

        public ExpressionContext(IExpressionContext parent) : base(parent) {
            _self = Properties.FromValue(this);
            var dataValues = Properties.FromValue(Data);
            _selfValues = PropertyProvider.Compose(_self, dataValues, DataProviders);
        }

        protected override bool TrySetMemberCore(string name, object value) {
            if (_self.HasProperty(name)) {
                _self.SetProperty(name, value);
            } else {
                Data[name] = value;
            }
            return true;
        }

        protected override bool TryGetMemberCore(string name, out object result) {
            if (_TryGetCore(name, out result)) {
                return true;
            }
            return base.TryGetMemberCore(name, out result);
        }

        protected override IEnumerable<string> GetDynamicMemberNamesCore() {
            return _self.Select(t => t.Key).Concat(Data.Keys);
        }

        private bool _TryGetCore(string key, out object result) {
            if (_selfValues.TryGetProperty(key, typeof(object), out result)) {
                return true;
            }
            result = null;
            return false;
        }

        public new ExpressionContext Clone() {
            return (ExpressionContext) base.Clone();
        }

        protected override DynamicExpressionContext CloneCore() {
            var result = new ExpressionContext(Parent);
            result.Data.AddMany(Data);
            result.DataProviders.AddMany(DataProviders);
            return result;
        }
    }
}
