//
// Copyright 2020-2021 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Linq;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    public sealed partial class InterpolatedStringExpression : Expression {

        private readonly InterpolatedStringContent[] _elements;

        public IReadOnlyList<InterpolatedStringContent> Elements {
            get {
                return _elements;
            }
        }

        internal InterpolatedStringExpression(InterpolatedStringContent[] elements) {
            _elements = elements;
        }

        public InterpolatedStringExpression Update(IEnumerable<InterpolatedStringContent> elements) {
            if (elements == null) {
                return this;
            }

            return Expression.InterpolatedString(elements.ToArray());
        }

        public override object Evaluate(IExpressionContext context) {
            return string.Concat(
                _elements.Select(e => e.ToExpression().Evaluate(context))
            );
        }
    }
}
