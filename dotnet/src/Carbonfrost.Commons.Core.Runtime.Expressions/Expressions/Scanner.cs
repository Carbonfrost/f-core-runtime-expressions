//
// Copyright 2013, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Text;
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    internal sealed class Scanner : IEnumerator<Token> {

        private static string endOfInput;
        private int errorPosition;

        private Token lookahead;
        private int pos;
        private string unexpectedlyFound;

        private readonly string expression;

        private string EndOfInput {
            get {
                if (endOfInput == null) {
                    endOfInput = "<EOF>";
                }
                return endOfInput;
            }
        }

        public bool IsError {
            get {
                return this.lookahead != null && this.lookahead.IsToken(TokenType.Error);
            }
        }

        private char Char {
            get { return this.expression[this.pos]; } }

        internal bool MoreChars {
            get { return this.pos < this.expression.Length; } }

        internal string UnexpectedlyFound {
            get { return this.unexpectedlyFound; } }

        public Token Current {
            get { return this.lookahead; } }

        object System.Collections.IEnumerator.Current {
            get { return Current; } }

        public bool EOF {
            get {
                return Type == TokenType.EndOfInput;
            }
        }

        public TokenType Type {
            get {
                return Current.Type;
            }
        }

        internal Scanner(string text) {
            this.expression = text;
            this.pos = 0;
            this.errorPosition = -1;
        }

        internal int GetErrorPosition() {
            return this.errorPosition;
        }

        internal bool IsNext(TokenType type) {
            return this.lookahead.IsToken(type);
        }

        internal string TakeValue() {
            string s = this.lookahead.Value;
            MoveNext();
            return s;
        }

        private bool ParseNumericOrPlus(int start) {
            if (IsParseHexNumber()) {
                this.pos += 2;
                this.SkipHexDigits();
                this.lookahead = new Token(TokenType.Number, Subtext(start));

                return true;

            } else if (IsNumberStart(this.expression[this.pos])) {
                if (this.Char == '+' || this.Char == '-') {
                    this.pos++;
                }

                this.SkipDigits();
                if (MoreChars && (this.Char == '.')) {
                    this.pos++;
                }

                if (MoreChars) {
                    this.SkipDigits();
                }

                string text = Subtext(start);
                if (text == "+")
                    return SetToken(Token.Plus);

                if (text == "-")
                    return SetToken(Token.Minus);

                return SetToken(new Token(TokenType.Number, text));

            } else {

                this.lookahead = Token.Error;
                this.errorPosition = start + 1;
                return false;
            }
        }

        private bool SetToken(Token token) {
            this.lookahead = token;
            return true;
        }

        private bool IsParseHexNumber() {
            if (expression.Length - this.pos > 2) {
                char c = expression[this.pos];
                char d = expression[this.pos + 1];

                return c == '0' && (d == 'x' || d == 'X');
            }

            return false;
        }

        private bool ParseProperty() {
            int start = this.pos;
            this.pos++; // move past $

            if (MoreChars && this.Char != '(') {
                this.lookahead = Token.Error;
                this.errorPosition = start + 1;
                this.unexpectedlyFound = Convert.ToString(this.expression[this.pos], CultureInfo.InvariantCulture);
                return false;
            }

            this.pos = ScanForPropertyExpressionEnd(this.expression, this.pos++);
            if (this.pos >= this.expression.Length) {
                this.lookahead = Token.Error;
                this.errorPosition = start + 1;
                this.unexpectedlyFound = this.EndOfInput;
                return false;
            }

            this.pos++;

            string tokenString = this.Subtext(start);
            this.lookahead = new Token(TokenType.Property, tokenString);
            return true;
        }

        private bool ParseQuotedString() {
            char startChar = this.Char;

            this.pos++;
            int start = this.pos;
            bool expandable = false;

            // TODO Revisit if different semantics should be applied depending upon
            // whether quotes or apostrophe is used
            // Look for expansion within quoted strings:
            // 'Hello, ${name}'

            // TODO Trap unterminated expansions here
            while (this.MoreChars && this.Char != startChar) {
                if (this.Char == '$'
                    && this.pos + 1 < this.expression.Length
                    && this.expression[this.pos + 1] == '{') {
                    expandable = true;
                }

                this.pos++;
            }

            if (this.pos >= this.expression.Length) {
                this.lookahead = Token.Error;
                this.errorPosition = start;
                return false;
            }

            string tokenString = this.Subtext(start);
            this.lookahead = new Token(TokenType.String, tokenString/*, expandable*/);
            this.pos++;
            return true;
        }

        private bool ParseRemaining() {
            int start = this.pos;

            if (IsNumberStart(this.Char)) {
                return ParseNumericOrPlus(start);

            } else if (IsSimpleStringStart(this.Char)) {
                return ParseSimpleStringOrFunction(start);

            } else {
                this.lookahead = Token.Error;
                this.errorPosition = start + 1;
                this.unexpectedlyFound = this.Char.ToString();
                return false;
            }
        }

        private bool ParseSimpleStringOrFunction(int start) {
            this.SkipSimpleStringChars();

            Token t = TryIdentAsKeyword(start);
            if (t != null) {
                this.lookahead = t;

            } else {
                int myStart = this.pos;
                this.SkipWhiteSpace();

                if (MoreChars && this.Char == '(') {
                    this.lookahead = new Token(TokenType.Function, this.expression.Substring(start, myStart - start));

                } else {
                    string tokenString = this.expression.Substring(start, myStart - start);
                    this.lookahead = new Token(TokenType.Identifier, tokenString);
                }
            }

            return true;
        }

        private Token TryIdentAsKeyword(int start) {
            string subtext = Subtext(start);
            switch (subtext) {
                case "and":
                    return Token.And;
                case "or":
                    return Token.Or;
                case "null":
                    return Token.Null;
                case "NaN":
                    return Token.NaN;
                case "undefined":
                    return Token.Undefined;
                case "true":
                    return Token.True;
                case "false":
                    return Token.False;
                default:
                    return null;
            }
        }

        private static int ScanForPropertyExpressionEnd(
            string expression, int index) {
            int count = 0;

            while (index < expression.Length) {
                char c = expression[index];

                // Paren balancing
                if (c == '(')
                    count++;

                else if (c == ')')
                    count--;

                if (count == 0)
                    return index;

                index++;
            }

            return index;
        }

        private static bool IsSimpleStringStart(char c) {
            if (c == '_')
                return true;
            else
                return char.IsLetter(c);
        }

        private static bool IsSimpleStringChar(char c) {
            if (IsSimpleStringStart(c))
                return true;
            else
                return char.IsDigit(c);
        }

        private void Skip2(Func<char, bool> predicate) {
            while (this.MoreChars && predicate(this.Char))
                this.pos++;
        }

        private void SkipSimpleStringChars() {
            Skip2(IsSimpleStringChar);
        }

        private void SkipWhiteSpace() {
            Skip2(char.IsWhiteSpace);
        }

        private void SkipDigits() {
            Skip2(char.IsDigit);
        }

        private void SkipHexDigits() {
            Skip2(IsHexDigit);
        }

        private string Subtext(int start) {
            return this.expression.Substring(start, this.pos - start);
        }

        void IDisposable.Dispose() {}

        public void RequireMoveNext() {
            if (MoveNext())
                return;
            else
                throw new NotImplementedException("Unterminated");
        }

        public bool MoveNext() {
            if (IsError)
                return false;

            if (this.lookahead != null && this.lookahead.IsToken(TokenType.EndOfInput))
                return true;

            this.SkipWhiteSpace();
            this.errorPosition = this.pos + 1;

            if (this.pos >= this.expression.Length) {
                this.lookahead = Token.EndOfInput;
                return true;
            }

            switch (this.Char) {
                case '!':
                    if (LA('=')) {
                        this.lookahead = Token.NotEqualTo;
                        this.pos += 2;

                    } else {
                        this.lookahead = Token.Not;
                        this.pos++;
                    }

                    return true;

                case '$':
                    return ParseProperty();

                case '\'':
                case '"':
                    return ParseQuotedString();

                case '(':
                    return Simple(Token.LeftParen);

                case ')':
                    return Simple(Token.RightParen);

                case '[':
                    return Simple(Token.LeftBracket);

                case ']':
                    return Simple(Token.RightBracket);

                case '*':
                    return Simple(Token.Asterisk);

                case '/':
                    return Simple(Token.Slash);

                case '.':
                    return Simple(Token.Dot);

                case ',':
                    return Simple(Token.Comma);

                case '<':
                    if (LA('=')) {
                        this.lookahead = Token.LessThanOrEqualTo;
                        this.pos += 2;

                    } else {
                        this.lookahead = Token.LessThan;
                        this.pos++;
                    }
                    return true;

                case '=':
                    if (LA('=')) {
                        this.lookahead = Token.EqualTo;
                        this.pos += 2;
                        return true;

                    } else {
                        this.errorPosition = this.pos + 2;

                        if ((this.pos + 1) < this.expression.Length) {
                            this.unexpectedlyFound = Convert.ToString(this.expression[this.pos + 1], CultureInfo.InvariantCulture);

                        } else {
                            this.unexpectedlyFound = this.EndOfInput;
                        }

                        this.pos++;
                        this.lookahead = Token.Error;
                        return false;
                    }

                case '>':
                    if (LA('=')) {
                        this.lookahead = Token.GreaterThanOrEqualTo;
                        this.pos += 2;

                    } else {
                        this.lookahead = Token.GreaterThan;
                        this.pos++;

                    }
                    return true;

                case '&':
                    if (LA('&')) {
                        this.lookahead = Token.And;
                        this.pos += 2;
                        return true;

                    } else {
                        this.lookahead = Token.BitAnd;
                        this.pos++;
                        return true;
                    }

                case '|':
                    if (LA('|')) {
                        this.lookahead = Token.Or;
                        this.pos += 2;
                        return true;

                    } else {
                        this.lookahead = Token.BitOr;
                        this.pos++;
                        return true;
                    }
            }

            return ParseRemaining();
        }

        private bool Simple(Token leftParen) {
            this.lookahead = leftParen;
            this.pos++;
            return true;
        }

        private bool LA(char c) {
            return (this.pos + 1) < this.expression.Length
                & this.expression[this.pos + 1] == c;
        }

        void System.Collections.IEnumerator.Reset()  {
            throw new NotSupportedException();
        }

        internal static bool IsHexDigit(char candidate) {
            if (char.IsDigit(candidate))
                return true;
            if (candidate >= 'a' && candidate <= 'f')
                return true;
            if (candidate >= 'A' && candidate <= 'F')
                return true;
            else
                return false;
        }

        internal static bool IsNumberStart(char candidate) {
            if (candidate == '+' || candidate == '-' || candidate == '.')
                return true;
            else
                return char.IsDigit(candidate);
        }

        public static bool IsPrintable(char c) {
            return Regex.IsMatch(@"\P{Cc}", c.ToString());
        }

        // Escape a quoted string
        public static StringBuilder Escape(string text) {
            StringBuilder sb = new StringBuilder(text);
            sb.Replace(@"\", @"\\");
            sb.Replace("\"", "\\\"");
            sb.Replace("\r", @"\r");
            sb.Replace("\n", @"\n");
            return sb.Replace("\t", @"\t");
        }
    }

}
