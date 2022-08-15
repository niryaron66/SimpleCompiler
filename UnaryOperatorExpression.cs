using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    class UnaryOperatorExpression : Expression
    {
        public string Operator { get; set; }
        public Expression Operand { get; set; }

        public override string ToString()
        {
            return Operator + Operand;
        }

        public override void Parse(TokensStack sTokens)
        {

            
            Token oper=sTokens.Pop();
            if(!(oper is Operator) || ( (((Operator)oper).Name!='!') &&  (((Operator)oper).Name!='-')))
                 throw new SyntaxErrorException("Expected UnaryOpeEXP operator, received " + oper, oper);
            this.Operator = ((Operator)oper).Name + "";
             this.Operand = Expression.Create(sTokens);
            //We transfer responsibility of the parsing to the created expression
            this.Operand.Parse(sTokens);



        }
    }
}
