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


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    public partial class BlockExpression : Expression {

        private readonly ReadOnlyCollection<Expression> _expressions;

        public ReadOnlyCollection<Expression> Expressions {
            get {
                return _expressions;
            }
        }

        internal BlockExpression(ReadOnlyCollection<Expression> items) {
            _expressions = items;
        }

        public override BlockExpression LiftToBlock() {
            return this;
        }

        public BlockExpression Update(IEnumerable<Expression> expressions) {
            if (expressions == null) {
                throw new ArgumentNullException("expressions");
            }

            if (expressions == _expressions) {
                return this;
            }

            return Block(expressions);
        }
    }
}
