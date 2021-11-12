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
            return flags;
        }

        public string ManageCommand(string command, List<string> flags)
        {
            string output = string.Empty;
            switch(command.ToLower())
            {
                case "cd":
                    output = fileHandler.ChangeCurrentDirectory(flags[0].Trim());
                    break;

                case "ls":
                    output = LS(flags);
                    break;

                case "out":
                    output = Out(flags);
                    break;

                case "back":
                    output = fileHandler.BackDirectory();
                    break;

                case "md5":
                    output = securityHandler.MD5Hash(fileHandler, flags[0]);
                    break;

                case "sha512":
                    output = securityHandler.MD5Hash(fileHandler, flags[0]);
                    break;

                case "clear":
                    Console.Clear();
                    output = null;
                    break;

                case "clr":
                    Console.Clear();
                    output = null;
                    break;
            }
            if (output == string.Empty)
            {
                return "Unknown Error";
            }
            return output;
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
            consoleHandler.HighlightWord(output[1]);
            return output[0];
        }
    }
}
