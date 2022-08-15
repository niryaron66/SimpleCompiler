using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class WhileStatement : StatetmentBase
    {
        public Expression Term { get; private set; }
        public List<StatetmentBase> Body { get; private set; }

        public override void Parse(TokensStack sTokens)
        {
            Body=new List<StatetmentBase>();
              //First, we remove the "while" token
            Token tWhil = sTokens.Pop();//while
             if (!(tWhil is Statement) || ((Statement)tWhil).Name != "while")
                throw new SyntaxErrorException("Expected function received: " + tWhil, tWhil);
            //remove the parthenses
            Token par=sTokens.Pop();
            if(!(par is Parentheses) || ((Parentheses)par).Name!='(')
                throw new SyntaxErrorException("Expected function name, received " + par, par);
            //Now, we create the correct Expression type based on the top token in the stack
            this.Term = Expression.Create(sTokens);
            //We transfer responsibility of the parsing to the created expression
            this.Term.Parse(sTokens);
            //After the expression was parsed, we expect to see )
            Token par2 = sTokens.Pop();//;
              if(!(par2 is Parentheses) || ((Parentheses)par2).Name!=')')
                throw new SyntaxErrorException("Expected function name, received " , par2);
            Token par3 = sTokens.Pop();//;
              if(!(par3 is Parentheses) || ((Parentheses)par3).Name!='{')
                throw new SyntaxErrorException("Expected function name, received " , par3);
            while (sTokens.Count > 0 && !(sTokens.Peek() is Parentheses))
            {
                //We create the correct Statement type (if, while, return, let) based on the top token in the stack
                Token tmp=sTokens.Peek();
                StatetmentBase s = StatetmentBase.Create(sTokens.Peek());
                //And call the Parse method of the statement to parse the different parts of the statement 
                if(s==null)
                { 
                     throw new SyntaxErrorException("expected stateement recieved:",tmp);
                }
                s.Parse(sTokens);
                Body.Add(s);
               
            }
            Token par4 = sTokens.Pop();//;
              if(!(par4 is Parentheses) || ((Parentheses)par4).Name!='}')
                throw new SyntaxErrorException("Expected function name, received ", par4);
        }

        public override string ToString()
        {
            string sWhile = "while(" + Term + "){\n";
            foreach (StatetmentBase s in Body)
                sWhile += "\t\t\t" + s + "\n";
            sWhile += "\t\t}";
            return sWhile;
        }

    }
}
