using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter
{
    class Nykredit
    {
        static Logger logger;
        String[] lines;
        int numberOfSupoerPortRecords;

        public Nykredit(String[] lines, Logger l)
        {
            this.lines = lines;
            logger = l;
        }

        public int Process(ref string emailBody, ref bool debugLevel, ref bool success, string fileName, string folder)
        {
            numberOfSupoerPortRecords = 0;

            logger.Write("    Nykrdit format");

            // Removing qoutes
            if (lines[0].IndexOf("\"") == 0)
            {
                for (int k = 1; k < lines.Length; k++)
                {
                    lines[k] = lines[k].Substring(1, lines[k].Length - 2);
                }
            }

            int depotAfstemning = 0;

            if (lines.Length > 2)
            {
                for (int k = 1; k < lines.Length - 1; k++)
                {
                    string[] fields = lines[k].Split(';');
                    if (debugLevel)
                    {
                        logger.WriteFields(fields);
                    }

                    if (lines[k].IndexOf("DB;") == 0)
                    {
                        depotAfstemning++;
                        ImpRecord impRecord = new ImpRecord(logger);
                        
                        if (fields.Length < 6)
                        {
                            emailBody += Environment.NewLine + "Nykredit KB record " + depotAfstemning + " has too few fields";
                            logger.Write("      KB record too few fields");
                        }
                        else
                        {
                            impRecord.setDepotNumber("  00000000000000000".Substring(0, 16 - fields[1].Length) + fields[1]);
                            impRecord.setIdCode(fields[2]);
                            impRecord.setAmount(fields[6]);
                            impRecord.setSettlementDate(fields[3]);

                            numberOfSupoerPortRecords++;
                            impRecord.writeDepot(fileName);
                        }
                    }
                    else
                    {
                        success = false;
                        logger.Write("      Ukendt fil format, " + lines[k]);
                    }
                }

                if (depotAfstemning > 0) logger.Write("      Depot afstemninger : " + depotAfstemning);
            }
            else
            {
                if (lines.Length == 2 && lines[1].IndexOf("TAIL;") == 0)
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
