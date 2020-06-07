//
// Copyright 2015 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    partial class BinaryOperator {

        class AddOperator : IBinaryOperator {

            public object Evaluate(object aObj, object bObj) {
                if (aObj is string || bObj is string) {
                    return string.Concat(aObj, bObj);
                }

                    if (aObj is decimal || bObj is decimal) {
                    var a = ParseDecimal(aObj);
                    var b = ParseDecimal(bObj);
                    return decimal.Add(a, b);
                }

                if (aObj is double || bObj is double || aObj is Undefined || bObj is Undefined) {
                    double a = ParseDouble(aObj);
                    double b = ParseDouble(bObj);
                    return a + b;
                }

                else {
                    long a = ParseInt64(aObj);
                    long b = ParseInt64(bObj);
                    return a + b;
                }
            }
        }
    }
}
