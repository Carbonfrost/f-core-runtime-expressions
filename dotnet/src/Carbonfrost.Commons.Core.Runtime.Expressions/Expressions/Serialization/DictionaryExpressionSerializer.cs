//
// Copyright 2015 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Core.Runtime.Expressions.Serialization {

    sealed class DictionaryExpressionSerializer : CompositeExpressionSerializer {

        protected override void EvalContent(object item,
                                            IExpressionSerializerContext context,
                                            Expression qual,
                                            IList<Expression> results) {

            var myItems = item as System.Collections.IEnumerable;
            if (myItems == null)
                throw new NotImplementedException();

            var items = myItems.Cast<object>();
            if (!items.Any()) {
                return;
            }

            var kvpType = items.First().GetType();
            var keyGetter = kvpType.GetTypeInfo().GetProperty("Key");
            var valueGetter = kvpType.GetTypeInfo().GetProperty("Value");

            var myResults = new List<Expression>();

            // TODO Estimate size of results list (performance)
            foreach (object component in items) {
                var key = keyGetter.GetValue(component, null);
                var value = valueGetter.GetValue(component, null);

                Expression pks = ExpressionSerializer.SerializeOrReference(key, context);
                Expression pvs = ExpressionSerializer.SerializeOrReference(value, context);

                var qualAdd = Expression.MemberAccess(qual, "Add");
                results.Add(Expression.Call(qualAdd,
                                            new []
                                            {
                                                pks,
                                                pvs
                                            }));
            }
        }
    }
}
