//
// Copyright 2012, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Collections.Generic;
using System.IO;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    public class ExpressionPrinter : ExpressionVisitor {

        private readonly TextWriter _output;

        public TextWriter Output {
            get {
                return _output;
            }
        }

        public ExpressionPrinter(TextWriter output) {
            if (output == null) {
                throw new ArgumentNullException(nameof(output));
            }

            _output = output;
        }

        protected override void VisitBinaryExpression(BinaryExpression expression) {
            if (expression == null) {
                throw new ArgumentNullException(nameof(expression));
            }

            WriteWithParens(expression.Left, expression.LeftNeedsParens);
            Output.Write(' ');
            Output.Write(expression.ExpressionType.GetOperatorString());
            Output.Write(' ');
            WriteWithParens(expression.Right, expression.RightNeedsParens);
        }

        protected override void VisitBlockExpression(BlockExpression expression) {
            if (expression == null) {
                throw new ArgumentNullException(nameof(expression));
            }

            bool needComma = false;
            Output.Write("{ ");
            foreach (var s in expression.Expressions) {
                if (needComma) {
                    Output.Write(", ");
                }

                Visit(s);
                needComma = true;
            }
            Output.Write(" }");
        }

        protected override void VisitUnaryExpression(UnaryExpression expression) {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            bool post = expression.ExpressionType.ToString().StartsWith("Post", StringComparison.Ordinal);
            if (!post) {
                Output.Write(expression.ExpressionType.GetOperatorString());
            }

            Visit(expression.Expression);

            if (post) {
                Output.Write(expression.ExpressionType.GetOperatorString());
            }

        }

        protected override void VisitNewArrayExpression(NewArrayExpression expression) {
            if (expression == null) {
                throw new ArgumentNullException(nameof(expression));
            }

            bool needComma = false;
            Output.Write("[ ");
            foreach (var s in expression.Expressions) {
                if (needComma) {
                    Output.Write(", ");
                }

                Visit(s);
                needComma = true;
            }
            Output.Write(" ]");
        }

        protected override void VisitNewObjectExpression(NewObjectExpression expression) {
            if (expression == null) {
                throw new ArgumentNullException(nameof(expression));
            }

            Output.Write("new ");
            Visit(expression.Expression);
            VisitArguments(expression.Arguments);
        }

        protected override void VisitConstantExpression(ConstantExpression expression) {
            if (expression == null) {
                throw new ArgumentNullException(nameof(expression));
            }

            WriteConstant(expression.Value);
        }

        protected override void VisitValueExpression(ValueExpression expression) {
            if (expression == null) {
                throw new ArgumentNullException(nameof(expression));
            }

            // TODO Could be more complex
            WriteConstant(expression.Value);
        }

        protected override void VisitNameExpression(NameExpression expression) {
            Output.Write(expression.Name);
        }

        protected override void VisitMemberAccessExpression(MemberAccessExpression expression) {
            Visit(expression.Expression);
            Output.Write('.');
            Output.Write(expression.Name);
        }

        protected override void VisitCallExpression(CallExpression expression) {
            Visit(expression.Expression);
            VisitArguments(expression.Arguments);
        }

        protected override void VisitInterpolatedStringExpression(InterpolatedStringExpression expression) {
            Output.Write("'");
            foreach (var s in expression.Elements) {
                if (s is InterpolatedStringTextContent tc) {
                    Output.Write(tc.Text);
                } else {
                    Output.Write("${");
                    Visit(s.ToExpression());
                    Output.Write("}");
                }
            }
            Output.Write("'");
        }

        void VisitArguments(IEnumerable<Expression> arguments) {
            Output.Write('(');

            bool comma = false;
            foreach (var arg in arguments) {
                if (comma) {
                    Output.Write(", ");
                }

                Visit(arg);
                comma = true;
            }

            Output.Write(')');
        }

        void WriteConstant(object value) {
            Output.Write(GetConstant(value));
        }

        string GetConstant(object value) {
            if (ReferenceEquals(value, null)) {
                return "null";
            }

            switch (Type.GetTypeCode(value.GetType())) {

                case TypeCode.Boolean:
                    return ((bool) value) ? "true" : "false";

                case TypeCode.Char:
                    return FormatChar((char) value);

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
                    return Convert.ToString(value);

                case TypeCode.String:
                    return string.Format("'{0}'", Scanner.Escape((string) value));

                case TypeCode.Empty:
                    break;
                case TypeCode.Object:
                    if (value is Undefined) {
                        return "undefined";
                    }

                    break;

                case TypeCode.DBNull:
                    break;

                case TypeCode.DateTime:
                default:
                    break;
            }

            throw new NotImplementedException();
        }

        void WriteWithParens(Expression e, bool needParens) {
            if (needParens) {
                Output.Write("(");
            }
            Visit(e);
            if (needParens) {
                Output.Write(")");
            }
        }

        static string FormatChar(char c) {
            if (Scanner.IsPrintable(c)) {
                return string.Format("'{0}'", c);
            }
            return string.Format("'\\u{0:x4}'", (int) c);
        }
    }

}
