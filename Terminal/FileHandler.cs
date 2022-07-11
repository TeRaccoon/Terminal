using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Terminal
{
    class FileHandler
    {
        private string directory;
        private string outputDirectory;
        DataHandler dataHandler = new DataHandler();
        public FileHandler()
        {
            directory = Environment.CurrentDirectory + @"\";
        }
        public string FixDirectory(string newDirectory)
        {
            newDirectory = newDirectory.Trim();
            newDirectory = newDirectory.Replace('/', Convert.ToChar(@"\"));
            if (newDirectory == string.Empty)
            {
                return newDirectory;
            }
            try
            {
                if (!newDirectory.Contains(@"\") && newDirectory[1] != ':')
                {
                    newDirectory = directory + newDirectory;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return newDirectory;
            }
            if (newDirectory[0] == '.' && newDirectory[1] == Convert.ToChar(@"\"))
            {
                newDirectory = directory + newDirectory.Substring(2);
            }
            if (newDirectory[newDirectory.Length - 1] != Convert.ToChar(@"\"))
            {
                newDirectory += @"\";
            }
            return newDirectory;
        }
        public string ChangeCurrentDirectory(string newDirectory)
        {
            newDirectory = FixDirectory(newDirectory);
            if (!Directory.Exists(newDirectory))
            {
                return "Error! Directory " + newDirectory + " does not exist!";
            }
            directory = newDirectory;
            return null;
        }

        public string GetCurrentDirectory()
        {
            return directory;
        }

        public List<string> GetFilesAndDirectories()
        {
            List<string> filesDirectories = Directory.GetDirectories(directory).ToList<string>();
            for (int i = 0; i < filesDirectories.Count; i++)
            {
                filesDirectories[i] += @"\";
            }
            filesDirectories.AddRange(Directory.GetFiles(directory).ToList<string>());
            return filesDirectories;
        }

        public string[] GetFileContents(string fileDirectory, string lengthString, string regexString)
        {
            int length = 0;
            if (lengthString != "noLength")
            {
                lengthString = lengthString.Substring(1);
                lengthString = lengthString.Trim();
                try
                {
                    length = Convert.ToInt32(lengthString);
                }
                catch
                {
                    return new string[] { "Invalid length specified!", null };
                }
            }
            char regexChar = 'x';
            if (regexString != "noRegex")
            {
                regexChar = regexString[0];
                regexString = regexString.Substring(2);
                regexString = regexString.Trim();
            }
            fileDirectory = ValidateFilePath(fileDirectory);
            if (File.Exists(fileDirectory))
            {
                if (length != 0)
                {
                    return new string[] { dataHandler.RegexString(dataHandler.AddNewLine(File.ReadAllLines(fileDirectory).Take(length).ToArray()), regexString, regexChar), regexString };
                }
                try
                {
                    return new string[] { dataHandler.RegexString(dataHandler.AddNewLine(File.ReadAllLines(fileDirectory)), regexString, regexChar), regexString };
                }
                catch
                {
                    return new string[] { "File is inaccessible! This is usually because it is a protected system file.", null };
                }
            }
            return new string[] { "Invalid file path!", null };
        }

        public string BackDirectory()
        {
            try
            {
                string parentDirectory = Directory.GetParent(directory.Substring(0, directory.Length - 1)).FullName;
                ChangeCurrentDirectory(parentDirectory);
                return null;
            }
            catch
            {
                return "Directory.GetParent returned null!";
            }
        }
        public string ValidateFilePath(string filePath)
        {
            filePath.Trim();
            filePath = FixDirectory(filePath);
            filePath = filePath.Substring(0, filePath.Length - 1);
            if (File.Exists(filePath))
            {
                return filePath;
            }
            return null;
        }
        public string ValidatedirectoryPath(string directoryPath)
        {
            directoryPath.Trim();
            directoryPath = FixDirectory(directoryPath);
            if (Directory.Exists(directoryPath))
            {
                return directoryPath;
            }
            return null;
        }

        public string AssessFile(string file)
        {
            file = ValidateFilePath(file);
            if (file != null)
            {
                string output = "\n";
                output += "File: " + file + "\nAttributes: " + File.GetAttributes(file) + "\nExtension: " + Path.GetExtension(file);
                output += "\n-----------------------------------------------------------------------------\n";
                output += String.Format("| {0,-28} {1,0} {2,28} |", "", "Access Controls", "");
                if (!File.GetAttributes(file).HasFlag(FileAttributes.System))
                {
                    output += "\n-----------------------------------------------------------------------------\n";
                    FileSecurity accessControlList = File.GetAccessControl(file);
                    AuthorizationRuleCollection arc = accessControlList.GetAccessRules(true, true, typeof(SecurityIdentifier));
                    foreach (FileSystemAccessRule fsar in arc)
                    {
                        output += String.Format("| {0,-35} | {1,35} |", fsar.IdentityReference.Translate(typeof(NTAccount)), fsar.FileSystemRights);
                        output += "\n-----------------------------------------------------------------------------\n";
                    }
                }
                else
                {
                    output += "Unretrieveable\n";
                }
                output += "\nAccess times:\n - Last access: " + File.GetLastAccessTime(file) + "\n - Last write time: " + File.GetLastWriteTime(file) + "\n - Creation time: " + File.GetCreationTime(file) + "\n";
                return output;
            }
            return "Invalid file path!";
        }
        public string AssessDirectory(string directoryPath)
        {
            directoryPath = ValidatedirectoryPath(directoryPath);
            if (directoryPath != null)
            {
                string output = string.Empty;
                string[] files = Directory.GetFiles(directoryPath);
                foreach (string file in files)
                {
                    output += AssessFile(file) + "\n";
                }
                return output;
            }
            return "Invalid directory path!";
        }

        public string MakeFile(string filePath)
        {
            filePath = FixDirectory(filePath);
            try
            {
                filePath = filePath.Substring(0, filePath.Length - 1);
                FileStream createdFile = File.Create(filePath);
                createdFile.Close();
                return "File create successfully!";
            }
            catch
            {
                return "Failed to create file!";
            }
        }

        public string MakeDirectory(string directoryPath)
        {
            directoryPath = FixDirectory(directoryPath);
            directoryPath = directoryPath.Substring(0, directoryPath.Length - 1);
            string newDirectoryLocation = directoryPath.Substring(0, directoryPath.LastIndexOf(Convert.ToChar(@"\")));
            if (Directory.GetDirectories(newDirectoryLocation).Contains(directoryPath))
            {
                return "A directory with that name already exists!";
            }
            try
            {
                Directory.CreateDirectory(directoryPath);
                return "Directory create successfully!";
            }
            catch
            {
                return "Failed to create directory!";
            }
        }

        public string DeleteDirectory(string directoryPath)
        {
            directoryPath = ValidatedirectoryPath(directoryPath);
            if (directoryPath == null)
            {
                return "The specified directory does not exist!";
            }
            Directory.Delete(directoryPath);
            return "Directory deleted successfully!";
        }
        public string DeleteFile(string filePath)
        {
            filePath = ValidateFilePath(filePath);
            if (filePath == null)
            {
                return "The specified file does not exist!";
            }
            File.Delete(filePath);
            return "File deleted successfully!";
        }

        public string OpenFile(string filePath)
        {
            filePath = ValidateFilePath(filePath);
            try
            {
                Process.Start(filePath);
                return "File opened successfully!";
            }
            catch
            {
                return "Failed to open file " + filePath + "!";
            }
        }

        public string CopyFile(string fileToCopy, string locationOfNewFile)
        {
            fileToCopy = ValidateFilePath(fileToCopy);
            locationOfNewFile = FixDirectory(locationOfNewFile);
            locationOfNewFile = locationOfNewFile.Substring(0, locationOfNewFile.Length - 1);
            try
            {
                File.Copy(fileToCopy, locationOfNewFile);
                return "File copied successfully!";
            }
            catch
            {
                return "Failed to copy " + fileToCopy + " to " + locationOfNewFile + "!";
            }
        }
        public string MoveFile(string fileToMove, string locationOfNewFile)
        {
            fileToMove = ValidateFilePath(fileToMove);
            locationOfNewFile = FixDirectory(locationOfNewFile);
            try
            {
                if (locationOfNewFile[locationOfNewFile.Length - 1] == Convert.ToChar(@"\"))
                {
                    locationOfNewFile += fileToMove.Replace(GetCurrentDirectory(), string.Empty);
                }
                File.Move(fileToMove, locationOfNewFile);
                return "File moved successfully!";
            }
            catch
            {
                return "Failed to move " + fileToMove + " to " + locationOfNewFile + "!";
            }
        }

        public void SetSaveOutput(string outputDirectory)
        {
            this.outputDirectory = outputDirectory;
        }
        public string SaveOutput(string output)
        {
            if (outputDirectory != null)
            {
                try
                {
                    File.WriteAllText(outputDirectory, output);
                }
                catch
                {
                    output = "Invalid file path!";
                }
                outputDirectory = null;
                return output;
            }
            return "Invalid file path!";
        }

        public string Find(string fileName, char regex)
        {
            string output = string.Empty;
            List<string> directories = GetAllDirectories(GetCurrentDirectory());
            List<string> files = new List<string>();
            files.AddRange(Directory.GetFiles(GetCurrentDirectory()));
            Console.WriteLine("Indexing...");
            int count = 0;
            foreach (string directory in directories)
            {
                count++;
                if (directory.Contains(fileName) || (regex == 'R' && directory.ToLower().Contains(fileName.ToLower())))
                {
                    output += "\n--------------------------------------------------------------------------------------------------------\n";
                    output += String.Format("| {0,-100} |", directory);
                }
                files.AddRange(Directory.GetFiles(directory));
            }
            Console.WriteLine("Searching...");
            //percent = Convert.ToInt32(files.Count / 100);
            foreach (string file in files)
            {
                string cleanFileName = file.Substring(file.LastIndexOf(Convert.ToChar(@"\")));
                if (cleanFileName.Contains(fileName) || (regex == 'R' && cleanFileName.ToLower().Contains(fileName.ToLower())))
                {
                    if (!output.Contains(file))
                    {
                        output += "\n--------------------------------------------------------------------------------------------------------\n";
                        output += String.Format("| {0,-100} |", file);
                    }
                }
            }
            if (output == string.Empty)
            {
                return "Unable to find any files with or containing the name " + fileName + "!\n";
            }
            output += "\n--------------------------------------------------------------------------------------------------------\n";
            return "\n" + output;
        }

        public List<string> GetAllDirectories(string initialDirectory)
        {
            List<string> directories = new List<string>();
            directories.AddRange(Directory.GetDirectories(initialDirectory));
            int cap = 0;
            int temp = 0;
            bool finished = false;
            double postedPercent = -1;
            while (!finished)
            {
                temp = directories.Count;
                if (cap != 0 && directories.Count != 0)
                {
                    double currentPercent = Convert.ToInt32(Convert.ToDouble(cap) / Convert.ToDouble(directories.Count) * 100d);
                    if (postedPercent != currentPercent)
                    {
                        Console.Clear();
                        Console.WriteLine(currentPercent + "%");
                        postedPercent = currentPercent;
                    }
                }
                for (int i = cap; i < directories.Count; i++)
                {
                    try
                    {
                        directories.AddRange(Directory.GetDirectories(directories[i]));
                    }
                    catch
                    {
                        //No directories
                    }
                }
                if (temp == directories.Count)
                {
                    finished = true;
                }
                cap = temp;
            }
            directories.Sort();
            return directories;
        }
    }
}
