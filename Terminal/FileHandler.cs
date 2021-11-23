using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Permissions;
using System.Security.AccessControl;
using System.Security;
using System.Security.Principal;
using System.Diagnostics;

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
                return "null";
            }
            if (!newDirectory.Contains(@"\") && newDirectory[1] != ':')
            {
                newDirectory = directory + newDirectory;
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
                output += "File: " + file + "\nAttributes:" + File.GetAttributes(file) + "Extension: " + Path.GetExtension(file) + "\nAccess Controls: ";
                if (!File.GetAttributes(file).HasFlag(FileAttributes.System))
                {
                    FileSecurity accessControlList = File.GetAccessControl(file);
                    AuthorizationRuleCollection arc = accessControlList.GetAccessRules(true, true, typeof(SecurityIdentifier));
                    foreach (FileSystemAccessRule fsar in arc)
                    {
                        output += fsar.IdentityReference.Translate(typeof(NTAccount)) + ": " + fsar.FileSystemRights + "\n";
                    }
                }
                else
                {
                    output += "Unretrieveable\n";
                }
                output += "Access times:\n - Last access: " + File.GetLastAccessTime(file) + "\n - Last write time: " + File.GetLastWriteTime(file) + "\n - Creation time: " + File.GetCreationTime(file) + "\n";
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
                    output += AssessFile(file);
                }
                return output;
            }
            return "Invalid directory path!";
        }

        public string MakeFile(string filePath)
        {
            filePath = FixDirectory(filePath);
            filePath = filePath.Substring(0, filePath.Length - 1);
            FileStream createdFile = File.Create(filePath);
            createdFile.Close();
            return "File create successfully!";
        }

        public string DeleteFile(string filePath)
        {
            filePath = ValidateFilePath(filePath);
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
    }
}
