using System.Collections.Generic;
using System.Threading.Tasks;
using TicketCore.Models.Tickets;
using TicketCore.ViewModels.Tickets;

namespace TicketCore.Data.Tickets.Command
{
    public interface ITicketCommand
    {
        Task<bool> AddTickets(long? userId, TicketSummary ticketSummary, TicketDetails ticketDetails,
           List<TicketAttachmentsViewModel> listofAttachment);

        Task<bool> AddTicketReply(long? userid, TicketReplyModel ticketReplyModel,
            TicketReplyDetailsModel ticketReplyDetailsModel,
            List<TicketAttachmentsViewModel> replyAttachment, TicketStatus ticketStatus,
            LatestTicketReplyStatusModel latestreply);

        bool DeleteAttachmentByAttachmentId(AttachmentsModel attachmentsModel,
            AttachmentDetailsModel attachmentDetailsModel);

        bool DeleteReplyAttachmentByAttachmentId(ReplyAttachmentModel replyAttachment,
            ReplyAttachmentDetailsModel replyAttachmentDetails);

        bool ChangeTicketPriority(RequestChangePriority requestChangePriority);

        Task<bool> UpdateTicket(long? userId, TicketsUserViewModel ticketsViewModel,
            List<TicketAttachmentsViewModel> listofAttachment);

        Task<bool> UpdateTicketReply(EditTicketReplyViewModel ticketsViewModel,
            List<TicketAttachmentsViewModel> replyAttachment, long? systemUser, long? ticketUser);

        Task<bool> UpdateResponseStatus(long? userId, long? ticketId, int? statusId);
        Task<bool> UpdateAssignTickettoUser(long userId, long ticketId);
        Task<bool> TransferDepartment(int? departmentId, long? ticketId);
        Task<bool> DeleteTicket(long? userId, long? ticketId, int? statusId);
        Task<bool> UndoDeleteTicket(long? ticketId);
        Task<bool> AssignTicketManually(RequestManualAssignViewModel requestmodel); 
        Task<bool> ReOpenTicket(long ticketId);
    }
}