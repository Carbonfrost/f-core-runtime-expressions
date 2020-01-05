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

using System;
using System.Linq;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Core.Runtime.Expressions {

    public class MemberAccessExpressionTests {

        [Fact]
        public void MemberAccess_should_obtain_item_by_name_qualified() {
            var ec = new ExpressionContext();
            ec.Data["hello"] = new {
                world = "w"
            };
            var expr = Expression.Parse("hello.world");
            Assert.Equal("w", expr.Evaluate(ec));
        }

        [Fact]
        public void MemberAccess_should_throw_reference_error() {
            var ec = new ExpressionContext();

            var expr = Expression.Parse("hello.world");
            var ex = Record.Exception(() => expr.Evaluate(ec));
            Assert.Contains("`hello' is not defined.", ex.Message);
        }

        [Fact]
        public void MemberAccess_should_ignore_case() {
            var ec = new ExpressionContext();
            ec.Data["hello"] = new {
                World = "w"
            };
            var expr = Expression.Parse("hello.world");
            Assert.Equal("w", expr.Evaluate(ec));
        }

        [Fact]
        public void MemberAccess_should_return_Undefined_on_missing_property() {
            var ec = new ExpressionContext();
            ec.Data["hello"] = new object();
            var expr = Expression.Parse("hello.world");
            Assert.Equal(Undefined.Value, expr.Evaluate(ec));
        }

        [Fact]
        public void MemberAccess_should_dereference_from_name_scope() {
            var ec = new ExpressionContext(null, new NameScope { { "hello", new { world = "w" } } });
            var expr = Expression.Parse("hello.world");
            Assert.Equal("w", expr.Evaluate(ec));
        }
    }
}
