//
// Copyright 2013 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public class DefaultExpressionParserTests {

        [Fact]
        public void Parse_unterminated_quoted_string() {
            var d = new DefaultExpressionParser();
            Assert.Throws<SyntaxErrorException>(() => d.Parse("3 + 'unterminated"));
        }

        [Fact]
        public void Parse_unexpected_token_error() {
            var d = new DefaultExpressionParser();
            var error = Record.Exception(() => d.Parse(")"));

            Assert.IsInstanceOf<SyntaxErrorException>(error);
            Assert.Equal("Unexpected `)'", error.Message);
        }

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
    }
}
