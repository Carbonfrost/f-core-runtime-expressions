//
// Copyright 2020-2021 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    public abstract class InterpolatedStringContent {

        internal abstract Expression ToExpression();

        public bool IsTextContent {
            get {
                return this is InterpolatedStringTextContent;
            }
        }

        public bool IsInterpolation {
            get {
                return this is Interpolation;
            }
        }

        public InterpolatedStringContentType InterpolatedStringContentType {
            get {
                return IsInterpolation ? InterpolatedStringContentType.Interpolation : InterpolatedStringContentType.TextContent;
            }
        }

        public static Interpolation Interpolation(Expression value) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }
            return new Interpolation(value);
        }

        public static InterpolatedStringTextContent TextContent(string text) {
            if (text == null) {
                throw new ArgumentNullException(nameof(text));
            }
            return new InterpolatedStringTextContent(text);
        }

        public static implicit operator InterpolatedStringContent(string value) {
            return TextContent(value);
        }

        public static implicit operator InterpolatedStringContent(Expression value) {
            return Interpolation(value);
        }

    }
}
