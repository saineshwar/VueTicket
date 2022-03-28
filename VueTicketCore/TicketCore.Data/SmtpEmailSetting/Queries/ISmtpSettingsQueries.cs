using System.Linq;
using TicketCore.Models.SmtpEmailSettings;
using TicketCore.ViewModels.SmtpEmailSettings;

namespace TicketCore.Data.SmtpEmailSetting.Queries
{
    public interface ISmtpSettingsQueries
    {
        SmtpEmailSettings GetDefaultEmailSettings();
        SmtpEmailSettingsViewModel EditSmtpSettings(int? smtpProviderId);
        SmtpEmailSettings GetEmailSettings(int? smtpProviderId);
        IQueryable<SmtpEmailSettingsGrid> ShowAllSmtpSettings(string sortColumn, string sortColumnDir, string search);
    }
}