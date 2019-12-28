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
using System.Collections.Generic;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    public partial class ExpressionVisitor<TResult> : IExpressionVisitor<TResult> {

        protected ExpressionVisitor() {}

        public TResult Visit(Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return expression.AcceptVisitor<TResult>(this);
        }

        public IEnumerable<TResult> Visit(IEnumerable<Expression> expressions) {
            if (expressions == null)
                throw new ArgumentNullException("expressions");

            foreach (var expression in expressions)
                yield return expression.AcceptVisitor<TResult>(this);
        }

        protected virtual TResult VisitBinaryExpression(BinaryExpression expression) {
            return DefaultVisit(expression);
        }

        protected virtual TResult VisitUnaryExpression(UnaryExpression expression) {
            return DefaultVisit(expression);
        }

        protected virtual TResult DefaultVisit(Expression expression) {
            return default(TResult);
        }

        TResult IExpressionVisitor<TResult>.Visit(BinaryExpression expression) {
            return VisitBinaryExpression(expression);
        }

        TResult IExpressionVisitor<TResult>.Visit(UnaryExpression expression) {
            return VisitUnaryExpression(expression);
        }

    }
}
