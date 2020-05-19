using System;
using System.IO;
using System.Reflection;

namespace Converter
{
    class Program
    {
        static void Main(string[] args)
        {
            String date = DateTime.Now.ToShortDateString();
            if (args.Length == 0)
            {
                Console.Write("Too few parameters!\n");
                Console.Write("  Input folder\n");
                Console.Write("  Output folder\n");
                Console.Write("  SMTP server\n");
                Console.Write("  Port\n");
                Console.Write("  Username\n");
                Console.Write("  Password\n");
                Console.Write("  Uses Ssl\n");
                Console.Write("  To email\n");
                Console.Write("  Fondkode file\n");
                Console.Write("  [Debug level]\n");
                Environment.Exit(5);
            }

            Logger logger = new Logger(args[1] + "\\" + date);
            bool debugLevel = false;

            if (args.Length < 9)
            {
                logger.Write("Too few parameters!", true);
                logger.Write("  Input folder", true);
                logger.Write("  Output folder", true);
                logger.Write("  SMTP server", true);
                logger.Write("  Port", true);
                logger.Write("  Username", true);
                logger.Write("  Password", true);
                logger.Write("  Uses Ssl", true);
                logger.Write("  To email", true);
                logger.Write("  Fondkode file", true);
                logger.Write("  [Debug level]", true);
                Environment.Exit(1);
            }


            Email email = new Email(args[2], Int16.Parse(args[3]), args[4], args[5], args[6], args[7], logger);

            if (Directory.Exists(args[1] + "\\" + date))
            {
                Console.Write("Program already run today.\n");
                Console.Write("To run againg output folder needs to be cleaned!\n");

                email.Send("Superport converter already run today.", "To run againg output folder needs to be cleaned!");
                Environment.Exit(2);
            }

            String emailBody = String.Empty;
            int numberOfSupoerPortRecords = 0;
            Directory.CreateDirectory(args[1] + "\\" + date);

            FondCode fondCode = new FondCode(logger);

            logger.Write("---- Start " + Assembly.GetCallingAssembly().GetName().Version +  " --------------", true);
            if (args.Length >= 10)
            {
                debugLevel = args[9].ToLower() == "true";
                if (debugLevel)
                {
                    logger.Write("Debug level : true", true);
                }
            }

            if (!fondCode.readFile(args[8], ref debugLevel))
            {
                email.Send("Superport cannot find fond code file.", "The parameter given for the fond code file is " + args[8] + " but cannot be found!");
                Environment.Exit(3);
            }

            logger.Write("Output directory : " + args[1] + "\\" + date);

            string[] banks = Directory.GetDirectories(args[0]);
            Array.Resize(ref banks, banks.Length + 1);
            banks[banks.Length - 1] = args[0] + "\\";
            ImpRecord impRecord = new ImpRecord(logger);
            impRecord.writeHead(args[1] + "\\" + date, date);

            for (int i = 0; i < banks.Length; i++)
            {
                logger.Write("Processing bank : " + banks[i].Substring(args[0].Length + 1), true);
                Directory.CreateDirectory(args[1] + "\\" + date + "\\" + banks[i].Substring(args[0].Length + 1));

                string[] files = Directory.GetFiles(banks[i]);
                if (files.Length == 0)
                {
                    logger.Write("  No files to process");
                    continue;
                }

                for (int j = 0; j < files.Length; j++)
                {
                    logger.Write("  Processing : " + files[j].Substring(banks[i].Length));
                    bool success = true;

                    string[] lines = System.IO.File.ReadAllLines(files[j]);

                    if (lines.Length > 0)
                    {
                        // If first line start with HEAD; then it is a Nykredit file, sometime whole lines is in quotes
                        if (lines[0].IndexOf("HEAD;") == 0 || lines[0].IndexOf("\"HEAD;") == 0)
                        {
                            Nykredit nykredit  = new Nykredit(lines, logger);
                            numberOfSupoerPortRecords += nykredit.Process(ref emailBody, ref debugLevel, ref success, args[1] + "\\" + date, banks[i]);
                        }
                        else if ((lines[0].IndexOf("\"DEPOTNR\";") == 0) || (lines[0].IndexOf("DEPOTNR;") == 0))
                        {
                            BankData jydskeBank = new BankData(lines, ref fondCode, logger);
                            numberOfSupoerPortRecords += jydskeBank.Process(ref emailBody, ref debugLevel, ref success, args[1] + "\\" + date, banks[i]);

                        }
                        else if ((lines[0].IndexOf("HEAD") == 0) && (lines[0][4] == 31))
                        {
                            DanskeBank danskeBank = new DanskeBank(lines, logger);
                            numberOfSupoerPortRecords += danskeBank.Process(ref emailBody, ref debugLevel, ref success, args[1] + "\\" + date);
                        }
                        else
                        {
                            success = false;
                            logger.Write("      Ukendt fil format");
                        }
                    }

                    // if process of file went ok, move to output folder
                    if (success)
                    {
                        Directory.Move(files[j], args[1] + "\\" + date + "\\" + banks[i].Substring(args[0].Length + 1) + "\\" + files[j].Substring(banks[i].Length + 1));
                    }
                }
            }
            impRecord.writeTail(args[1] + "\\" + date, numberOfSupoerPortRecords);

            logger.Write("", true);
            logger.Write("---- Depot successfully ----", true);
            logger.Write("Generated superport file in : "+ args[1] + "\\" + date, true);
            logger.Write("                        has : " + numberOfSupoerPortRecords + " records.", true);
            logger.Write("---- Ended ---------------------", true);

            if (emailBody != String.Empty)
            {
                email.Send("Superport converter has error.", emailBody);
                Environment.Exit(2);
            }

            email.Send("Superport Depot converted " + numberOfSupoerPortRecords + " records", 
                "Depot ended sucessfully, and converted " + numberOfSupoerPortRecords + " records.");
        }
    }
}
