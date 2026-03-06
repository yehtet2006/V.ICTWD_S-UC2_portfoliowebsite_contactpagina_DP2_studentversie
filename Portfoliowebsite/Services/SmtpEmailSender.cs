using System.Net;
using System.Net.Mail;

namespace Portfoliowebsite.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        public async Task SendAsync(string Name, string Email, string Subject, string Message)
        {
            var smtp = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                EnableSsl = true,
                // Mailtrap gebruikt geen SSL, dus we zetten het uit
                Credentials = new NetworkCredential("dfe430e3142769", "4f0c583ffe0815"),
            };

            var mail = new MailMessage();
            mail.From = new MailAddress("noreply@example.com", "Website");

            mail.To.Add("contact@example.com");

            mail.Subject = $"Contact: {Subject}";
            mail.Body = $"Naam: {Name}\nEmail: {Email}\nBericht:\n{Message}";

            await smtp.SendMailAsync(mail);
        }
    }
}