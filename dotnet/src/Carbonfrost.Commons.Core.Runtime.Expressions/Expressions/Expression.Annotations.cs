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

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    partial class Expression {

        private AnnotationList _annotations = AnnotationList.Empty;

        public void AddAnnotation(object annotation) {
            if (annotation == null) {
                throw new ArgumentNullException("annotation");
            }

            _annotations = _annotations.Add(annotation);
        }

        public bool HasAnnotation<T>() where T : class {
            return _annotations.OfType<T>().Any();
        }

        public bool HasAnnotation(object instance) {
            return _annotations.Contains(instance);
        }

        public T Annotation<T>() where T : class {
            return _annotations.OfType<T>().FirstOrDefault();
        }

        public T AnnotationOrDefault<T>(T defaultValue) where T: class {
            return Annotation<T>() ?? defaultValue;
        }

        public object Annotation(Type type) {
            if (type == null) {
                throw new ArgumentNullException("type");
            }

            return _annotations.OfType(type).FirstOrDefault();
        }

        public IEnumerable<T> Annotations<T>() where T : class {
            return _annotations.OfType<T>();
        }

        public IEnumerable<object> Annotations(Type type) {
            return _annotations.OfType(type);
        }

        public void RemoveAnnotations<T>() where T : class {
            _annotations = _annotations.RemoveOfType(typeof(T));
        }

        public void RemoveAnnotations(Type type) {
            if (type == null) {
                throw new ArgumentNullException("type");
            }

            _annotations = _annotations.RemoveOfType(type);
        }

        public void RemoveAnnotation(object value) {
            if (value == null) {
                throw new ArgumentNullException("value");
            }

            _annotations = _annotations.Remove(value);
        }
    }

}
