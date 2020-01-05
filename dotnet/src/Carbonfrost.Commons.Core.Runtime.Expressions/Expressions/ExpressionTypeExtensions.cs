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

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    public static class ExpressionTypeExtensions {

        public static string GetOperatorString(this ExpressionType expressionType) {

            switch (expressionType) {
                case ExpressionType.Unknown:
                    return string.Empty;

                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return "+";

                case ExpressionType.Divide:
                    return "/";

                case ExpressionType.DivideAssign:
                    return "/=";

                case ExpressionType.AddAssign:
                case ExpressionType.AddAssignChecked:
                    return "+=";

                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return "*";

                case ExpressionType.MultiplyAssign:
                case ExpressionType.MultiplyAssignChecked:
                    return "*=";

                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return "-";

                case ExpressionType.SubtractAssign:
                case ExpressionType.SubtractAssignChecked:
                    return "-=";

                case ExpressionType.Power:
                    return "^^";

                case ExpressionType.PowerAssign:
                    return "^^=";

                case ExpressionType.RightShift:
                    return ">>";

                case ExpressionType.RightShiftAssign:
                    return ">>=";

                case ExpressionType.LeftShift:
                    return "<<";

                case ExpressionType.LeftShiftAssign:
                    return "<<=";

                case ExpressionType.Modulo:
                    return "%";

                case ExpressionType.ModuloAssign:
                    return "%=";

                case ExpressionType.And:
                    return "&&";

                case ExpressionType.AndAssign:
                    return "&&=";

                case ExpressionType.AndAlso:
                    return "&&";

                case ExpressionType.GreaterThan:
                    return ">";

                case ExpressionType.GreaterThanOrEqual:
                    return ">=";

                case ExpressionType.LessThan:
                    return "<";

                case ExpressionType.LessThanOrEqual:
                    return "<=";

                case ExpressionType.Or:
                    return "||";

                case ExpressionType.OrAssign:
                    return "||=";

                case ExpressionType.OrElse:
                    return "||";

                case ExpressionType.Coalesce:
                    return "??";

                case ExpressionType.EqualTo:
                    return "==";

                case ExpressionType.NotEqualTo:
                    return "!=";

                case ExpressionType.Assign:
                    return "=";

                case ExpressionType.ExclusiveOr:
                    return "^";

                case ExpressionType.ExclusiveOrAssign:
                    return "^=";

                case ExpressionType.PostIncrement:
                case ExpressionType.PreIncrement:
                    return "++";

                case ExpressionType.PostDecrement:
                case ExpressionType.PreDecrement:
                    return "--";

                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                    return "-";

                case ExpressionType.OnesComplement:
                    return "~";

                case ExpressionType.Not:
                    return "!";

                case ExpressionType.Conditional:
                    return "?:";

                case ExpressionType.MemberAccess:
                    return ".";

                default:
                    return string.Empty;
            }
        }

    }
}
