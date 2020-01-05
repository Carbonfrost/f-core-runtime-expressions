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

    public partial class NewObjectExpression : Expression {

        private readonly Expression _expression;
        private readonly IReadOnlyCollection<Expression> _arguments;

        public Expression Expression {
            get {
                return _expression;
            }
        }

        public IReadOnlyCollection<Expression> Arguments {
            get {
                return _arguments;
            }
        }

        internal NewObjectExpression(Expression expression, IEnumerable<Expression> arguments) {
            this._expression = expression;
            this._arguments = arguments.ToArray();
        }

        public NewObjectExpression Update(Expression expression, IEnumerable<Expression> arguments) {
            if (object.ReferenceEquals(null, expression) && arguments == null) {
                return this;
            }

            return Expression.NewObject(expression, arguments);
        }
    }
}
