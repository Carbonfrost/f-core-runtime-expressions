//
// Copyright 2014, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    sealed class SingletonAnnotationList : AnnotationList {

        private readonly object _value;

        public SingletonAnnotationList(object value) {
            _value = value;
        }

        public override IEnumerable<T> OfType<T>() {
            var t = _value as T;

            if (t == null) {
                return Enumerable.Empty<T>();
            }
            return new T[] { t };
        }

        public override bool Contains(object annotation) {
            return object.Equals(_value, annotation);
        }

        public override AnnotationList Add(object annotation) {
            if (annotation == null) {
                throw new ArgumentNullException("annotation");
            }
            return new DefaultAnnotationList(_value, annotation);
        }

        public override AnnotationList RemoveOfType(Type type) {
            if (type == null)
                throw new ArgumentNullException("type");

            if (type.GetTypeInfo().IsInstanceOfType(_value)) {
                return Empty;
            }

            return this;
        }

        public override AnnotationList Remove(object annotation) {
            if (annotation == null) {
                throw new ArgumentNullException("annotation");
            }
            if (_value == annotation) {
                return Empty;
            }
            return this;
        }

        public override IEnumerable<object> OfType(Type type) {
            if (type == null) {
                throw new ArgumentNullException("type");
            }

            if (type.GetTypeInfo().IsInstanceOfType(_value)) {
                return new object[] { _value };
            }
            return Enumerable.Empty<object>();

        }
    }

}
