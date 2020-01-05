//
// Copyright 2006, 2008, 2010, 2012 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    public partial class ConstantExpression : Expression {

        private readonly object value;
        private readonly Type type;

        public new object Value {
            get { return value; }
        }

        internal ConstantExpression(object value, Type type) {
            this.value = value;
            this.type = type;
        }

        public ConstantExpression Update(object value) {
            if (object.ReferenceEquals(null, value))
                return this;
            else
                return Expression.Constant(value);
        }

        public override object Evaluate(IExpressionContext context) {
            return this.Value;
        }
    }
}
