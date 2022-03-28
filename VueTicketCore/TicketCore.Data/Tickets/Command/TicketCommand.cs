using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TicketCore.Models.Tickets;
using TicketCore.ViewModels.Tickets;

namespace TicketCore.Data.Tickets.Command
{
    public class TicketCommand : ITicketCommand
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        private readonly ILogger<TicketCommand> _logger;
        private readonly IConfiguration _configuration;
        public TicketCommand(VueTicketDbContext vueTicketDbContext, ILogger<TicketCommand> logger, IConfiguration configuration)
        {
            _vueTicketDbContext = vueTicketDbContext;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<bool> AddTickets(long? userId, TicketSummary ticketSummary, TicketDetails ticketDetails,
            List<TicketAttachmentsViewModel> listofAttachment)
        {



            try
            {
                await _vueTicketDbContext.TicketSummary.AddAsync(ticketSummary);
                await _vueTicketDbContext.SaveChangesAsync();

                ticketDetails.TicketId = ticketSummary.TicketId;

                await _vueTicketDbContext.TicketDetails.AddAsync(ticketDetails);
                await _vueTicketDbContext.SaveChangesAsync();

                var latestTicketReply = new LatestTicketReplyStatusModel()
                {
                    TicketId = ticketSummary.TicketId,
                    TicketReplyLatestId = 0,
                };

                await _vueTicketDbContext.LatestTicketReplyStatusModel.AddAsync(latestTicketReply);
                await _vueTicketDbContext.SaveChangesAsync();



                foreach (var attach in listofAttachment)
                {
                    var attachmentsModel = new AttachmentsModel()
                    {
                        AttachmentId = 0,
                        OriginalAttachmentName = attach.OriginalAttachmentName,
                        GenerateAttachmentName = attach.GenerateAttachmentName,
                        AttachmentType = attach.AttachmentType,
                        TicketId = ticketSummary.TicketId,
                        CreatedBy = attach.CreatedBy,
                        CreatedOn = attach.CreatedOn,
                        BucketName = attach.BucketName,
                        DirectoryName = attach.DirectoryName
                    };

                    await _vueTicketDbContext.AttachmentsModel.AddAsync(attachmentsModel);
                    await _vueTicketDbContext.SaveChangesAsync();

                    var attachmentDetailsModel = new AttachmentDetailsModel()
                    {
                        AttachmentDetailsId = 0,
                        TicketId = ticketSummary.TicketId,
                        CreatedBy = attach.CreatedBy,
                        AttachmentId = attachmentsModel.AttachmentId,
                        AttachmentBase64 = attach.AttachmentBase64
                    };

                    await _vueTicketDbContext.AttachmentDetailsModel.AddAsync(attachmentDetailsModel);
                    await _vueTicketDbContext.SaveChangesAsync();
                }



                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "BugCommand:AddBug");
                return false;
            }
        }

        public async Task<bool> AddTicketReply(long? userid, TicketReplyModel ticketReplyModel,
            TicketReplyDetailsModel ticketReplyDetailsModel,
            List<TicketAttachmentsViewModel> replyAttachment, TicketStatus ticketStatus, LatestTicketReplyStatusModel latestreply)
        {


            try
            {
                await _vueTicketDbContext.TicketReply.AddAsync(ticketReplyModel);
                await _vueTicketDbContext.SaveChangesAsync();

                ticketReplyDetailsModel.TicketReplyId = ticketReplyModel.TicketReplyId;

                await _vueTicketDbContext.TicketReplyDetails.AddAsync(ticketReplyDetailsModel);
                await _vueTicketDbContext.SaveChangesAsync();

                foreach (var attach in replyAttachment)
                {
                    var replyattachmentsModel = new ReplyAttachmentModel()
                    {
                        ReplyAttachmentId = 0,
                        OriginalAttachmentName = attach.OriginalAttachmentName,
                        GenerateAttachmentName = attach.GenerateAttachmentName,
                        AttachmentType = attach.AttachmentType,
                        TicketId = ticketReplyModel.TicketId,
                        CreatedBy = attach.CreatedBy,
                        CreatedOn = attach.CreatedOn,
                        BucketName = attach.BucketName,
                        DirectoryName = attach.DirectoryName,
                        TicketReplyId = ticketReplyModel.TicketReplyId
                    };

                    await _vueTicketDbContext.ReplyAttachmentModel.AddAsync(replyattachmentsModel);
                    await _vueTicketDbContext.SaveChangesAsync();

                    var replyattachmentDetailsModel = new ReplyAttachmentDetailsModel()
                    {
                        ReplyAttachmentId = replyattachmentsModel.ReplyAttachmentId,
                        TicketId = ticketReplyModel.TicketId,
                        CreatedBy = attach.CreatedBy,
                        ReplyAttachmentDetailsId = 0,
                        AttachmentBase64 = attach.AttachmentBase64
                    };

                    await _vueTicketDbContext.ReplyAttachmentDetailsModel.AddAsync(replyattachmentDetailsModel);
                    await _vueTicketDbContext.SaveChangesAsync();
                }

                _vueTicketDbContext.Entry(ticketStatus).State = EntityState.Modified;
                await _vueTicketDbContext.SaveChangesAsync();

                latestreply.RepliedUserId = userid;

                _vueTicketDbContext.Entry(latestreply).State = EntityState.Modified;
                await _vueTicketDbContext.SaveChangesAsync();

                return true;
            }
            catch (System.Exception ex)
            {

                throw;
            }


        }

        public bool DeleteAttachmentByAttachmentId(AttachmentsModel attachmentsModel, AttachmentDetailsModel attachmentDetailsModel)
        {
            using var transactionScope = new TransactionScope();
            try
            {

                _vueTicketDbContext.Entry(attachmentsModel).State = EntityState.Deleted;
                _vueTicketDbContext.SaveChanges();

                _vueTicketDbContext.Entry(attachmentDetailsModel).State = EntityState.Deleted;
                _vueTicketDbContext.SaveChanges();

                transactionScope.Complete();

                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "TicketCommand:DeleteAttachmentByAttachmentId");
                return false;
            }
        }
        public bool DeleteReplyAttachmentByAttachmentId(ReplyAttachmentModel replyAttachment, ReplyAttachmentDetailsModel replyAttachmentDetails)
        {
            using var transactionScope = new TransactionScope();
            try
            {

                _vueTicketDbContext.Entry(replyAttachment).State = EntityState.Deleted;
                _vueTicketDbContext.SaveChanges();

                _vueTicketDbContext.Entry(replyAttachmentDetails).State = EntityState.Deleted;
                _vueTicketDbContext.SaveChanges();

                transactionScope.Complete();

                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "TicketCommand:DeleteReplyAttachmentByAttachmentId");
                return false;
            }
        }
        public bool ChangeTicketPriority(RequestChangePriority requestChangePriority)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                var param = new DynamicParameters();
                param.Add("@PriorityId", requestChangePriority.PriorityId);
                param.Add("@TicketId", requestChangePriority.TicketId);
                var result = con.Execute("Usp_ChangeTicketPriority", param, transaction, 0,
                    CommandType.StoredProcedure);

                if (result > 0)
                {
                    transaction.Commit();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateTicket(long? userId, TicketsUserViewModel ticketsViewModel, List<TicketAttachmentsViewModel> listofAttachment)
        {
            try
            {
                await using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                var param = new DynamicParameters();
                param.Add("@TicketId", ticketsViewModel.TicketId);
                param.Add("@ModifiedBy", userId);
                param.Add("@Message", ticketsViewModel.Message);
                param.Add("@Subject", ticketsViewModel.Subject);
                var resultticket = await con.ExecuteAsync("Usp_UpdateUserTicket", param, transaction, 0, CommandType.StoredProcedure);

                if (resultticket > 0)
                {
                    foreach (var attach in listofAttachment)
                    {

                        var paramAttachments = new DynamicParameters();
                        paramAttachments.Add("@OriginalAttachmentName", attach.OriginalAttachmentName);
                        paramAttachments.Add("@GenerateAttachmentName", attach.GenerateAttachmentName);
                        paramAttachments.Add("@AttachmentType", attach.AttachmentType);
                        paramAttachments.Add("@CreatedBy", attach.CreatedBy);
                        paramAttachments.Add("@TicketId", ticketsViewModel.TicketId);
                        paramAttachments.Add("@BucketName", attach.BucketName);
                        paramAttachments.Add("@DirectoryName", attach.DirectoryName);
                        paramAttachments.Add("@AttachmentId", dbType: DbType.Int64,
                            direction: ParameterDirection.Output);
                        var resultAttachments = await con.ExecuteAsync("Usp_InsertAttachments", paramAttachments, transaction,
                            0, CommandType.StoredProcedure);
                        long attachmentId = paramAttachments.Get<Int64>("@AttachmentId");


                        var paramAttachmentsdetail = new DynamicParameters();
                        paramAttachmentsdetail.Add("@AttachmentBase64", attach.AttachmentBase64);
                        paramAttachmentsdetail.Add("@AttachmentId", attachmentId);
                        paramAttachmentsdetail.Add("@CreatedBy", attach.CreatedBy);
                        paramAttachmentsdetail.Add("@TicketId", ticketsViewModel.TicketId);
                        var resultAttachmentsdetail = await con.ExecuteAsync("Usp_InsertAttachmentsDetails",
                            paramAttachmentsdetail, transaction, 0, CommandType.StoredProcedure);
                    }


                    transaction.Commit();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateTicketReply(EditTicketReplyViewModel ticketsViewModel, List<TicketAttachmentsViewModel> replyAttachment, long? systemUser, long? ticketUser)
        {
            try
            {
                await using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                var param = new DynamicParameters();
                param.Add("@TicketReplyId", ticketsViewModel.TicketReplyId);
                param.Add("@Message", ticketsViewModel.Message);
                param.Add("@Note", ticketsViewModel.Note);
                param.Add("@SystemUser", systemUser);
                param.Add("@TicketUser", ticketUser);

                var result = await con.ExecuteAsync("Usp_UpdateTicketReply", param, transaction, 0, CommandType.StoredProcedure);

                if (replyAttachment != null)
                {
                    foreach (var attach in replyAttachment)
                    {
                        var paramAttachments = new DynamicParameters();
                        paramAttachments.Add("@OriginalAttachmentName", attach.OriginalAttachmentName);
                        paramAttachments.Add("@GenerateAttachmentName", attach.GenerateAttachmentName);
                        paramAttachments.Add("@AttachmentType", attach.AttachmentType);
                        paramAttachments.Add("@CreatedBy", attach.CreatedBy);
                        paramAttachments.Add("@TicketId", ticketsViewModel.TicketId);
                        paramAttachments.Add("@TicketReplyId", ticketsViewModel.TicketReplyId);
                        paramAttachments.Add("@BucketName", attach.BucketName);
                        paramAttachments.Add("@DirectoryName", attach.DirectoryName);
                        paramAttachments.Add("@AttachmentBase64", attach.AttachmentBase64);

                        var resultAttachments = await con.ExecuteAsync("Usp_Insert_ReplyAttachment", paramAttachments, transaction, 0, CommandType.StoredProcedure);

                    }
                }

                if (result > 0)
                {
                    transaction.Commit();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateResponseStatus(long? userId, long? ticketId, int? statusId)
        {
            try
            {
                await using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                var param = new DynamicParameters();
                param.Add("@TicketId", ticketId);
                param.Add("@StatusId", statusId);
                param.Add("@UserId", userId);
                var resultticket = await con.ExecuteAsync("Usp_UpdateResponseStatus", param, transaction, 0, CommandType.StoredProcedure);

                if (resultticket > 0)
                {

                    transaction.Commit();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAssignTickettoUser(long userId, long ticketId)
        {
            try
            {
                await using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                param.Add("@TicketId", ticketId);
                var result = await con.ExecuteAsync("Usp_AssignTickettoUserbyUserId", param, transaction, 0,
                    CommandType.StoredProcedure);

                if (result > 0)
                {
                    transaction.Commit();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> TransferDepartment(int? departmentId, long? ticketId)
        {
            try
            {
                await using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                var param = new DynamicParameters();
                param.Add("@DepartmentId", departmentId);
                param.Add("@TicketId", ticketId);
                var result = await con.ExecuteAsync("Usp_UpdateDepartment", param, transaction, 0,
                    CommandType.StoredProcedure);

                if (result > 0)
                {
                    transaction.Commit();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> DeleteTicket(long? userId, long? ticketId, int? statusId)
        {
            try
            {
                await using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                param.Add("@StatusId", statusId);
                param.Add("@TicketId", ticketId);
                var result = await con.ExecuteAsync("Usp_DeleteTicket", param, transaction, 0, CommandType.StoredProcedure);

                if (result > 0)
                {
                    transaction.Commit();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UndoDeleteTicket(long? ticketId)
        {
            try
            {
                await using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                var param = new DynamicParameters();
                param.Add("@TicketId", ticketId);
                var result = await con.ExecuteAsync("Usp_Undo_DeleteTicket", param, transaction, 0, CommandType.StoredProcedure);

                if (result > 0)
                {
                    transaction.Commit();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> AssignTicketManually(RequestManualAssignViewModel requestmodel)
        {
            try
            {
                await using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                var param = new DynamicParameters();
                param.Add("@TicketId", requestmodel.TicketId);
                param.Add("@PriorityId", requestmodel.PriorityId);
                param.Add("@CreatedUserId", requestmodel.CreatedUserId);
                param.Add("@AssignedtoUserId", requestmodel.AssignedtoUserId);
                param.Add("@AssignedbyUserId", requestmodel.AssignedbyUserId);
                param.Add("@DepartmentId", requestmodel.DepartmentId);
                var result = await con.ExecuteAsync("Usp_InsertintoTicketStatus_Manually_Custom", param, transaction, 0, CommandType.StoredProcedure);

                if (result > 0)
                {
                    transaction.Commit();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> ReOpenTicket(long ticketId)
        {
            try
            {
                await using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                var param = new DynamicParameters();
                param.Add("@TicketId", ticketId);
                var result = await con.ExecuteAsync("Usp_UpdateReopenClosedTicket", param, transaction, 0,
                    CommandType.StoredProcedure);

                if (result > 0)
                {
                    transaction.Commit();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}