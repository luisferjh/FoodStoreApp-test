using OnlineStoreApp.UseCases.Interfaces;
using System.Net;
using System.Net.Mail;

namespace OnlineStoreApp.UseCases.Services
{
    public class SenderEmailService : ISenderEmailService
    {
        public SenderEmailService()
        {

        }

        public async Task SendEmail(string to, string buyer)
        {

            string sender = to;
            string password = "tucontraseña";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(sender, password),
                EnableSsl = true,
            };

            var message = new MailMessage(sender, to)
            {
                Subject = "Invoice Receipt",
                Body = $"Congratulations for you purchase {buyer}"
            };

            try
            {
                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                return;
            }

        }
    }
}
