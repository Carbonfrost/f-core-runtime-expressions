//
// Copyright 2006, 2008, 2010, 2012-2013, 2015 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.IO;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    public abstract partial class Expression {

        protected internal abstract void AcceptVisitor(IExpressionVisitor visitor);
        protected internal abstract TResult AcceptVisitor<TArgument, TResult>(IExpressionVisitor<TArgument, TResult> visitor, TArgument argument);
        protected internal abstract TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor);

        public abstract ExpressionType ExpressionType { get; }

        public virtual bool CanReduce {
            get {
                return false;
            }
        }

        public virtual Expression Reduce() {
            throw CoreRuntimeExpressionsFailure.CannotReduceExpression();
        }

        public virtual BlockExpression LiftToBlock() {
            return Block(this);
        }

        public object Evaluate() {
            return Evaluate(ExpressionContext.Empty);
        }

        public virtual object Evaluate(IExpressionContext context) {
            throw CoreRuntimeExpressionsFailure.CannotEvaluateNonConstantExpression();
        }

        public void WriteTo(TextWriter output) {
            new ExpressionPrinter(output).Visit(this);
        }

        public override string ToString() {
            StringWriter sw = new StringWriter();
            WriteTo(sw);
            return sw.ToString();
        }

        public static Expression FromBoolean(bool? value) {
            if (value.HasValue)
                return value.Value ? Expression.True : Expression.False;
            else
                return NullConstant(typeof(bool?));
        }

        public static Expression FromByte(byte? value) {
            if (value.HasValue)
                return Expression.Constant(value.Value);
            else
                return NullConstant(typeof(byte?));
        }

        public static Expression FromDateTime(DateTime? value) {
            if (value.HasValue)
                return Expression.Constant(value.Value);
            else
                return NullConstant(typeof(DateTime?));
        }

        public static Expression FromDateTimeOffset(DateTimeOffset? value) {
            if (value.HasValue)
                return Expression.Constant(value.Value);
            else
                return NullConstant(typeof(DateTimeOffset?));
        }

        public static Expression FromDecimal(decimal? value) {
            if (value.HasValue)
                return Expression.Constant(value.Value);
            else
                return NullConstant(typeof(Decimal?));
        }

        public static Expression FromDouble(double? value) {
            if (value.HasValue)
                return Expression.Constant(value.Value);
            else
                return NullConstant(typeof(Double?));
        }

        public static Expression FromGuid(Guid? value) {
            if (value.HasValue)
                return Expression.Constant(value.Value);
            else
                return NullConstant(typeof(Guid?));
        }

        public static Expression FromInt16(short? value) {
            if (value.HasValue)
                return Expression.Constant(value.Value);
            else
                return NullConstant(typeof(short?));
        }

        public static Expression FromInt32(int? value) {
            if (value.HasValue)
                return Expression.Constant(value.Value);
            else
                return NullConstant(typeof(int?));
        }

        public static Expression FromInt64(long? value) {
            if (value.HasValue)
                return Expression.Constant(value.Value);
            else
                return NullConstant(typeof(long?));
        }

        public static Expression FromSingle(float? value) {
            if (value.HasValue)
                return Expression.Constant(value.Value);
            else
                return NullConstant(typeof(float?));
        }

        public static Expression FromString(string value) {
            if (value == null)
                return NullConstant(typeof(string));
            else
                return Expression.Constant(value);
        }

        public static implicit operator Expression(bool? value) {
            return FromBoolean(value);
        }

        public static implicit operator Expression(byte? value)  {
            return FromByte(value);
        }

        public static implicit operator Expression(DateTime? value)  {
            return FromDateTime(value);
        }

        public static implicit operator Expression(DateTimeOffset? value)  {
            return FromDateTimeOffset(value);
        }

        public static implicit operator Expression(decimal? value)  {
            return FromDecimal(value);
        }

        public static implicit operator Expression(Guid? value)  {
            return FromGuid(value);
        }

        public static implicit operator Expression(short? value)  {
            return FromInt16(value);
        }

        public static implicit operator Expression(int? value)  {
            return FromInt32(value);
        }

        public static implicit operator Expression(long? value)  {
            return FromInt64(value);
        }

        public static implicit operator Expression(double? value)  {
            return FromDouble(value);
        }

        public static implicit operator Expression(float? value)  {
            return FromSingle(value);
        }

        public static implicit operator Expression(string value)  {
            return FromString(value);
        }

        internal static Expression Implicit(object any) {
            if (any == null)
                return Null;

            switch (Type.GetTypeCode(any.GetType())) {
                case TypeCode.Empty:
                case TypeCode.Object:
                    Expression e = any as Expression;
                    if (e == null)
                        return Value(any);
                    else
                        return e;

                case TypeCode.DBNull:
                    return Null;

                case TypeCode.Boolean:
                    return FromBoolean((bool?) any);

                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                    return FromInt16((short?) any);

                case TypeCode.Int32:
                case TypeCode.UInt16:
                    return FromInt32((int?) any);

                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return FromInt64((long?) any);

                case TypeCode.Single:
                    return FromSingle((float?) any);

                case TypeCode.Double:
                    return FromDouble((double?) any);

                case TypeCode.Decimal:
                    return FromDecimal((decimal?) any);

                case TypeCode.String:
                    return FromString((string) any);

                case TypeCode.DateTime:
                default:
                    return Value(any);
            }
        }

        internal static object EvaluateDereference(Expression expr, IExpressionContext context) {
            var result = expr.Evaluate(context);
            if (result is Undefined && expr is NameExpression) {
                throw CoreRuntimeExpressionsFailure.ReferenceError(expr.ToString());
            }
            return result;
        }

        internal static bool IsLowerPrecedence(ExpressionType self, ExpressionType other) {
            return Precedence(self) > Precedence(other);
        }

        internal static int Precedence(ExpressionType t) {
            switch (t) {
                    // case ExpressionType.Comma;
                    // return 0;
                case ExpressionType.MemberAccess:
                case ExpressionType.ArrayIndex:
                case ExpressionType.NewObject:
                    return 18;

                case ExpressionType.Call:
                    return 17;

                case ExpressionType.PostIncrement:
                case ExpressionType.PostDecrement:
                    return 16;

                case ExpressionType.Not:
                case ExpressionType.OnesComplement:
                    // case ExpressionType.UnaryPlus:
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.PreIncrement:
                case ExpressionType.PreDecrement:
                    return 15;

                case ExpressionType.Divide:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Power:
                case ExpressionType.Modulo:
                    return 14;

                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return 13;

                case ExpressionType.LeftShift:
                case ExpressionType.RightShift:
                    return 12;

                case ExpressionType.LessThan:
                case ExpressionType.GreaterThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThanOrEqual:
                    return 11;

                case ExpressionType.EqualTo:
                case ExpressionType.NotEqualTo:
                    return 10;

                case ExpressionType.AndAlso:
                    return 6;

                case ExpressionType.OrElse:
                    return 5;

                case ExpressionType.Conditional:
                    return 4;

                case ExpressionType.And:
                case ExpressionType.Or:
                    return 5;
            }
            return -1;
        }

    }
}
