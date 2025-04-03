using System;
using System.Collections.Generic;

namespace BISAYA__
{
    internal abstract class Expr
    {
        public interface IVisitor<R>
        {
            R VisitBinaryExpr(Binary expr);
            R VisitGroupingExpr(Grouping expr);
            R VisitLiteralExpr(Literal expr);
            R VisitUnaryExpr(Unary expr);
        }

        internal class Binary : Expr
        {
            internal Binary(Expr left, Token operatorToken, Expr right)
            {
                this.left = left;
                this.operatorToken = operatorToken;
                this.right = right;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitBinaryExpr(this);
            }

            internal readonly Expr left;
            internal readonly Token operatorToken;
            internal readonly Expr right;
        }
        internal class Grouping : Expr
        {
            internal Grouping(Expr expression)
            {
                this.expression = expression;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitGroupingExpr(this);
            }

            internal readonly Expr expression;
        }
        internal class Literal : Expr
        {
            internal Literal(object value)
            {
                this.value = value;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitLiteralExpr(this);
            }

            internal readonly object value;
        }
        internal class Unary : Expr
        {
            internal Unary(Token operatorToken, Expr right)
            {
                this.operatorToken = operatorToken;
                this.right = right;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitUnaryExpr(this);
            }

            internal readonly Token operatorToken;
            internal readonly Expr right;
        }

        public abstract R Accept<R>(IVisitor<R> visitor);
    }
}
