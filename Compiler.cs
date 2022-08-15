using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class Compiler
    {

        private Dictionary<string, int> m_dSymbolTable;
        private int m_cLocals;

        public Compiler()
        {
            m_dSymbolTable = new Dictionary<string, int>();
            m_cLocals = 0;

        }

        public List<string> Compile(string sInputFile)
        {
            List<string> lCodeLines = ReadFile(sInputFile);
            List<Token> lTokens = Tokenize(lCodeLines);
            TokensStack sTokens = new TokensStack();
            for (int i = lTokens.Count - 1; i >= 0; i--)
                sTokens.Push(lTokens[i]);
            JackProgram program = Parse(sTokens);
            return null;
        }

        private JackProgram Parse(TokensStack sTokens)
        {
            JackProgram program = new JackProgram();
            program.Parse(sTokens);
            return program;
        }

        public List<string> Compile(List<string> lLines)
        {

            List<string> lCompiledCode = new List<string>();
            foreach (string sExpression in lLines)
            {
                List<string> lAssembly = Compile(sExpression);
                lCompiledCode.Add("// " + sExpression);
                lCompiledCode.AddRange(lAssembly);
            }
            return lCompiledCode;
        }



        public List<string> ReadFile(string sFileName)
        {
            StreamReader sr = new StreamReader(sFileName);
            List<string> lCodeLines = new List<string>();
            while (!sr.EndOfStream)
            {
                lCodeLines.Add(sr.ReadLine());
            }
            sr.Close();
            return lCodeLines;
        }
         private List<string> Split(string s, char[] aDelimiters)
        {
            List<string> lTokens = new List<string>();
            while (s.Length > 0)
            {
                string sToken = "";
                int i = 0;
                for (i = 0; i < s.Length; i++)
                {
                    if (aDelimiters.Contains(s[i]))
                    {
                        if (sToken.Length > 0)
                            lTokens.Add(sToken);
                        lTokens.Add(s[i] + "");
                        break;
                    }
                    else
                        sToken += s[i];
                }
                if (i == s.Length)
                {
                    lTokens.Add(sToken);
                    s = "";
                }
                else
                    s = s.Substring(i + 1);
            }
            return lTokens;
        }

        public List<Token> Tokenize(List<string> lCodeLines)
        {
   
            char[] tokenim =
{ '*', '+', '-', '/', '<', '>', '&', '=', '|', '~','(', ')', '[', ']', '{', '}', ',', ';' ,'\t',' '};

            List<Token> lTokens = new List<Token>();
            for(int i=0;i<lCodeLines.Count;i++)
            {
                string thisLine = lCodeLines[i];
                if(thisLine.Length==0)
                    continue;
                //this is no parthe line
                if(thisLine[0]=='/' && thisLine[1]=='/')
                    continue;
                if(thisLine.Length!=1)
                {
                    //this is no eara
                    if(thisLine[0] != '/' && thisLine[1] != '/')
                    {
                        if (thisLine[0] == '\t')
                            thisLine = thisLine.Substring(1);
                        List<string> words = Split(lCodeLines[i], tokenim);
                        int charCounter = 0;
                        for (int j = 0; j < words.Count; j++)
                        {
                            charCounter += words[j].Length;
                            if (Token.Statements.Contains(words[j]))
                            {
                                int x = 0;
                                if (charCounter - words[j].Length-1 < 0)
                                    x = 0;
                                else
                                    x = charCounter - words[j].Length;
                                Statement t = new Statement(words[j], i, x);
                                lTokens.Add(t);
                            }

                            else if (Token.VarTypes.Contains(words[j]))
                            {
                                int x = 0;
                                if (charCounter - words[j].Length-1 < 0)
                                    x = 0;
                                else
                                    x = charCounter - words[j].Length;
                                VarType t = new VarType(words[j], i, x);
                                lTokens.Add(t);
                            }
                            else if (Token.Constants.Contains(words[j]))
                            {
                                int x = 0;
                                if (charCounter - words[j].Length-1 < 0)
                                    x = 0;
                                else
                                    x = charCounter - words[j].Length;
                                Constant t = new Constant(words[j], i, x);
                                lTokens.Add(t);
                            }
                            else if (Token.Parentheses.Contains(words[j][0]))
                            {
                                Parentheses t = new Parentheses(words[j][0], i, charCounter-1);
                                lTokens.Add(t);
                            }
                            else if (Token.Operators.Contains(words[j][0]))
                            {
                                if (words[j][0] == '/' && words[j + 1][0] == '/')
                                    break;
                                else if (j > 0)
                                {
                                    if (words[j - 1][0] == '/' && words[j][0] == '/')
                                        break;
                                }
                                Operator t = new Operator(words[j][0], i, charCounter-1);
                                lTokens.Add(t);
                            }
                            else if (Token.Separators.Contains(words[j][0]))
                            {
                                Separator t = new Separator(words[j][0], i, charCounter-1);
                                lTokens.Add(t);
                            }
                            else if (words[j][0] >= 'a' && words[j][0] <= 'z' || words[j][0] >= 'A' && words[j][0] <= 'Z')
                            {
                                if (words[j].Length > 1)
                                {
                                    int position = charCounter - words[j].Length ;
                                    Identifier t = new Identifier(words[j], i, position);
                                    lTokens.Add(t);
                                }
                                else
                                {
                                    Identifier t = new Identifier(words[j], i, charCounter-1);
                                    lTokens.Add(t);
                                }
                            }
                            else if (words[j][0] >= '0' && words[j][0] <= '9')
                            {
                                if (int.TryParse(words[j], out int number) == true)
                                {
                                    number = int.Parse(words[j]);
                                    int position = charCounter - words[j].Length + 1;
                                    Number t = new Number(words[j], i, position-1);
                                    lTokens.Add(t);
                                }
                                else
                                {
                                    int pos = charCounter - words[j].Length + 1;
                                    Number num= new Number("111", i, pos);
                                    throw new SyntaxErrorException("not real number",num);
                                }
                            
                            }


                            }
                        }
                    }
                else
                {
                    
                    if (Token.Parentheses.Contains(thisLine[0]))
                    {
                        Parentheses t = new Parentheses(thisLine[0], i, 0);
                        lTokens.Add(t);
                    }
     
                    else if (Token.Operators.Contains(thisLine[0]))
                    {

                        Operator t = new Operator(thisLine[0], i, 0);
                        lTokens.Add(t);
                    }
                    else if (Token.Separators.Contains(thisLine[0]))
                    {
                        Separator t = new Separator(thisLine[0], i, 0);
                        lTokens.Add(t);
                    }
                }

            }

            return lTokens;
        }

    }
        }

    
