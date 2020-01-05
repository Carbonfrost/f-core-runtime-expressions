//
// Copyright 2015, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Runtime.Expressions {

    public class ExpressionSerializerTests {

        [Fact]
        public void This_should_be_contained_as_an_adapter() {
            Assert.Contains("ExpressionSerializer", App.GetAdapterRoleNames());
        }

        [ExpressionSerializer(typeof(BExpressionSerializer))]
        class B {

        }

        class BExpressionSerializer : ExpressionSerializer {
            public override Expression ConvertToExpression(
                object value,
                IExpressionSerializerContext context) {
                return Expression.Null;
            }
        }

        class C {
            public Expression ToExpression() {
                return Expression.Null;
            }
        }

        [ExpressionSerializer(typeof(BExpressionSerializer))]
        class C1 {

            public Expression ToExpression() {
                return Expression.False;
            }
        }

        class D {

            public static D Parse(string text) {
                return new D();
            }

            public override string ToString() {
                return "text";
            }
        }

        [Fact]
        public void GetExpressionSerializer_should_obtain_via_metadata() {
            Assert.IsInstanceOf<BExpressionSerializer>(ExpressionSerializer.GetExpressionSerializer(new B()));
        }

        [Fact]
        public void GetExpressionSerializer_should_obtain_conventional_serializer() {
            Assert.IsInstanceOf<InvokeExpressionSerializer>(ExpressionSerializer.GetExpressionSerializer(new C()));
        }

        [Fact]
        public void GetExpressionSerializer_should_obtain_conventional_serializer_unless_overridden() {
            Assert.IsInstanceOf<BExpressionSerializer>(ExpressionSerializer.GetExpressionSerializer(new C1()));
        }

        [Fact]
        public void Serialize_should_process_Guid() {
            Guid g = new Guid("1D67720C-A341-457B-8E5C-AE6856A49A0B");
            Assert.Equal("new System.Guid('1d67720c-a341-457b-8e5c-ae6856a49a0b')", Expression.Serialize(g).ToString());
        }


        [Fact]
        public void Serialize_should_process_Uri() {
            Uri g = new Uri("https://example.io");
            Assert.Equal("new System.Uri('https://example.io', System.UriKind.Absolute)", Expression.Serialize(g).ToString());
        }

        [Fact]
        public void Serialize_should_obtain_and_apply_custom_serializer() {
            Assert.Equal("null", Expression.Serialize(new C()).ToString());
        }

        [Fact]
        public void Serialize_should_obtain_string_parse_serializer_if_available() {
            string fullName = typeof(D).FullName.Replace('+', '.');
            Assert.Equal(fullName + ".Parse('text')", Expression.Serialize(new D()).ToString());
        }
    }
}
