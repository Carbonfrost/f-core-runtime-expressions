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

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    class CompositeExpressionContext : IExpressionContext {

        private readonly IExpressionContext[] _contexts;

        public CompositeExpressionContext(IExpressionContext[] contexts) {
            _contexts = contexts;
        }

        public bool TryGetIndex(object[] indexes, out object result) {
            foreach (var m in _contexts) {
                if (m.TryGetIndex(indexes, out result)) {
                    return true;
                }
            }
            result = null;
            return false;
        }

        public bool TryGetMember(string name, out object result) {
            foreach (var m in _contexts) {
                if (m.TryGetMember(name, out result)) {
                    return true;
                }
            }
            result = null;
            return false;
        }

        public bool TrySetMember(string name, object value) {
            foreach (var m in _contexts) {
                if (m.TrySetMember(name, value)) {
                    return true;
                }
            }
            return false;
        }

        public void MakeReadOnly() {
            IsReadOnly = true;
        }

        public bool IsReadOnly {
            get;
            private set;
        }

        public object GetService(Type serviceType) {
            return null;
        }
    }
}
