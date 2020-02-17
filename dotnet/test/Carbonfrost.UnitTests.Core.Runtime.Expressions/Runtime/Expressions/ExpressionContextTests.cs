//
// Copyright 2015, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Core.Runtime.Expressions {

    public class ExpressionContextTests {

        [Fact]
        public void TryGetIndex_should_require_index_rank_one() {
            dynamic ec = new ExpressionContext();
            Assert.Throws<ArgumentException>(() => ec["a", "b"].ToString());
        }

        [Fact]
        public void TryGetIndex_should_imply_undefined_for_null_index() {
            dynamic ec = new ExpressionContext();
            Assert.Equal(Undefined.Value, ec[null]);
        }

        [Fact]
        public void TryGetIndex_should_resolve_items_from_name_scope() {
            var obj = new object();
            var ns = new NameScope {
                { "c", obj }
            };
            dynamic ec = ExpressionContext.FromNameScope(ns);
            Assert.Same(obj, ec["c"]);
        }

        [Fact]
        public void TryGetMember_using_dynamic_member_should_get_member() {
            dynamic subject = new ExpressionContext();
            subject.A = "hello";

            object result;
            Assert.True(((IExpressionContext) subject).TryGetMember("A", out result));
            Assert.Equal("hello", result);
        }

        [Fact]
        public void Clone_should_copy_collections() {
            var ec = new ExpressionContext();
            ec.Data["a"] = "a";
            ec.DataProviders.AddNew("b", PropertyProvider.Null);

            var clone = ec.Clone();
            Assert.NotSame(ec.DataProviders, clone.DataProviders);
            Assert.NotSame(ec.Data, clone.Data);
            Assert.SetEqual(ec.DataProviders, clone.DataProviders);
            Assert.SetEqual(ec.Data, clone.Data);
        }
    }
}
