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

    class ReadOnlyExpressionContext : DynamicExpressionContext {
        private readonly IExpressionContext _inner;

        public ReadOnlyExpressionContext(IExpressionContext inner) {
            _inner = inner;
        }

        protected override bool TrySetMemberCore(string name, object value) {
            value = null;
            return false;
        }

        protected override bool TryGetMemberCore(string name, out object result) {
            return _inner.TryGetMember(name, out result);
        }

        protected override IEnumerable<string> GetDynamicMemberNamesCore() {
            if (_inner is DynamicExpressionContext s) {
                return s.GetDynamicMemberNames();
            }
            return Array.Empty<string>();
        }
    }
}
