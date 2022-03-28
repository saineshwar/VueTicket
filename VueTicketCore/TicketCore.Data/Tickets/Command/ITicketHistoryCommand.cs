using TicketCore.Models.Tickets;

namespace TicketCore.Data.Tickets.Command
{
    public interface ITicketHistoryCommand
    {
        void TicketHistory(TicketHistoryModel ticketHistory);
    }
}