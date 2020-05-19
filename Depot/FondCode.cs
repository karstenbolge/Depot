using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter
{
    class FondCode
    {
        static Logger logger;
        String filePath;
        Dictionary<string, string> dict = new Dictionary<string, string>();

        public FondCode(Logger l)
        {
            logger = l;
        }

        public bool readFile(String filePath, ref bool debugLevel)
        {
            this.filePath = filePath;
            try
            {
                string[] lines = System.IO.File.ReadAllLines(filePath);
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] fields = lines[i].Split(';');
                    if (fields.Length > 1)
                    {
                        dict.Add(fields[0], fields[1]);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Write(e.Message);
                return false;
            }

            return true;
        }

        public string getIsin(string key)
        {
            string value = string.Empty;

            if (!dict.TryGetValue(key.Trim(), out value))
            {
                logger.Write("Try to get Isin code for " + key + " but it was not found in the file " + filePath);
                return string.Empty;
            }

            return value;
        }
    }
}
