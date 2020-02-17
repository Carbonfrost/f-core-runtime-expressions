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

using System.Collections.Generic;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    partial class ExpressionContext {

        public static readonly IExpressionContext Null = new NullExpressionContext();
        public static readonly DynamicExpressionContext Empty = ReadOnly(Null);

        public static DynamicExpressionContext ReadOnly(IExpressionContext context) {
            if (context == null) {
                return Empty;
            }
            return new ReadOnlyExpressionContext(context);
        }

        public static IExpressionContext Compose(params IExpressionContext[] items) {
            return Compose((IEnumerable<IExpressionContext>) items);
        }

        public static IExpressionContext Compose(IEnumerable<IExpressionContext> items) {
            return Utility.OptimalComposite(
                items,
                i => new CompositeExpressionContext(i),
                Empty
            );
        }

        public static DynamicExpressionContext FromNameScope(INameScope nameScope) {
            return new NameScopeExpressionContext(nameScope);
        }
    }
}
