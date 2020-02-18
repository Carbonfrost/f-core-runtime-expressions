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

using System;
using System.Collections.Generic;
using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Core.Runtime.Expressions {

    public class AnnotationListTests {

        internal IEnumerable<AnnotationList> Subjects {
            get {
                return new [] {
                    AnnotationList.Empty,
                    new SingletonAnnotationList(new object()),
                    new DefaultAnnotationList(new object(), new object()),
                };
            }
        }

        [Fact]
        public void Add_to_Empty_will_have_singleton_list() {
            var anno = new object();
            var result = AnnotationList.Empty.Add(anno);
            Assert.IsInstanceOf<SingletonAnnotationList>(result);
        }

        [Fact]
        public void Add_to_Singleton_will_have_default_list() {
            var anno = new object();
            var result = new SingletonAnnotationList(anno).Add(anno);
            Assert.IsInstanceOf<DefaultAnnotationList>(result);
        }

        [Theory]
        [PropertyData(nameof(Subjects))]
        internal void Add_will_throw_on_null(AnnotationList subj) {
            Assert.Throws<ArgumentNullException>(() => subj.Add(null));
        }

        [Theory]
        [PropertyData(nameof(Subjects))]
        internal void Remove_will_throw_on_null(AnnotationList subj) {
            Assert.Throws<ArgumentNullException>(() => subj.Remove(null));
        }

        [Theory]
        [PropertyData(nameof(Subjects))]
        internal void RemoveOfType_will_throw_on_null(AnnotationList subj) {
            Assert.Throws<ArgumentNullException>(() => subj.RemoveOfType(null));
        }

        [Theory]
        [PropertyData(nameof(Subjects))]
        internal void Contains_should_determine_if_item_is_present(AnnotationList subj) {
            var anno = new PAnnotation();
            var result = subj.Add(anno);

            Assert.True(result.Contains(anno));
        }

        [Theory]
        [PropertyData(nameof(Subjects))]
        internal void Contains_should_determine_if_item_is_not_present(AnnotationList subj) {
            var anno = new PAnnotation();
            Assert.False(subj.Contains(anno));
        }

        [Theory]
        [PropertyData(nameof(Subjects))]
        internal void OfType_should_return_instances(AnnotationList subj) {
            var anno = new PAnnotation();
            var result = subj.Add(anno);

            Assert.Equal(new [] { anno }, result.OfType<PAnnotation>());
            Assert.Equal(new [] { anno }, result.OfType(typeof(PAnnotation)));
        }

        [Theory]
        [PropertyData(nameof(Subjects))]
        internal void OfType_should_be_empty_for_wrong_type(AnnotationList subj) {
            Assert.Empty(subj.OfType<PAnnotation>());
            Assert.Empty(subj.OfType(typeof(PAnnotation)));
        }

        class PAnnotation {}
    }
}
