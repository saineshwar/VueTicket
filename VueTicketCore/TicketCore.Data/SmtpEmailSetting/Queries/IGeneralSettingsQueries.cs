using TicketCore.ViewModels.SmtpEmailSettings;

namespace TicketCore.Data.SmtpEmailSetting.Queries
{
    public interface IGeneralSettingsQueries
    {
        GeneralSettingsViewModel GetGeneralSetting();
    }
}