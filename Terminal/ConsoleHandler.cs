using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal
{
    class ConsoleHandler
    {
        private string wordToHighlight = string.Empty;
        public void Output(string output, ConsoleColor fColour)
        {
            Console.ForegroundColor = fColour;
            if (output.Contains(wordToHighlight) && wordToHighlight != string.Empty && wordToHighlight != null)
            {
                string[] splitOutput = output.Split(new string[] { wordToHighlight }, StringSplitOptions.None);
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
        public void HighlightWord(string wordToHighlight)
        {
            this.wordToHighlight = wordToHighlight;
        }
    }
}
