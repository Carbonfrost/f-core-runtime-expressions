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

    public enum ExpressionType {
        Unknown,

        // Arithmetic
        Add,
        AddChecked,
        AddAssign,
        AddAssignChecked,
        Divide,
        DivideAssign,
        Multiply,
        MultiplyAssign,
        MultiplyAssignChecked,
        MultiplyChecked,
        Subtract,
        SubtractAssign,
        SubtractAssignChecked,
        SubtractChecked,
        Power,
        PowerAssign,
        RightShift,
        RightShiftAssign,
        LeftShift,
        LeftShiftAssign,
        Modulo,
        ModuloAssign,

        // Unary,
        PostIncrement,
        PostDecrement,
        PreIncrement,
        PreDecrement,
        Negate,
        NegateChecked,
        OnesComplement,
        Not,

        // Logical
        And,
        AndAssign,
        AndAlso,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        Or,
        OrAssign,
        OrElse,

        // Misc
        Constant,
        ArrayIndex,
        Coalesce,
        Value,
        EqualTo,
        NotEqualTo,
        Assign,
        ExclusiveOr,
        ExclusiveOrAssign,
        Block,
        Conditional,
        MemberAccess,
        Name,
        Call,
        NewObject,
        NewArray,
        Lambda,

    }
}
