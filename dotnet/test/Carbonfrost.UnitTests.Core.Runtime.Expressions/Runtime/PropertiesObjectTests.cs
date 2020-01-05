//
// Copyright 2014 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.UnitTests.Core.Runtime {

    public class PropertiesObjectTests {

        [Fact]
        public void GetMember_should_bind_trivial() {
            var it = new PropertiesObject();
            it.Properties.SetProperty("hello", "world");
            dynamic po = (dynamic) it;
            Assert.Equal("world", po.hello);
        }

        [Fact]
        public void SetMember_should_bind_trivial() {
            var pObject = new PropertiesObject();
            dynamic po = pObject;
            po.hello = "world";
            Assert.Equal("world", pObject.Properties.GetProperty("hello"));
        }

        [Fact]
        public void GetProperty_will_evaluate_expression_when_it_is_used() {
            var pp = new PropertiesObject();
            pp.Properties["A"] = Expression.Parse("5");
            Assert.Equal(5, pp.Properties.GetProperty("A"));
        }

        [Fact]
        public void GetProperty_will_evaluate_expression_and_bind_expression_context_when_it_is_used() {
            var pp = new PropertiesObject();
            pp.Properties["A"] = 2;
            pp.Properties["B"] = 10;
            pp.Properties["C"] = Expression.Parse("A + B");

            Assert.Equal((long) 12, pp.Properties.GetProperty("C"));
        }

        [Fact]
        public void GetPropertyOfT_will_convert_to_required_type() {
            var pp = new PropertiesObject();
            pp.Properties["A"] = "https://";
            pp.Properties["B"] = "www.example.com";
            pp.Properties["C"] = Expression.Parse("A + B");

            Assert.Equal(new Uri("https://www.example.com"), pp.Properties.GetProperty<Uri>("C"));
        }

        [Fact]
        public void GetPropertyOfT_will_convert_to_required_type_from_array() {
            var pp = new PropertiesObject();
            pp.Properties["A"] = "https://";
            pp.Properties["B"] = "www.example.com";
            pp.Properties["C"] = Expression.NewArray(Expression.Name("A"), Expression.Name("B"));

            Assert.Equal(new Uri("https://www.example.com"), pp.Properties.GetProperty<Uri>("C"));
        }

        [Fact]
        public void GetProperty_will_obtain_expression_when_Expression_is_requested() {
            var pp = new PropertiesObject();
            var exp = Expression.Parse("5");
            pp.Properties.SetProperty("A", exp);
            Assert.Equal(exp, pp.Properties.GetProperty<Expression>("A"));
        }

        [Fact]
        public void CreateExpressionContext_should_contain_my_properties_in_the_context() {
            var derived = new DerivedObject {
                S = "S",
            };
            var ec = derived.CreateContext_();
            Assert.Equal("S", ec.Evaluate("s"));
        }

        [Fact]
        public void SetProperty_preevaluates_and_will_obtain_constant_expression_when_Expression_binding_is_disabled() {
            var derived = new BindingModes();
            derived.Properties.SetProperty("C", Expression.Parse("'hello ' + 'world'"));

            Assert.Null(derived.Properties.GetProperty<Expression>("C"));
            Assert.Equal("hello world", derived.Properties.GetString("C"));
        }

        class BindingModes : PropertiesObject {

            [Expression(BindingMode.Disabled)]
            public string C {
                get { return Properties.GetString("C"); }
                set { Properties.SetProperty("C", value); } }
        }

        class DerivedObject : PropertiesObject {

            public string S {
                get { return Properties.GetString("S"); }
                set { Properties.SetProperty("S", value); } }

            public string T {
                get { return Properties.GetString("T"); }
                set { Properties.SetProperty("T", value); } }

            public ExpressionContext CreateContext_() {
                return CreateExpressionContext();
            }
        }
    }
}
