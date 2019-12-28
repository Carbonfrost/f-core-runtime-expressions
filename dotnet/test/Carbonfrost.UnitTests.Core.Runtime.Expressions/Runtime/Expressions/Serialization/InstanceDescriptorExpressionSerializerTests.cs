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

using System.Net.Sockets;
using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Core.Runtime.Expressions {

    public class InstanceDescriptorExpressionSerializerTests {

        [Fact]
        public void Serialize_should_process_enum_nominal() {
            Assert.Equal("Carbonfrost.Commons.Core.Runtime.Expressions.ExpressionType.Add",
                Expression.Serialize(ExpressionType.Add).ToString());
        }

        [Fact]
        public void Serialize_should_process_flags_enum() {
            var expr = Expression.Serialize(SocketFlags.Broadcast | SocketFlags.Peek);
            Assert.Equal("System.Enum.ToObject(typeof(System.Net.Sockets.SocketFlags), 1026)",
                expr.ToString());

            Assert.Equal(typeof(SocketFlags), expr.Annotation<TypeBinding>().Type);
        }
    }
}
