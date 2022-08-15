using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class VarDeclaration : JackProgramElement
    {

        public enum VarTypeEnum { Int, Array, Char, Bool, Invalid };
        public VarTypeEnum Type { get; private set; }
        public List<string> Names { get; private set; }

        public string Name { 
            get 
            {
                if (Names.Count != 1)
                    throw new SyntaxErrorException("Incorrect number of names for var declaration", null);
                return Names[0]; 
            } 
        }


        public VarDeclaration()
        {
            Names = new List<string>();
        }

        public VarDeclaration(Token tType, Token tName)
        {
            Names = new List<string>();
            SetType(tType);
            AddName(tName);
        }


        private void AddName(Token tName)
        {
            if (!(tName is Identifier))
                throw new SyntaxErrorException("Expected var name, received " + tName, tName);
            string sName = ((Identifier)tName).Name;
            Names.Add(sName);
        }

        private void SetType(Token tType)
        {
            if (!(tType is VarType))
            {
                throw new SyntaxErrorException("Expected var type, received " + tType, tType);
            }
            Type = GetVarType(tType);
            if (Type == VarTypeEnum.Invalid)
                throw new SyntaxErrorException("Expected var type, received " + tType, tType);
            
        }

        public static VarTypeEnum GetVarType(Token t)
        {
            if (t is VarType)
                return GetVarType(((VarType)t).Name);
            return VarTypeEnum.Invalid;
        }

        public static VarTypeEnum GetVarType(string sName)
        {
            if (sName == "int")
                return VarTypeEnum.Int;
            if (sName == "char")
                return VarTypeEnum.Char;
            if (sName == "array")
                return VarTypeEnum.Array;
            if (sName == "bool")
                return VarTypeEnum.Bool;
            return VarTypeEnum.Invalid;
        }

        public override void Parse(TokensStack sTokens)
        {
            Token tVar = sTokens.Pop();
            if (!(tVar is Statement) || (((Statement)tVar).Name != "var"))
                throw new SyntaxErrorException("Expected var, received " + tVar, tVar);
            Token tType = sTokens.Pop();
            if(!(tType is VarType))
                 throw new SyntaxErrorException("Expected varType, received " + tType, tType);
            SetType(tType);

            Token tEnd = null;
            do
            {
                Token tName = sTokens.Pop();
                AddName(tName);
                tEnd = sTokens.Pop();
            }
            while (tEnd.ToString() == ",");
            
            if(!(tEnd is Separator) || ((Separator)tEnd).Name!=';')
                throw new SyntaxErrorException("Expected ; received " + tEnd, tEnd);

        }

        public override string ToString()
        {
            string s = "var " + Type;
            for(int i = 0; i < Names.Count - 1; i++)
            {
                s += " " + Names[i] + ",";
            }
            s += " " + Names.Last() + ";";
            return s;
        }
    }
}
