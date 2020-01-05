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

    public partial class ExpressionVisitor<TArgument, TResult> : IExpressionVisitor<TArgument, TResult> {

        protected ExpressionVisitor() {}

        public TResult Visit(Expression expression, TArgument argument) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }

            return expression.AcceptVisitor<TArgument, TResult>(this, argument);
        }

        protected virtual TResult VisitBinaryExpression(BinaryExpression expression, TArgument argument) {
            return DefaultVisit(expression, argument);
        }

        protected virtual TResult VisitUnaryExpression(UnaryExpression expression, TArgument argument) {
            return DefaultVisit(expression, argument);
        }

        protected virtual TResult DefaultVisit(Expression expression, TArgument argument) {
            return default(TResult);
        }

        TResult IExpressionVisitor<TArgument, TResult>.Visit(BinaryExpression expression, TArgument argument) {
            return VisitBinaryExpression(expression, argument);
        }

        TResult IExpressionVisitor<TArgument, TResult>.Visit(UnaryExpression expression, TArgument argument) {
            return VisitUnaryExpression(expression, argument);
        }
    }
}
