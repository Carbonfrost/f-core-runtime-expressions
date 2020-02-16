    // Generated at 02/16/2020 00:14:11

using System;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Core.Runtime.Expressions {

    // TODO Need Checked and Assigned variants
    public partial class Expression {

        public static BinaryExpression MakeBinary(ExpressionType expressionType, Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            switch (expressionType) {
                case ExpressionType.Add:
                    return Add(left, right);

                case ExpressionType.AddChecked:
                    return AddChecked(left, right);

                case ExpressionType.Subtract:
                    return Subtract(left, right);

                case ExpressionType.SubtractChecked:
                    return SubtractChecked(left, right);

                case ExpressionType.Divide:
                    return Divide(left, right);


                case ExpressionType.Multiply:
                    return Multiply(left, right);

                case ExpressionType.MultiplyChecked:
                    return MultiplyChecked(left, right);

                case ExpressionType.LeftShift:
                    return LeftShift(left, right);


                case ExpressionType.RightShift:
                    return RightShift(left, right);


                case ExpressionType.Power:
                    return Power(left, right);


                case ExpressionType.Modulo:
                    return Modulo(left, right);


                case ExpressionType.And:
                    return And(left, right);


                case ExpressionType.AndAlso:
                    return AndAlso(left, right);


                case ExpressionType.Or:
                    return Or(left, right);


                case ExpressionType.OrElse:
                    return OrElse(left, right);


                case ExpressionType.EqualTo:
                    return EqualTo(left, right);


                case ExpressionType.GreaterThan:
                    return GreaterThan(left, right);


                case ExpressionType.GreaterThanOrEqual:
                    return GreaterThanOrEqual(left, right);


                case ExpressionType.LessThan:
                    return LessThan(left, right);


                case ExpressionType.LessThanOrEqual:
                    return LessThanOrEqual(left, right);


                case ExpressionType.NotEqualTo:
                    return NotEqualTo(left, right);


                case ExpressionType.Coalesce:
                    return Coalesce(left, right);


                case ExpressionType.AddAssign:
                    return AddAssign(left, right);

                case ExpressionType.AddAssignChecked:
                    return AddAssignChecked(left, right);

                case ExpressionType.SubtractAssign:
                    return SubtractAssign(left, right);

                case ExpressionType.SubtractAssignChecked:
                    return SubtractAssignChecked(left, right);

                case ExpressionType.DivideAssign:
                    return DivideAssign(left, right);


                case ExpressionType.MultiplyAssign:
                    return MultiplyAssign(left, right);

                case ExpressionType.MultiplyAssignChecked:
                    return MultiplyAssignChecked(left, right);

                case ExpressionType.LeftShiftAssign:
                    return LeftShiftAssign(left, right);


                case ExpressionType.RightShiftAssign:
                    return RightShiftAssign(left, right);


                case ExpressionType.PowerAssign:
                    return PowerAssign(left, right);


                case ExpressionType.ModuloAssign:
                    return ModuloAssign(left, right);


                case ExpressionType.AndAssign:
                    return AndAssign(left, right);


                case ExpressionType.OrAssign:
                    return OrAssign(left, right);


                default:
                    throw CoreRuntimeExpressionsFailure.NotBinaryExpressionType("expressionType", expressionType);
            }
        }

        public static UnaryExpression MakeUnary(ExpressionType expressionType, Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            switch (expressionType) {
                case ExpressionType.PostDecrement:
                    return PostDecrement(expression);
                case ExpressionType.PostIncrement:
                    return PostIncrement(expression);
                case ExpressionType.PreDecrement:
                    return PreDecrement(expression);
                case ExpressionType.PreIncrement:
                    return PreIncrement(expression);
                case ExpressionType.Negate:
                    return Negate(expression);
                case ExpressionType.NegateChecked:
                    return NegateChecked(expression);
                case ExpressionType.Not:
                    return Not(expression);
                case ExpressionType.OnesComplement:
                    return OnesComplement(expression);
                default:
                    throw CoreRuntimeExpressionsFailure.NotBinaryExpressionType("expressionType", expressionType);
            }
        }

        public static UnaryExpression PostDecrement(Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return new UnaryExpression(ExpressionType.PostDecrement, expression);
        }

        public virtual Expression PostDecrement() {
            return PostDecrement(this);
        }

        public static UnaryExpression PostIncrement(Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return new UnaryExpression(ExpressionType.PostIncrement, expression);
        }

        public virtual Expression PostIncrement() {
            return PostIncrement(this);
        }

        public static UnaryExpression PreDecrement(Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return new UnaryExpression(ExpressionType.PreDecrement, expression);
        }

        public virtual Expression PreDecrement() {
            return PreDecrement(this);
        }

        public static UnaryExpression PreIncrement(Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return new UnaryExpression(ExpressionType.PreIncrement, expression);
        }

        public virtual Expression PreIncrement() {
            return PreIncrement(this);
        }

        public static UnaryExpression Negate(Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return new UnaryExpression(ExpressionType.Negate, expression);
        }

        public virtual Expression Negate() {
            return Negate(this);
        }

        public static UnaryExpression NegateChecked(Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return new UnaryExpression(ExpressionType.NegateChecked, expression);
        }

        public virtual Expression NegateChecked() {
            return NegateChecked(this);
        }

        public static UnaryExpression Not(Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return new UnaryExpression(ExpressionType.Not, expression);
        }

        public virtual Expression Not() {
            return Not(this);
        }

        public static UnaryExpression OnesComplement(Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return new UnaryExpression(ExpressionType.OnesComplement, expression);
        }

        public virtual Expression OnesComplement() {
            return OnesComplement(this);
        }


        public static BinaryExpression Add(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.Add, left, right);
        }

        public virtual Expression Add(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return Add(this, right);
        }

        public static BinaryExpression AddChecked(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.AddChecked, left, right);
        }


        public static BinaryExpression Subtract(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.Subtract, left, right);
        }

        public virtual Expression Subtract(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return Subtract(this, right);
        }

        public static BinaryExpression SubtractChecked(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.SubtractChecked, left, right);
        }


        public static BinaryExpression Divide(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.Divide, left, right);
        }

        public virtual Expression Divide(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return Divide(this, right);
        }



        public static BinaryExpression Multiply(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.Multiply, left, right);
        }

        public virtual Expression Multiply(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return Multiply(this, right);
        }

        public static BinaryExpression MultiplyChecked(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.MultiplyChecked, left, right);
        }


        public static BinaryExpression LeftShift(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.LeftShift, left, right);
        }

        public virtual Expression LeftShift(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return LeftShift(this, right);
        }



        public static BinaryExpression RightShift(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.RightShift, left, right);
        }

        public virtual Expression RightShift(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return RightShift(this, right);
        }



        public static BinaryExpression Power(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.Power, left, right);
        }

        public virtual Expression Power(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return Power(this, right);
        }



        public static BinaryExpression Modulo(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.Modulo, left, right);
        }

        public virtual Expression Modulo(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return Modulo(this, right);
        }



        public static BinaryExpression And(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.And, left, right);
        }

        public virtual Expression And(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return And(this, right);
        }



        public static BinaryExpression AndAlso(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.AndAlso, left, right);
        }

        public virtual Expression AndAlso(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return AndAlso(this, right);
        }



        public static BinaryExpression Or(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.Or, left, right);
        }

        public virtual Expression Or(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return Or(this, right);
        }



        public static BinaryExpression OrElse(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.OrElse, left, right);
        }

        public virtual Expression OrElse(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return OrElse(this, right);
        }



        public static BinaryExpression EqualTo(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.EqualTo, left, right);
        }

        public virtual Expression EqualTo(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return EqualTo(this, right);
        }



        public static BinaryExpression GreaterThan(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.GreaterThan, left, right);
        }

        public virtual Expression GreaterThan(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return GreaterThan(this, right);
        }



        public static BinaryExpression GreaterThanOrEqual(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.GreaterThanOrEqual, left, right);
        }

        public virtual Expression GreaterThanOrEqual(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return GreaterThanOrEqual(this, right);
        }



        public static BinaryExpression LessThan(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.LessThan, left, right);
        }

        public virtual Expression LessThan(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return LessThan(this, right);
        }



        public static BinaryExpression LessThanOrEqual(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.LessThanOrEqual, left, right);
        }

        public virtual Expression LessThanOrEqual(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return LessThanOrEqual(this, right);
        }



        public static BinaryExpression NotEqualTo(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.NotEqualTo, left, right);
        }

        public virtual Expression NotEqualTo(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return NotEqualTo(this, right);
        }



        public static BinaryExpression Coalesce(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.Coalesce, left, right);
        }

        public virtual Expression Coalesce(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return Coalesce(this, right);
        }



        public static BinaryExpression AddAssign(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.AddAssign, left, right);
        }

        public virtual Expression AddAssign(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return AddAssign(this, right);
        }

        public static BinaryExpression AddAssignChecked(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.AddAssignChecked, left, right);
        }


        public static BinaryExpression SubtractAssign(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.SubtractAssign, left, right);
        }

        public virtual Expression SubtractAssign(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return SubtractAssign(this, right);
        }

        public static BinaryExpression SubtractAssignChecked(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.SubtractAssignChecked, left, right);
        }


        public static BinaryExpression DivideAssign(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.DivideAssign, left, right);
        }

        public virtual Expression DivideAssign(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return DivideAssign(this, right);
        }



        public static BinaryExpression MultiplyAssign(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.MultiplyAssign, left, right);
        }

        public virtual Expression MultiplyAssign(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return MultiplyAssign(this, right);
        }

        public static BinaryExpression MultiplyAssignChecked(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.MultiplyAssignChecked, left, right);
        }


        public static BinaryExpression LeftShiftAssign(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.LeftShiftAssign, left, right);
        }

        public virtual Expression LeftShiftAssign(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return LeftShiftAssign(this, right);
        }



        public static BinaryExpression RightShiftAssign(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.RightShiftAssign, left, right);
        }

        public virtual Expression RightShiftAssign(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return RightShiftAssign(this, right);
        }



        public static BinaryExpression PowerAssign(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.PowerAssign, left, right);
        }

        public virtual Expression PowerAssign(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return PowerAssign(this, right);
        }



        public static BinaryExpression ModuloAssign(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.ModuloAssign, left, right);
        }

        public virtual Expression ModuloAssign(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return ModuloAssign(this, right);
        }



        public static BinaryExpression AndAssign(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.AndAssign, left, right);
        }

        public virtual Expression AndAssign(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return AndAssign(this, right);
        }



        public static BinaryExpression OrAssign(Expression left, Expression right) {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new BinaryExpression(ExpressionType.OrAssign, left, right);
        }

        public virtual Expression OrAssign(Expression right) {
            if (right == null)
                throw new ArgumentNullException("right");

            return OrAssign(this, right);
        }



    }


    public partial interface IExpressionVisitor {
        void Visit(ValueExpression expression);
    }

    public partial class ExpressionVisitor {

        protected virtual void VisitValueExpression(ValueExpression expression) {
            DefaultVisit(expression);
        }

        void IExpressionVisitor.Visit(ValueExpression expression) {
            VisitValueExpression(expression);
        }
    }

    public partial interface IExpressionVisitor<TArgument, TResult> {
        TResult Visit(ValueExpression expression, TArgument argument);
    }

    public partial class ExpressionVisitor<TArgument, TResult> {

        protected virtual TResult VisitValueExpression(ValueExpression expression, TArgument argument) {
            return DefaultVisit(expression, argument);
        }

        TResult IExpressionVisitor<TArgument,TResult>.Visit(ValueExpression expression, TArgument argument) {
            return VisitValueExpression(expression, argument);
        }
    }

    public partial interface IExpressionVisitor<TResult> {
        TResult Visit(ValueExpression expression);
    }

    public partial class ExpressionVisitor<TResult> {

        protected virtual TResult VisitValueExpression(ValueExpression expression) {
            return DefaultVisit(expression);
        }

        TResult IExpressionVisitor<TResult>.Visit(ValueExpression expression) {
            return VisitValueExpression(expression);
        }

    }

    public partial class ValueExpression {

        public override ExpressionType ExpressionType {
            get {
                return ExpressionType.Value;
            }
        }

        protected internal override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this);
        }

        protected internal override TResult AcceptVisitor<TArgument, TResult>(IExpressionVisitor<TArgument, TResult> visitor, TArgument argument) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this, argument);
        }

        protected internal override void AcceptVisitor(IExpressionVisitor visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            visitor.Visit(this);
        }
    }


    public partial interface IExpressionVisitor {
        void Visit(ConstantExpression expression);
    }

    public partial class ExpressionVisitor {

        protected virtual void VisitConstantExpression(ConstantExpression expression) {
            DefaultVisit(expression);
        }

        void IExpressionVisitor.Visit(ConstantExpression expression) {
            VisitConstantExpression(expression);
        }
    }

    public partial interface IExpressionVisitor<TArgument, TResult> {
        TResult Visit(ConstantExpression expression, TArgument argument);
    }

    public partial class ExpressionVisitor<TArgument, TResult> {

        protected virtual TResult VisitConstantExpression(ConstantExpression expression, TArgument argument) {
            return DefaultVisit(expression, argument);
        }

        TResult IExpressionVisitor<TArgument,TResult>.Visit(ConstantExpression expression, TArgument argument) {
            return VisitConstantExpression(expression, argument);
        }
    }

    public partial interface IExpressionVisitor<TResult> {
        TResult Visit(ConstantExpression expression);
    }

    public partial class ExpressionVisitor<TResult> {

        protected virtual TResult VisitConstantExpression(ConstantExpression expression) {
            return DefaultVisit(expression);
        }

        TResult IExpressionVisitor<TResult>.Visit(ConstantExpression expression) {
            return VisitConstantExpression(expression);
        }

    }

    public partial class ConstantExpression {

        public override ExpressionType ExpressionType {
            get {
                return ExpressionType.Constant;
            }
        }

        protected internal override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this);
        }

        protected internal override TResult AcceptVisitor<TArgument, TResult>(IExpressionVisitor<TArgument, TResult> visitor, TArgument argument) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this, argument);
        }

        protected internal override void AcceptVisitor(IExpressionVisitor visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            visitor.Visit(this);
        }
    }


    public partial interface IExpressionVisitor {
        void Visit(BlockExpression expression);
    }

    public partial class ExpressionVisitor {

        protected virtual void VisitBlockExpression(BlockExpression expression) {
            DefaultVisit(expression);
        }

        void IExpressionVisitor.Visit(BlockExpression expression) {
            VisitBlockExpression(expression);
        }
    }

    public partial interface IExpressionVisitor<TArgument, TResult> {
        TResult Visit(BlockExpression expression, TArgument argument);
    }

    public partial class ExpressionVisitor<TArgument, TResult> {

        protected virtual TResult VisitBlockExpression(BlockExpression expression, TArgument argument) {
            return DefaultVisit(expression, argument);
        }

        TResult IExpressionVisitor<TArgument,TResult>.Visit(BlockExpression expression, TArgument argument) {
            return VisitBlockExpression(expression, argument);
        }
    }

    public partial interface IExpressionVisitor<TResult> {
        TResult Visit(BlockExpression expression);
    }

    public partial class ExpressionVisitor<TResult> {

        protected virtual TResult VisitBlockExpression(BlockExpression expression) {
            return DefaultVisit(expression);
        }

        TResult IExpressionVisitor<TResult>.Visit(BlockExpression expression) {
            return VisitBlockExpression(expression);
        }

    }

    public partial class BlockExpression {

        public override ExpressionType ExpressionType {
            get {
                return ExpressionType.Block;
            }
        }

        protected internal override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this);
        }

        protected internal override TResult AcceptVisitor<TArgument, TResult>(IExpressionVisitor<TArgument, TResult> visitor, TArgument argument) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this, argument);
        }

        protected internal override void AcceptVisitor(IExpressionVisitor visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            visitor.Visit(this);
        }
    }


    public partial interface IExpressionVisitor {
        void Visit(ConditionalExpression expression);
    }

    public partial class ExpressionVisitor {

        protected virtual void VisitConditionalExpression(ConditionalExpression expression) {
            DefaultVisit(expression);
        }

        void IExpressionVisitor.Visit(ConditionalExpression expression) {
            VisitConditionalExpression(expression);
        }
    }

    public partial interface IExpressionVisitor<TArgument, TResult> {
        TResult Visit(ConditionalExpression expression, TArgument argument);
    }

    public partial class ExpressionVisitor<TArgument, TResult> {

        protected virtual TResult VisitConditionalExpression(ConditionalExpression expression, TArgument argument) {
            return DefaultVisit(expression, argument);
        }

        TResult IExpressionVisitor<TArgument,TResult>.Visit(ConditionalExpression expression, TArgument argument) {
            return VisitConditionalExpression(expression, argument);
        }
    }

    public partial interface IExpressionVisitor<TResult> {
        TResult Visit(ConditionalExpression expression);
    }

    public partial class ExpressionVisitor<TResult> {

        protected virtual TResult VisitConditionalExpression(ConditionalExpression expression) {
            return DefaultVisit(expression);
        }

        TResult IExpressionVisitor<TResult>.Visit(ConditionalExpression expression) {
            return VisitConditionalExpression(expression);
        }

    }

    public partial class ConditionalExpression {

        public override ExpressionType ExpressionType {
            get {
                return ExpressionType.Conditional;
            }
        }

        protected internal override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this);
        }

        protected internal override TResult AcceptVisitor<TArgument, TResult>(IExpressionVisitor<TArgument, TResult> visitor, TArgument argument) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this, argument);
        }

        protected internal override void AcceptVisitor(IExpressionVisitor visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            visitor.Visit(this);
        }
    }


    public partial interface IExpressionVisitor {
        void Visit(MemberAccessExpression expression);
    }

    public partial class ExpressionVisitor {

        protected virtual void VisitMemberAccessExpression(MemberAccessExpression expression) {
            DefaultVisit(expression);
        }

        void IExpressionVisitor.Visit(MemberAccessExpression expression) {
            VisitMemberAccessExpression(expression);
        }
    }

    public partial interface IExpressionVisitor<TArgument, TResult> {
        TResult Visit(MemberAccessExpression expression, TArgument argument);
    }

    public partial class ExpressionVisitor<TArgument, TResult> {

        protected virtual TResult VisitMemberAccessExpression(MemberAccessExpression expression, TArgument argument) {
            return DefaultVisit(expression, argument);
        }

        TResult IExpressionVisitor<TArgument,TResult>.Visit(MemberAccessExpression expression, TArgument argument) {
            return VisitMemberAccessExpression(expression, argument);
        }
    }

    public partial interface IExpressionVisitor<TResult> {
        TResult Visit(MemberAccessExpression expression);
    }

    public partial class ExpressionVisitor<TResult> {

        protected virtual TResult VisitMemberAccessExpression(MemberAccessExpression expression) {
            return DefaultVisit(expression);
        }

        TResult IExpressionVisitor<TResult>.Visit(MemberAccessExpression expression) {
            return VisitMemberAccessExpression(expression);
        }

    }

    public partial class MemberAccessExpression {

        public override ExpressionType ExpressionType {
            get {
                return ExpressionType.MemberAccess;
            }
        }

        protected internal override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this);
        }

        protected internal override TResult AcceptVisitor<TArgument, TResult>(IExpressionVisitor<TArgument, TResult> visitor, TArgument argument) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this, argument);
        }

        protected internal override void AcceptVisitor(IExpressionVisitor visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            visitor.Visit(this);
        }
    }


    public partial interface IExpressionVisitor {
        void Visit(NameExpression expression);
    }

    public partial class ExpressionVisitor {

        protected virtual void VisitNameExpression(NameExpression expression) {
            DefaultVisit(expression);
        }

        void IExpressionVisitor.Visit(NameExpression expression) {
            VisitNameExpression(expression);
        }
    }

    public partial interface IExpressionVisitor<TArgument, TResult> {
        TResult Visit(NameExpression expression, TArgument argument);
    }

    public partial class ExpressionVisitor<TArgument, TResult> {

        protected virtual TResult VisitNameExpression(NameExpression expression, TArgument argument) {
            return DefaultVisit(expression, argument);
        }

        TResult IExpressionVisitor<TArgument,TResult>.Visit(NameExpression expression, TArgument argument) {
            return VisitNameExpression(expression, argument);
        }
    }

    public partial interface IExpressionVisitor<TResult> {
        TResult Visit(NameExpression expression);
    }

    public partial class ExpressionVisitor<TResult> {

        protected virtual TResult VisitNameExpression(NameExpression expression) {
            return DefaultVisit(expression);
        }

        TResult IExpressionVisitor<TResult>.Visit(NameExpression expression) {
            return VisitNameExpression(expression);
        }

    }

    public partial class NameExpression {

        public override ExpressionType ExpressionType {
            get {
                return ExpressionType.Name;
            }
        }

        protected internal override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this);
        }

        protected internal override TResult AcceptVisitor<TArgument, TResult>(IExpressionVisitor<TArgument, TResult> visitor, TArgument argument) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this, argument);
        }

        protected internal override void AcceptVisitor(IExpressionVisitor visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            visitor.Visit(this);
        }
    }


    public partial interface IExpressionVisitor {
        void Visit(CallExpression expression);
    }

    public partial class ExpressionVisitor {

        protected virtual void VisitCallExpression(CallExpression expression) {
            DefaultVisit(expression);
        }

        void IExpressionVisitor.Visit(CallExpression expression) {
            VisitCallExpression(expression);
        }
    }

    public partial interface IExpressionVisitor<TArgument, TResult> {
        TResult Visit(CallExpression expression, TArgument argument);
    }

    public partial class ExpressionVisitor<TArgument, TResult> {

        protected virtual TResult VisitCallExpression(CallExpression expression, TArgument argument) {
            return DefaultVisit(expression, argument);
        }

        TResult IExpressionVisitor<TArgument,TResult>.Visit(CallExpression expression, TArgument argument) {
            return VisitCallExpression(expression, argument);
        }
    }

    public partial interface IExpressionVisitor<TResult> {
        TResult Visit(CallExpression expression);
    }

    public partial class ExpressionVisitor<TResult> {

        protected virtual TResult VisitCallExpression(CallExpression expression) {
            return DefaultVisit(expression);
        }

        TResult IExpressionVisitor<TResult>.Visit(CallExpression expression) {
            return VisitCallExpression(expression);
        }

    }

    public partial class CallExpression {

        public override ExpressionType ExpressionType {
            get {
                return ExpressionType.Call;
            }
        }

        protected internal override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this);
        }

        protected internal override TResult AcceptVisitor<TArgument, TResult>(IExpressionVisitor<TArgument, TResult> visitor, TArgument argument) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this, argument);
        }

        protected internal override void AcceptVisitor(IExpressionVisitor visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            visitor.Visit(this);
        }
    }


    public partial interface IExpressionVisitor {
        void Visit(NewObjectExpression expression);
    }

    public partial class ExpressionVisitor {

        protected virtual void VisitNewObjectExpression(NewObjectExpression expression) {
            DefaultVisit(expression);
        }

        void IExpressionVisitor.Visit(NewObjectExpression expression) {
            VisitNewObjectExpression(expression);
        }
    }

    public partial interface IExpressionVisitor<TArgument, TResult> {
        TResult Visit(NewObjectExpression expression, TArgument argument);
    }

    public partial class ExpressionVisitor<TArgument, TResult> {

        protected virtual TResult VisitNewObjectExpression(NewObjectExpression expression, TArgument argument) {
            return DefaultVisit(expression, argument);
        }

        TResult IExpressionVisitor<TArgument,TResult>.Visit(NewObjectExpression expression, TArgument argument) {
            return VisitNewObjectExpression(expression, argument);
        }
    }

    public partial interface IExpressionVisitor<TResult> {
        TResult Visit(NewObjectExpression expression);
    }

    public partial class ExpressionVisitor<TResult> {

        protected virtual TResult VisitNewObjectExpression(NewObjectExpression expression) {
            return DefaultVisit(expression);
        }

        TResult IExpressionVisitor<TResult>.Visit(NewObjectExpression expression) {
            return VisitNewObjectExpression(expression);
        }

    }

    public partial class NewObjectExpression {

        public override ExpressionType ExpressionType {
            get {
                return ExpressionType.NewObject;
            }
        }

        protected internal override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this);
        }

        protected internal override TResult AcceptVisitor<TArgument, TResult>(IExpressionVisitor<TArgument, TResult> visitor, TArgument argument) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this, argument);
        }

        protected internal override void AcceptVisitor(IExpressionVisitor visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            visitor.Visit(this);
        }
    }


    public partial interface IExpressionVisitor {
        void Visit(NewArrayExpression expression);
    }

    public partial class ExpressionVisitor {

        protected virtual void VisitNewArrayExpression(NewArrayExpression expression) {
            DefaultVisit(expression);
        }

        void IExpressionVisitor.Visit(NewArrayExpression expression) {
            VisitNewArrayExpression(expression);
        }
    }

    public partial interface IExpressionVisitor<TArgument, TResult> {
        TResult Visit(NewArrayExpression expression, TArgument argument);
    }

    public partial class ExpressionVisitor<TArgument, TResult> {

        protected virtual TResult VisitNewArrayExpression(NewArrayExpression expression, TArgument argument) {
            return DefaultVisit(expression, argument);
        }

        TResult IExpressionVisitor<TArgument,TResult>.Visit(NewArrayExpression expression, TArgument argument) {
            return VisitNewArrayExpression(expression, argument);
        }
    }

    public partial interface IExpressionVisitor<TResult> {
        TResult Visit(NewArrayExpression expression);
    }

    public partial class ExpressionVisitor<TResult> {

        protected virtual TResult VisitNewArrayExpression(NewArrayExpression expression) {
            return DefaultVisit(expression);
        }

        TResult IExpressionVisitor<TResult>.Visit(NewArrayExpression expression) {
            return VisitNewArrayExpression(expression);
        }

    }

    public partial class NewArrayExpression {

        public override ExpressionType ExpressionType {
            get {
                return ExpressionType.NewArray;
            }
        }

        protected internal override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this);
        }

        protected internal override TResult AcceptVisitor<TArgument, TResult>(IExpressionVisitor<TArgument, TResult> visitor, TArgument argument) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this, argument);
        }

        protected internal override void AcceptVisitor(IExpressionVisitor visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            visitor.Visit(this);
        }
    }


    public partial interface IExpressionVisitor {
        void Visit(LambdaExpression expression);
    }

    public partial class ExpressionVisitor {

        protected virtual void VisitLambdaExpression(LambdaExpression expression) {
            DefaultVisit(expression);
        }

        void IExpressionVisitor.Visit(LambdaExpression expression) {
            VisitLambdaExpression(expression);
        }
    }

    public partial interface IExpressionVisitor<TArgument, TResult> {
        TResult Visit(LambdaExpression expression, TArgument argument);
    }

    public partial class ExpressionVisitor<TArgument, TResult> {

        protected virtual TResult VisitLambdaExpression(LambdaExpression expression, TArgument argument) {
            return DefaultVisit(expression, argument);
        }

        TResult IExpressionVisitor<TArgument,TResult>.Visit(LambdaExpression expression, TArgument argument) {
            return VisitLambdaExpression(expression, argument);
        }
    }

    public partial interface IExpressionVisitor<TResult> {
        TResult Visit(LambdaExpression expression);
    }

    public partial class ExpressionVisitor<TResult> {

        protected virtual TResult VisitLambdaExpression(LambdaExpression expression) {
            return DefaultVisit(expression);
        }

        TResult IExpressionVisitor<TResult>.Visit(LambdaExpression expression) {
            return VisitLambdaExpression(expression);
        }

    }

    public partial class LambdaExpression {

        public override ExpressionType ExpressionType {
            get {
                return ExpressionType.Lambda;
            }
        }

        protected internal override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this);
        }

        protected internal override TResult AcceptVisitor<TArgument, TResult>(IExpressionVisitor<TArgument, TResult> visitor, TArgument argument) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            return visitor.Visit(this, argument);
        }

        protected internal override void AcceptVisitor(IExpressionVisitor visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            visitor.Visit(this);
        }
    }


 }
