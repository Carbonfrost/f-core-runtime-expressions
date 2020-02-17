//
// Copyright 2015 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

        // TODO Not every combination is being tested

        [Fact]
        public void Add_should_apply_String_concat_nominal() {
            var expr = Expression.Constant("left ").Add(Expression.Constant("/ right"));
            Assert.Equal("left / right", expr.Evaluate());
        }

        [Fact]
        public void Add_should_sum_int32_nominal() {
            var expr = Expression.Constant(5).Add(Expression.Constant(4));
            Assert.Equal((long) 9, expr.Evaluate());
        }

        [Theory]
        [InlineData("400", "not number")]
        [InlineData("not number", "400")]
        [InlineData("", "400")]
        [InlineData("0x20", "32")]
        public void Add_should_string_concat(string a, string b) {
            var expr = Expression.Constant(a).Add(Expression.Constant(b));
            Assert.Equal(a + b, expr.Evaluate());
        }

        // TODO Should be Int32, not double

        [Theory]
        [InlineData((double) 400)]
        [InlineData((float) 400)]
        [InlineData((int) 400)]
        [InlineData((uint) 400)]
        [InlineData((long) 400)]
        [InlineData((ulong) 400)]
        [InlineData((short) 400)]
        [InlineData((ushort) 400)]
        public void Add_should_coerce_to_decimal(object any) {
            var expr = Expression.Constant(any).Add(Expression.Constant(20m));
            Assert.Equal(420m, expr.Evaluate());
        }

        [Theory]
        [InlineData((byte) 4)]
        [InlineData((sbyte) 4)]
        public void Add_should_coerce_to_decimal_byte(object any) {
            var expr = Expression.Constant(any).Add(Expression.Constant(416m));
            Assert.Equal(420m, expr.Evaluate());
        }

        [Theory]
        [InlineData((double) 400)]
        [InlineData((float) 400)]
        [InlineData((int) 400)]
        [InlineData((uint) 400)]
        [InlineData((long) 400)]
        [InlineData((ulong) 400)]
        [InlineData((short) 400)]
        [InlineData((ushort) 400)]
        public void Add_should_coerce_to_double(object any) {
            var expr = Expression.Constant(any).Add(Expression.Constant(20d));
            Assert.Equal(420d, expr.Evaluate());
        }

        [Theory]
        [InlineData((byte) 4)]
        [InlineData((sbyte) 4)]
        public void Add_should_coerce_to_double_byte(object any) {
            var expr = Expression.Constant(any).Add(Expression.Constant(416d));
            Assert.Equal(420d, expr.Evaluate());
        }

        [Fact]
        public void Add_should_generate_user_message_not_defined_variables() {
            var expr = Expression.Constant(10).Add(Expression.Name("hello"));
            var ex = Record.Exception(
                () => expr.Evaluate()
            );
            Console.WriteLine(ex);
            Assert.Contains("`hello' is not defined.", ex.Message);
        }

        [Fact]
        public void Add_should_generate_Nan_for_undefined() {
            var exprs = new [] {
                Expression.Add(Expression.Undefined, Expression.Constant(3)),
                Expression.Add(Expression.Undefined, Expression.Undefined),
                Expression.Add(Expression.Constant(3), Expression.Undefined),
            };

            foreach (var e in exprs) {
                // Expected conversion to NaN
                Assert.Equal(double.NaN, e.Evaluate());
            }
        }
    }
}
