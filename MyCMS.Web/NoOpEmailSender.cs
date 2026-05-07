using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace MyCMS.Web
{
    public class NoOpEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // No-op email sender - does nothing
            return Task.CompletedTask;
        }
    }
}
