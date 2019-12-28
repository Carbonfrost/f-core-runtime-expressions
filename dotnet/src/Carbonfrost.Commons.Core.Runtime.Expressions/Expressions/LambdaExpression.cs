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

    public partial class LambdaExpression : Expression {

        private readonly IReadOnlyList<LambdaParameter> _parameters;

        public IReadOnlyList<LambdaParameter> Parameters {
            get {
                return _parameters;
            }
        }

        public Expression Body {
            get;
            private set;
        }

        public IExpressionContext Closure {
            get;
            private set;
        }

        internal LambdaExpression(Expression body, IExpressionContext closure, IEnumerable<LambdaParameter> parameters) {
            Body = body;
            Closure = closure;

            if (parameters == null) {
                _parameters = Array.Empty<LambdaParameter>();
            } else {
                _parameters = parameters.ToArray();
            }
        }

        public LambdaExpression Update(Expression body = null,
                                       ExpressionContext closure = null,
                                       IEnumerable<LambdaParameter> parameters = null) {
            if (body == null && closure == null && parameters == null) {
                return this;
            }

            return new LambdaExpression(body ?? Body, closure ?? Closure, parameters ?? Parameters);
        }

        internal object DoCall(IEnumerable<object> arguments, IExpressionContext context) {
            var myArguments = new ExpressionContext();
            int index = 0;
            foreach (var arg in arguments) {
                myArguments.Data[Parameters[index++].Name] = arg;
            }

            var combined = ExpressionContext.Combine(myArguments, context, Closure);
            return Body.Evaluate(combined);
        }
    }
}
