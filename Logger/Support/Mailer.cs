using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Luilliarcec.Logger.Support
{
    class Mailer
    {
        /// <summary>
        /// Sender name
        /// </summary>
        private string MailFromName { get; set; }

        /// <summary>
        /// Sender email
        /// </summary>
        private string MailUsername { get; set; }

        /// <summary>
        /// Sender password
        /// </summary>
        private string MailPassword { get; set; }

        /// <summary>
        /// Receiver email
        /// </summary>
        private string MailTo { get; set; }

        /// <summary>
        /// Mailer constructor without parameters
        /// </summary>
        public Mailer()
        {
            MailFromName = ConfigurationManager.AppSettings.Get("MAIL_FROM_NAME");
            MailUsername = ConfigurationManager.AppSettings.Get("MAIL_USERNAME");
            MailPassword = ConfigurationManager.AppSettings.Get("MAIL_PASSWORD");
            MailTo = ConfigurationManager.AppSettings.Get("MAIL_TO");
        }

        /// <summary>
        /// Mailer constructor receives parameters
        /// </summary>
        /// <param name="from_name">Sender name</param>
        /// <param name="username">Sender email</param>
        /// <param name="password">Sender password</param>
        /// <param name="to">Receiver email</param>
        public Mailer(string from_name, string username, string password, string to)
        {
            MailFromName = from_name;
            MailUsername = username;
            MailPassword = password;
            MailTo = to;
        }

        /// <summary>
        /// Create the email with the main data
        /// </summary>
        /// <returns>MailMessage</returns>
        private MailMessage Make()
        {
            var mail = new MailMessage
            {
                SubjectEncoding = Encoding.UTF8,
                Subject = $"{MailFromName} - Errors Log - {DateTime.Now}",
                IsBodyHtml = true,
            };

            mail.To.Clear();
            mail.To.Add(MailTo);
            mail.From = new MailAddress(MailUsername, MailFromName, Encoding.UTF8);

            return mail;
        }

        /// <summary>
        /// Send the mail asynchronously
        /// </summary>
        /// <param name="path">Path file</param>
        /// <param name="SendCompleted">Event that runs when mail delivery is complete</param>
        public void SendAsync(string path, SendCompletedEventHandler SendCompleted)
        {
            var file = new Attachment(path);
            var mail = Make();
            mail.Attachments.Add(file);

            var client = new SmtpClient
            {
                Credentials = new NetworkCredential(MailUsername, MailPassword),
                Host = MailUsername.Contains("gmail") ? "smtp.gmail.com" : "smtp-mail.outlook.com",
                Port = 587,
                EnableSsl = true
            };

            client.SendCompleted += (s, e) =>
            {
                client.Dispose();
                mail.Dispose();
            };
            client.SendCompleted += SendCompleted;

            client.SendAsync(mail, mail);
        }
    }
}
