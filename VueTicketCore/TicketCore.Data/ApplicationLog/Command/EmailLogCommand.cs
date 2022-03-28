using TicketCore.Models.ApplicationLog;

namespace TicketCore.Data.ApplicationLog.Command
{
    public class EmailLogCommand : IEmailLogCommand
    {
        private readonly VueTicketDbContext _vueTicketDb;
        public EmailLogCommand(VueTicketDbContext vueTicketDb)
        {
            _vueTicketDb = vueTicketDb;
        }

        public void InsertEmailLogs(EmailLogsModel objEmailLogsModel)
        {
            try
            {
                objEmailLogsModel.EmailLogId = 0;
                _vueTicketDb.EmailLogsModel.Add(objEmailLogsModel);
                _vueTicketDb.SaveChanges();

            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}