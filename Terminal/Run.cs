using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal
{
    class Run
    {
        FileHandler fileHandler = new FileHandler();
        ConsoleHandler consoleHandler = new ConsoleHandler();
        public Run()
        {
            InputHandler inputHandler = new InputHandler(fileHandler, consoleHandler);
            string output = string.Empty;
            while (output != "exit")
            {
                Console.Write(fileHandler.GetCurrentDirectory() + " > ");
                List<string> inputData = inputHandler.SplitInput(Console.ReadLine());
                List<string> flagArgumentData = inputData;
                string command = inputData[0];
                flagArgumentData.RemoveAt(0);
                output = inputHandler.ManageCommand(command, flagArgumentData);
                consoleHandler.Output(output + "\n", ConsoleColor.White);
            }
        }
    }
}
