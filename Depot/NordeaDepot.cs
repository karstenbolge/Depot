using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter
{
    class NordeaDepot
    {
        static Logger logger;
        String filePath;
        Dictionary<string, string> dict = new Dictionary<string, string>();

        public NordeaDepot(Logger l)
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
                    if (fields.Length > 1 && fields[2].Length > 0)
                    {
                        dict.Add(fields[2], fields[1]);
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

        public string getDepot(string konto)
        {
            string value = string.Empty;
            konto = konto.Trim();
            konto = konto.TrimStart('0');
            if (!dict.TryGetValue(konto, out value))
            {
                logger.Write("Try to get Depot number code for account " + konto + " but it was not found in the file " + filePath);
                return string.Empty;
            }

            return value;
        }
    }
}
