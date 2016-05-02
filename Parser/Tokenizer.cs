using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Parser
{
    public class Tokenizer
    {
        List<Token> tokenList;

        public class Token
        {
            private String _type;
            private String _value;
            private int _codeLine;

            public Token(String type, String value, int codeLine)
            {
                _type = type;
                _value = value;
                _codeLine = codeLine;
            }

            public String type
            {
                get { return _type; }
                set { _type = value; }
            }

            public String value
            {
                get { return _value; }
                set { _value = value; }
            }

            public int codeLine
            {
                get { return _codeLine; }
                set { _codeLine = value; }
            }
        }

        public List<Token> CreateTokens(String code, int codeLine, String[] validTokens)
        {
            tokenList = new List<Token>();

            //if the code is > 0, it works
            if (code.Length > 0)
            {
                //everything that will not be a statement, identifier, or the head and tail
                List<String> delimiters = new List<String> { "+", "-", ":=", ";", ",", "(", ")" };
                String pattern = "(" + String.Join("|", delimiters.Select(d => Regex.Escape(d)).ToArray()) + ")";

                //first, split the code into lines with Regex, so we can include the delimiters listed above as well
                String[] partArray = Regex.Split(code, pattern);

                for (int i = 0; i < partArray.Length; i++)
                {
                    //going through some of the data, the empty string popped up some, so skip those
                    if (partArray[i] != "")
                    {
                        switch (partArray[i])
                        {
                            case "begin":
                                tokenList.Add(new Token("head", partArray[i], codeLine));
                                break;
                            case "write":
                                tokenList.Add(new Token("method", partArray[i], codeLine));
                                break;
                            case "read":
                                tokenList.Add(new Token("method", partArray[i], codeLine));
                                break;
                            case "end":
                                tokenList.Add(new Token("tail", partArray[i], codeLine));
                                break;
                            case ":=":
                                tokenList.Add(new Token("equals", partArray[i], codeLine));
                                break;
                            case "+":
                                tokenList.Add(new Token("operator", partArray[i], codeLine));
                                break;
                            case "-":
                                tokenList.Add(new Token("operator", partArray[i], codeLine));
                                break;
                            case ";":
                                tokenList.Add(new Token("delimiter", partArray[i], codeLine));
                                break;
                            case ",":
                                tokenList.Add(new Token("separator", partArray[i], codeLine));
                                break;
                            case "(":
                                tokenList.Add(new Token("leftP", partArray[i], codeLine));
                                break;
                            case ")":
                                tokenList.Add(new Token("rightP", partArray[i], codeLine));
                                break;
                            default: //this is a user defined identifier or an integer, do a little testing and either call it good, or throw an error

                                //first check for an integer
                                int x = 0;
                                bool isInt = int.TryParse(partArray[i], out x);
                                if (isInt)
                                {
                                    //this is an integer, so add token
                                    tokenList.Add(new Token("number", partArray[i], codeLine));
                                }
                                //this check is that the string does begin with a -> z, A -> Z, or a "_", if it does, it's good
                                //otherwise that's an error
                                else if (Regex.IsMatch((new String(new char[] { partArray[i][0] })), @"[a-zA-Z_]"))
                                {
                                    //the identifier begins correctly, so we need to also check that it only contains correct things
                                    if (Regex.IsMatch((new String(new char[] { partArray[i][0] })), @"[\da-zA-Z_]"))
                                    {
                                        tokenList.Add(new Token("identifier", partArray[i], codeLine));
                                    }
                                    else
                                    {
                                        throw (new ParserMain.ParserException(codeLine, 101));
                                    }
                                }
                                else
                                {
                                    throw (new ParserMain.ParserException(codeLine, 101));
                                }
                            break;
                        }
                    }
                }
            }
            return tokenList;
        }
    }
}
