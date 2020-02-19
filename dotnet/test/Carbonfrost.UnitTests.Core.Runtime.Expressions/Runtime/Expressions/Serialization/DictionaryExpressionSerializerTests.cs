//
// Copyright 2015, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Core.Runtime.Expressions {

    public class DictionaryExpressionSerializerTests {

        [XFact(Reason = "Pending introduction of TemplateExpression")]
        public void Serialize_should_process_dictionary_nominal() {
            var dict = new Dictionary<string, int> {
                { "a", 9 },
                { "b", 52 }
            };

            Assert.Equal(CompressWS(@"{
                dictionary21 = new System.Collections.Generic.Dictionary<System.String,System.Int32>(),
                { dictionary21.Add('a', 9),
                  dictionary21.Add('b', 52) }
                }"), CompressWS(Expression.Serialize(dict).ToString()));
        }

        static string CompressWS(string str) {
            // Compressing WS simplifies comparison
            return Regex.Replace(str, @"\s+", " ").Trim();
        }
    }
}
