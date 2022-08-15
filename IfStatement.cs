using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class IfStatement : StatetmentBase
    {
        public Expression Term { get; private set; }
        public List<StatetmentBase> DoIfTrue { get; private set; }
        public List<StatetmentBase> DoIfFalse { get; private set; }

        public override void Parse(TokensStack sTokens)
        {
            DoIfFalse=new List<StatetmentBase>();
            DoIfTrue=new List<StatetmentBase>();
            //First, we remove the "if" token
            Token tIf = sTokens.Pop();//if
             if (!(tIf is Statement) || ((Statement)tIf).Name != "if")
                throw new SyntaxErrorException("Expected function received: " + tIf, tIf);
             Token par = sTokens.Pop();//;
              if(!(par is Parentheses) || ((Parentheses)par).Name!='(')
                throw new SyntaxErrorException("Expected function name, received " + par, par);
            //Now, we create the correct Expression type based on the top token in the stack
            Term = Expression.Create(sTokens);
            //We transfer responsibility of the parsing to the created expression
            Term.Parse(sTokens);
            Token p = sTokens.Pop();//;
              if(!(p is Parentheses) || ((Parentheses)p).Name!=')')
                throw new SyntaxErrorException("Expected if name, received ", p);
            Token param = sTokens.Pop();//;
              if(!(param is Parentheses) || ((Parentheses)param).Name!='{')
                throw new SyntaxErrorException("Expected if name, received " , param);
              while (sTokens.Count > 0 && !(sTokens.Peek() is Parentheses))
            {
                //We create the correct Statement type (if, while, return, let) based on the top token in the stack
                StatetmentBase s = StatetmentBase.Create(sTokens.Peek());
                //And call the Parse method of the statement to parse the different parts of the statement 
                s.Parse(sTokens);
                DoIfTrue.Add(s);
            }
              Token pa = sTokens.Pop();//;
              if(!(pa is Parentheses) || ((Parentheses)pa).Name!='}')
                throw new SyntaxErrorException("Expected if name, received " , pa);
            if(sTokens.Count > 0 && (sTokens.Peek() is Statement) && ((Statement)sTokens.Peek()).Name=="else")
            {
                Token tElse=sTokens.Pop(); // else
                Token par2 = sTokens.Pop();//{
                 if(!(par2 is Parentheses) || ((Parentheses)par2).Name!='{')
                throw new SyntaxErrorException("Expected if name, received " + par2, par2);
                 while (sTokens.Count > 0 && !(sTokens.Peek() is Parentheses))
            {
                //We create the correct Statement type (if, while, return, let) based on the top token in the stack
                StatetmentBase s = StatetmentBase.Create(sTokens.Peek());
                //And call the Parse method of the statement to parse the different parts of the statement 
                s.Parse(sTokens);
                DoIfFalse.Add(s);
            }
                 Token para = sTokens.Pop();//}
                 if(!(para is Parentheses) || (((Parentheses)para).Name!='}'))
                throw new SyntaxErrorException("Expected if name, received ", para);

            }
            
        }

        public override string ToString()
        {
            string sIf = "if(" + Term + "){\n";
            foreach (StatetmentBase s in DoIfTrue)
                sIf += "\t\t\t" + s + "\n";
            sIf += "\t\t}";
            if (DoIfFalse.Count > 0)
            {
                sIf += "else{";
                foreach (StatetmentBase s in DoIfFalse)
                    sIf += "\t\t\t" + s + "\n";
                sIf += "\t\t}";
            }
            return sIf;
        }

    }
}
