//
// Copyright 2014 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public partial interface IExpressionVisitor {
        void Visit(BinaryExpression expression);
        void Visit(UnaryExpression expression);
    }

    public partial interface IExpressionVisitor<TArgument, TResult> {
        TResult Visit(BinaryExpression expression, TArgument argument);
        TResult Visit(UnaryExpression expression, TArgument argument);
    }

    public partial interface IExpressionVisitor<TResult> {
        TResult Visit(BinaryExpression expression);
        TResult Visit(UnaryExpression expression);
    }
}
