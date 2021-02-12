//
// Copyright 2015, 2021 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Core.Runtime.Expressions {

    public class InterpolatedStringExpressionTests {

        [Fact]
        public void Evaluate_should_bind_variables() {
            var expr = Expression.InterpolatedString(
                InterpolatedStringContent.TextContent("hello, is it "),
                InterpolatedStringContent.Interpolation(
                    Expression.Name("who")
                ),
                InterpolatedStringContent.TextContent(" "),
                InterpolatedStringContent.Interpolation(
                    Expression.Name("for")
                ),
                InterpolatedStringContent.TextContent(" looking for?")
            );
            var ec = new ExpressionContext {
                Data = {
                    ["who"] = "me",
                    ["for"] = "you're",
                }
            };
            Assert.Equal("hello, is it me you're looking for?", expr.Evaluate(ec));
        }

        [Fact]
        public void Evaluate_should_initialize_using_implicit_operator() {
            var expr = Expression.InterpolatedString(
                "hello, it's ",
                Expression.Name("me")
            );
            Assert.Equal("`hello, it's ${me}`", expr.ToString());
        }
    }
}
