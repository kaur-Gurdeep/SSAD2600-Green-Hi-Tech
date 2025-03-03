using GreenHiTech.ViewModels;
using SendGrid;
namespace GreenHiTech.Data.Services
{
    public interface IEmailService
    {
        Task<Response> SendSingleEmail(EmailVM payload);
    }
}
