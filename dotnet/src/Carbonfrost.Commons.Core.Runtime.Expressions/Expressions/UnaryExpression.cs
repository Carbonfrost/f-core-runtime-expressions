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


using System;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    public class UnaryExpression : Expression {

        private readonly ExpressionType expressionType;
        private readonly Expression expression;

        public Expression Expression {
            get { return expression; }
        }

        internal UnaryExpression(ExpressionType expressionType,
                                 Expression expression) {
            this.expressionType = expressionType;
            this.expression = expression;
        }

        public UnaryExpression Update(Expression expression = null) {
            if (expression == null || expression == this.Expression)
                return this;
            else
                return Expression.MakeUnary(this.ExpressionType, expression);
        }

        public override object Evaluate(IExpressionContext context) {
            var op = UnaryOperator.FindOperator(ExpressionType);
            return op.Evaluate(Expression.Evaluate(context));
        }

        public override ExpressionType ExpressionType {
            get {
                return this.expressionType;;
            }
        }

        protected internal override TResult AcceptVisitor<TArgument, TResult>(IExpressionVisitor<TArgument, TResult> visitor, TArgument argument) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this, argument);
        }

        protected internal override void AcceptVisitor(IExpressionVisitor visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            visitor.Visit(this);
        }

        protected internal override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this);
        }
    }
}
