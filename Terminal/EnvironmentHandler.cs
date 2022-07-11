using System;
using System.Threading;

namespace Terminal
{
    class EnvironmentHandler
    {
        ConsoleHandler consoleHandler = new ConsoleHandler();
        public string WhoAmI()
        {
            string output = "\nMachine Name: " + Environment.MachineName;
            output += "\nUser Name: " + Environment.UserName;
            output += "\nUser Domain Name: " + Environment.UserDomainName;
            output += "\nOS Version: " + Environment.OSVersion;
            output += "\nProcessor Count: " + Environment.ProcessorCount;

            return output + "\n";
        }

        public void CPUUsage(ResourceHandler resourceHandler, int pollingRate) //Outputs the CPU usage to the console
        {
            ThreadHandler threadHandler = new ThreadHandler();
            Thread inputBreaker = new Thread(threadHandler.InputBreaker);
            inputBreaker.Start(); //Starts a new thread to read input to stop the method
            while (!threadHandler.GetBreakRequest()) //While the new thread hasn't detected any input
            {
                int cpuUsage = Convert.ToInt32(Math.Round(Convert.ToDouble(resourceHandler.GetCPUUsage()), 0)); //Converts the CPU usage to into a rounded value
                consoleHandler.Output("CPU Usage: " + cpuUsage + "%\n", ConsoleColor.White);
                consoleHandler.OutputAsBar(cpuUsage); //Outputs the data as a progress bar
                Thread.Sleep(pollingRate);
                Console.Clear();
            }
            inputBreaker.Abort(); //Aborts new thread
        }
        public void RAMUsage(ResourceHandler resourceHandler, int pollingRate)
        {
            ThreadHandler threadHandler = new ThreadHandler();
            Thread inputBreaker = new Thread(threadHandler.InputBreaker);
            inputBreaker.Start();
            while (!threadHandler.GetBreakRequest())
            {
                int ramUsage = Convert.ToInt32(Math.Round(Convert.ToDouble(resourceHandler.GetRAMUsage()), 0));
                consoleHandler.Output("RAM Usage: " + ramUsage + "%\n", ConsoleColor.White);
                consoleHandler.OutputAsBar(ramUsage);
                Thread.Sleep(pollingRate);
                Console.Clear();
            }
            inputBreaker.Abort();
        }
    }
}
