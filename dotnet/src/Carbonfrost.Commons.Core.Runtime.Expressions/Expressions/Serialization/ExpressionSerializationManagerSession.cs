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
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Core.Runtime.Expressions.Serialization {

    class ExpressionSerializationManagerSession : IExpressionSerializerContext, IDisposable {

        private readonly Dictionary<Type, int> _variables = new Dictionary<Type, int>();
        private readonly ObjectIDGenerator _seen = new ObjectIDGenerator();

        private readonly Dictionary<long, string> _variableNames = new Dictionary<long, string>();

        public bool IsDisposed {
            get;
            private set;
        }


        public string GetVariable(object value) {
            bool firstTime;
            long id = _seen.HasId(value, out firstTime);

            if (id == 0) {
                return null;
            }
            return _variableNames.GetValueOrDefault(id);
        }

        public string DefineVariable(object value) {
            bool firstTime;
            long id = _seen.GetId(value, out firstTime);

            if (firstTime) {
                Type variableType = value.GetType();
                string baseName = GetBaseName(variableType);
                int existing = _variables.GetValueOrDefault(variableType);
                _variables[variableType] = ++existing;
                _variableNames[id] = baseName + existing;
            }

            return _variableNames[id];
        }

        static string GetBaseName(Type variableType) {
            string result = Regex.Replace(variableType.Name, @"[\+`/]", "");

            if (variableType.IsArray) {
                result = variableType.GetElementType().Name + "Array";
            }

            return result.ToLowerInvariant();
        }

        public object Instance {
            get;
            private set;
        }

        public PropertyInfo PropertyInfo {
            get;
            private set;
        }

        public ExpressionSerializationManagerSession Parent {
            get;
            private set;
        }

        public ExpressionSerializationManagerSession() {
        }

        public ExpressionSerializationManagerSession(object instance, PropertyInfo prop) {
            Instance = instance;
            PropertyInfo = prop;
        }

        public object GetService(Type serviceType) {
            if (serviceType.IsInstanceOfType(this)) {
                return this;
            }

            return null;
        }

        public void Dispose() {
            IsDisposed = true;
        }
    }
}
