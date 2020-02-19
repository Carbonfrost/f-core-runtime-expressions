//
// Copyright 2014, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Linq;
using System.Reflection;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    sealed class DefaultAnnotationList : AnnotationList {

        private object[] _annotations;
        private int _count;
        private int _lastAddIndex;

        public DefaultAnnotationList(object one, object two) {
            _annotations = new [] { one, two };
            _count = 2;
        }

        public override IEnumerable<T> OfType<T>() {
            return _annotations.OfType<T>();
        }

        public override bool Contains(object annotation) {
            return _annotations.Contains(annotation);
        }

        public override AnnotationList Add(object annotation) {
            if (annotation == null) {
                throw new ArgumentNullException(nameof(annotation));
            }
            int index = -1;

            if (_count == _annotations.Length) {
                Array.Resize(ref _annotations, _annotations.Length * 2);
            }

            // Search for an empty space
            for (int i = 0; i < _annotations.Length; i++) {
                if (_annotations[(i + _lastAddIndex) % _annotations.Length] == null) {
                    index = i;
                }
            }

            _lastAddIndex = index;
            _count += 1;
            _annotations[index] = annotation;
            return this;
        }

        public override AnnotationList RemoveOfType(Type type) {
            if (type == null) {
                throw new ArgumentNullException(nameof(type));
            }
            for (int i = 0; i < _annotations.Length; i++) {
                if (type.GetTypeInfo().IsInstanceOfType(_annotations[i])) {
                    _annotations[i] = null;
                    _count--;
                }
            }
            if (_count == 0) {
                return Empty;
            }
            return this;
        }

        public override AnnotationList Remove(object annotation) {
            if (annotation == null) {
                throw new ArgumentNullException(nameof(annotation));
            }
            for (int i = 0; i < _annotations.Length; i++) {
                if (object.Equals(_annotations[i], annotation)) {
                    _annotations[i] = null;
                    _count--;
                }
            }
            if (_count == 0) {
                return Empty;
            }
            return this;
        }

        public override IEnumerable<object> OfType(Type type) {
            if (type == null) {
                throw new ArgumentNullException(nameof(type));
            }
            return _annotations.Where(type.IsInstanceOfType);
        }

        // For tests
        internal object[] GetArray() {
            return _annotations;
        }

    }
}

