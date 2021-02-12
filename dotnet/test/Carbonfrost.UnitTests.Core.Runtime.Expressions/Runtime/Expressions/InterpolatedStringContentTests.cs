//
// Copyright 2021 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

    public class InterpolatedStringContentTests {

        [Fact]
        public void Interpolation_should_provide_correct_values() {
            var example = InterpolatedStringContent.TextContent("hello");

            Assert.False(example.IsInterpolation);
            Assert.Equal(InterpolatedStringContentType.TextContent, example.InterpolatedStringContentType);
        }

        [Fact]
        public void TextContent_should_provide_correct_values() {
            var example = InterpolatedStringContent.Interpolation(Expression.Name("who"));

            Assert.True(example.IsInterpolation);
            Assert.Equal(InterpolatedStringContentType.Interpolation, example.InterpolatedStringContentType);
        }
    }
}
