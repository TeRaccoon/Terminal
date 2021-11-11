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
        public InputHandler(FileHandler fileHandler)
        {
            this.fileHandler = fileHandler;
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
                switch (flag)
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
            string output = string.Empty;
            string length = "noLength";
            string regex = "noRegext";
            for (int i = 1; i < flags.Count; i++)
            {
                if (flags[i] == "l")
                {
                    length = flags[i];
                }
                else if (flags[i] == "R")
                {
                    regex = "R" + flags[i];
                }
                else if (flags[i] == "r")
                {
                    regex = "r" + flags[i];
                }
            }
            output = fileHandler.GetFileContents(flags[0], length, regex);
            return output;
        }
    }
}
