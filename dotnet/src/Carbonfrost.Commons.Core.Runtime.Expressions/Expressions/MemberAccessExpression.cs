//
// Copyright 2013 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    public partial class MemberAccessExpression : Expression {

        private readonly Expression expression;
        private readonly string name;

        public Expression Expression {
            get {
                return expression;
            }
        }

        public new string Name {
            get {
                return name;
            }
        }

        internal MemberAccessExpression(Expression expression, string name) {
            this.expression = expression;
            this.name = name;
        }

        public MemberAccessExpression Update(Expression expression = null, string name = null) {
            if ((expression == null || expression == this.Expression) && (string.IsNullOrEmpty(name) || name == this.Name))
                return this;
            else
                return new MemberAccessExpression(expression ?? Expression, name ?? Name);
        }

        public override object Evaluate(IExpressionContext context) {
            var left = EvaluateDereference(Expression, context);
            var leftType = left.GetType();
            var pi = leftType.GetProperty(Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (pi == null) {
                return Core.Runtime.Undefined.Value;
            }
            return pi.GetValue(left);
        }
    }
}
