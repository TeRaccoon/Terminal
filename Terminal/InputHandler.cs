using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal
{
    class InputHandler
    {
        private FileHandler fileHandler;
        private ConsoleHandler consoleHandler;
        private SecurityHandler securityHandler = new SecurityHandler();
        private ProcessHandler processHandler = new ProcessHandler();
        public InputHandler(FileHandler fileHandler, ConsoleHandler consoleHandler)
        {
            this.fileHandler = fileHandler;
            this.consoleHandler = consoleHandler;
        }
        public List<string> SplitInput(string input)
        {
            string command = input.Split(' ')[0];
            List<string> flags = input.Substring(command.Length).Split('-').ToList<string>();
            flags.Insert(0, command);
            for (int i = 0; i < flags.Count; i++)
            {
                flags[i] = flags[i].Trim();
            }
            return flags;
        }

        public string ManageCommand(string command, List<string> flags)
        {
            string output = string.Empty;
            switch(command.ToLower())
            {
                case "cd":
                    output = fileHandler.ChangeCurrentDirectory(flags[0]);
                    break;

                case "ls":
                    output = LS(flags);
                    break;

                case "out":
                    output = Out(flags);
                    break;

                case "make":
                    output = fileHandler.MakeFile(flags[0]);
                    break;

                case "del":
                    output = fileHandler.DeleteFile(flags[0]);
                    break;

                case "open":
                    output = fileHandler.OpenFile(flags[0]);
                    break;

                case "back":
                    output = fileHandler.BackDirectory();
                    break;

                case "pslist":
                    output = PSList(flags);
                    break;

                case "killall":
                    output = processHandler.KillAll(flags[0]);
                    break;

                case "kill":
                    output = Kill(flags[0]);
                    break;

                case "md5":
                    output = securityHandler.MD5Hash(fileHandler, flags[0]);
                    break;

                case "sha512":
                    output = securityHandler.SHA512Hash(fileHandler, flags[0]);
                    break;

                case "file":
                    output = fileHandler.AssessFile(flags[0]);
                    break;

                case "directory":
                    output = fileHandler.AssessDirectory(flags[0]);
                    break;

                case "clear":
                    Console.Clear();
                    output = null;
                    break;

                case "clr":
                    Console.Clear();
                    output = null;
                    break;

                case "help":
                    flags[0] = @"Manual.txt";
                    output = Out(flags);
                    break;

                case "suicide":
                    Environment.Exit(0);
                    break;
            }
            if (output == string.Empty)
            {
                return "Unknown Error. Usually caused by the developer making a mistake with coding and not setting a correct console ouput variable again >:^(";
            }
            flags.RemoveAt(0);
            foreach (string flag in flags)
            {
                switch (flag[0])
                {
                    case 'F':
                        if (output != null)
                        {
                            fileHandler.SetSaveOutput(flag.Substring(2));
                            output = fileHandler.SaveOutput(output);
                        }
                        break;
                }
            }
            return output;
        }

        public string Kill(string processID)
        {
            try
            {
                return processHandler.Kill(Convert.ToInt32(processID));
            }
            catch
            {
                return "Process ID was in the correct format!";
            }
        }
        public bool CheckFlags(int flagsRequired, List<string> flags)
        {
            for (int i = 0; i < flags.Count; i++)
            {
                if ((flags[i] == null || flags[i] == "") && i < flagsRequired)
                {
                    return false;
                }
            }
            return true;
        }

        public string LS(List<string> flags)
        {
            List<string> filesDirectories = fileHandler.GetFilesAndDirectories();
            foreach (string flag in flags)
            {
                switch (flag.Trim())
                {
                    case "S":
                        for (int i = 0; i < filesDirectories.Count; i++)
                        {
                            filesDirectories[i] = filesDirectories[i].Replace(fileHandler.GetCurrentDirectory(), string.Empty);
                            if (filesDirectories[i][0] == Convert.ToChar(@"\"))
                            {
                                filesDirectories[i] = filesDirectories[i].Substring(1);
                            }
                        }
                        break;

                    case "o":
                        filesDirectories.Sort();
                        break;
                }
            }
            string output = string.Empty;
            foreach (string fileDirectory in filesDirectories)
            {
                output += fileDirectory + "\n";
            }
            return output;
        }

        private string PSList(List<string> flags)
        {
            string regex = "noRegex";
            bool clean = false;
            for (int i = 1; i < flags.Count; i++)
            {
                if (flags[i][0] == 'R')
                {
                    regex = "R" + flags[i];
                }
                else if (flags[i][0] == 'r')
                {
                    regex = "r" + flags[i];
                }
                else if (flags[i][0] == 'p')
                {
                    clean = true;
                }
            }
            string output = processHandler.ListProcesses(regex, clean);
            return output;
        }

        private string Out(List<string> flags)
        {
            string length = "noLength";
            string regex = "noRegex";
            for (int i = 1; i < flags.Count; i++)
            {
                if (flags[i][0] == 'l')
                {
                    length = flags[i];
                }
                else if (flags[i][0] == 'R')
                {
                    regex = "R" + flags[i];
                }
                else if (flags[i][0] == 'r')
                {
                    regex = "r" + flags[i];
                }
            }
            string[] output = fileHandler.GetFileContents(flags[0], length, regex);
            consoleHandler.HighlightWord(output[1], regex[0]);
            return output[0];
        }
    }
}
