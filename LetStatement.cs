using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class LetStatement : StatetmentBase
    {
        public string Variable { get; set; }
        public Expression Value { get; set; }

        public override string ToString()
        {
            return "let " + Variable + " = " + Value + ";";
        }

        public override void Parse(TokensStack sTokens)
        {
              Token tLet = sTokens.Pop();//while
             if (!(tLet is Statement) || ((Statement)tLet).Name != "let")
                throw new SyntaxErrorException("Expected let received: " + tLet, tLet);
               //need to be ID
               Token id=sTokens.Pop();
            if(!(id is Identifier))
                throw new SyntaxErrorException("Expected let received: " + id, id);
            this.Variable=((Identifier)id).Name; 
            //need to be = 
            Token del=sTokens.Pop();
            if(!(del is Operator) || ((Operator)del).Name!='=')
                 throw new SyntaxErrorException("Expected let received: " + del, del);
            this.Value = Expression.Create(sTokens);
            //We transfer responsibility of the parsing to the created expression
            if(Value==null)
                    throw new SyntaxErrorException("Expected statement recivied",null);
            this.Value.Parse(sTokens);
            //After the expression was parsed, we expect to see ;
            Token tEnd = sTokens.Pop();//should be ;
            if(!(tEnd is Separator) || ((Separator)tEnd).Name!=';')
                throw new SyntaxErrorException("Expected let received: " + tEnd, tEnd);



        }

    }
}
