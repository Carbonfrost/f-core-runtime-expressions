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

    public partial class DefaultExpressionParserTests {

        [Fact]
        public void Parse_interpolated_string() {
            var d = new DefaultExpressionParser();
            var result = d.Parse("`Hello ${Friend}`");
            Assert.IsInstanceOf<InterpolatedStringExpression>(result);
            var ise = (InterpolatedStringExpression) result;

            Assert.Equal(
                "Hello ",
                ((InterpolatedStringTextContent) ise.Elements[0]).Text
            );
            Assert.IsInstanceOf(
                typeof(NameExpression),
                ((Interpolation) ise.Elements[1]).Value
            );
        }

        [Fact]
        public void Parse_interpolated_string_escaped_sequence() {
            var d = new DefaultExpressionParser();
            var result = d.Parse("`Hello \\${Friend}`");
            Assert.IsInstanceOf<ConstantExpression>(result);
            Assert.Equal(
                "Hello \\${Friend}",
                ((ConstantExpression) result).Value
            );
        }

        [Theory]
        [InlineData('\'')]
        [InlineData('"')]
        public void Parse_interpolated_string_should_not_be_processed_on_quotes(char q) {
            var d = new DefaultExpressionParser();
            var result = d.Parse($"{q}Hello ${{Friend}}{q}");
            Assert.IsInstanceOf<ConstantExpression>(result);
            Assert.Equal(
                $"Hello ${{Friend}}",
                ((ConstantExpression) result).Value
            );
        }
    }
}
