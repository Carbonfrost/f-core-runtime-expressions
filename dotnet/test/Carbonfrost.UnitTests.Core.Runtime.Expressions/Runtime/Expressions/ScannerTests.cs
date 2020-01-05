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

using System;
using System.Linq;
using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Spec;
using TokenType = Carbonfrost.Commons.Core.Runtime.Expressions.TokenType;

namespace Carbonfrost.UnitTests.Core.Runtime.Expressions {

    public class ScannerTests {

        [Fact]
        public void test_scanner_nominal() {
            Scanner s = new Scanner("2 + 2 and 'text'");
            Assert.True(s.MoveNext());
            Assert.Equal(TokenType.Number, s.Current.Type);
            Assert.Equal("2", s.Current.Value);

            Assert.True(s.MoveNext());
            Assert.Equal(TokenType.Plus, s.Current.Type);
            Assert.Equal("+", s.Current.Value);

            Assert.True(s.MoveNext());
            Assert.Equal(TokenType.Number, s.Current.Type);
            Assert.Equal("2", s.Current.Value);

            Assert.True(s.MoveNext());
            Assert.Equal(TokenType.And, s.Current.Type);
            Assert.Equal("and", s.Current.Value);

            Assert.True(s.MoveNext());
            Assert.Equal(TokenType.String, s.Current.Type);
            Assert.Equal("text", s.Current.Value);
        }

        [Fact]
        public void test_scanner_negative_number_ws() {
            Scanner s = new Scanner("10-8");
            Assert.True(s.MoveNext());
            Assert.Equal(TokenType.Number, s.Current.Type);
            Assert.Equal("10", s.Current.Value);

            Assert.True(s.MoveNext());
            Assert.Equal(TokenType.Number, s.Current.Type);
            Assert.Equal("-8", s.Current.Value);
        }

        [Fact]
        public void test_scanner_string_syntax_nominal() {
            Scanner s = new Scanner("\"text\" 'text'");
            Assert.True(s.MoveNext());
            Assert.Equal(TokenType.String, s.Current.Type);
            Assert.Equal("text", s.Current.Value);

            Assert.True(s.MoveNext());
            Assert.Equal(TokenType.String, s.Current.Type);
            Assert.Equal("text", s.Current.Value);
        }

        [Fact]
        public void test_scanner_function_call() {
            Scanner s = new Scanner("theta(x + y * 560 - sigma())");
            Assert.True(s.MoveNext());
            Assert.Equal(TokenType.Function, s.Current.Type);
            Assert.Equal("theta", s.Current.Value);

            Assert.True(s.MoveNext());
            Assert.Equal(TokenType.LeftParen, s.Current.Type);

            Assert.True(s.MoveNext());
            Assert.Equal(TokenType.Identifier, s.Current.Type);
            Assert.Equal("x", s.Current.Value);

            Assert.True(s.MoveNext());

            Assert.True(s.MoveNext());
            Assert.Equal(TokenType.Identifier, s.Current.Type);
            Assert.Equal("y", s.Current.Value);

            Assert.True(s.MoveNext());
            Assert.True(s.MoveNext());
            Assert.True(s.MoveNext());

            Assert.True(s.MoveNext());
            Assert.Equal(TokenType.Function, s.Current.Type);
            Assert.Equal("sigma", s.Current.Value);

            Assert.True(s.MoveNext());

            Assert.True(s.MoveNext());
            Assert.Equal(TokenType.RightParen, s.Current.Type);

            Assert.True(s.MoveNext());
            Assert.Equal(TokenType.RightParen, s.Current.Type);
        }


        [Fact]
        public void test_scanner_string_unterminated() {
            Scanner s = new Scanner("'unterminated...");
            Assert.False(s.MoveNext());
            Assert.True(s.IsError);
        }

    }
}
