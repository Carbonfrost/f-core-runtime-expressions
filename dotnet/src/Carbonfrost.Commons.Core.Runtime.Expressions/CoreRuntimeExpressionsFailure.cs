//
// Copyright 2019 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Core.Runtime.Expressions.Resources;

namespace Carbonfrost.Commons.Core.Runtime {

    static class CoreRuntimeExpressionsFailure {

        public static ArgumentException NotBinaryExpressionType(string argumentName, ExpressionType binaryType) {
            return Failure.Prepare(new ArgumentException(SR.NotBinaryExpressionType(binaryType), argumentName));
        }

        public static InvalidOperationException RequireSerializationManagerSession() {
            return Failure.Prepare(new InvalidOperationException(SR.RequireSerializationManagerSession()));
        }

        public static InvalidCastException CannotCastUndefined(Type type) {
            return Failure.Prepare(new InvalidCastException());
        }

        public static Exception ReferenceError(string name) {
            return Failure.Prepare(new Exception(SR.ReferenceError(name)));
        }

        public static InvalidOperationException CannotReduceExpression() {
            return Failure.Prepare(new InvalidOperationException(SR.CannotReduceExpression()));
        }

        public static InvalidOperationException CannotEvaluateNonConstantExpression() {
            return Failure.Prepare(new InvalidOperationException(SR.CannotEvaluateNonConstantExpression()));
        }

        public static ArgumentException RequiresStaticFieldOrProperty() {
            return Failure.Prepare(new ArgumentException(SR.RequiresStaticFieldOrProperty()));
        }

        public static ArgumentException ExpressionContextInvalidIndexCount(string argName) {
            return Failure.Prepare(new ArgumentException(SR.ExpressionContextInvalidIndexCount(), argName));
        }
    }
}
