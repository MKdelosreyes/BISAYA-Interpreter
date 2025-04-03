using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BISAYA__
{
    internal abstract class Expr
    {
        public class Binary : Expr
        {
            public Binary(Expr left, Token operatorToken, Expr right)
            {
                this.left = left;
                this.opertr = operatorToken;
                this.right = right;
            }

            public Expr left { get; }
            public Token opertr { get; }
            public Expr right { get; }
        }
    }
}
