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

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    public class ExpressionRewriter : ExpressionVisitor<Expression> {

        public Expression Rewrite(Expression expression) {
            return Visit(expression);
        }

        protected override Expression DefaultVisit(Expression expression) {
            return expression;
        }

        protected override Expression VisitBinaryExpression(BinaryExpression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return expression.Update(Visit(expression.Left), Visit(expression.Right));
        }

        protected override Expression VisitBlockExpression(BlockExpression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return expression.Update(Visit(expression.Expressions));
        }

        protected override Expression VisitCallExpression(CallExpression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return expression.Update(Visit(expression.Expression), Visit(expression.Arguments));
        }

        protected override Expression VisitConditionalExpression(ConditionalExpression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return expression.Update(Visit(expression.Test),
                                     Visit(expression.WhenTrue),
                                     Visit(expression.WhenFalse));
        }

        protected override Expression VisitMemberAccessExpression(MemberAccessExpression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return expression.Update(Visit(expression.Expression));
        }

        protected override Expression VisitUnaryExpression(UnaryExpression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return expression.Update(Visit(expression.Expression));
        }

        protected override Expression VisitValueExpression(ValueExpression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return expression;
        }
    }
}
