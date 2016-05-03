using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommandLineParser
{
    class Parser
    {
        String[] enteredCode; //The scrubbed data once it has been entered, line by line
        String[] validTokens; //the valid tokens in the code system
        SortedList errors; //the errors of the code system
        List<Tokenizer.Token> tokens = new List<Tokenizer.Token>(); //The current tokens being processed
        Tokenizer tokenizer = new Tokenizer(); //the tokenizer for the parser
        Stack parenthesisChecker = new Stack(); //used to make sure there are a matching number of parenthesis
        String outputString = ""; //the string that will be returned by the "Run" method
        String text = ""; //the input from the file

        public Parser()
        {
            /*****************************************************
            * This is hardcoded at run-time, so it never changes *
            *****************************************************/

            //Construct the errors Hashtable
            errors = new SortedList();
            errors.Add(101, "(Error 1) Invalid identifier found. Only integers or identifiers \nbeginning with \"a-z\", \"A-Z\", and \"_\" are accepted.");
            errors.Add(102, "(Error 2) Parenthesis must have matching pairs and they \ncannot contain nothing.");
            errors.Add(103, "(Error 3) The assignment statement must have an expression \nfollowing the \":=\".");
            errors.Add(104, "(Error 4) Statements cannot have anything after the delimiter \n\";\" on the same line.");
            errors.Add(105, "(Error 5) READ can only a single Identifier or Identifiers \nseparated with commas within it.");
            errors.Add(106, "(Error 6) WRITE can only have valid expressions within it \n(Ex: 3 - (x + 4)).");
            errors.Add(109, "(Error 9) Statements must end with a \";\".");
            errors.Add(110, "(Error 10) Code must start with \"BEGIN\", end with \"END\", and \nhave at least one statement (It is not case sensitive).");
            errors.Add(111, "(Error 11) Unkown Error.");
            errors.Add(112, "(Error 12) Statements must begin with \"READ\", \"WRITE\", or an \nIdentifier. (It is not case sensitive).");

            //Construct the validTokens array (mostly reserved things)
            validTokens = new String[]{ "begin",
                                        "write",
                                        "read",
                                        "end",
                                        ":=",
                                        "+",
                                        "-",
                                        ";",
                                        ",",
                                        "(",
                                        ")"
                                      };
        }

        public String Run(String fileName)
        {
            //We know that the file exists, so read in all of the text
            text = System.IO.File.ReadAllText(fileName);

            //run only if there is code to run
            if (text.Length > 0)
            {
                //Scrub the code in the codeTextBox and store it
                ScrubCode();

                try
                {
                    //If code line number is below amount needed, throw an error
                    if (enteredCode.Length < 3)
                    {
                        throw (new ParserException(1, 110));
                    }

                    //Next retrive all of the tokens from the data
                    for (int i = 0; i < enteredCode.Length; i++)
                    {
                        tokens.AddRange(tokenizer.CreateTokens(enteredCode[i], i + 1, validTokens));
                    }

                    //Now with all of the tokens, we want to parse through and ensure everything is dandy, otherwise throw errors
                    ParseCode();
                }
                catch (ParserException ex)
                {
                    //print the error and we are good to go
                    outputString = "ERROR at line " + ex.errorLine + ": " + errors.GetByIndex(errors.IndexOfKey(ex.errorCode));
                    parenthesisChecker.Clear();
                    tokens.Clear();
                    return outputString;
                }

                //we're done, so print the success
                outputString = "SUCCESS! Code has ran smoothly.";
                parenthesisChecker.Clear();
                tokens.Clear();
                return outputString;
            }
            else
            {
                outputString = "The file is empty.";
                return outputString;
            }
        }

        private void ScrubCode()
        {
            //first, split the code into lines
            String[] textArray = text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            
            //if there are empty spots in the array, remove them
            List<String> removeList = textArray.ToList();
            while (removeList.Contains("") || removeList.Contains(" "))
            {
                removeList.Remove("");
                removeList.Remove(" ");
            }
            textArray = removeList.ToArray();

            //next, remove all white space and make everything lowercase for easy handling
            for (int i = 0; i < textArray.Length; i++)
            {
                textArray[i] = textArray[i].Replace(" ", "").ToLower();
            }

            enteredCode = textArray;
        }

        private void ParseCode()
        {
            //for easy handling, give the tokens data to an array
            Tokenizer.Token[] tokenArray = tokens.ToArray();

            //From here, everything is a statement, until we hit the "tail" token
            int i = 1;
            Tokenizer.Token currentToken = tokenArray[i];
            while (currentToken.type != "tail")
            {
                //get the number of tokens in this statement
                int j = i + 1;
                while (tokenArray[i].codeLine == tokenArray[j].codeLine)
                {
                    j++;
                }

                //once we have the length of this statement(j-i), make sure it is a valid statement
                if (tokenArray[i].type != "method" && tokenArray[i].type != "identifier")
                {
                    throw (new ParserException(tokens[i].codeLine, 112));
                }
                if (tokenArray[j - 1].type != "delimiter")
                {
                    throw (new ParserException(tokens[i].codeLine, 109));
                }

                //also make sure it has a matching number of parethesis
                try
                {
                    for (int k = i; tokenArray[k] != tokenArray[j - 1]; k++)
                    {
                        if (tokenArray[k].type == "leftP")
                        {
                            parenthesisChecker.Push(1);
                        }
                        if (tokenArray[k].type == "rightP")
                        {
                            parenthesisChecker.Pop();
                        }

                        //also, if at any point we hit an empty set of parenthesis, throw that error
                        if (tokenArray[k].type == "leftP" && tokenArray[k+1].type == "rightP")
                        {
                            throw (new Exception());
                        }
                    }

                    if (parenthesisChecker.Count != 0) //if something is on the stack, that means there is more leftP than rightP
                    {
                        throw (new Exception());
                    }
                }
                catch(Exception exc) //if an exception is cought, that means something is wrong with parentesis.
                {
                    throw (new ParserException(tokens[i].codeLine, 102));
                }

                //the methods case
                if (tokenArray[i].type == "method")
                {
                    if (tokenArray[i].value == "read")
                    {
                        i++; //next token
                        if (tokenArray[i].type == "leftP")
                        {
                            i++; //next token

                            while (tokenArray[i].type == "identifier" && //current part
                                (tokenArray[i + 1].type == "separator" || tokenArray[i + 1].type == "rightP")) //look ahead
                            {
                                i += 2; //if you increment it by two, it will either land on the next identifier in case of a separator, or it will land on the delimiter, which is the end
                            }

                            //if we exit and the rightP is not i - 1, or we are not on the delimiter, or somthing is past the delimiter, we are having another read error
                            if (tokenArray[i].type != "delimiter" || tokenArray[i - 1].type != "rightP" || (tokenArray[i].type == "delimiter" && tokenArray[i].codeLine == tokenArray[i + 1].codeLine))
                            {
                                //if there was something past the delimiter, that is it's own error, otherwise it's the generic read error
                                if (tokenArray[i].type == "delimiter" && tokenArray[i].codeLine == tokenArray[i + 1].codeLine)
                                {
                                    throw (new ParserException(tokens[i].codeLine, 104));
                                }
                                else
                                {
                                    throw (new ParserException(tokens[i].codeLine, 105));
                                }
                            }
                        }
                        else //throw a read error
                        {
                            throw (new ParserException(tokens[i].codeLine, 105));
                        }
                    }
                    else //value == write
                    {
                        i++; //next token
                        if (tokenArray[i].type == "leftP")
                        {
                            i++; //next token

                            while ((tokenArray[i].type == "identifier" || tokenArray[i].type == "leftP" || tokenArray[i].type == "separator" || tokenArray[i].type == "number") && //current part
                                    ((tokenArray[i + 1].type == "identifier" || tokenArray[i + 1].type == "number" || tokenArray[i + 1].type == "separator" || tokenArray[i + 1].type == "operator" || tokenArray[i + 1].type == "rightP" || tokenArray[i + 1].type == "leftP")) || //look ahead
                                    (tokenArray[i].type == "operator" && tokenArray[i - 1].type == "rightP") || //case of operator following a rightP
                                    (tokenArray[i].type == "rightP" && tokenArray[i + 1].type != "delimiter")) //multiple rightP can cause this
                            {
                                if (tokenArray[i].type == "leftP" || tokenArray[i].type == "separator" || tokenArray[i].type == "operator")
                                {
                                    i++;
                                }
                                else
                                {
                                    i += 2;
                                }
                            }

                            //for the case of the ending being having multiple ")"
                            while (tokenArray[i].type == "rightP" && (tokenArray[i + 1].type == "delimiter" || tokenArray[i + 1].type == "rightP"))
                            {
                                i++;
                            }

                            //if we exit and the rightP is not i - 1, or we are not on the delimiter, we are having another write error
                            if (tokenArray[i].type != "delimiter" || tokenArray[i - 1].type != "rightP" || (tokenArray[i].type == "delimiter" && tokenArray[i].codeLine == tokenArray[i + 1].codeLine))
                            {
                                //if there was something past the delimiter, that is it's own error, otherwise it's the generic write error
                                if (tokenArray[i].type == "delimiter" && tokenArray[i].codeLine == tokenArray[i + 1].codeLine)
                                {
                                    throw (new ParserException(tokens[i].codeLine, 104));
                                }
                                else
                                {
                                    throw (new ParserException(tokens[i].codeLine, 106));
                                }
                            }
                        }
                        else //throw a write error
                        {
                            throw (new ParserException(tokens[i].codeLine, 106));
                        }
                    }
                }
                //the equals case
                else if (tokenArray[i].type == "identifier")
                {
                    i++;//next token

                    if (tokenArray[i].type == "equals")
                    {
                        i++; //next token

                        while ((tokenArray[i].type == "identifier" || tokenArray[i].type == "number" || tokenArray[i].type == "leftP") && //current part
                                    ((tokenArray[i + 1].type == "identifier" || tokenArray[i + 1].type == "number" || tokenArray[i + 1].type == "operator" || tokenArray[i + 1].type == "rightP" || tokenArray[i + 1].type == "leftP")) || //look ahead
                                    (tokenArray[i].type == "operator" && tokenArray[i - 1].type == "rightP") || //case of operator following a rightP
                                    (tokenArray[i].type == "rightP" && tokenArray[i + 1].type != "delimiter")) //multiple rightP can cause this
                        {
                            if (tokenArray[i].type == "leftP" || tokenArray[i].type == "operator" || tokenArray[i + 1].type == "delimiter")
                            {
                                i++;
                            }
                            else
                            {
                                i += 2;
                            }
                        }

                        //for the case of the ending being having multiple ")"
                        while (tokenArray[i].type == "rightP" && (tokenArray[i + 1].type == "delimiter" || tokenArray[i + 1].type == "rightP"))
                        {
                            i++;
                        }

                        //case if the assignment is one thing
                        if ((tokenArray[i].type == "identifier" || tokenArray[i].type == "number") && tokenArray[i + 1].type == "delimiter")
                        {
                            i++;
                        }

                        //if we exit and we are not on the delimiter, we are having another write error
                        if (tokenArray[i].type != "delimiter" || (tokenArray[i].type == "delimiter" && tokenArray[i].codeLine == tokenArray[i + 1].codeLine))
                        {
                            //if there was something past the delimiter, that is it's own error, otherwise it's the generic assignment error
                            if (tokenArray[i].type == "delimiter" && tokenArray[i].codeLine == tokenArray[i + 1].codeLine)
                            {
                                throw (new ParserException(tokens[i].codeLine, 104));
                            }
                            else
                            {
                                throw (new ParserException(tokens[i].codeLine, 103));
                            }
                        }
                    }
                    else //throw error
                    {
                        throw (new ParserException(tokens[i].codeLine, 103));
                    }
                }
                else
                {
                    throw (new ParserException(tokens[i].codeLine, 112));
                }

                i = j; //go to the next line of code
                currentToken = tokenArray[i];
            }
        }

        //An exception for the CreateTokens method
        public class ParserException : Exception
        {
            int _errorLine;
            int _errorCode;

            public ParserException(int errorLine, int errorCode)
            {
                _errorLine = errorLine;
                _errorCode = errorCode;
            }

            public int errorLine
            {
                get { return _errorLine; }
                set { _errorLine = value; }
            }

            public int errorCode
            {
                get { return _errorCode; }
                set { _errorCode = value; }
            }
        }
    }
}
