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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    sealed class DefaultAnnotationList : AnnotationList {

        private object[] _annotations;

        public DefaultAnnotationList(object one, object two) {
            _annotations = new [] { one, two };
        }

        public override IEnumerable<T> OfType<T>() {
            return _annotations.OfType<T>();
        }

        public override bool Contains(object annotation) {
            return _annotations.Contains(annotation);
        }

        public override AnnotationList Add(object annotation) {
            object[] annotations = _annotations;
            int index = 0;

            // Search for an empty space
            while ((index < annotations.Length) && (annotations[index] != null)) {
                index++;
            }

            // Ensure capacity
            if (index == annotations.Length) {
                Array.Resize(ref annotations, index * 2);
                _annotations = annotations;
            }

            annotations[index] = annotation;
            return this;
        }

        public override AnnotationList RemoveOfType(Type type) {
            for (int i = 0; i < _annotations.Length; i++) {
                if (type.GetTypeInfo().IsInstanceOfType(_annotations[i])) {
                    _annotations[i] = null;
                }
            }
            return this;
        }

        public override AnnotationList Remove(object annotation) {
            for (int i = 0; i < _annotations.Length; i++) {
                if (object.Equals(_annotations[i], annotation)) {
                    _annotations[i] = null;
                }
            }
            return this;
        }

        public override IEnumerable<object> OfType(Type type) {
            return _annotations.Where(type.IsInstanceOfType);
        }
    }
}
