//
// Copyright 2015 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Core.Runtime.Expressions {

    public class DynamicExpressionContextTests {

        [Fact]
        public void GetDynamicMemberNames_should_contain_parent() {
            var subject = new DynamicExpressionContext();
            Assert.Equal(new [] { "Parent" }, subject.GetDynamicMemberNames());
        }

        class PDynamic : DynamicExpressionContext {
            public string N { get; set; }
        }

        [Fact]
        public void TrySetMember_using_existing_property_should_persist_value() {
            var subject = new PDynamic();

            Assert.True(((IExpressionContext) subject).TrySetMember("N", "hello"));
            Assert.Equal("hello", subject.N);
        }

        [Fact]
        public void TryGetMember_using_existing_property_should_get_member() {
            var subject = new PDynamic { N = "hello" };

            object result;
            Assert.True(((IExpressionContext) subject).TryGetMember("N", out result));
            Assert.Equal("hello", result);
        }

    }
}
