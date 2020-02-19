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

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    class NameScopeExpressionContext : DynamicExpressionContext {

        private readonly INameScope _nameScope;

        public NameScopeExpressionContext(INameScope nameScope) {
            _nameScope = nameScope;
        }

        protected override bool TrySetMemberCore(string name, object value) {
            _nameScope.RegisterName(name, value);
            return true;
        }

        protected override bool TryGetMemberCore(string name, out object result) {
            result = _nameScope.FindName(name);
            return result != null;
        }

        protected override IEnumerable<string> GetDynamicMemberNamesCore() {
            return Array.Empty<string>();
        }



        public override object GetService(Type serviceType) {
            if (serviceType == typeof(INameScope)) {
                return _nameScope;
            }
            return base.GetService(serviceType);
        }
    }
}
