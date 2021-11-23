using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Terminal
{
    class ConsoleHandler
    {
        private string wordToHighlight = string.Empty;
        private char regexType = ' ';
        public void Output(string output, ConsoleColor fColour)
        {
            Console.ForegroundColor = fColour;
            if (wordToHighlight != string.Empty && wordToHighlight != null && (output.Contains(wordToHighlight) || (output.ToLower().Contains(wordToHighlight.ToLower()) && regexType == 'R')))
            {
                string[] splitOutput;
                if (regexType == 'R')
                {
                    splitOutput = Regex.Split(output, wordToHighlight, RegexOptions.IgnoreCase);
                }
                else
                {
                    splitOutput = output.Split(new string[] { wordToHighlight }, StringSplitOptions.None);
                }
                for (int i = 0; i < splitOutput.Length - 1; i++)
                {
                    Console.Write(splitOutput[i]);
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(wordToHighlight);
                    Console.ResetColor();
                }
                Console.Write(splitOutput[splitOutput.Length - 1]);
            }
            else
            {
                Console.Write(output);
                Console.ResetColor();
            }
        }
        public void HighlightWord(string wordToHighlight, char regexType)
        {
            this.wordToHighlight = wordToHighlight;
            this.regexType = regexType;
        }
    }
}
