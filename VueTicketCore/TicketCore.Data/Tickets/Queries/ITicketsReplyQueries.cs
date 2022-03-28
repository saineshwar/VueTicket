using System.Collections.Generic;
using TicketCore.ViewModels.Tickets;

namespace TicketCore.Data.Tickets.Queries
{
    public interface ITicketsReplyQueries
    {
        List<ViewTicketReplyHistoryModel> ListofHistoryTicketReplies(long? ticketId);
    }
}