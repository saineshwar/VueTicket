using TicketCore.Models.Audit;

namespace TicketCore.Data.Audit.Command
{
    public interface IAuditCommand
    {
        void InsertAuditData(AuditModel objaudittb);
    }
}