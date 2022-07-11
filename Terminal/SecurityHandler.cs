using System;
using System.IO;
using System.Security.Cryptography;

namespace Terminal
{
    class SecurityHandler
    {
        public string MD5Hash(FileHandler fileHandler, string directory)
        {
            directory = directory.Trim();
            directory = fileHandler.ValidateFilePath(directory);
            if (directory != null)
            {
                byte[] data = MD5.Create().ComputeHash(File.OpenRead(directory));
                return BitConverter.ToString(data).Replace("-", string.Empty).ToLowerInvariant();
            }
            else
            {
                return "Invalid file directory!";
            }
        }
        public string SHA512Hash(FileHandler fileHandler, string inputDirectory)
        {
            inputDirectory = fileHandler.ValidateFilePath(inputDirectory);
            if (inputDirectory != null)
            {
                byte[] data = SHA512.Create().ComputeHash(File.OpenRead(inputDirectory));
                return BitConverter.ToString(data).Replace("-", string.Empty).ToLowerInvariant();
            }
            else
            {
                return "Invalid file directory!";
            }
        }
        public string SHA256Hash(FileHandler fileHandler, string inputDirectory)
        {
            inputDirectory = fileHandler.ValidateFilePath(inputDirectory);
            if (inputDirectory != null)
            {
                byte[] data = SHA256.Create().ComputeHash(File.OpenRead(inputDirectory));
                return BitConverter.ToString(data).Replace("-", string.Empty).ToLowerInvariant();
            }
            else
            {
                return "Invalid file directory!";
            }
        }
    }
}
