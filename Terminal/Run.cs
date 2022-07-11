using System;
using System.Collections.Generic;

namespace Terminal
{
    class Run
    {
        FileHandler fileHandler = new FileHandler();
        ConsoleHandler consoleHandler = new ConsoleHandler();
        public Run()
        {
            InputHandler inputHandler = new InputHandler(fileHandler, consoleHandler);
            ThreadHandler threadHandler = new ThreadHandler();
            string output = string.Empty;
            threadHandler.StartKeyOverride();
            while (output != "exit")
            {
                Console.Write(fileHandler.GetCurrentDirectory() + " > ");
                
                List<string> inputData = inputHandler.SplitInput(Console.ReadLine());
                //threadHandler.EndKeyOverride();
                List<string> flagArgumentData = inputData;
                string command = inputData[0];
                flagArgumentData.RemoveAt(0);
                output = inputHandler.ManageCommand(command, flagArgumentData);
                consoleHandler.Output(output + "\n", ConsoleColor.White);
            }
        }
    }
}
