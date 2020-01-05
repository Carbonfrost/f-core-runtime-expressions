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

    public partial class UnaryExpressionTests {

        [Theory]
        [InlineData("false")]
        [InlineData("0")]
        [InlineData("''")]
        [InlineData("null")]
        [InlineData("undefined")]
        [InlineData("NaN")]
        public void Evaluate_negation_should_be_falsy(string expression) {
            Assert.Equal(true, Expression.Parse(expression).Negate().Evaluate());
            Assert.Equal(false, Expression.Parse(expression).Negate().Negate().Evaluate());
        }

        [Theory]
        [InlineData("true")]
        [InlineData("-1")]
        [InlineData("'text'")]
        [InlineData("10.5")]
        public void Evaluate_negation_should_be_truthy(string expression) {
            Assert.Equal(false, Expression.Parse(expression).Negate().Evaluate());
            Assert.Equal(true, Expression.Parse(expression).Negate().Negate().Evaluate());
        }

    }
}
