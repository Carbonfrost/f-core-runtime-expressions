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

    public static class ExpressionParser {

        public static readonly IExpressionParser Null = new NullImpl();
        public static readonly IExpressionParser Default = new DefaultExpressionParser();

        public static Expression Parse(this IExpressionParser parser,
                                       string text) {
            if (parser == null) {
                throw new ArgumentNullException("parser");
            }

            return parser.Parse(text, null);
        }

        private class NullImpl : IExpressionParser {

            Expression IExpressionParser.Parse(string expression, ExpressionContext context) {
                throw new NotImplementedException();
            }
        }
    }
}
