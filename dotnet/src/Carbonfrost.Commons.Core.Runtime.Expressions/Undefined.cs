//
// Copyright 2014, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Globalization;

namespace Carbonfrost.Commons.Core.Runtime {

    public sealed class Undefined : IConvertible {

        public static readonly object Value = new Undefined();

        private Undefined() {}

        TypeCode IConvertible.GetTypeCode() {
            return TypeCode.Object;
        }

        public static implicit operator bool(Undefined value) {
            return ((IConvertible) value).ToBoolean(CultureInfo.CurrentCulture);
        }

        public static implicit operator float(Undefined value) {
            return ((IConvertible) value).ToSingle(CultureInfo.CurrentCulture);
        }

        public static implicit operator double(Undefined value) {
            return ((IConvertible) value).ToDouble(CultureInfo.CurrentCulture);
        }

        public static implicit operator string(Undefined value) {
            return ((IConvertible) value).ToString(CultureInfo.CurrentCulture);
        }

        public static bool operator true(Undefined value) {
            return false;
        }

        public static bool operator false(Undefined value) {
            return true;
        }

        public override string ToString() {
            return "undefined";
        }

        bool IConvertible.ToBoolean(IFormatProvider provider) {
            return false;
        }

        char IConvertible.ToChar(IFormatProvider provider) {
            throw CoreRuntimeExpressionsFailure.CannotCastUndefined(typeof(char));
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider) {
            throw CoreRuntimeExpressionsFailure.CannotCastUndefined(typeof(sbyte));
        }

        byte IConvertible.ToByte(IFormatProvider provider) {
            throw CoreRuntimeExpressionsFailure.CannotCastUndefined(typeof(byte));
        }

        short IConvertible.ToInt16(IFormatProvider provider) {
            throw CoreRuntimeExpressionsFailure.CannotCastUndefined(typeof(short));
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider) {
            throw CoreRuntimeExpressionsFailure.CannotCastUndefined(typeof(ushort));
        }

        int IConvertible.ToInt32(IFormatProvider provider) {
            throw CoreRuntimeExpressionsFailure.CannotCastUndefined(typeof(int));
        }

        uint IConvertible.ToUInt32(IFormatProvider provider) {
            throw CoreRuntimeExpressionsFailure.CannotCastUndefined(typeof(uint));
        }

        long IConvertible.ToInt64(IFormatProvider provider) {
            throw CoreRuntimeExpressionsFailure.CannotCastUndefined(typeof(long));
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider) {
            throw CoreRuntimeExpressionsFailure.CannotCastUndefined(typeof(UInt64));
        }

        float IConvertible.ToSingle(IFormatProvider provider) {
            return float.NaN;
        }

        double IConvertible.ToDouble(IFormatProvider provider) {
            return double.NaN;
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider) {
            throw CoreRuntimeExpressionsFailure.CannotCastUndefined(typeof(decimal));
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) {
            throw CoreRuntimeExpressionsFailure.CannotCastUndefined(typeof(DateTime));
        }

        string IConvertible.ToString(IFormatProvider provider) {
            return ToString();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) {
            var c = (IConvertible) this;

            if (typeof(Boolean) == conversionType) {
                return c.ToBoolean(null);
            }
            if (typeof(Byte) == conversionType) {
                return c.ToByte(null);
            }
            if (typeof(Char) == conversionType) {
                return c.ToChar(null);
            }
            if (typeof(DateTime) == conversionType) {
                return c.ToDateTime(null);
            }
            if (typeof(Decimal) == conversionType) {
                return c.ToDecimal(null);
            }
            if (typeof(Double) == conversionType) {
                return c.ToDouble(null);
            }
            if (typeof(Single) == conversionType) {
                return c.ToSingle(null);
            }
            if (typeof(Int32) == conversionType) {
                return c.ToInt32(null);
            }
            if (typeof(Int64) == conversionType) {
                return c.ToInt64(null);
            }
            if (typeof(SByte) == conversionType) {
                return c.ToSByte(null);
            }
            if (typeof(Int16) == conversionType) {
                return c.ToInt16(null);
            }
            if (typeof(String) == conversionType) {
                return c.ToString(null);
            }
            if (typeof(UInt32) == conversionType) {
                return c.ToUInt32(null);
            }
            if (typeof(UInt64) == conversionType) {
                return c.ToUInt64(null);
            }
            if (typeof(UInt16) == conversionType) {
                return c.ToUInt16(null);
            }

            throw CoreRuntimeExpressionsFailure.CannotCastUndefined(conversionType);
        }

    }
}
