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
using System.Linq;
using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Core.Runtime.Expressions {

    public class LambdaExpressionTests {

        [Fact]
        public void Lambda_should_evaluate_in_context_of_closure() {
            var body = Expression.Parse("a + b");
            var closure = new ExpressionContext {
                Data = {
                    { "a", 4 },
                    { "b", 2 } },
            };
            var lambda = Expression.Call(
                Expression.Lambda(body, closure, null));
            Assert.Equal((long) 6, lambda.Evaluate());
        }

        [Fact]
        public void Lambda_should_evaluate_prefer_parameters_to_closure_variables() {
            var body = Expression.Parse("a + b");
            var closure = new ExpressionContext {
                Data = {
                    { "a", 4 },
                    { "b", 2 } },
            };
            var lambda = Expression.Call(
                Expression.Lambda(body, closure, new [] { new LambdaParameter("a") }), 5);
            Assert.Equal((long) 7, lambda.Evaluate());
        }
    }
}
