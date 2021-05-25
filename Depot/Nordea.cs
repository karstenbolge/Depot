using System;
using System.Globalization;

// Beware nordea bank is using decimal point, hence culrura varial en-US
namespace Converter
{
    class Nordea
    {
        static Logger logger;
        String[] lines;
        NordeaDepot nordeaDepot;
        int numberOfSupoerPortRecords;

        public Nordea(String[] lines, ref NordeaDepot nordeaDepot, Logger l)
        {
            this.lines = lines;
            this.nordeaDepot = nordeaDepot;
            logger = l;
        }

        public String splitTransactioNumber(String t)
        {
            String lastPart = t.Split(' ')[t.Split(' ').Length - 1];
            String[] datePart = lastPart.Split('-');
            String dotPart = lastPart.Split('.')[lastPart.Split('.').Length - 1];

            if (datePart.Length < 2)
            {
                return t;
            }

            if (dotPart.Length < 1)
            {
                return t;
            }

            return datePart[0] + datePart[1] + datePart[2].Substring(0, 2) + dotPart;
        }

        public int Process(ref string emailBody, ref bool debugLevel, ref bool success, string fileName)
        {
            numberOfSupoerPortRecords = 0;

            logger.Write("    Nordea format");

            int depotAfstemninger = 0;

            if (lines.Length > 0)
            {
                for (int k = 0; k < lines.Length; k++)
                {
                    string[] fields = lines[k].Split((char)31);
                    if (debugLevel)
                    {
                        logger.WriteFields(fields);
                    }

                    depotAfstemninger++;
                    ImpRecord impRecord = new ImpRecord(logger);

                    if (fields.Length < 5)
                    {
                        emailBody += Environment.NewLine + "Nordea record " + depotAfstemninger + " has too few fields";
                        logger.Write("      Record too few fields");
                    }
                    else
                    {
                        string account = fields[0];
                        if (account.Length > 10)
                        {
                            account = account.Substring(account.Length - 10);
                        }
                        
                        string depot = nordeaDepot.getDepot(account);
                        if (depot.Equals(string.Empty))
                        {
                            success = false;
                            emailBody += "Try to get Nordea Depot code for account " + account + " but was not found in the Nordea depot file.\n";
                        }
                        impRecord.setDepotNumber("0000000000" + depot, false, 10);

                        impRecord.setIdCode(fields[3]);
                        impRecord.setAmount(fields[4]);

                        numberOfSupoerPortRecords++;
                        impRecord.writeDepot(fileName);
                    }
                }
                if (depotAfstemninger > 0) logger.Write("      Depot afstemninger : " + depotAfstemninger);
            }
            else
            {
                if (lines.Length == 2 && lines[1].IndexOf("TAIL") == 0)
                {
                    logger.Write("      Filen indeholder ingen rcords");
                }
                else
                {
                    success = false;
                    logger.Write("      Ukendt fil format");
                }
            }

            return numberOfSupoerPortRecords;
        }
    }
}
