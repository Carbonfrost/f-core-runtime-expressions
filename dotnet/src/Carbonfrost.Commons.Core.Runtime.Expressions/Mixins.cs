//
// Copyright 2010 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.IO;
using System.Linq;
using System.Reflection;

namespace Carbonfrost.Commons.Core.Runtime {

    static class Mixin {

        internal static void AddMany<T>(this ICollection<T> self, IEnumerable<T> items) {
            foreach (var o in items) {
                self.Add(o);
            }
        }

        internal static TValue GetValueOrCache<TKey, TValue>(this IDictionary<TKey, TValue> source,
                                                             TKey key,
                                                             Func<TValue> cacheFunction = null)
        {
            TValue v;
            if (!source.TryGetValue(key, out v))
            {
                source.Add(key, v = cacheFunction());
            }
            return v;
        }

        internal static TValue GetValueOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> source,
            TKey key,
            TValue defaultValue = default(TValue)) {

            TValue e;
            if (source.TryGetValue(key, out e))
            {
                return e;
            }
            return defaultValue;
        }
    }
}
