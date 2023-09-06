using System.Net.Mail;
using System.Net;

namespace MyFirstMVC
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "hussainkhozema110@gmail.com";
            var password = "RqaJPbnAtDH1v6UK";

            var client = new SmtpClient("smtp-relay.brevo.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mail, password)
            };

            return client.SendMailAsync(
                new MailMessage(from: mail,
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
