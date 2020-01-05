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
using System.Collections;
using System.Linq;
using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Core.Runtime.Expressions {

    public class NewArrayExpressionTests {

        [Fact]
        public void Evaluate_should_convert_to_array() {
            var expr = Expression.NewArray(Expression.Constant("left"), Expression.Constant(2));
            var array = ((IEnumerable) expr.Evaluate()).Cast<object>().ToArray();
            Assert.Equal(new object[] { "left", 2 }, array);
        }
    }
}
