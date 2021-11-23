using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Terminal
{
    class ProcessHandler
    {
        DataHandler dataHandler = new DataHandler();
        public string ListProcesses(string regexString, bool clean)
        {
            char regexChar = 'x';
            if (regexString != "noRegex")
            {
                regexChar = regexString[0];
                regexString = regexString.Substring(2);
                regexString = regexString.Trim();
            }
            Process[] processList = Process.GetProcesses();
            if (clean)
            {
                List<Process> cleanedProcessList = new List<Process>();
                List<string> seenProcesses = new List<string>();
                for (int i = 0; i < processList.Length; i++)
                {
                    if (!seenProcesses.Contains(processList[i].ProcessName))
                    {
                        seenProcesses.Add(processList[i].ProcessName);
                    }
                }
                seenProcesses.Sort();
                foreach (string processName in seenProcesses)
                {
                    cleanedProcessList.Add(Process.GetProcessesByName(processName)[0]);
                }
                processList = cleanedProcessList.ToArray();
            }
            List<string> output = new List<string>();
            foreach (Process process in processList)
            {
                output.Add(String.Format("|  {0,-52}  |  {1,10}  |", process.ProcessName, process.Id));
                output.Add("-------------------------------------------------------------------------");
            }
            return dataHandler.RegexString(dataHandler.AddNewLine(output.ToArray()), regexString, regexChar);
        }
        public string KillAll(string processName)
        {
            bool killed = false;
            Process[] processlist = Process.GetProcesses();
            foreach (Process process in processlist)
            {
                if (process.ProcessName == processName)
                {
                    process.Kill();
                    killed = true;
                }
            }
            if (killed)
            {
                return "Processes with the name " + processName + " killed successfully!";
            }
            return "No process with the name " + processName + " was found!";
        }

        public string Kill(int processID)
        {
            bool killed = false;
            Process[] processlist = Process.GetProcesses();
            foreach (Process process in processlist)
            {
                if (process.Id == processID)
                {
                    process.Kill();
                    killed = true;
                }
            }
            if (killed)
            {
                return "Processes with the ID " + processID + " killed successfully!";
            }
            return "No process with the name " + processID + " was found!";
        }
    }
}
