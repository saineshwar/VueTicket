using TicketCore.Models.SmtpEmailSettings;

namespace TicketCore.Data.SmtpEmailSetting.Command
{
    public interface ISmtpSettingsCommand
    {
        int SaveSmtpSettings(SmtpEmailSettings smtpEmailSettings);
        int UpdateSmtpSettings(SmtpEmailSettings smtpEmailSettings);
        int SettingDefaultConnection(int? smtpProviderId);
    }
}