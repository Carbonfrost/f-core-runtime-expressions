//
// Copyright 2013 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Globalization;
using System.Linq;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    class DefaultExpressionParser : IExpressionParser {

        public bool Checked { get; set; }

        public Expression Parse(string expression, ExpressionContext context) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }
            if (string.IsNullOrEmpty(expression)) {
                throw Failure.EmptyString("expression");
            }

            Scanner s = new Scanner(expression);
            if (s.MoveNext()) {
                return ParseRoot(s);
            }

            throw new NotImplementedException();
        }

        private Expression ParseRoot(Scanner s) {
            return LHSExpression(s);
        }

        // exp_member -> exp_primary |  exp_function ;
        private Expression MemberExpression(Scanner s) {
            if (s.Type == TokenType.Function) {
                return FunctionCallExpression(s);
            }

            return ExpUnary(s);
        }

        Expression ExpUnary(Scanner s) {
            switch (s.Type) {
                case TokenType.Not:
                    s.RequireMoveNext();
                    return Expression.Not(ExpUnary(s));

                case TokenType.Minus:
                    s.RequireMoveNext();
                    return Expression.Negate(ExpUnary(s));
            }

            return PrimaryExpression(s);
        }

        private Expression FunctionCallExpression(Scanner s) {
            string funcName = s.TakeValue();
            NameExpression left = Expression.Name(funcName);
            return Expression.Call(left, ParseArgList(s).ToArray());
        }

        //  exp_lhs -> exp_member
        //      |  'new' exp_member
        //      |  exp_lhs arguments
        //      |  exp_lhs '[' expression ']'
        //      |  exp_lhs '.' Identifier ;

        // exp_lhs '.' Identifier  '.' Identifier
        private Expression LHSExpression(Scanner s) {
            // TODO New

            Expression left = MemberExpression(s);
            while (!s.EOF) {

                switch (s.Type) {
                    case TokenType.Dot:
                        s.RequireMoveNext();
                        left = MemberAccess(left, s);
                        break;

                    case TokenType.Plus:
                    case TokenType.Minus:
                    case TokenType.Asterisk:
                    case TokenType.Slash:
                    case TokenType.EqualTo:
                    case TokenType.NotEqualTo:
                    case TokenType.GreaterThan:
                    case TokenType.GreaterThanOrEqualTo:
                    case TokenType.LessThan:
                    case TokenType.LessThanOrEqualTo:
                    case TokenType.And:
                    case TokenType.Or:
                        left = ParseBinaryExpr(s, left);
                        break;

                    case TokenType.Comma:
                    case TokenType.RightBracket:
                    case TokenType.RightParen:
                    case TokenType.NaN:
                    case TokenType.Undefined:
                    case TokenType.Null:
                        return left;

                    case TokenType.Number:
                        // No ws such as 4-4 ==>  4 - 4.
                        if (s.Current.Value[0] == '-')
                            left = Expression.MakeBinary(
                                ExpressionType.Subtract,
                                left,
                                -Int32.Parse(s.TakeValue()));
                        else
                            throw new NotImplementedException();
                        break;

                    default:
                        throw new NotImplementedException(s.TakeValue());
                }
            }

            return left;
        }

        Expression MemberAccess(Expression left, Scanner s) {
            if (s.Type == TokenType.Identifier) {
                return Expression.MemberAccess(left, s.TakeValue());

            } else if (s.Type == TokenType.Function) {
                string funcName = s.TakeValue();
                left = Expression.MemberAccess(left, funcName);
                return Expression.Call(left, ParseArgList(s).ToArray());

            } else {
                throw new NotImplementedException("Not ident." + s.Type);
            }
        }

        // primary_expression -> IDENT | null | true | false | DECIMAL
        //              | HEXA | REAL | CHAR | STRING | '(' expression ')'
        private Expression PrimaryExpression(Scanner s) {
            switch (s.Type) {
                case TokenType.Identifier:
                    return new NameExpression(s.TakeValue());

                case TokenType.True:
                    s.MoveNext();
                    return Expression.True;

                case TokenType.False:
                    s.MoveNext();
                    return Expression.False;

                case TokenType.Undefined:
                    return Expression.Undefined;

                case TokenType.NaN:
                    return Expression.NaN;

                case TokenType.Null:
                    s.MoveNext();
                    return Expression.Null;

                case TokenType.Number:
                    return ConvertNumber(s.TakeValue());

                case TokenType.String:
                    return Expression.Constant(s.TakeValue());

                case TokenType.LeftParen:
                    s.MoveNext();
                    var it = LHSExpression(s);
                    s.MoveNext();
                    return it;
            }

            throw new NotImplementedException(s.Type.ToString());
        }

        // exp_function -> 'function' Identifier? '(' parameters ')' '{' function_body '}' ;

        private static Expression IsPrimaryExpr(Scanner s) {
            switch (s.Type) {
                case TokenType.Identifier:
                case TokenType.True:
                case TokenType.False:
                case TokenType.Null:
                case TokenType.Number:
                case TokenType.String:
                case TokenType.LeftParen:
                    return true;
            }
            return false;
        }

        private static Expression ConvertNumber(string str) {
            if (str.IndexOf('.') < 0) {
                if (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    return Expression.Constant(int.Parse(str.Substring(2), NumberStyles.HexNumber));

                return Expression.Constant(int.Parse(str));
            } else
                return Expression.Constant(decimal.Parse(str));
        }

        private ExpressionType MapExprType(TokenType type) {
            switch (type) {
                case TokenType.Plus:
                    return Checked ? ExpressionType.AddChecked : ExpressionType.Add;
                case TokenType.Minus:
                    return Checked ? ExpressionType.SubtractChecked : ExpressionType.Subtract;
                case TokenType.Asterisk:
                    return Checked ? ExpressionType.MultiplyChecked : ExpressionType.Multiply;
                case TokenType.Slash:
                    return ExpressionType.Divide;
                case TokenType.EqualTo:
                    return ExpressionType.EqualTo;
                case TokenType.NotEqualTo:
                    return ExpressionType.NotEqualTo;
                case TokenType.And:
                    return ExpressionType.AndAlso;
                case TokenType.Or:
                    return ExpressionType.OrElse;
                case TokenType.GreaterThan:
                    return ExpressionType.GreaterThan;
                case TokenType.GreaterThanOrEqualTo:
                    return ExpressionType.GreaterThanOrEqual;
                case TokenType.LessThan:
                    return ExpressionType.LessThan;
                case TokenType.LessThanOrEqualTo:
                    return ExpressionType.LessThanOrEqual;
                default:
                    return ExpressionType.Unknown;
            }
        }

        bool IsLowerPrecedence(TokenType self, TokenType other) {
            return Expression.IsLowerPrecedence(MapExprType(self), MapExprType(other));
        }

        Expression ParseBinaryExpr(Scanner s, Expression left) {
            var tokenType = s.Type;
            var type = MapExprType(tokenType);
            s.RequireMoveNext();
            var right = MemberExpression(s);

            if (s.EOF || MapExprType(s.Type) == ExpressionType.Unknown) {
                return Expression.MakeBinary(type, left, right);

            } else if (IsLowerPrecedence(tokenType, s.Type)) {
                return Expression.MakeBinary(type, left, right);

            } else {
                return Expression.MakeBinary(type, left, ParseBinaryExpr(s, right));
            }
        }

        private IEnumerable<Expression> ParseArgList(Scanner s, TokenType expect = TokenType.RightParen) {
            if (s.TakeValue() != "(") {
                throw new NotImplementedException();
            }

            IList<Expression> result = new List<Expression>();
            while (s.Type != expect) {
                result.Add(LHSExpression(s));

                if (s.Type == TokenType.Comma)
                    s.MoveNext();
                else if (s.Type == expect) {
                }
                else {
                    throw new NotImplementedException("pal: " + s.Type.ToString());
                }
            }

            if (s.Type == expect) {
                s.MoveNext();
            } else {
                throw new NotImplementedException(s.Type.ToString());
            }
            return result;
        }
    }
}
