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
using System;
using System.Collections.Generic;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    partial class BinaryOperator {

        private static readonly IDictionary<ExpressionType, IBinaryOperator> _operatorLookup
            = new Dictionary<ExpressionType, IBinaryOperator> {

            { ExpressionType.Add, new AddOperator() },
        };

        internal static IBinaryOperator FindOperator(ExpressionType type) {
            return _operatorLookup.GetValueOrDefault(type);
        }

        protected static decimal ParseDecimal(object c) {
            if (c is decimal) {
                return (decimal) c;
            }
            return Convert.ToDecimal(c);
        }

        protected static bool IsDoubleText(object c) {
            if (c == null) {
                return false;
            }
            if (c is string) {
                double dummy;
                return double.TryParse(c.ToString(), out dummy);
            }

            switch (Type.GetTypeCode(c.GetType())) {
                case TypeCode.Empty:
                case TypeCode.Object:
                case TypeCode.DBNull:
                    return false;

                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;

                case TypeCode.DateTime:
                case TypeCode.String:
                    return false;
            }

            return false;
        }

        protected static double ParseDouble(object c) {
            if (c is double) {
                return (double) c;
            }
            if (c is Undefined) {
                return double.NaN;
            }
            return Convert.ToDouble(c);
        }

        protected static long ParseInt64(object c) {
            if (c is long) {
                return (long) c;
            }
            return Convert.ToInt64(c);
        }
    }
}
