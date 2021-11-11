using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal
{
    class DataHandler
    {
        public string RegexString(string[] data, string regex, char regexType)
        {
            if (regex == "noRegex")
            {
                return string.Join("", data);
            }
            string output = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                if ((data[i].ToLower().Contains(regex.ToLower()) && regexType == 'R') || (data[i].Contains(regex) && regexType == 'r'))
                {
                    output += data[i] + "\n";
                }
            }
            if (output == string.Empty)
            {
                return "No matches found!";
            }
            else
            {
                return output;
            }
        }
    }
}
