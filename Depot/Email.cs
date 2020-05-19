using System;
using System.Net.Mail;

namespace Converter
{
    class Email
    {
        String server;
        int port;
        String username;
        String password;
        String toEmail;
        bool ssl;

        static Logger logger;

        public Email(String server, int port, String username, String password, String encryption, String toEmail, Logger l)
        {
            this.server = server;
            this.port = port;
            this.username = username;
            this.password = password;
            logger = l;
            this.ssl = (encryption.ToLower() == "true" || encryption.ToLower() == "yes");
            this.toEmail = toEmail;
        }

        public void Send(String subjet, String message)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(server);

                mail.From = new MailAddress("karstenbolge@builtit.dk");
                mail.To.Add(toEmail);
                mail.Subject = subjet;
                mail.Body = message;

                SmtpServer.Port = port;
                SmtpServer.Credentials = new System.Net.NetworkCredential(username, password);
                SmtpServer.EnableSsl = ssl;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                logger.Write("Error sending email: " + ex.ToString());
            }
        }
    }
}
