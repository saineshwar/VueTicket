using System.Collections.Generic;
using System.Threading.Tasks;
using TicketCore.Models.Tickets;
using TicketCore.ViewModels.Tickets;
using TicketCore.ViewModels.Tickets.Grids;

namespace TicketCore.Data.Tickets.Queries
{
    public interface ITicketQueries
    {
        #region User
        int GetUserEndTicketsCount(long createdBy, string search, int? statusId, int? searchin, int? departmentId, int? priorityId);

        List<UserTicketGridViewModel> GetUserEndTicketList(long createdBy, int? currentpage, int? pageSize,
            int? departmentId, string search,
            int? searchin,
            int? statusId, int? priorityId);

        #endregion

        #region Agent
        int GetAgentEndTicketsCount(long createdBy, string search, int? statusId, int? searchin, int? departmentId,
          int? priorityId);

        List<UserTicketGridViewModel> GetAgentEndTicketList(long createdBy, int? currentpage, int? pageSize,
            int? departmentId, string search,
            int? searchin,
            int? statusId, int? priorityId);

        int GetAllClosedAgentEndTicketsCount(long createdBy, string search, int? searchin,
            int? departmentId, int? priorityId);

        List<UserTicketGridViewModel> GetAllClosedAgentEndTicketList(long createdBy, int? currentpage, int? pageSize,
            int? departmentId, string search,
            int? searchin,
            int? priorityId);

        #endregion

        #region Agent Created Ticket
        int GetAgentEndCreatedTicketsCount(long createdBy, string search, int? statusId, int? searchin,
            int? departmentId, int? priorityId);

        List<UserTicketGridViewModel> GetAgentEndCreatedTicketList(long createdBy, int? currentpage, int? pageSize,
            int? departmentId, string search,
            int? searchin,
            int? statusId, int? priorityId);
        #endregion

        #region AgentManager

        int GetAgentManagerEndTicketsCount(long? createdBy, string search, int? statusId, int? searchin,
          int? departmentId, int? priorityId);

        List<AgentManagerTicketGridViewModel> GetAgentManagerEndTicketList(long? createdBy, int? currentpage, int? pageSize,
            int? departmentId, string search,
            int? searchin,
            int? statusId, int? priorityId);

        int GetAllDeletedAgentManagerEndTicketsCount(long? createdBy, string search, int? searchin, int? departmentId,
            int? priorityId);

        List<AgentManagerTicketGridViewModel> GetAllDeletedAgentManagerEndTicketList(long? createdBy, int? currentpage,
            int? pageSize, int? departmentId, string search,
            int? searchin,
            int? priorityId);

        int GetAllDeletedAgentEndTicketsCount(long createdBy, string search, int? searchin,
            int? departmentId, int? priorityId);

        List<UserTicketGridViewModel> GetAllDeletedAgentEndTicketList(long createdBy, int? currentpage, int? pageSize,
            int? departmentId, string search,
            int? searchin,
            int? priorityId);
        #endregion

        #region Administrator
        int GetAdministratorEndTicketsCount(long? createdBy, string search, int? statusId, int? searchin,
            int? departmentId, int? priorityId);

        List<AgentManagerTicketGridViewModel> GetAdministratorEndTicketList(long? createdBy, int? currentpage,
            int? pageSize, int? departmentId, string search,
            int? searchin,
            int? statusId, int? priorityId);


        #endregion


        List<AttachmentsModel> GetListAttachmentsByticketId(long? ticketId);
        Task<TicketStatus> GetTicketStatusbyTicketId(long? ticketId);
        AttachmentsModel GetAttachmentsByticketId(long ticketId, long attachmentId);
        AttachmentDetailsModel GetAttachmentDetailsByAttachmentId(long ticketId, long attachmentId);
        ReplyAttachmentModel GetReplyAttachmentsByTicketId(long? ticketId, long? replyAttachmentId);
        ReplyAttachmentDetailsModel GetReplyAttachmentDetailsByAttachmentId(long? ticketId, long? replyAttachmentId);


        EditTicketViewModel GetTicketbyTicketId(long? ticketId);
        bool CheckAttachmentsExistbyTicketId(long? ticketId);
        EditTicketReplyViewModel GetTicketReplyDetailsbyTicketId(long? ticketId, long? ticketReplyId);
        List<ReplyAttachmentModel> GetReplyAttachmentsListByTicketId(long? ticketId, long? ticketReplyId);
        bool ReplyAttachmentsExistbyTicketId(long? ticketId, long? ticketReplyId);
        TicketStatusResponse GetCurrentStatusResponse(long? ticketId);

        int GetAllUnAssignedTicketCount(string search, int? statusId, int? searchin, int? departmentId,
            int? priorityId);

        List<AgentManagerTicketGridViewModel> GetAllUnAssignedTicketList(int? currentpage, int? pageSize,
            int? departmentId, string search,
            int? searchin,
            int? statusId, int? priorityId);
        Task<LatestTicketReplyStatusModel> GetLatestTicketReplybyId(long? ticketId);

        int GetAllClosedAdministratorEndTicketsCount(long? createdBy, string search, int? statusId, int? searchin,
            int? departmentId, int? priorityId);

        List<AgentManagerTicketGridViewModel> GetAllClosedAdministratorEndTicketList(long? createdBy, int? currentpage,
            int? pageSize, int? departmentId, string search,
            int? searchin,
            int? statusId, int? priorityId);

        int GetAllClosedAgentManagerEndTicketsCount(long? createdBy, string search, int? searchin, int? departmentId,
            int? priorityId);

        List<AgentManagerTicketGridViewModel> GetAllClosedAgentManagerEndTicketList(long? createdBy, int? currentpage,
            int? pageSize, int? departmentId, string search,
            int? searchin,
            int? priorityId);

        TicketDetailsEmailViewModel GetCreatedTicketUserDetails(long? ticketId);
        TicketDetailsEmailViewModel GetAssignedAgentDetails(long? ticketId);
    }
}