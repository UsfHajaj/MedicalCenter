using MedicalCenter.Services.Interfaces;
using System.Net.Mail;

namespace MedicalCenter.Services.impelementation
{
    public class EmailService:IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(Message massage)
        {
            try
            {
                var emailUserName=_configuration["EmailSettings:EmailUsername"];
                var emailPassword = _configuration["EmailSettings:EmailPassword"];

                using var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential(emailUserName, emailPassword),
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                var emailMassage =new MailMessage
                {
                    From = new MailAddress(emailUserName),
                    Subject = massage.Subject,
                    Body = massage.Body,
                    IsBodyHtml = true,
                };
                foreach (var item in massage.To)
                {
                    emailMassage.To.Add(item);
                }
                smtpClient.Send(emailMassage);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
                throw;
            }
        }
    }
}
