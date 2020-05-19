using System;
using System.IO;

namespace Converter
{
    public class Logger
    {
        private string path = string.Empty;

        public Logger(String s)
        {
            path = s;
        }

        public void WriteFields(string[] fields)
        {
            for(int i = 0; i < fields.Length; i++)
            {
                Write("Field : " + i + " value : " + fields[i], false);
            }
        }

        public void Write(string logMessage, bool console = false)
        {
            try
            {
                if (console) Console.Write(logMessage + "\n");
                using (StreamWriter w = File.AppendText(path + "\\" + "log.txt"))
                {
                    Write(logMessage, w);
                }
            }
            catch (Exception)
            {
            }
        }

        private void Write(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToUniversalTime(), logMessage);
            }
            catch (Exception)
            {
            }
        }
    }
}
