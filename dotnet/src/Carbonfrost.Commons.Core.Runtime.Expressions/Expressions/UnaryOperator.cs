//
// Copyright 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    partial class UnaryOperator {

        private static readonly IDictionary<ExpressionType, IUnaryOperator> _operatorLookup
            = new Dictionary<ExpressionType, IUnaryOperator> {
            { ExpressionType.Negate,new NegateOperator() },
        };

        internal static IUnaryOperator FindOperator(ExpressionType type) {
            return _operatorLookup.GetValueOrDefault(type);
        }

        private static bool IsFalsy(object x) {
            if (x is Undefined) {
                return true;
            }
            if (x == null) {
                return true;
            }

            switch (Type.GetTypeCode(x.GetType())) {
                case TypeCode.Empty:
                case TypeCode.Object:
                case TypeCode.DBNull:
                case TypeCode.Boolean:
                    return !(bool) x;

                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                    return Convert.ToInt32(x) == 0;

                case TypeCode.Char:
                    return (char) x == '\0';

                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return Convert.ToInt64(x) == 0;

                case TypeCode.Single:
                    return double.IsNaN((float) x) || Math.Abs((float)x) < float.Epsilon;
                case TypeCode.Double:
                    return double.IsNaN((double) x) || Math.Abs((double)x) < double.Epsilon;
                case TypeCode.Decimal:
                    return ((decimal) x) == 0;
                case TypeCode.DateTime:
                    break;
                case TypeCode.String:
                    return ((string) x) == string.Empty;
            }
            return false;
        }

    }
}
