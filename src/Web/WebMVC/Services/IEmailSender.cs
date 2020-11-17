using System.Threading.Tasks;

namespace ASPNET_Core_1_0.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
