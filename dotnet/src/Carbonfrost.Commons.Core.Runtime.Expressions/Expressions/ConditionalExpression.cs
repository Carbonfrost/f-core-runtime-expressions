//
// Copyright 2012 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    public partial class ConditionalExpression : Expression {

        private readonly Expression whenTrue;
        private readonly Expression whenFalse;

        public Expression Test { get; private set; }

        public Expression WhenTrue {
            get {
                return whenTrue;
            }
        }

        public Expression WhenFalse {
            get {
                return whenFalse;
            }
        }

        internal ConditionalExpression(Expression test, Expression whenTrue, Expression whenFalse) {
            this.Test = test;
            this.whenTrue = whenTrue;
            this.whenFalse = whenFalse;
        }

        public ConditionalExpression Update(Expression test, Expression whenTrue, Expression whenFalse) {
            if (test == null && whenTrue == null && whenFalse == null)
                return this;
            else
                return Expression.Conditional(test ?? Test, whenTrue ?? WhenTrue, whenFalse ?? WhenFalse);
        }
    }
}
