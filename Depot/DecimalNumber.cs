using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter
{
    class DecimalNumber
    {
        int cifre;
        int decimals;
        bool allowNegative;
        bool isBlank;
        bool isNegative;
        String pre;
        String post;
        const String zeros = "0000000000000000000000000000000";
        const String blanks = "                               ";
        Logger logger;
        bool noLeadingZeros;

        public DecimalNumber(int cifre, int decimals, bool allowNegative, bool noLeadingZeros, Logger logger)
        {
            this.cifre = cifre;
            this.decimals = decimals;
            this.allowNegative = allowNegative;
            this.logger = logger;
            this.noLeadingZeros = noLeadingZeros;


            isBlank = false;
            pre = string.Empty;
            post = string.Empty;
        }

        public void setBlank()
        {
            isBlank = true;
        }

        public void setDecimalNumber(String s)
        {
            pre = string.Empty;
            post = string.Empty;
            isNegative = false;

            if (allowNegative)
            {
                if (s.IndexOf('-') != -1)
                {
                    isNegative = true;
                    s = s.Replace("-", string.Empty);
                }
            }

            int i = 0;
            while (i < s.Length)
            {
                if ((s[i] >= '0' && s[i] <= '9') || s[i] == '.' || s[i] == ',')
                {
                    i++;
                }
                else
                {
                    s = s.Replace(s[i].ToString(), string.Empty); ;
                }
            }

            int point = s.IndexOf('.');
            if (point == -1)
            {
                point = s.IndexOf(',');
            }

            if (point == -1)
            {
                point = s.Length;
            }
            
            pre = s.Substring(max(0, point - cifre), min(point, cifre));
            if (noLeadingZeros)
            {
                while (pre.Length > 1 && pre.IndexOf('0') == 0) pre = pre.Substring(1);
            }
                
            if (point < s.Length)
            {
                post = s.Substring(point + 1, min(s.Length - point - 1, decimals));
            }
        }

        public String getDecimalNumber()
        {
            if (noLeadingZeros)
            {
                if (isBlank) return "0.00";

                return (isNegative ? "-" : "") + pre + (decimals > 0 ? "." : "") + post + zeros.Substring(0, minZero(decimals - post.Length));
            }

            if (isBlank)
            {
                return blanks.Substring(0, cifre + decimals + 1);
            }

            if (isNegative)
            {
                return zeros.Substring(0, minZero(cifre - pre.Length)) + pre + (decimals > 0 ? "." : "") + post + zeros.Substring(0, minZero(decimals - post.Length - 1)) + "-";
            }

            return zeros.Substring(0, minZero(cifre - pre.Length)) + pre + (decimals > 0?  "." : "") + post + zeros.Substring(0, minZero(decimals - post.Length));
        }

        private int minZero(int x)
        {
            return x < 0 ? 0 : x;
        }

        private int min(int x, int y)
        {
            return x < y ? x : y;
        }

        private int max(int x, int y)
        {
            return x > y ? x : y;
        }
    }
}
