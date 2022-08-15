using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class BinaryOperationExpression : Expression
    {
        public string Operator { get;  set; }
        public Expression Operand1 { get;  set; }
        public Expression Operand2 { get;  set; }

        public override string ToString()
        {
            return "(" + Operand1 + " " + Operator + " " + Operand2 + ")";
        }

        public override void Parse(TokensStack sTokens)
        {
            Token par = sTokens.Pop();//(
              if(!(par is Parentheses) || ((Parentheses)par).Name!='(')
                throw new SyntaxErrorException("Expected binaryOperationExp par, received " + par, par);
              Operand1 = Expression.Create(sTokens);
            //We transfer responsibility of the parsing to the created expression
            Operand1.Parse(sTokens);
            Token oper=sTokens.Pop();
            if(!(oper is Operator) || (   (((Operator)oper).Name!='*') && (((Operator)oper).Name!='+') && (((Operator)oper).Name!='-') 
                && (((Operator)oper).Name!='/') && (((Operator)oper).Name!='<') && (((Operator)oper).Name!='>') && (((Operator)oper).Name!='=')))
                 throw new SyntaxErrorException("Expected binaryOperationExp operator, received " + oper, oper);
            this.Operator = (((Operator)oper).Name+"");
             Operand2 = Expression.Create(sTokens);
            //We transfer responsibility of the parsing to the created expression
            Operand2.Parse(sTokens);
            Token par2 = sTokens.Pop();//(
              if(!(par2 is Parentheses) || ((Parentheses)par2).Name!=')')
                throw new SyntaxErrorException("Expected binaryOperationExp par, received " + par2, par2);


        }
    }
}
