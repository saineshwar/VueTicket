using TicketCore.Models.SmtpEmailSettings;

namespace TicketCore.Data.SmtpEmailSetting.Command
{
    public interface IGeneralSettingsCommand
    {
        void InsertGeneralSetting(GeneralSettings generalSettings);
        void UpdateGeneralSetting(GeneralSettings generalSettings);
    }
}