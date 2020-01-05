//
// Copyright 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public partial class CallExpression : Expression {

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


        public CallExpression(Expression expression,
                              IEnumerable<Expression> arguments) {
            this._expression = expression;
            this._arguments = arguments.ToArray();
        }

        public CallExpression Update(Expression expression, IEnumerable<Expression> arguments) {
            if (object.ReferenceEquals(null, expression) && arguments == null)
                return this;
            else
                return Expression.Call(expression, arguments);
        }

        public override object Evaluate(IExpressionContext context) {
            var lambda = Expression as LambdaExpression;
            if (lambda != null) {
                return lambda.DoCall(Arguments.Select(t => t.Evaluate(context)), context);
            }
            return base.Evaluate(context);
        }

    }
}
