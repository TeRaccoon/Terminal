using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal
{
    class ConsoleHandler
    {
        public void Output(string output, ConsoleColor fColour)
        {
            Console.ForegroundColor = fColour;
            Console.Write(output);
            Console.ResetColor();
        }
    }
}
