using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using TicketCore.Models.Tickets;
using TicketCore.ViewModels.Tickets;
using TicketCore.ViewModels.Tickets.Grids;

namespace TicketCore.Data.Tickets.Queries
{
    public class TicketQueries : ITicketQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        private readonly IConfiguration _configuration;
        public TicketQueries(VueTicketDbContext vueTicketDbContext, IConfiguration configuration)
        {
            _vueTicketDbContext = vueTicketDbContext;
            _configuration = configuration;
        }

        #region User
        public int GetUserEndTicketsCount(long createdBy, string search, int? statusId, int? searchin, int? departmentId, int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@UserId", createdBy);
            param.Add("@search", search);
            param.Add("@searchin", searchin);
            param.Add("@statusId", statusId);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            return con.Query<int>("Usp_Ticket_UserEnd_Grid_Count", param, null, false, 0,
                    CommandType.StoredProcedure).FirstOrDefault();

        }

        public List<UserTicketGridViewModel> GetUserEndTicketList(long createdBy, int? currentpage, int? pageSize, int? departmentId, string search,
            int? searchin,
            int? statusId, int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@CreatedBy", createdBy);
            param.Add("@Currentpage", currentpage);
            param.Add("@PageSize", pageSize);
            param.Add("@Search", search);
            param.Add("@Searchin", searchin);
            param.Add("@StatusId", statusId);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            var data = con.Query<UserTicketGridViewModel>("Usp_Ticket_UserEnd_Grid_List", param, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }
        #endregion

        #region Agent
        public int GetAgentEndTicketsCount(long createdBy, string search, int? statusId, int? searchin, int? departmentId, int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@UserId", createdBy);
            param.Add("@search", search);
            param.Add("@searchin", searchin);
            param.Add("@statusId", statusId);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            return con.Query<int>("Usp_Tickets_GetTicketsAgentCount", param, null, false, 0,
                CommandType.StoredProcedure).FirstOrDefault();

        }

        public List<UserTicketGridViewModel> GetAgentEndTicketList(long createdBy, int? currentpage, int? pageSize, int? departmentId, string search,
            int? searchin,
            int? statusId, int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@CreatedBy", createdBy);
            param.Add("@Currentpage", currentpage);
            param.Add("@PageSize", pageSize);
            param.Add("@Search", search);
            param.Add("@Searchin", searchin);
            param.Add("@StatusId", statusId);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            var data = con.Query<UserTicketGridViewModel>("Usp_Tickets_GetTicketsAgent_Grid_List", param, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }


        public int GetAllDeletedAgentEndTicketsCount(long createdBy, string search, int? searchin, int? departmentId, int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@UserId", createdBy);
            param.Add("@search", search);
            param.Add("@searchin", searchin);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            return con.Query<int>("Usp_Tickets_GetAllDeletedTicketsAgentCount", param, null, false, 0,
                CommandType.StoredProcedure).FirstOrDefault();

        }


        public List<UserTicketGridViewModel> GetAllDeletedAgentEndTicketList(long createdBy, int? currentpage, int? pageSize, int? departmentId, string search,
            int? searchin,
            int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@CreatedBy", createdBy);
            param.Add("@Currentpage", currentpage);
            param.Add("@PageSize", pageSize);
            param.Add("@Search", search);
            param.Add("@Searchin", searchin);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            var data = con.Query<UserTicketGridViewModel>("Usp_Tickets_GetAllDeletedTicketsAgent_Grid_List", param, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }



        public int GetAllClosedAgentEndTicketsCount(long createdBy, string search, int? searchin, int? departmentId, int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@UserId", createdBy);
            param.Add("@search", search);
            param.Add("@searchin", searchin);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            return con.Query<int>("Usp_Tickets_GetAllClosedTicketsAgentCount", param, null, false, 0,
                CommandType.StoredProcedure).FirstOrDefault();

        }

        public List<UserTicketGridViewModel> GetAllClosedAgentEndTicketList(long createdBy, int? currentpage, int? pageSize, int? departmentId, string search,
            int? searchin,
            int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@CreatedBy", createdBy);
            param.Add("@Currentpage", currentpage);
            param.Add("@PageSize", pageSize);
            param.Add("@Search", search);
            param.Add("@Searchin", searchin);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            var data = con.Query<UserTicketGridViewModel>("Usp_Tickets_GetAllClosedTicketsAgent_Grid_List", param, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }



        #endregion

        #region Agent Created Ticket
        public int GetAgentEndCreatedTicketsCount(long createdBy, string search, int? statusId, int? searchin, int? departmentId, int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@UserId", createdBy);
            param.Add("@search", search);
            param.Add("@searchin", searchin);
            param.Add("@statusId", statusId);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            return con.Query<int>("Usp_Tickets_GetCreatedTicketsAgentCount", param, null, false, 0,
                CommandType.StoredProcedure).FirstOrDefault();

        }


        public List<UserTicketGridViewModel> GetAgentEndCreatedTicketList(long createdBy, int? currentpage, int? pageSize, int? departmentId, string search,
            int? searchin,
            int? statusId, int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@CreatedBy", createdBy);
            param.Add("@Currentpage", currentpage);
            param.Add("@PageSize", pageSize);
            param.Add("@Search", search);
            param.Add("@Searchin", searchin);
            param.Add("@StatusId", statusId);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            var data = con.Query<UserTicketGridViewModel>("Usp_Tickets_GetCreatedTicketsAgent_Grid_List", param, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }
        #endregion

        #region AgentManager

        public int GetAgentManagerEndTicketsCount(long? createdBy, string search, int? statusId, int? searchin, int? departmentId, int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@AgentUserId", createdBy);
            param.Add("@search", search);
            param.Add("@searchin", searchin);
            param.Add("@statusId", statusId);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            return con.Query<int>("Usp_Tickets_GetTicketsAgentManagerCount", param, null, false, 0,
                CommandType.StoredProcedure).FirstOrDefault();

        }

        public List<AgentManagerTicketGridViewModel> GetAgentManagerEndTicketList(long? createdBy, int? currentpage, int? pageSize, int? departmentId, string search,
            int? searchin,
            int? statusId, int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@AgentUserId", createdBy);
            param.Add("@Currentpage", currentpage);
            param.Add("@PageSize", pageSize);
            param.Add("@Search", search);
            param.Add("@Searchin", searchin);
            param.Add("@StatusId", statusId);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            var data = con.Query<AgentManagerTicketGridViewModel>("Usp_Tickets_GetTicketsAgentManager_Grid_List", param, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }


        public int GetAllDeletedAgentManagerEndTicketsCount(long? createdBy, string search, int? searchin, int? departmentId, int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@AgentUserId", createdBy);
            param.Add("@search", search);
            param.Add("@searchin", searchin);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            return con.Query<int>("Usp_Tickets_GetAllDeletedTicketsAgentManagerCount", param, null, false, 0,
                CommandType.StoredProcedure).FirstOrDefault();

        }

        public List<AgentManagerTicketGridViewModel> GetAllDeletedAgentManagerEndTicketList(long? createdBy, int? currentpage, int? pageSize, int? departmentId, string search,
            int? searchin,
             int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@AgentUserId", createdBy);
            param.Add("@Currentpage", currentpage);
            param.Add("@PageSize", pageSize);
            param.Add("@Search", search);
            param.Add("@Searchin", searchin);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            var data = con.Query<AgentManagerTicketGridViewModel>("Usp_Tickets_GetAllDeletedTicketsAgentManager_Grid_List", param, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }



        #endregion

        #region Administrator

        public int GetAdministratorEndTicketsCount(long? createdBy, string search, int? statusId, int? searchin, int? departmentId, int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@AgentUserId", createdBy);
            param.Add("@search", search);
            param.Add("@searchin", searchin);
            param.Add("@statusId", statusId);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            return con.Query<int>("Usp_Tickets_GetTicketsAdministratorCount", param, null, false, 0,
                CommandType.StoredProcedure).FirstOrDefault();

        }

        public List<AgentManagerTicketGridViewModel> GetAdministratorEndTicketList(long? createdBy, int? currentpage, int? pageSize, int? departmentId, string search,
            int? searchin,
            int? statusId, int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@AgentUserId", createdBy);
            param.Add("@Currentpage", currentpage);
            param.Add("@PageSize", pageSize);
            param.Add("@Search", search);
            param.Add("@Searchin", searchin);
            param.Add("@StatusId", statusId);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            var data = con.Query<AgentManagerTicketGridViewModel>("Usp_Tickets_GetTicketsAdministrator_Grid_List", param, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }


        public int GetAllClosedAdministratorEndTicketsCount(long? createdBy, string search, int? statusId, int? searchin, int? departmentId, int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@AgentUserId", createdBy);
            param.Add("@search", search);
            param.Add("@searchin", searchin);
            param.Add("@statusId", statusId);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            return con.Query<int>("Usp_Tickets_GetAllClosedTicketsAdministratorCount", param, null, false, 0,
                CommandType.StoredProcedure).FirstOrDefault();

        }

        public List<AgentManagerTicketGridViewModel> GetAllClosedAdministratorEndTicketList(long? createdBy, int? currentpage, int? pageSize, int? departmentId, string search,
            int? searchin,
            int? statusId, int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@AgentUserId", createdBy);
            param.Add("@Currentpage", currentpage);
            param.Add("@PageSize", pageSize);
            param.Add("@Search", search);
            param.Add("@Searchin", searchin);
            param.Add("@StatusId", statusId);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            var data = con.Query<AgentManagerTicketGridViewModel>("Usp_Tickets_GetAllClosedTicketsAdministrator_Grid_List", param, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }



        #endregion


        public List<AttachmentsModel> GetListAttachmentsByticketId(long? ticketId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _vueTicketDbContext.AttachmentsModel.AsNoTracking()
                                       where attachments.TicketId == ticketId
                                       select attachments).ToList();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TicketStatus> GetTicketStatusbyTicketId(long? ticketId)
        {
            try
            {
                var ticketStatus = await (from ticketstatus in _vueTicketDbContext.TicketStatus.AsNoTracking()
                                          where ticketstatus.TicketId == ticketId
                                          select ticketstatus).FirstOrDefaultAsync();
                return ticketStatus;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AttachmentsModel GetAttachmentsByticketId(long ticketId, long attachmentId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _vueTicketDbContext.AttachmentsModel.AsNoTracking()
                                       where attachments.TicketId == ticketId && attachments.AttachmentId == attachmentId
                                       select attachments).FirstOrDefault();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AttachmentDetailsModel GetAttachmentDetailsByAttachmentId(long ticketId, long attachmentId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _vueTicketDbContext.AttachmentDetailsModel.AsNoTracking()
                                       where attachments.TicketId == ticketId && attachments.AttachmentId == attachmentId
                                       select attachments).FirstOrDefault();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public ReplyAttachmentModel GetReplyAttachmentsByTicketId(long? ticketId, long? replyAttachmentId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _vueTicketDbContext.ReplyAttachmentModel.AsNoTracking()
                                       where attachments.TicketId == ticketId && attachments.ReplyAttachmentId == replyAttachmentId
                                       select attachments).FirstOrDefault();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ReplyAttachmentDetailsModel GetReplyAttachmentDetailsByAttachmentId(long? ticketId, long? replyAttachmentId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _vueTicketDbContext.ReplyAttachmentDetailsModel.AsNoTracking()
                                       where attachments.TicketId == ticketId && attachments.ReplyAttachmentId == replyAttachmentId
                                       select attachments).FirstOrDefault();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public EditTicketViewModel GetTicketbyTicketId(long? ticketId)
        {
            try
            {
                var ticket = (from ticketsummary in _vueTicketDbContext.TicketSummary.AsNoTracking()
                              join ticketDetail in _vueTicketDbContext.TicketDetails on ticketsummary.TicketId equals ticketDetail.TicketId
                              where ticketsummary.TicketId == ticketId
                              select new EditTicketViewModel()
                              {
                                  TicketId = ticketsummary.TicketId,
                                  DepartmentId = ticketsummary.DepartmentId,
                                  PriorityId = ticketsummary.PriorityId,
                                  TrackingId = ticketsummary.TrackingId,
                                  Message = ticketDetail.Message,
                                  Subject = ticketDetail.Subject,
                                  CreatedBy = ticketsummary.CreatedBy


                              }).FirstOrDefault();

                return ticket;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckAttachmentsExistbyTicketId(long? ticketId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _vueTicketDbContext.AttachmentsModel
                                       where attachments.TicketId == ticketId
                                       select attachments.AttachmentId).Any();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public EditTicketReplyViewModel GetTicketReplyDetailsbyTicketId(long? ticketId, long? ticketReplyId)
        {
            try
            {
                var data = (from ticketReply in _vueTicketDbContext.TicketReply.AsNoTracking()
                            join ticketReplyDetails in _vueTicketDbContext.TicketReplyDetails on ticketReply.TicketReplyId equals ticketReplyDetails.TicketReplyId
                            where ticketReply.TicketId == ticketId && ticketReply.TicketReplyId == ticketReplyId
                            select new EditTicketReplyViewModel()
                            {
                                TicketId = ticketReply.TicketId,
                                TicketReplyId = ticketReply.TicketReplyId,
                                Message = ticketReplyDetails.Message,
                                TicketReplyDetailsId = ticketReplyDetails.TicketReplyDetailsId,
                                Note = ticketReplyDetails.Note
                            }).FirstOrDefault();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReplyAttachmentModel> GetReplyAttachmentsListByTicketId(long? ticketId, long? ticketReplyId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _vueTicketDbContext.ReplyAttachmentModel.AsNoTracking()
                                       where attachments.TicketId == ticketId && attachments.TicketReplyId == ticketReplyId
                                       select attachments).ToList();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ReplyAttachmentsExistbyTicketId(long? ticketId, long? ticketReplyId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _vueTicketDbContext.ReplyAttachmentModel
                                       where attachments.TicketId == ticketId && attachments.TicketReplyId == ticketReplyId
                                       select attachments.ReplyAttachmentId).Any();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public TicketStatusResponse GetCurrentStatusResponse(long? ticketId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@TicketId", ticketId);
            return con.Query<TicketStatusResponse>("Usp_GetCurrentStatusofResponsebyTicketId", param, null, false, 0,
                CommandType.StoredProcedure).FirstOrDefault();

        }

        public int GetAllUnAssignedTicketCount(string search, int? statusId, int? searchin, int? departmentId, int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();

            param.Add("@search", search);
            param.Add("@searchin", searchin);
            param.Add("@statusId", statusId);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            return con.Query<int>("Usp_Ticket_UnAssigned_Grid_Count", param, null, false, 0,
                CommandType.StoredProcedure).FirstOrDefault();

        }

        public List<AgentManagerTicketGridViewModel> GetAllUnAssignedTicketList(int? currentpage, int? pageSize, int? departmentId, string search,
            int? searchin,
            int? statusId, int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();

            param.Add("@Currentpage", currentpage);
            param.Add("@PageSize", pageSize);
            param.Add("@Search", search);
            param.Add("@Searchin", searchin);
            param.Add("@StatusId", statusId);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            var data = con.Query<AgentManagerTicketGridViewModel>("Usp_Ticket_UnAssignedTicket_Grid_List", param, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }

        public async Task<LatestTicketReplyStatusModel> GetLatestTicketReplybyId(long? ticketId)
        {
            try
            {
                var ticketStatus = await (from ticketstatus in _vueTicketDbContext.LatestTicketReplyStatusModel.AsNoTracking()
                                          where ticketstatus.TicketId == ticketId
                                          select ticketstatus).FirstOrDefaultAsync();
                return ticketStatus;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetAllClosedAgentManagerEndTicketsCount(long? createdBy, string search, int? searchin, int? departmentId, int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@AgentUserId", createdBy);
            param.Add("@search", search);
            param.Add("@searchin", searchin);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            return con.Query<int>("Usp_Tickets_GetAllClosedTicketsAgentManagerCount", param, null, false, 0,
                CommandType.StoredProcedure).FirstOrDefault();

        }

        public List<AgentManagerTicketGridViewModel> GetAllClosedAgentManagerEndTicketList(long? createdBy, int? currentpage, int? pageSize, int? departmentId, string search,
            int? searchin,
            int? priorityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@AgentUserId", createdBy);
            param.Add("@Currentpage", currentpage);
            param.Add("@PageSize", pageSize);
            param.Add("@Search", search);
            param.Add("@Searchin", searchin);
            param.Add("@DepartmentId", departmentId);
            param.Add("@PriorityId", priorityId);
            var data = con.Query<AgentManagerTicketGridViewModel>("Usp_Tickets_GetAllClosedTicketsAgentManager_Grid_List", param, commandType: CommandType.StoredProcedure).ToList();
            return data;
        }


        public TicketDetailsEmailViewModel GetAssignedAgentDetails(long? ticketId)
        {
            try
            {
                var bug = (from ticketTracking in _vueTicketDbContext.TicketStatus.AsNoTracking()
                           join userMaster in _vueTicketDbContext.UserMasters on ticketTracking.AssignedTicketUserId equals userMaster.UserId
                           where ticketTracking.TicketId == ticketId
                           select new TicketDetailsEmailViewModel()
                           {
                               RecipientFullName = $"{userMaster.FirstName} {userMaster.LastName}",
                               RecipientEmailId = userMaster.EmailId
                           }).FirstOrDefault();

                return bug;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public TicketDetailsEmailViewModel GetCreatedTicketUserDetails(long? ticketId)
        {
            try
            {
                var bug = (from ticketTracking in _vueTicketDbContext.TicketStatus.AsNoTracking()
                    join userMaster in _vueTicketDbContext.UserMasters on ticketTracking.CreatedTicketUserId equals userMaster.UserId
                    where ticketTracking.TicketId == ticketId
                    select new TicketDetailsEmailViewModel()
                    {
                        RecipientFullName = $"{userMaster.FirstName} {userMaster.LastName}",
                        RecipientEmailId = userMaster.EmailId
                    }).FirstOrDefault();

                return bug;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}