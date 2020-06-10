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
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Core.Runtime.Expressions {

    public class ExpressionTests {

        [Fact]
        public void test_parse_identifier_nominal() {
            var e = Expression.Parse("cartoon");
            Assert.IsInstanceOf<NameExpression>(e);

            NameExpression ne = (NameExpression) e;
            Assert.Equal("cartoon", ne.Name);
        }

        [Fact]
        public void test_parse_identifier_member_access_nominal() {
            var e = Expression.Parse("a.b.c.d");
            Assert.IsInstanceOf<MemberAccessExpression>(e);
            Assert.Equal("a.b.c.d", e.ToString());
        }

        [Fact]
        public void test_parse_boolean_literal_nominal() {
            var e = Expression.Parse("true");
            Assert.IsInstanceOf<ConstantExpression>(e);

            ConstantExpression ne = (ConstantExpression) e;
            Assert.Equal(true, ne.Value);
        }

        [Fact]
        public void test_parse_member_access_nominal() {
            var e = Expression.Parse("cartoon.network");
            Assert.IsInstanceOf<MemberAccessExpression>(e);
            Assert.Equal("cartoon.network", e.ToString());
        }

        [Fact]
        public void test_parse_integer() {
            var e = Expression.Parse("50");
            Assert.IsInstanceOf<ConstantExpression>(e);
            Assert.Equal("50", e.ToString());
            Assert.Equal(50, e.Evaluate());
        }

        [Fact]
        public void test_parse_decimal() {
            var e = Expression.Parse("50.4");
            Assert.IsInstanceOf<ConstantExpression>(e);
            Assert.Equal("50.4", e.ToString());
            Assert.Equal(50.4M, e.Evaluate());
        }

        [Fact]
        public void test_parse_binary_operator_nominal() {
            var e = Expression.Parse("4 * 0xA0");
            Assert.IsInstanceOf<BinaryExpression>(e);
            Assert.Equal("4 * 160", e.ToString());
        }

        [Fact]
        public void test_parse_binary_operator_ws_negative_number() {
            var e = Expression.Parse("4 + -4");
            Assert.IsInstanceOf<BinaryExpression>(e);
            Assert.Equal("4 + -4", e.ToString());
        }

        [Fact]
        public void test_parse_binary_operator_ws_minus() {
            var e = Expression.Parse("4-4");
            Assert.IsInstanceOf<BinaryExpression>(e);
            Assert.Equal("4 - 4", e.ToString());
        }

        [Fact]
        public void test_parse_call_binary_operator_nominal() {
            var e = Expression.Parse("sin(x) + cos(y)");
            Assert.IsInstanceOf<BinaryExpression>(e);

            var bin = (BinaryExpression) e;
            Assert.IsInstanceOf<CallExpression>(bin.Left);
            Assert.Equal(1, ((CallExpression) bin.Left).Arguments.Count);

            Assert.Equal("sin(x)", bin.Left.ToString());
            Assert.Equal("cos(y)", bin.Right.ToString());
            Assert.Equal("sin(x) + cos(y)", e.ToString());
        }

        [Fact]
        public void test_parse_call_empty_arg_list() {
            var e = Expression.Parse("nop()");
            Assert.IsInstanceOf<CallExpression>(e);
            Assert.Equal("nop()", e.ToString());

            var call = (CallExpression) e;
            Assert.IsInstanceOf<NameExpression>(call.Expression);
        }

        [Fact]
        public void test_parse_call_complex_arg_list() {
            var e = Expression.Parse("theta(x + y * 560 - sigma())");
            Assert.IsInstanceOf<CallExpression>(e);
            Assert.Equal("theta(x + y * 560 - sigma())", e.ToString());
        }

        [Fact]
        public void test_parse_call_qualfied_name_simple() {
            var e = Expression.Parse("Math.abs(-3)");
            Assert.IsInstanceOf<CallExpression>(e);
            Assert.Equal("Math.abs(-3)", e.ToString());

            var call = (CallExpression) e;
            Assert.IsInstanceOf<MemberAccessExpression>(call.Expression);
            Assert.Equal("Math.abs", call.Expression.ToString());
        }

        [Fact]
        public void Parse_should_handle_NaN() {
            var e = Expression.Parse("NaN");
            Assert.Equal(double.NaN, e.Evaluate());
        }

        [Fact]
        public void Parse_should_handle_Null() {
            var e = Expression.Parse("null");
            Assert.Null(e.Evaluate());
        }

        [Fact]
        public void Parse_should_handle_Undefined() {
            var e = Expression.Parse("undefined");
            Assert.Equal(Undefined.Value, e.Evaluate());
        }

        [Fact]
        public void Parse_should_handle_unary_operators() {
            var e = Expression.Parse("!item");
            Assert.Equal(ExpressionType.Not, e.ExpressionType);
            Assert.Equal("!item", e.ToString());

            e = Expression.Parse("-item");
            Assert.Equal(ExpressionType.Negate, e.ExpressionType);
            Assert.Equal("-item", e.ToString());
        }

        [Fact]
        public void Parse_should_handle_binary_operator_logical() {
            var e = Expression.Parse("null == assembly");
            Assert.Equal(ExpressionType.EqualTo, e.ExpressionType);

            e = Expression.Parse("null != assembly");
            Assert.Equal(ExpressionType.NotEqualTo, e.ExpressionType);

            e = Expression.Parse("null && assembly");
            Assert.Equal(ExpressionType.AndAlso, e.ExpressionType);

            e = Expression.Parse("null || assembly");
            Assert.Equal(ExpressionType.OrElse, e.ExpressionType);
        }

        [Fact]
        public void Parse_should_handle_expression_parens() {
            var e = Expression.Parse("(a + b) / 2");
            Assert.Equal(ExpressionType.Divide, e.ExpressionType);
            Assert.Equal(ExpressionType.Add, ((BinaryExpression) e).Left.ExpressionType);
            Assert.Equal("(a + b) / 2", e.ToString());
        }

        [Fact]
        public void Parse_should_handle_expression_parens_nested() {
            var e = Expression.Parse("((a + (-b))) / 2");
            Assert.Equal(ExpressionType.Divide, e.ExpressionType);
            Assert.Equal(ExpressionType.Add, ((BinaryExpression) e).Left.ExpressionType);
            Assert.Equal("(a + -b) / 2", e.ToString());
        }

        [Fact]
        public void Parse_should_handle_interpolated_strings() {
            var e = Expression.Parse("'hello ${a + b / 2}${time()}'");
            Assert.Equal("'hello ${a + b / 2}${time()}'", e.ToString());
        }

        [Theory]
        [InlineData(ExpressionType.Add, ExpressionType.Multiply)]
        [InlineData(ExpressionType.Add, ExpressionType.MultiplyChecked)]
        [InlineData(ExpressionType.Subtract, ExpressionType.MultiplyChecked)]
        [InlineData(ExpressionType.AndAlso, ExpressionType.Multiply)]
        [InlineData(ExpressionType.Add, ExpressionType.Divide)]
        public void IsLowerPrecedence_should_apply_to_order_ops(ExpressionType left, ExpressionType right) {
            Assert.False(Expression.IsLowerPrecedence(left, right));
        }
    }
}
