using System;
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

        public void OutputAsBar(int data)
        {
            int originalData = data;
            data = Convert.ToInt32(data / 25);
            switch (data)
            {
                case 3:
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;

                case 2:
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;

                case 1:
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;

                case 0:
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;
            }
            for (int i = -1; i < Convert.ToInt32(originalData / 4); i++)
            {
                Console.Write(" ");
            }
            Console.ResetColor();
        }
    }
}
