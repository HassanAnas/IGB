using System.Net;
using System.Net.Mail;

namespace IGB.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            try
            {
                _configuration = configuration;
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        public void SendEmail(string to, string subject, string body)
        {
            try
            {
            string smtpServer = _configuration["SmtpSettings:Server"];
            int port = int.Parse(_configuration["SmtpSettings:Port"]);
            string userName = _configuration["SmtpSettings:UserName"];
            string password = _configuration["SmtpSettings:Password"];
            string displayName = "Tihani"; // Change this to the name you want to display

            using (var client = new SmtpClient(smtpServer, port))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(userName, password);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(userName, displayName), // Include display name here
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(to);

                client.Send(mailMessage);
            }

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
