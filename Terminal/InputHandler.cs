using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Security.Principal;

namespace Terminal
{
    class InputHandler
    {
        private FileHandler fileHandler;
        private ConsoleHandler consoleHandler;
        private SecurityHandler securityHandler = new SecurityHandler();
        private ProcessHandler processHandler = new ProcessHandler();
        private EnvironmentHandler environmentHandler = new EnvironmentHandler();
        private NetworkHandler networkHandler = new NetworkHandler();
        private ResourceHandler resourceHandler = new ResourceHandler();
        private DataHandler dataHandler = new DataHandler();
        private string manual;
        public InputHandler(FileHandler fileHandler, ConsoleHandler consoleHandler)
        {
            this.fileHandler = fileHandler;
            this.consoleHandler = consoleHandler;
            manual = fileHandler.GetFileContents(@"Manual.txt", "noLength", "noRegex")[0];
        }
        public List<string> SplitInput(string input)
        {
            string command = input.Split(' ')[0];
            List<string> flags = input.Substring(command.Length).Split('-').ToList<string>();
            flags.Insert(0, command);
            for (int i = 0; i < flags.Count; i++)
            {
                flags[i] = flags[i].Trim();
                if (flags[i] == string.Empty)
                {
                    flags.RemoveAt(i);
                }
            }
            return flags;
        }

        public string ManageCommand(string command, List<string> flags)
        {
            string output = string.Empty;
            try
            {
                switch (command.ToLower())
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

                    case "touch":
                        output = fileHandler.MakeFile(flags[0]);
                        break;

                    case "mkdir":
                        output = fileHandler.MakeDirectory(flags[0]);
                        break;

                    case "rmdir":
                        output = fileHandler.DeleteDirectory(flags[0]);
                        break;

                    case "del":
                        output = fileHandler.DeleteFile(flags[0]);
                        break;

                    case "cp":
                        output = CopyFile(flags);
                        break;

                    case "mv":
                        output = MoveFile(flags);
                        break;

                    case "open":
                        output = fileHandler.OpenFile(flags[0]);
                        break;

                    case "back":
                        output = fileHandler.BackDirectory();
                        break;

                    case "ps":
                        output = PSList(flags);
                        break;

                    case "killall":
                        output = processHandler.KillAll(flags[0]);
                        break;

                    case "kill":
                        output = Kill(flags[0]);
                        break;

                    case "sysm":
                        //output = Sysm(flags[0]);
                        break;

                    //case "netcheck":
                    //    output = networkHandler.

                    case "whoami":
                        output = environmentHandler.WhoAmI();
                        break;

                    case "ipconfig":
                        output = networkHandler.IPConfig();
                        break;

                    case "ping":
                        output = Ping(flags);
                        break;

                    case "md5":
                        output = securityHandler.MD5Hash(fileHandler, flags[0]);
                        break;

                    case "sha512":
                        output = securityHandler.SHA512Hash(fileHandler, flags[0]);
                        break;

                    case "sha256":
                        output = securityHandler.SHA256Hash(fileHandler, flags[0]);
                        break;

                    case "file":
                        output = fileHandler.AssessFile(flags[0]);
                        break;

                    case "directory":
                        output = fileHandler.AssessDirectory(flags[0]);
                        break;

                    case "find":
                        output = Find(flags);
                        break;

                    case "cpu":
                        output = CPUUsage(flags[0]);
                        break;

                    case "ram":
                        output = RAMUsage(flags[0]);
                        break;

                    case "sudo":
                        output = Sudo();
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
                        output = manual;
                        break;

                    case "paste":
                        break;

                    case "suicide":
                        Environment.Exit(0);
                        break;
                }
            }
            catch
            {
                return "This one is on you. I coded everything correctly and I'm sure of it. Please double check your syntax!";
            }
            if (output == string.Empty)
            {
                return "Either you entered a command that didn't exist (most common) or I (the developer) forgot to assign the output variable after calling a function.";
            }
            if (dataHandler.GetCorrectFlag() == false)
            {
                dataHandler.SetCorrectFlag(true);
                return "Incorrect flag usage at --> " + flags[0];
            }
            try
            {
                flags.Remove(string.Empty);
                foreach (string flag in flags)
                {
                    switch (flag[0])
                    {
                        case 'F':
                            if (output != null)
                            {
                                try
                                {
                                    string saveDirectory = fileHandler.FixDirectory(flag.Substring(2));
                                    fileHandler.SetSaveOutput(saveDirectory.Substring(0, saveDirectory.Length - 1));
                                    output = fileHandler.SaveOutput(output);
                                }
                                catch
                                {
                                    return "Failed to save to directory! Likely because a directory was not specified!";
                                }
                            }
                            break;
                    }
                }
            }
            catch
            {
                return "This one is on you. I coded everything correctly and I'm sure of it. Please double check your syntax!";
            }

            return output;
        }

        public string Sudo()
        {
            try
            {
                AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                CheckAdministrator();
                PrincipalPermission principalPerm = new PrincipalPermission(null, @"BUILTIN\Administrators");
                principalPerm.Demand();
                return "Permissions demanded successfully. User is now elevated!";
            }
            catch
            {
                return "Request for principal permission failed!";
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
        static void CheckAdministrator()
        {
            Console.WriteLine("User is an administrator");
        }
        public string CPUUsage(string pollingRate)
        {
            Console.Clear();
            if (pollingRate == string.Empty)
            {
                environmentHandler.CPUUsage(resourceHandler, 500);
            }
            else
            {
                try
                {
                    pollingRate = pollingRate.Substring(1);
                    pollingRate = pollingRate.Trim();
                    environmentHandler.CPUUsage(resourceHandler, Convert.ToInt32(pollingRate));
                }
                catch
                {
                    return "Incorrect polling rate specified!";
                }
            }
            return null;
        }
        public string RAMUsage(string pollingRate)
        {
            if (pollingRate == string.Empty)
            {
                environmentHandler.RAMUsage(resourceHandler, 500);
            }
            else
            {
                try
                {
                    pollingRate = pollingRate.Substring(1);
                    pollingRate = pollingRate.Trim();
                    environmentHandler.RAMUsage(resourceHandler, Convert.ToInt32(pollingRate));
                }
                catch
                {
                    return "Incorrect polling rate specified!";
                }
            }
            return null;
        }
        public string Find(List<string> flags)
        {
            char regex = 'r';
            for (int i = 1; i < flags.Count; i++)
            {
                if (flags[i][0] == 'R')
                {
                    regex = 'R';
                }
            }
            consoleHandler.HighlightWord(flags[0], regex);
            return fileHandler.Find(flags[0], regex);
        }

        public string CopyFile(List<string> flags)
        {
            string[] directories = flags[0].Split(' ');
            try
            {
                return fileHandler.CopyFile(directories[0], directories[1]);
            }
            catch
            {
                return "Incorrect format!";
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
        public string MoveFile(List<string> flags)
        {
            string[] directories = flags[0].Split(' ');
            try
            {
                return fileHandler.MoveFile(directories[0], directories[1]);
            }
            catch
            {
                return "Incorrect format!";
            }
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

        public string Ping(List<string> flags)
        {
            string target = flags[0];
            int times = 5;
            if (String.IsNullOrEmpty(target))
            {
                return "Please specify a target!";
            }
            string temp = flags[0];
            flags.RemoveAt(0);
            foreach (string flag in flags)
            {
                if (flag[0] == 'c')
                {
                    try
                    {
                        times = Convert.ToInt32(flag.Trim().Substring(1));
                        if (times < 1)
                        {
                            return "Invalid ping count specified!";
                        }
                    }
                    catch
                    {
                        return "Invalid ping count specified!";
                    }
                }
                else
                {
                    return "Incorrect flag usage at --> " + flag;
                }
            }
            flags.Insert(0, temp);
            networkHandler.Ping(target, times);
            return null;
        }
        public string LS(List<string> flags)
        {
            List<string> filesDirectories = fileHandler.GetFilesAndDirectories();
            for (int k = 0; k < filesDirectories.Count; k++)
            {
                filesDirectories[k] = filesDirectories[k].Replace(fileHandler.GetCurrentDirectory(), string.Empty);
                if (filesDirectories[k][0] == Convert.ToChar(@"\"))
                {
                    filesDirectories[k] = filesDirectories[k].Substring(1);
                }
            }
            for (int i = 0; i < flags.Count; i++)
            {
                switch (flags[i].Trim())
                {
                    case "S":
                        for (int k = 0; k < filesDirectories.Count; k++)
                        {
                            filesDirectories[k] = fileHandler.GetCurrentDirectory() + filesDirectories[k];
                        }
                        
                        break;

                    case "o":
                        filesDirectories.Sort();
                        break;

                    default:
                        dataHandler.SetCorrectFlag(false);
                        break;
                }
            }
            string output = string.Empty;
            foreach (string fileDirectory in filesDirectories)
            {
                output += fileDirectory + "\n";
            }
            if (output == string.Empty)
            {
                return "No files or directories could be found...";
            }
            return output;
        }

        private string PSList(List<string> flags)
        {
            flags.Remove(string.Empty);
            string regex = "noRegex";
            bool clean = false;
            for (int i = 0; i < flags.Count; i++)
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
                else if (flags[i][0] != 'F')
                {
                    return "Incorrect flag usage at --> " + flags[i];
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
                else
                {
                    return "Incorrect flag usage at --> " + flags[i];
                }
            }
            string[] output = fileHandler.GetFileContents(flags[0], length, regex);
            consoleHandler.HighlightWord(output[1], regex[0]);
            return output[0];
        }
    }
}
