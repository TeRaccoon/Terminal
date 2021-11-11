using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Terminal
{
    class FileHandler
    {
        private string directory;
        DataHandler dataHandler = new DataHandler();
        public FileHandler()
        {
            directory = Environment.CurrentDirectory;
        }
        public string ChangeCurrentDirectory(string newDirectory) // PLEASE CHANGE VARIABLE NAME MY HEAD HURTS
        {
            if (newDirectory[0] == '.' && newDirectory[1] == Convert.ToChar(@"\"))
            {
                newDirectory = directory + newDirectory.Substring(2);
            }
            if (newDirectory[newDirectory.Length - 1] != Convert.ToChar(@"\"))
            {
                newDirectory += @"\";
            }
            if (!Directory.Exists(newDirectory))
            {
                return "Error! Directory " + directory + " does not exist!";
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

        public string GetFileContents(string fileDirectory, string lengthString, string regexString)
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
                    return "Invalid length specified!";
                }
            }
            if (regexString != "noRegex")
            {
                regexString = regexString.Substring(1);
                regexString = regexString.Trim();
            }

            fileDirectory = fileDirectory.Trim();
            if (!fileDirectory.Contains(@"\"))
            {
                fileDirectory = directory + @"\" + fileDirectory;
            }
            else
            {
                if (fileDirectory[0] == '.' && fileDirectory[1] == Convert.ToChar(@"\"))
                {
                    fileDirectory = directory + fileDirectory.Substring(2);
                }
            }
            if (File.Exists(fileDirectory))
            {
                if (length != 0)
                {
                    return dataHandler.RegexString(File.ReadAllLines(fileDirectory).Take(length).ToArray(), regexString);
                }
                return dataHandler.RegexString(File.ReadAllLines(fileDirectory), regexString);
            }
            return "The file " + fileDirectory + " does not exist!";
        }
    }
}
