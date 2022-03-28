using TicketCore.Models.ApplicationLog;

namespace TicketCore.Data.ApplicationLog.Command
{
    public interface IEmailLogCommand
    {
        void InsertEmailLogs(EmailLogsModel objEmailLogsModel);
    }
}