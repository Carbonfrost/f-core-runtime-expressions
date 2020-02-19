//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
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

    public class DefaultAnnotationListTests {

        [Fact]
        public void Add_will_resize_array() {
            var anno = new object();
            var result = new DefaultAnnotationList(anno, anno);
            result.Add(anno);

            Assert.Equal(4, result.GetArray().Length);
        }

        [Fact]
        public void Remove_then_Add_will_set_item() {
            var anno1 = new object();
            var anno2 = new object();
            var list = new DefaultAnnotationList(
                anno1,
                anno2
            ).Remove(anno1).Add(anno2) as DefaultAnnotationList;

            var array = list.GetArray();
            Assert.Equal(new [] { anno2, anno2 }, array);
        }

        [Fact]
        public void Remove_returns_Empty_when_made_empty() {
            var anno = new object();
            var result = new DefaultAnnotationList(anno, anno).Remove(anno);

            Assert.IsInstanceOf<EmptyAnnotationList>(result);
        }

        [Fact]
        public void Remove_clears_item_in_array() {
            var anno1 = new object();
            var anno2 = new object();
            var list = new DefaultAnnotationList(
                anno1,
                anno2
            ).Remove(anno1) as DefaultAnnotationList;

            var array = list.GetArray();
            Assert.Equal(new [] { null, anno2 }, array);
        }

        [Fact]
        public void RemoveOfType_returns_Empty_when_made_empty() {
            var result = new DefaultAnnotationList(
                new object(),
                new object()
            ).RemoveOfType(typeof(object));

            Assert.IsInstanceOf<EmptyAnnotationList>(result);
        }

    }
}
