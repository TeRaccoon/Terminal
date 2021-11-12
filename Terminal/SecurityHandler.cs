using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

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
        public string SHA512Hash(FileHandler fileHandler, string directory)
        {
            directory = directory.Trim();
            directory = fileHandler.ValidateFilePath(directory);
            if (directory != null)
            {
                byte[] data = SHA512.Create().ComputeHash(File.OpenRead(directory));
                return BitConverter.ToString(data).Replace("-", string.Empty).ToLowerInvariant();
            }
            else
            {
                return "Invalid file directory!";
            }
        }
    }
}
