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
    class Program
    {
        static private String inputFile;
        static private String outputString;
        static private Parser parser = new Parser();

        static void Main()
        {
            while(true)
            {
                //some nice heading, then the real program begins
                Console.WriteLine("Parser");
                Console.WriteLine("Enter the name for a file obtaining code below.");
                Console.WriteLine("Press \"Enter\" to obtain feedback and enter \"Quit\" to stop.");
                Console.WriteLine("--------------------------------------------------------------------------------");

                //get input
                inputFile = Console.ReadLine();

                //if the input was "quit" we want to exit the parser
                if (inputFile.ToLower() == "quit")
                {
                    break;
                }
                else //make sure the file entered is found
                {
                    if (!File.Exists(inputFile))
                    {
                        //the file does not exist, 
                        outputString = "That file does not exist. Please try again.";
                    }
                    else
                    {
                        //run the parser
                        outputString = parser.Run(inputFile);
                    }
                }

                //print parser results
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("Output:");
                Console.WriteLine(outputString);
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
