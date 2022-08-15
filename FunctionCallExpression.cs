using System;
using System.Collections.Generic;

namespace SimpleCompiler
{
    public class FunctionCallExpression : Expression
    {
        public string FunctionName { get; private set; }
        public List<Expression> Args { get; private set; }
        public Expression tmp { get; private set; }
        public override void Parse(TokensStack sTokens)
        {
            Args=new List<Expression>();
            Token tName = sTokens.Pop();
            //|| !Program.isContain( ((Identifier)tName).Name)
            if(!(tName is Identifier) )
                throw new SyntaxErrorException("Expected functionCall name, received " , tName);
            this.FunctionName = ((Identifier)tName).Name;
            Token t = sTokens.Pop(); //(
            if(!(t is Parentheses) || ((Parentheses)t).Name!='(')
                throw new SyntaxErrorException("Expected functionCall par, received ", t);
            //Now we extract the arguments from the stack until we see a closing parathesis

            while(sTokens.Count > 0)//)
            {
                //For each argument there should be a type, and a name
                tmp = Expression.Create(sTokens);
            //We transfer responsibility of the parsing to the created expression
                if(tmp==null)
                    throw new SyntaxErrorException("Expected statement recivied",null);
                tmp.Parse(sTokens);
                Args.Add(tmp);
                //If there is a comma, then there is another argument
                if (sTokens.Count > 0 && sTokens.Peek() is Separator)//,
                { 
                    if( ((Separator)(sTokens.Peek())).Name!=',')
                        throw new SyntaxErrorException("Expected FunctionCall comma recieved" + sTokens.Peek(),sTokens.Peek());
                    sTokens.Pop(); 

                }
                if(sTokens.Peek() is Parentheses && sTokens.Peek(1) is Parentheses && sTokens.Peek(2) is Parentheses && sTokens.Peek(3) is Separator && (((Separator)(sTokens.Peek(3))).Name==';' ))
                    break;
                  if(sTokens.Peek() is Parentheses && sTokens.Peek(1) is Parentheses && sTokens.Peek(2) is Separator &&( ((Separator)(sTokens.Peek(2))).Name==';' ))
                    break;
                if(sTokens.Peek() is Parentheses && sTokens.Peek(1) is Separator &&( ((Separator)(sTokens.Peek(1))).Name==';' ))
                    break;
            }
            Token par = sTokens.Pop(); //)
            if(!(par is Parentheses) || ((Parentheses)par).Name!=')')
                throw new SyntaxErrorException("Expected functionCall par, received " + par , par);
        }

        public override string ToString()
        {
            string sFunction = FunctionName + "(";
            for (int i = 0; i < Args.Count - 1; i++)
                sFunction += Args[i] + ",";
            if (Args.Count > 0)
                sFunction += Args[Args.Count - 1];
            sFunction += ")";
            return sFunction;
        }
    }
}