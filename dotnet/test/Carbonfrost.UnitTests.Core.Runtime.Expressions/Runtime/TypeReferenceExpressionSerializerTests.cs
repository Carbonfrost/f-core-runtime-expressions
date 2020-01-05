//
// Copyright 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public class TypeReferenceExpressionSerializerTests {

        [Fact]
        public void Get_should_apply_Parse_method_description() {
            TypeReference tr = TypeReference.Parse("System.Int32, mscorlib");
            var exp = Expression.Serialize(tr);
            // On desktop, culture/pkt aren't generated, but they are on netcore/ag
            Assert.Matches(@"^Carbonfrost\.Commons\.Core.Runtime\.TypeReference\.Parse\('System.Int32, mscorlib(, Culture=neutral, PublicKeyToken=null)?'\)$", exp.ToString());
        }
    }
}
