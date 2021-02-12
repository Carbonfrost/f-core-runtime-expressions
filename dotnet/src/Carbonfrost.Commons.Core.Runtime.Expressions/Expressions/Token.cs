//
// Copyright 2013, 2016, 2020-2021 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Core.Runtime.Expressions.Resources;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    internal enum TokenType {
        Comma,
        Dot,
        LeftParen,
        RightParen,
        LeftBracket,
        RightBracket,
        LessThan,
        GreaterThan,
        LessThanOrEqualTo,
        GreaterThanOrEqualTo,
        And,
        BitAnd,
        BitOr,
        Or,
        EqualTo,
        NotEqualTo,
        Not,

        Plus,
        Minus,
        Slash,
        Asterisk,

        True,
        False,
        Null,
        Undefined,
        NaN,

        Property,
        String,
        Number,
        Identifier,

        Error,

        Function,
        EndOfInput,
    }

    internal class StringToken : Token {

        public readonly bool expandable;

        public bool Expandable { get { return expandable; } }

        internal StringToken(string tokenString, bool expandable)
            : base(TokenType.String, tokenString, expandable) {
            this.expandable = expandable;
        }
    }

    internal class ErrorToken : Token {

        public readonly string Message;
        public readonly int ErrorPosition;

        internal ErrorToken(int position, string message) : base(TokenType.Error, null, false) {
            Message = message;
            ErrorPosition = position;
        }
    }

    internal class Token {

        internal static readonly Token And = new Token(TokenType.And);
        internal static readonly Token BitAnd = new Token(TokenType.BitAnd);
        internal static readonly Token BitOr = new Token(TokenType.BitOr);
        internal static readonly Token Comma = new Token(TokenType.Comma);
        internal static readonly Token Dot = new Token(TokenType.Dot);
        internal static readonly Token EndOfInput = new Token(TokenType.EndOfInput);
        internal static readonly Token EqualTo = new Token(TokenType.EqualTo);
        internal static readonly Token False = new Token(TokenType.False);
        internal static readonly Token GreaterThan = new Token(TokenType.GreaterThan);
        internal static readonly Token GreaterThanOrEqualTo = new Token(TokenType.GreaterThanOrEqualTo);
        internal static readonly Token LeftBracket = new Token(TokenType.LeftBracket);
        internal static readonly Token LeftParen = new Token(TokenType.LeftParen);
        internal static readonly Token LessThan = new Token(TokenType.LessThan);
        internal static readonly Token LessThanOrEqualTo = new Token(TokenType.LessThanOrEqualTo);
        internal static readonly Token NaN = new Token(TokenType.NaN);
        internal static readonly Token Not = new Token(TokenType.Not);
        internal static readonly Token NotEqualTo = new Token(TokenType.NotEqualTo);
        internal static readonly Token Null = new Token(TokenType.Null);
        internal static readonly Token Or = new Token(TokenType.Or);
        internal static readonly Token RightBracket = new Token(TokenType.RightBracket);
        internal static readonly Token RightParen = new Token(TokenType.RightParen);
        internal static readonly Token Minus = new Token(TokenType.Minus);
        internal static readonly Token Plus = new Token(TokenType.Plus);
        internal static readonly Token Slash = new Token(TokenType.Slash);
        internal static readonly Token Asterisk = new Token(TokenType.Asterisk);
        internal static readonly Token True = new Token(TokenType.True);
        internal static readonly Token Undefined = new Token(TokenType.Undefined);

        internal static ErrorToken UnexpectedlyFound(int position, char unexpectedlyFound) {
            var message = SR.ParserUnexpectedlyFound(Convert.ToString(unexpectedlyFound, CultureInfo.InvariantCulture));
            return new ErrorToken(position, message);
        }

        internal static ErrorToken UnexpectedlyFound(int position, string unexpectedlyFound) {
            var message = SR.ParserUnexpectedlyFound(unexpectedlyFound);
            return new ErrorToken(position, message);
        }

        internal static Token Error(int position, string message) {
            return new ErrorToken(position, message);
        }

        private readonly string _text;
        private readonly TokenType _type;
        private readonly bool _expandable;

        private Token(TokenType type) : this(type, null, false) {}

        internal Token(TokenType type, string tokenString, bool expandable) {
            _type = type;
            _text = tokenString;
            _expandable = expandable;
        }

        internal TokenType Type {
            get {
                return _type;
            }
        }

        public bool IsExpandable {
            get {
                return _expandable;
            }
        }

        internal string Value {
            get {
                if (_text != null)
                    return _text;

                switch (_type) {
                    case TokenType.Comma:
                        return ",";

                    case TokenType.LeftParen:
                        return "(";

                    case TokenType.RightParen:
                        return ")";

                    case TokenType.LeftBracket:
                        return "[";

                    case TokenType.RightBracket:
                        return "]";

                    case TokenType.Dot:
                        return ".";

                    case TokenType.LessThan:
                        return "<";

                    case TokenType.GreaterThan:
                        return ">";

                    case TokenType.LessThanOrEqualTo:
                        return "<=";

                    case TokenType.GreaterThanOrEqualTo:
                        return ">=";

                    case TokenType.And:
                        return "and";

                    case TokenType.Or:
                        return "or";

                    case TokenType.EqualTo:
                        return "==";

                    case TokenType.NotEqualTo:
                        return "!=";

                    case TokenType.Not:
                        return "!";

                    case TokenType.Plus:
                        return "+";

                    case TokenType.Asterisk:
                        return "*";

                    case TokenType.Slash:
                        return "/";

                    case TokenType.Minus:
                        return "-";

                    case TokenType.True:
                        return "true";

                    case TokenType.False:
                        return "false";

                    case TokenType.Null:
                        return "null";

                    case TokenType.Undefined:
                        return "undefined";

                    case TokenType.NaN:
                        return "NaN";

                    case TokenType.BitAnd:
                        return "&";

                    case TokenType.EndOfInput:
                    case TokenType.Error:
                        return null;

                    default:
                        CoreRuntimeExpressionsWarning.UnexpectedUnreachableCode(_type);
                        return null;
                }
            }
        }

        internal bool IsToken(TokenType type) {
            return _type == type;
        }

        // TODO Use bracket counting ... the expression $(a+b+(c+d)) is valid, for instance

        private static readonly Regex EXPR_FORMAT = new Regex(@"(?<!\\)\$ (
(\{ (?<Expression> [^\}]+) \}) | (?<Expression> [:a-z0-9_\.]+) )", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        internal IEnumerable<Token> Expand() {
            return ExpandCore().Where(t => t.Value.Length > 0);
        }

        private IEnumerable<Token> ExpandCore() {
            string text = Value;
            MatchCollection matches = EXPR_FORMAT.Matches(text);
            int previousIndex = 0;
            foreach (Match match in matches) {
                yield return new Token(TokenType.String, text.Substring(previousIndex, match.Index - previousIndex), false);

                string expText = match.Groups["Expression"].Value;
                yield return new Token(TokenType.Property, expText, false);
                previousIndex = match.Index + match.Length;
            }
            yield return new Token(TokenType.String, text.Substring(previousIndex, text.Length - previousIndex), false);
        }

    }
}
