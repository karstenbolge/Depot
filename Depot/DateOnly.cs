using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter
{
    class DateOnly
    {

        Int16 date = 1;
        Int16 month = 1;
        Int16 year = 0;

        static Logger logger;
        const String spaces = "                                            ";

        public DateOnly(Logger l)
        {
            logger = l;
        }

        public void setDate(String s)
        {
            if (s.Length < 6)
            {
                logger.Write("Date to short : " + s);
                return;
            }

            if (s.Length == 6)
            {
                try
                {
                    date = Int16.Parse(s.Substring(0, 2));
                    if (date > 31)
                    {
                        logger.Write("Wrong date: " + date);
                        date = 31;
                    }

                    month = Int16.Parse(s.Substring(2, 2));
                    if (month > 12)
                    {
                        logger.Write("Wrong month: " + month);
                        month = 12;
                    }

                    year = Int16.Parse(s.Substring(4, 2));

                    if (date > 30 && (month == 4 || month == 6 || month == 9 || month == 11))
                    {
                        logger.Write("Wrong date: " + date);
                        date = 30;
                    }

                    if (date > 29 && month == 2)
                    {
                        logger.Write("Wrong date: " + date);
                        date = 29;
                    }
                }
                catch (FormatException)
                {
                    logger.Write("Wrong date formata : " + s);
                }
            }

            if (s.Length == 10)
            {
                if (s[2] == '-')
                {
                    try
                    {
                        date = Int16.Parse(s.Substring(0, 2));
                        if (date > 31)
                        {
                            logger.Write("Wrong date: " + date);
                            date = 31;
                        }

                        month = Int16.Parse(s.Substring(3, 2));
                        if (month > 12)
                        {
                            logger.Write("Wrong month: " + month);
                            month = 12;
                        }

                        year = Int16.Parse(s.Substring(8, 2));

                        if (date > 30 && (month == 4 || month == 6 || month == 9 || month == 11))
                        {
                            logger.Write("Wrong date: " + date);
                            date = 30;
                        }

                        if (date > 29 && month == 2)
                        {
                            logger.Write("Wrong date: " + date);
                            date = 29;
                        }
                    }
                    catch (FormatException)
                    {
                        logger.Write("Wrong date formata : " + s);
                    }
                } else if (s[4] == '-') {
                    try
                    {
                        date = Int16.Parse(s.Substring(8, 2));
                        if (date > 31)
                        {
                            logger.Write("Wrong date: " + date);
                            date = 31;
                        }

                        month = Int16.Parse(s.Substring(5, 2));
                        if (month > 12)
                        {
                            logger.Write("Wrong month: " + month);
                            month = 12;
                        }

                        year = Int16.Parse(s.Substring(2, 2));

                        if (date > 30 && (month == 4 || month == 6 || month == 9 || month == 11))
                        {
                            logger.Write("Wrong date: " + date);
                            date = 30;
                        }

                        if (date > 29 && month == 2)
                        {
                            logger.Write("Wrong date: " + date);
                            date = 29;
                        }
                    }
                    catch (FormatException)
                    {
                        logger.Write("Wrong date formata : " + s);
                    }
                }
            }

            if (s.Length == 8)
            {
                try
                {
                    date = Int16.Parse(s.Substring(6, 2));
                    if (date > 31)
                    {
                        logger.Write("Wrong date: " + date);
                        date = 31;
                    }

                    month = Int16.Parse(s.Substring(4, 2));
                    if (month > 12)
                    {
                        logger.Write("Wrong month: " + month);
                        month = 12;
                    }

                    year = Int16.Parse(s.Substring(2, 2));

                    if (date > 30 && (month == 4 || month == 6 || month == 9 || month == 11))
                    {
                        logger.Write("Wrong date: " + date);
                        date = 30;
                    }

                    if (date > 29 && month == 2)
                    {
                        logger.Write("Wrong date: " + date);
                        date = 29;
                    }
                }
                catch (FormatException)
                {
                    logger.Write("Wrong date formata : " + s);
                }
            }
        }

        public String getDate()
        {
            return date.ToString("00") + month.ToString("00") + year.ToString("00");
        }
    }
}
