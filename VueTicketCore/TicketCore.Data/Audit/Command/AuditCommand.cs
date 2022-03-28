using TicketCore.Models.Audit;

namespace TicketCore.Data.Audit.Command
{
    public class AuditCommand : IAuditCommand
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public AuditCommand(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }

        public void InsertAuditData(AuditModel objaudittb)
        {
            try
            {
                _vueTicketDbContext.AuditModel.Add(objaudittb);
                _vueTicketDbContext.SaveChanges();

            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}