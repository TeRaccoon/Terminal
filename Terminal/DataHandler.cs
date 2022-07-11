namespace Terminal
{
    class DataHandler
    {
        private bool state = true;
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
                    if (data[i].Substring(data[i].Length - 3, 2) != "\n")
                    {
                        output += data[i];
                    }
                    else
                    {
                        output += data[i] + "\n";
                    }
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

        public string[] AddNewLine(string[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] += "\n";
            }
            return data;
        }

        public void SetCorrectFlag(bool state)
        {
            this.state = state;
        }
        public bool GetCorrectFlag()
        {
            return state;
        }
    }
}
