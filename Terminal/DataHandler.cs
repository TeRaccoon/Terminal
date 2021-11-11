using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal
{
    class DataHandler
    {
        public string RegexString(string[] data, string regex)
        {
            if (regex == string.Empty)
            {
                return string.Join("", data);
            }
            string output = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                if ((data[i].ToLower().Contains(regex.ToLower()) && regex[i] == 'R') || (data[i].Contains(regex) &&  regex[i] == 'r'))
                {
                    output += data[i] + "\n";
                }
            }
            return output;
        }
    }
}
