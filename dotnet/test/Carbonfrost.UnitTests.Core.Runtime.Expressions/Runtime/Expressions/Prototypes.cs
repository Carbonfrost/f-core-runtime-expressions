//
// Copyright 2014 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Linq;
using Carbonfrost.Commons.Core.Runtime.Expressions;

namespace Carbonfrost.UnitTests.Core.Runtime.Expressions {

    public class Alpha {

        public bool A { get; set; }
        public bool? AA { get; set; }
        public byte B { get; set; }
        public byte? BB { get; set; }
        public byte[] C { get; set; }
        public char D { get; set; }
        public char? DD { get; set; }
        public DateTime E { get; set; }
        public DateTime? EE { get; set; }
        public decimal F { get; set; }
        public decimal? FF { get; set; }
        public double G { get; set; }
        public double? GG { get; set; }
        public short H { get; set; }
        public short? HH { get; set; }
        public int I { get; set; }
        public int? II { get; set; }
        public long J { get; set; }
        public long? JJ { get; set; }
        public sbyte K { get; set; }
        public sbyte? KK { get; set; }
        public float L { get; set; }
        public float? LL { get; set; }
        public string M { get; set; }
        public ushort N { get; set; }
        public ushort? NN { get; set; }
        public uint O { get; set; }
        public uint? OO { get; set; }
        public ulong P { get; set; }
        public ulong? PP { get; set; }
        public Uri Q { get; set; }
        public TimeSpan R { get; set; }
        public TimeSpan? RR { get; set; }
        public Guid S { get; set; }
        public Guid? SS { get; set; }
        public DateTimeOffset T { get; set; }
        public DateTimeOffset? TT { get; set; }
        public Type U { get; set; }
    }

    public class Alpha2 {
        public bool A { get; set; }
        public string B { get; set; }
    }

    public class Bravo {

        readonly List<Alpha2> items = new List<Alpha2>();

        [ExpressionSerializationMode(ExpressionSerializationMode.Content)]
        public IList<Alpha2> Items {
            get {
                return items;
            }
        }

    }

    public class Charlie {

        public Alpha2 A { get; set; }
        public char B { get; set; }
        public Charlie C { get; set; }

    }

    public class Delta {

        private readonly IList<Echo> e = new List<Echo>();

        public IList<Echo> E {
            get {
                return e;
            }
        }

    }

    public class Echo {

        private readonly IList<Echo> _children = new List<Echo>();

        public Echo Parent { get; set; }

        [ExpressionSerializationMode(ExpressionSerializationMode.Content)]
        public IList<Echo> Children {
            get {
                return _children;
            }
        }

    }

    public class Foxtrot : Echo {

        public Echo Owner { get; set; }
    }
}
