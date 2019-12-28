//
// Copyright 2014, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public partial class NewArrayExpression : Expression {

        private readonly IReadOnlyCollection<Expression> _expressions;

        public IReadOnlyCollection<Expression> Expressions {
            get {
                return _expressions;
            }
        }

        internal NewArrayExpression(IEnumerable<Expression> expressions) {
            this._expressions = expressions.ToArray();
        }

        public override object Evaluate(IExpressionContext context) {
            return new SZArray(Expressions.Select(t => t.Evaluate(context)));
        }

        public NewArrayExpression Update(IEnumerable<Expression> arguments) {
            if (arguments == null)
                return this;
            else
                return Expression.NewArray(arguments);
        }

        // We have a ToString() on this "array"
        class SZArray : List<object> {

            public SZArray(IEnumerable<object> collection) : base(collection) {
            }

            public override string ToString() {
                return string.Concat(this);
            }
        }
    }
}
