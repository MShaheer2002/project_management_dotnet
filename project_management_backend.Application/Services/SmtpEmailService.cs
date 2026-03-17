
using System.Net;
using System.Net.Mail;

namespace project_management_backend.Application.Interface
{
    public class SmtpEmailService : IEmailService
    {
        private readonly string host;
        private readonly int port;
        private readonly string user;
        private readonly string pass;
        private readonly string fromEmail;

        public SmtpEmailService(string host, int port, string user, string pass, string fromEmail)
        {
            this.host = host;
            this.port = port;
            this.user = user;
            this.pass = pass;
            this.fromEmail = fromEmail;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
           using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(user,pass),
                EnableSsl = true,
            };

            var mail = new MailMessage(fromEmail, toEmail, subject, body)
            {
                IsBodyHtml = true,
            };

            await client.SendMailAsync(mail);

        }
    }
}