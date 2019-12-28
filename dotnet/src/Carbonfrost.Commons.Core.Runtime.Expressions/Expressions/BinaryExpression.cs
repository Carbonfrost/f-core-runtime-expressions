//
// Copyright 2006, 2008, 2010, 2015 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public class BinaryExpression : Expression {

        private readonly Expression left;
        private readonly Expression right;
        private readonly ExpressionType expressionType;

        public Expression Right {
            get { return right; }
        }

        public Expression Left {
            get { return left; }
        }

        internal bool LeftNeedsParens {
            get {
                return Left is BinaryExpression
                    && (Expression.Precedence(Left.ExpressionType) < Expression.Precedence(ExpressionType));
            }
        }

        internal bool RightNeedsParens {
            get {
                return Right is BinaryExpression
                    && (Expression.Precedence(Right.ExpressionType) <= Expression.Precedence(ExpressionType));
            }
        }

        public bool IsChecked {
            get { return ExpressionType.ToString().Contains("Checked"); }
        }

        public bool IsAssignment {
            get { return ExpressionType.ToString().Contains("Assign"); }
        }

        internal BinaryExpression(ExpressionType expressionType,
                                  Expression left,
                                  Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");

            if (right == null)
                throw new ArgumentNullException("right");

            this.left = left;
            this.right = right;
            this.expressionType = expressionType;
        }

        public BinaryExpression Update(Expression left, Expression right) {
            if (left == null && right == null)
                return this;

            return new BinaryExpression(this.ExpressionType, left ?? Left, right ?? Right);
        }

        protected internal override void AcceptVisitor(IExpressionVisitor visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            visitor.Visit(this);
        }

        protected internal override TResult AcceptVisitor<TArgument, TResult>(IExpressionVisitor<TArgument, TResult> visitor, TArgument argument) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this, argument);
        }

        protected internal override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this);
        }

        public override object Evaluate(IExpressionContext context) {
            var op = BinaryOperator.FindOperator(ExpressionType);
            return op.Evaluate(EvaluateDereference(Left, context), EvaluateDereference(Right, context));
        }

        public override ExpressionType ExpressionType {
            get {
                return expressionType;
            }
        }
    }
}
