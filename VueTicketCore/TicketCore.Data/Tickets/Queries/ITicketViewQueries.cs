using System.Collections.Generic;
using TicketCore.Models.Tickets;
using TicketCore.ViewModels.Tickets;

namespace TicketCore.Data.Tickets.Queries
{
    public interface ITicketViewQueries
    {
        TicketDetailsViewModel TicketsDetailsbyticketId(long? ticketId);
        bool CheckTrackingIdExists(long? ticketId);
        EscalatedUserViewModel GetTicketEscalatedToUserNames(long? ticketId);
        List<ReplyAttachmentModel> GetListReplyAttachmentsByAttachmentId(long? ticketId, long? ticketReplyId);
        List<TicketHistoryResponse> ListofTicketHistorybyTicket(long? ticketId);
    }
}