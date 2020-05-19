using System;
using System.IO;

namespace Converter
{
    public class ImpRecord
    {
        const string FILE_NAME = "PfKonto.Imp";
        Logger logger;

        String depotNumber;//Depot nummer
        String idCode;//Idkode
        DecimalNumber amount;//Stk
        String settlementDate;//Valør Dato

        const String spaces = "                                            ";
        const String zeros = "00000000000000000000000000000000000000000000";

        public ImpRecord(Logger l)
        {
            logger = l;

            depotNumber = string.Empty;
            idCode = string.Empty;
            amount = new DecimalNumber(12, 0, false, true, logger);//Stk
            settlementDate = string.Empty;
        }

        public void setSettlementDate(String s)
        {
            settlementDate = s;
        }

        public String getSettlementDate()
        {
            return settlementDate;
        }

        public void setIdCode(String s)
        {
            if (s.Length > 12)
            {
                logger.Write("IdCode : " + s + " too long!");
            }

            idCode = s.Substring(0, s.Length > 12 ? 12 : s.Length);
        }

        public String getIdCode()
        {
            return spaces.Substring(0, 12 - idCode.Length) + idCode;
        }

        public void setDepotNumber(String s, bool removePaddingZero = false, int lastXdigits = 0)
        {
            if (lastXdigits > 0 && s.Length > lastXdigits)
            {
                s = s.Substring(s.Length - lastXdigits);
            }

            if (removePaddingZero)
            {
                while (s.Length > 0 && s[0] == '0')
                {
                    s = s.Substring(1);
                }
            }

            if (s.Length > 16)
            {
                logger.Write("DepotNumber : " + s + " too long!");
            }

            depotNumber = s.Substring(0, s.Length > 16 ? 16 : s.Length);
        }

        public String getDepotNumber()
        {
            return spaces.Substring(0, 16 - depotNumber.Length) + depotNumber;
        }

        public void blankAmount()
        {
            amount.setBlank();
        }

        public void setAmount(String s)
        {
            amount.setDecimalNumber(s);
        }

        public String getAmount()
        {
            return amount.getDecimalNumber();
        }

        public void writeDepot(String path)
        {
            using (StreamWriter w = File.AppendText(path + "\\" + FILE_NAME))
            {
                w.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}",
                    (char)0x1F,
                    (char)0x1F,
                    getDepotNumber(),
                    (char)0x1F,
                    getIdCode(),
                    (char)0x1F,
                    getAmount(),
                    (char)0x1F,
                    0,
                    (char)0x1F,
                    getSettlementDate(),
                    (char)0x1F,
                    (char)0x1F,
                    (char)0x1F,
                    (char)0x1F,
                    (char)0x1F);
            }
        }

        public void writeHead(String path, String date)
        {
            using (StreamWriter w = File.AppendText(path + "\\" + FILE_NAME))
            {
                w.WriteLine("HEAD{0}{1}{2}{3}    ",
                    (char)0x1F,
                    date.Substring(6, 4),
                    date.Substring(3, 2),
                    date.Substring(0, 2));
            }
        }

        public void writeTail(String path, int numberOfRecords)
        {
            using (StreamWriter w = File.AppendText(path + "\\" + FILE_NAME))
            {
                w.WriteLine("TAIL{0}{1}        ",
                    (char)0x1F,
                    numberOfRecords.ToString("000000"));
            }
        }

        public void createEmptyFile(String path)
        {
            File.Create(path + "\\" + FILE_NAME).Dispose();
        }
    }
}
