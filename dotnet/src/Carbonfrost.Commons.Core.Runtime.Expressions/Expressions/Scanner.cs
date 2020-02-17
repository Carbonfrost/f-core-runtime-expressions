//
// Copyright 2013, 2016, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Text;
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    internal sealed class Scanner : IEnumerator<Token> {

        private Token _current;
        private int _pos;
        private readonly string _expression;

        private const string EndOfInput = "<EOF>";

        public bool IsError {
            get {
                return _current != null && _current.IsToken(TokenType.Error);
            }
        }

        private char Char {
            get {
                return _expression[_pos];
            }
        }

        internal bool MoreChars {
            get {
                return _pos < _expression.Length;
             }
        }

        private bool PastEOF {
            get {
                return _pos >= _expression.Length;
            }
        }

        public Token Current {
            get {
                return _current;
            }
        }

        object System.Collections.IEnumerator.Current {
            get {
                return Current;
            }
        }

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
            _expression = text;
            _pos = 0;
        }

        internal bool IsNext(TokenType type) {
            return _current.IsToken(type);
        }

        internal string TakeValue() {
            string s = _current.Value;
            MoveNext();
            return s;
        }

        private bool ParseNumericOrPlus(int start) {
            if (IsParseHexNumber()) {
                _pos += 2;
                this.SkipHexDigits();
                _current = new Token(TokenType.Number, Subtext(start));

                return true;

            } else if (IsNumberStart(Char)) {
                if (this.Char == '+' || this.Char == '-') {
                    _pos++;
                }

                this.SkipDigits();
                if (MoreChars && (this.Char == '.')) {
                    _pos++;
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
                _current = Token.UnexpectedlyFound(start + 1, Char.ToString());
                return false;
            }
        }

        private bool SetToken(Token token) {
            _current = token;
            return true;
        }

        private bool IsParseHexNumber() {
            if (_expression.Length - _pos > 2) {
                char c = _expression[_pos];
                char d = _expression[_pos + 1];

                return c == '0' && (d == 'x' || d == 'X');
            }

            return false;
        }

        private bool ParseProperty() {
            int start = _pos;
            _pos++; // move past $

            if (MoreChars && this.Char != '(') {
                _current = Token.UnexpectedlyFound(start + 1, Char);
                return false;
            }

            _pos = ScanForPropertyExpressionEnd(_expression, _pos++);
            if (PastEOF) {
                _current = Token.UnexpectedlyFound(start + 1, EndOfInput);
                return false;
            }

            _pos++;

            string tokenString = this.Subtext(start);
            _current = new Token(TokenType.Property, tokenString);
            return true;
        }

        private bool ParseQuotedString() {
            char startChar = this.Char;

            _pos++;
            int start = _pos;
            bool expandable = false;

            // TODO Revisit if different semantics should be applied depending upon
            // whether quotes or apostrophe is used
            // Look for expansion within quoted strings:
            // 'Hello, ${name}'

            // TODO Trap unterminated expansions here
            while (this.MoreChars && this.Char != startChar) {
                if (this.Char == '$'
                    && _pos + 1 < _expression.Length
                    && _expression[_pos + 1] == '{') {
                    expandable = true;
                }

                _pos++;
            }

            if (PastEOF) {
                _current = Token.Error(start, EndOfInput);
                return false;
            }

            string tokenString = this.Subtext(start);
            _current = new Token(TokenType.String, tokenString/*, expandable*/);
            _pos++;
            return true;
        }

        private bool ParseRemaining() {
            int start = _pos;

            if (IsNumberStart(this.Char)) {
                return ParseNumericOrPlus(start);

            } else if (IsSimpleStringStart(this.Char)) {
                return ParseSimpleStringOrFunction(start);

            } else {
                _current = Token.UnexpectedlyFound(start + 1, Char);
                return false;
            }
        }

        private bool ParseSimpleStringOrFunction(int start) {
            this.SkipSimpleStringChars();

            Token t = TryIdentAsKeyword(start);
            if (t != null) {
                _current = t;

            } else {
                int myStart = _pos;
                this.SkipWhiteSpace();

                if (MoreChars && this.Char == '(') {
                    _current = new Token(TokenType.Function, _expression.Substring(start, myStart - start));

                } else {
                    string tokenString = _expression.Substring(start, myStart - start);
                    _current = new Token(TokenType.Identifier, tokenString);
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
                _pos++;
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
            return _expression.Substring(start, _pos - start);
        }

        void IDisposable.Dispose() {}

        public bool MoveNext() {
            if (IsError)
                return false;

            if (_current != null && _current.IsToken(TokenType.EndOfInput))
                return true;

            this.SkipWhiteSpace();
            // this.errorPosition = _pos + 1;

            if (PastEOF) {
                _current = Token.EndOfInput;
                return true;
            }

            switch (this.Char) {
                case '!':
                    if (LA('=')) {
                        _current = Token.NotEqualTo;
                        _pos += 2;

                    } else {
                        _current = Token.Not;
                        _pos++;
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
                        _current = Token.LessThanOrEqualTo;
                        _pos += 2;

                    } else {
                        _current = Token.LessThan;
                        _pos++;
                    }
                    return true;

                case '=':
                    if (LA('=')) {
                        _current = Token.EqualTo;
                        _pos += 2;
                        return true;

                    } else {
                        int errorPosition = _pos + 2;

                        if ((_pos + 1) < _expression.Length) {
                            _current = Token.UnexpectedlyFound(errorPosition, _expression[_pos + 1]);

                        } else {
                            _current = Token.UnexpectedlyFound(errorPosition, EndOfInput);
                        }

                        _pos++;

                        return false;
                    }

                case '>':
                    if (LA('=')) {
                        _current = Token.GreaterThanOrEqualTo;
                        _pos += 2;

                    } else {
                        _current = Token.GreaterThan;
                        _pos++;

                    }
                    return true;

                case '&':
                    if (LA('&')) {
                        _current = Token.And;
                        _pos += 2;
                        return true;

                    } else {
                        _current = Token.BitAnd;
                        _pos++;
                        return true;
                    }

                case '|':
                    if (LA('|')) {
                        _current = Token.Or;
                        _pos += 2;
                        return true;

                    } else {
                        _current = Token.BitOr;
                        _pos++;
                        return true;
                    }
            }

            return ParseRemaining();
        }

        private bool Simple(Token leftParen) {
            _current = leftParen;
            _pos++;
            return true;
        }

        private bool LA(char c) {
            return (_pos + 1) < _expression.Length
                & _expression[_pos + 1] == c;
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
