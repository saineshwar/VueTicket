using System.Threading.Tasks;

namespace TicketCore.Services.MailHelper
{
    public interface IMailingService
    {
        bool SendEmailAsync(SendingMailRequest mailRequest);
    }
}