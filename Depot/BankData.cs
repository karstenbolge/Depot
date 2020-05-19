using System;
using System.Collections.Generic;
using System.Globalization;

namespace Converter
{
    class BankData
    {
        static Logger logger;
        String[] lines;
        int numberOfSupoerPortRecords;
        FondCode fondCode;
        List<string> annuls;

        private bool isAnul(string anul)
        {
            return annuls.Find(x => x.CompareTo(anul) == 0) != null;
        }

        public BankData(String[] lines, ref FondCode fondCode, Logger l)
        {
            this.lines = lines;
            this.fondCode = fondCode;
            logger = l;
            annuls = new List<string>();
        }

        public int Process(ref string emailBody, ref bool debugLevel, ref bool success, string fileName, string folder)
        {
            numberOfSupoerPortRecords = 0;

            logger.Write("    Jyske Bank format");

            if (lines.Length > 1)
            {
                int depotAfsteminger = 0;

                for (int k = 1; k < lines.Length; k++)
                {
                    string[] fields = lines[k].Split(';');
                    if (debugLevel)
                    {
                        logger.WriteFields(fields);
                    }

                    depotAfsteminger++;
                    ImpRecord impRecord = new ImpRecord(logger);

                    string depot = fields[0];
                    if (depot.Length > 14)
                    {
                        depot = depot.Substring(depot.Length - 14);
                    }
                    impRecord.setDepotNumber(depot);
                    impRecord.setIdCode(fields[2]);
                    impRecord.setAmount(fields[11]);
                    impRecord.setSettlementDate(fields[16].Substring(0, 4) + fields[16].Substring(5, 2) + fields[16].Substring(8, 2));

                    numberOfSupoerPortRecords++;
                    impRecord.writeDepot(fileName);
                }

                if (depotAfsteminger > 0) logger.Write("      Depot Afsteminger : " + depotAfsteminger);
            }

            return numberOfSupoerPortRecords;
        }
    }
}
