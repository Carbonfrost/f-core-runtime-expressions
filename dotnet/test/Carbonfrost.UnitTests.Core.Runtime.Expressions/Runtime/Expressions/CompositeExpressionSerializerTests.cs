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
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Core.Runtime.Expressions {

    public class CompositeExpressionSerializerTests {

        [Fact]
        public void Serialize_should_process_composite_instance_items() {
            var b = new Bravo();
            b.Items.Add(new Alpha2 { A = true, B = "Hello" });
            b.Items.Add(new Alpha2 { A = false, B = "World" });
            string actual = Expression.Serialize(b).ToString();

            Assert.Equal(CompressWS(@"{ bravo1 = new Carbonfrost.UnitTests.Core.Runtime.Expressions.Bravo(),
  {
      { alpha21 = new Carbonfrost.UnitTests.Core.Runtime.Expressions.Alpha2(),
        alpha21.A = true,
        alpha21.B = 'Hello'
      },
      bravo1.Items.Add(alpha21),
      { alpha22 = new Carbonfrost.UnitTests.Core.Runtime.Expressions.Alpha2(),
        alpha22.A = false,
        alpha22.B = 'World'
      },
      bravo1.Items.Add(alpha22)
  } }"), CompressWS(actual));
        }

        [Fact]
        public void Serialize_should_process_object_graphs() {
            var c = new Charlie();
            c.C = c;

            string actual = CompressWS(Expression.Serialize(c).ToString());

            Assert.Equal(CompressWS(@"{ charlie1 = new Carbonfrost.UnitTests.Core.Runtime.Expressions.Charlie(),
                    charlie1.A = null,
                    charlie1.B = '\u0000',
                    charlie1.C = charlie1 }"), actual);
        }

        [Fact]
        public void Serialize_should_process_object_graphs_circular() {
            var e = new Echo();
            var f = new Foxtrot { Owner = e, Parent = e };
            e.Children.Add(f);

            string actual = CompressWS(Expression.Serialize(f).ToString());

            Assert.Equal(CompressWS(@"{
                foxtrot1 = new Carbonfrost.UnitTests.Core.Runtime.Expressions.Foxtrot(),
                foxtrot1.Owner = {
                    echo1 = new Carbonfrost.UnitTests.Core.Runtime.Expressions.Echo(),
                    echo1.Parent = null,
                    { echo1.Children.Add(foxtrot1) }
                }, foxtrot1.Parent = echo1 }"), actual);
        }

        static string CompressWS(string str) {
            // Compressing WS simplifies comparison
            return Regex.Replace(str, @"\s+", " ").Trim();
        }
    }
}
