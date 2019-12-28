//
// Copyright 2012 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.IO;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    public class ExpressionPrinter : ExpressionVisitor {

        private readonly TextWriter output;

        public TextWriter Output {
            get { return output; }
        }

        public ExpressionPrinter(TextWriter output) {
            if (output == null)
                throw new ArgumentNullException("output");

            this.output = output;
        }

        protected override void VisitBinaryExpression(BinaryExpression expression) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }

            WriteWithParens(expression.Left, expression.LeftNeedsParens);
            output.Write(' ');
            output.Write(expression.ExpressionType.GetOperatorString());
            output.Write(' ');
            WriteWithParens(expression.Right, expression.RightNeedsParens);
        }

        protected override void VisitBlockExpression(BlockExpression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            bool needComma = false;
            output.Write("{ ");
            foreach (var s in expression.Expressions) {
                if (needComma)
                    output.Write(", ");

                Visit(s);
                needComma = true;
            }
            output.Write(" }");
        }

        protected override void VisitUnaryExpression(UnaryExpression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            bool post = expression.ExpressionType.ToString().StartsWith("Post", StringComparison.Ordinal);
            if (!post)
                output.Write(expression.ExpressionType.GetOperatorString());

            Visit(expression.Expression);

            if (post)
                output.Write(expression.ExpressionType.GetOperatorString());

        }

        protected override void VisitNewArrayExpression(NewArrayExpression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            bool needComma = false;
            output.Write("[ ");
            foreach (var s in expression.Expressions) {
                if (needComma)
                    output.Write(", ");

                Visit(s);
                needComma = true;
            }
            output.Write(" ]");
        }

        protected override void VisitNewObjectExpression(NewObjectExpression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            output.Write("new ");
            Visit(expression.Expression);
            VisitArguments(expression.Arguments);
        }

        protected override void VisitConstantExpression(ConstantExpression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            WriteConstant(expression.Value);
        }

        protected override void VisitValueExpression(ValueExpression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            // TODO Could be more complex
            WriteConstant(expression.Value);
        }

        protected override void VisitNameExpression(NameExpression expression) {
            output.Write(expression.Name);
        }

        protected override void VisitMemberAccessExpression(MemberAccessExpression expression) {
            Visit(expression.Expression);
            output.Write('.');
            output.Write(expression.Name);
        }

        protected override void VisitCallExpression(CallExpression expression) {
            Visit(expression.Expression);
            VisitArguments(expression.Arguments);
        }

        void VisitArguments(IEnumerable<Expression> arguments) {
            output.Write('(');

            bool comma = false;
            foreach (var arg in arguments) {
                if (comma) {
                    output.Write(", ");
                }

                Visit(arg);
                comma = true;
            }

            output.Write(')');
        }

        void WriteConstant(object value) {
            output.Write(GetConstant(value));
        }

        string GetConstant(object value) {
            if (ReferenceEquals(value, null))
                return "null";

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
                output.Write("(");
            }
            Visit(e);
            if (needParens) {
                output.Write(")");
            }
        }

        static string FormatChar(char c) {
            if (Scanner.IsPrintable(c))
                return string.Format("'{0}'", c);
            else
                return string.Format("'\\u{0:x4}'", (int) c);
        }
    }

}
