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

    public partial class BinaryExpressionTests {

        [Fact]
        public void NeedParens_should_be_true_for_binary_expressions() {
            var left = Expression.Constant(3);
            var right = Expression.Parse("8 + 2");
            var bin = Expression.Multiply(left, right); // 3 * (8 + 2)
            Assert.False(bin.LeftNeedsParens);
            Assert.True(bin.RightNeedsParens);
        }

        [Fact]
        public void NeedParens_should_be_false_for_order_of_ops() {
            var left = Expression.Parse("4 + 2");
            var right = Expression.Parse("8 * 2");
            var bin = Expression.Add(left, right); // 4 + 2 + 8 * 2
            Assert.False(bin.LeftNeedsParens);
            Assert.False(bin.RightNeedsParens);
        }
    }
}
