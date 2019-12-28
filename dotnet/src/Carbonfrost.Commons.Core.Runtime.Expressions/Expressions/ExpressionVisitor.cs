//
// Copyright 2006, 2008, 2010 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public partial class ExpressionVisitor : IExpressionVisitor {

        protected ExpressionVisitor() {}

        public void Visit(Expression expression) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }

            expression.AcceptVisitor(this);
        }

        protected virtual void VisitBinaryExpression(BinaryExpression expression) {
            DefaultVisit(expression);
        }

        protected virtual void VisitUnaryExpression(UnaryExpression expression) {
            DefaultVisit(expression);
        }

        protected virtual void DefaultVisit(Expression expression) {}

        void IExpressionVisitor.Visit(BinaryExpression expression) {
            VisitBinaryExpression(expression);
        }

        void IExpressionVisitor.Visit(UnaryExpression expression) {
            VisitUnaryExpression(expression);
        }
    }
}
