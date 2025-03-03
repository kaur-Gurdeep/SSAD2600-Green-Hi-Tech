using SendGrid.Helpers.Mail;
using SendGrid;
using GreenHiTech.ViewModels;

namespace GreenHiTech.Data.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Response> SendSingleEmail(EmailVM payload)
        {
            var apiKey = _configuration["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("gagakaur188@gmail.com",
                                        "Gurdeep Kaur");
            var to = new EmailAddress(payload.Email);

            var msg = MailHelper.CreateSingleEmail(from, to, payload.Subject,
                                                   payload.Body, payload.Body);

            return await client.SendEmailAsync(msg);
        }

    }
}
