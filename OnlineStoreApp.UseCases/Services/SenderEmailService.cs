using Microsoft.Extensions.Options;
using OnlineStore.DTOs;
using OnlineStoreApp.UseCases.Helpers;
using OnlineStoreApp.UseCases.Interfaces;
using System.Net;
using System.Net.Mail;

namespace OnlineStoreApp.UseCases.Services
{
    public class SenderEmailService : ISenderEmailService
    {
        private readonly EmailProvider _emailProvider;

        public SenderEmailService(IOptions<EmailProvider> keyValuesConfiguration)
        {
            _emailProvider = keyValuesConfiguration.Value;
        }

        public async Task SendEmail(string to, string buyer, OrderResponseDTO orderResponse)
        {

            string sender = _emailProvider.EmailSender;
            string password = _emailProvider.KeyEmail;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(sender, password),
                EnableSsl = true,
            };

            var body = $"Dear {buyer},\n\nWe are pleased to confirm your order for the following items:\n\n";

            foreach (var item in orderResponse.orderDetailDtos)
            {
                body += $"-{item.Food}--quantity: {item.Quantity}--price: {item.SubTotal}\n";
            }

            body += $"\nTotal number of items: {orderResponse.orderDetailDtos.Count}\n";
            body += $"Total purchase amount: {orderResponse.Orden.Total}\n\n";
            body += "Thank you so much for your purchase.\n\nIf you have any questions or special requests, please feel free to contact us. We are here to assist you with anything you need.\n\nThank you again for choosing us!\n\nSincerely,\nStoreApp";

            var message = new MailMessage(sender, to)
            {
                Subject = "Order Confirmation",
                Body = body
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
