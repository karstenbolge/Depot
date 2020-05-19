using System;
using System.Globalization;

// Beware danske bank is using decimal point, hence culrura varial en-US
namespace Converter
{
    public class DanskeBank
    {
        static Logger logger;
        String[] lines;
        int numberOfSupoerPortRecords;

        public DanskeBank(String[] lines, Logger l)
        {
            this.lines = lines;
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

            logger.Write("    Danske Bank format");

            int depotAfstemninger = 0;

            if (lines.Length > 2)
            {
                for (int k = 1; k < lines.Length - 1; k++)
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
                        emailBody += Environment.NewLine + "Danske bank record " + depotAfstemninger + " has too few fields";
                        logger.Write("      Record too few fields");
                    }
                    else
                    {
                        string depot = fields[2];
                        if (depot.Length > 14)
                        {
                            depot = depot.Substring(depot.Length - 14);
                        }
                        impRecord.setDepotNumber(depot);
                        impRecord.setIdCode(fields[3]);
                        impRecord.setAmount(fields[4]);
                        impRecord.setSettlementDate(fields[6]);

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
