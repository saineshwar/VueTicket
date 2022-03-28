using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TicketCore.Data.Tickets.Command;
using TicketCore.Models.Knowledgebase;
using TicketCore.ViewModels.Knowledgebase;
using TicketCore.ViewModels.Tickets;

namespace TicketCore.Data.Knowledgebase.Command
{
    public class KnowledgebaseCommand : IKnowledgebaseCommand
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        private readonly ILogger<KnowledgebaseCommand> _logger;
        private readonly IConfiguration _configuration;
        public KnowledgebaseCommand(VueTicketDbContext vueTicketDbContext, ILogger<KnowledgebaseCommand> logger, IConfiguration configuration)
        {
            _vueTicketDbContext = vueTicketDbContext;
            _logger = logger;
            _configuration = configuration;

        }

        public async Task<bool> AddKnowledgebase(
            KnowledgebaseModel knowledgebase,
            List<KnowledgebaseAttachmentViewModel> listknowledgebaseAttachments,
            KnowledgebaseDetails knowledgebaseDetails
        )
        {
            try
            {
                await _vueTicketDbContext.Knowledgebase.AddAsync(knowledgebase);
                await _vueTicketDbContext.SaveChangesAsync();

                knowledgebaseDetails.KnowledgebaseId = knowledgebase.KnowledgebaseId;

                await _vueTicketDbContext.KnowledgebaseDetails.AddAsync(knowledgebaseDetails);
                await _vueTicketDbContext.SaveChangesAsync();

                foreach (var attach in listknowledgebaseAttachments)
                {
                    var attachmentsModel = new KnowledgebaseAttachments()
                    {
                        KnowledgebaseAttachmentsId = 0,
                        OriginalAttachmentName = attach.OriginalAttachmentName,
                        GenerateAttachmentName = attach.GenerateAttachmentName,
                        AttachmentType = attach.AttachmentType,
                        KnowledgebaseId = knowledgebase.KnowledgebaseId,
                        CreatedBy = attach.CreatedBy,
                        CreatedOn = attach.CreatedOn,
                        BucketName = attach.BucketName,
                        DirectoryName = attach.DirectoryName
                    };

                    await _vueTicketDbContext.KnowledgebaseAttachments.AddAsync(attachmentsModel);
                    await _vueTicketDbContext.SaveChangesAsync();

                    var attachmentDetailsModel = new KnowledgebaseAttachmentsDetails()
                    {
                        KnowledgebaseAttachmentsDetailsId = 0,
                        KnowledgebaseId = knowledgebase.KnowledgebaseId,
                        CreatedBy = attach.CreatedBy,
                        KnowledgebaseAttachmentsId = attachmentsModel.KnowledgebaseAttachmentsId,
                        AttachmentBase64 = attach.AttachmentBase64
                    };

                    await _vueTicketDbContext.KnowledgebaseAttachmentsDetails.AddAsync(attachmentDetailsModel);
                    await _vueTicketDbContext.SaveChangesAsync();
                }



                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "KnowledgebaseCommand:AddKnowledgebase");
                return false;
            }
        }

        public async Task<bool> Update(
            KnowledgebaseModel knowledgebase,
            List<KnowledgebaseAttachmentViewModel> listknowledgebaseAttachments,
            KnowledgebaseDetails knowledgebaseDetails
        )
        {
            using SqlConnectionManager sqlDataAccessManager = new SqlConnectionManager(_configuration);
            var (connection, transaction) = sqlDataAccessManager.StartTransaction();
            try
            {


                var param = new DynamicParameters();
                param.Add("@KnowledgebaseTypeId", knowledgebase.KnowledgebaseTypeId);
                param.Add("@Subject", knowledgebase.Subject);
                param.Add("@KnowledgebaseId", knowledgebase.KnowledgebaseId);
                param.Add("@Status", knowledgebase.Status);
                param.Add("@CreatedBy", knowledgebase.CreatedBy);
                param.Add("@DepartmentId", knowledgebase.DepartmentId);

                var result = await connection.ExecuteAsync("Usp_UpdateKnowledgebase", param, transaction, 0, CommandType.StoredProcedure);

                var paramknowledgebaseDetails = new DynamicParameters();
                paramknowledgebaseDetails.Add("@Contents", knowledgebaseDetails.Contents);
                paramknowledgebaseDetails.Add("@Keywords", knowledgebaseDetails.Keywords);
                paramknowledgebaseDetails.Add("@KnowledgebaseId", knowledgebase.KnowledgebaseId);
                var resultknowledgebaseDetails = await connection.ExecuteAsync("Usp_UpdateKnowledgebaseDetails", paramknowledgebaseDetails, transaction, 0, CommandType.StoredProcedure);

                if (listknowledgebaseAttachments != null)
                {
                    foreach (var knowledgebaseAttachment in listknowledgebaseAttachments)
                    {

                        var paramknowledgebaseAttachment = new DynamicParameters();
                        paramknowledgebaseAttachment.Add("@KnowledgebaseId", knowledgebase.KnowledgebaseId);
                        paramknowledgebaseAttachment.Add("@OriginalAttachmentName", knowledgebaseAttachment.OriginalAttachmentName);
                        paramknowledgebaseAttachment.Add("@GenerateAttachmentName", knowledgebaseAttachment.GenerateAttachmentName);
                        paramknowledgebaseAttachment.Add("@AttachmentType", knowledgebaseAttachment.AttachmentType);
                        paramknowledgebaseAttachment.Add("@CreatedOn", knowledgebaseAttachment.CreatedOn);
                        paramknowledgebaseAttachment.Add("@CreatedBy", knowledgebaseAttachment.CreatedBy);
                        paramknowledgebaseAttachment.Add("@BucketName", knowledgebaseAttachment.BucketName);
                        paramknowledgebaseAttachment.Add("@DirectoryName", knowledgebaseAttachment.DirectoryName);
                        paramknowledgebaseAttachment.Add("@KnowledgebaseAttachmentsId", dbType: DbType.Int64, direction: ParameterDirection.Output);
                        await connection.ExecuteAsync("Usp_InsertKnowledgebaseAttachments", paramknowledgebaseAttachment, transaction, 0, CommandType.StoredProcedure);

                        long knowledgebaseAttachmentsId = paramknowledgebaseAttachment.Get<Int64>("@KnowledgebaseAttachmentsId");

                        var paramKnowledgebaseAttachmentsDetails = new DynamicParameters();
                        paramKnowledgebaseAttachmentsDetails.Add("@AttachmentBase64", knowledgebaseAttachment.AttachmentBase64);
                        paramKnowledgebaseAttachmentsDetails.Add("@KnowledgebaseId", knowledgebase.KnowledgebaseId);
                        paramKnowledgebaseAttachmentsDetails.Add("@KnowledgebaseAttachmentsId", knowledgebaseAttachmentsId);
                        paramKnowledgebaseAttachmentsDetails.Add("@CreatedBy", knowledgebaseAttachment.CreatedBy);
                        var resultKnowledgebaseAttachmentsDetails = await connection.ExecuteAsync("Usp_InsertKnowledgebaseAttachmentsDetails", paramKnowledgebaseAttachmentsDetails, transaction, 0, CommandType.StoredProcedure);

                    }
                }

                if (result > 0)
                {
                    sqlDataAccessManager.Commit();
                    return true;
                }
                else
                {
                    sqlDataAccessManager.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> DeleteAttachmentByAttachmentId(KnowledgebaseAttachments attachmentsModel, KnowledgebaseAttachmentsDetails attachmentDetailsModel)
        {
            using var transactionScope = new TransactionScope();
            try
            {

                _vueTicketDbContext.Entry(attachmentsModel).State = EntityState.Deleted;
                await _vueTicketDbContext.SaveChangesAsync();

                _vueTicketDbContext.Entry(attachmentDetailsModel).State = EntityState.Deleted;
                await _vueTicketDbContext.SaveChangesAsync();

                transactionScope.Complete();

                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "KnowledgebaseCommand:DeleteAttachmentByAttachmentId");
                return false;
            }
        }


        public async Task<bool> UpdateStatus(RequestKnowledgebaseChange requestKnowledgebase, long modifiedBy)
        {
            using SqlConnectionManager sqlDataAccessManager = new SqlConnectionManager(_configuration);
            var (connection, transaction) = sqlDataAccessManager.StartTransaction();
            try
            {


                var param = new DynamicParameters();
                param.Add("@KnowledgebaseId", requestKnowledgebase.KnowledgebaseId);
                param.Add("@Status", requestKnowledgebase.Status);
                param.Add("@ModifiedBy", modifiedBy);
                
                var result = await connection.ExecuteAsync("Usp_UpdateKnowledgebaseStatus", param, transaction, 0, CommandType.StoredProcedure);

                if (result > 0)
                {
                    sqlDataAccessManager.Commit();
                    return true;
                }
                else
                {
                    sqlDataAccessManager.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }

    }
}