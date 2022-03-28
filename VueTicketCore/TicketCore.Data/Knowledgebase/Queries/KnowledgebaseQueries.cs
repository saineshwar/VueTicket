using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using TicketCore.Models.Knowledgebase;
using TicketCore.Models.Tickets;
using TicketCore.ViewModels.Knowledgebase;
using TicketCore.ViewModels.Usermaster;

namespace TicketCore.Data.Knowledgebase.Queries
{
    public class KnowledgebaseQueries : IKnowledgebaseQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        private readonly IConfiguration _configuration;
        public KnowledgebaseQueries(VueTicketDbContext vueTicketDbContext, IConfiguration configuration)
        {
            _vueTicketDbContext = vueTicketDbContext;
            _configuration = configuration;
        }

        public IQueryable<KnowledgebaseGridViewModel> ShowAllKnowledgebase(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryableskb = (from kb in _vueTicketDbContext.Knowledgebase
                                    join kbt in _vueTicketDbContext.KnowledgebaseType on kb.KnowledgebaseTypeId equals kbt.KnowledgebaseTypeId
                                    join department in _vueTicketDbContext.Department on kb.DepartmentId equals department.DepartmentId

                                    select new KnowledgebaseGridViewModel()
                                    {
                                        KnowledgebaseId = kb.KnowledgebaseId,
                                        DepartmentName = department.DepartmentName,
                                        KnowledgebaseTypeName = kbt.KnowledgebaseTypeName,
                                        Subject = kb.Subject,
                                        CreateDate = kb.CreatedOn,
                                        Status = kb.Status == true ? "Active" : "InActive",
                                        DepartmentId = kb.DepartmentId

                                    }).AsQueryable();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryableskb = queryableskb.OrderBy(sortColumn + " " + sortColumnDir);
                }
                else
                {
                    queryableskb = queryableskb.OrderByDescending(x => x.KnowledgebaseId);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    queryableskb = queryableskb.Where(m => m.Subject.Contains(search) || m.Subject.Contains(search));
                }

                return queryableskb;

            }
            catch (Exception)
            {
                throw;
            }
        }


        public EditKnowledgebaseViewModel GetKnowledgeBaseDetailsbyId(long KnowledgebaseId)
        {
            var queryableskb = (from kb in _vueTicketDbContext.Knowledgebase
                                join kbt in _vueTicketDbContext.KnowledgebaseDetails on kb.KnowledgebaseId equals kbt.KnowledgebaseId
                                join department in _vueTicketDbContext.Department on kb.DepartmentId equals department.DepartmentId
                                where kb.KnowledgebaseId == KnowledgebaseId
                                select new EditKnowledgebaseViewModel()
                                {
                                    KnowledgebaseId = kb.KnowledgebaseId,
                                    KnowledgebaseDetailsId = kbt.KnowledgebaseDetailsId,
                                    Contents = kbt.Contents,
                                    Subject = kb.Subject,
                                    Status = kb.Status,
                                    DepartmentId = kb.DepartmentId,
                                    Keywords = kbt.Keywords,
                                    KnowledgebaseTypeId = kb.KnowledgebaseTypeId

                                }).FirstOrDefault();

            return queryableskb;
        }

        public List<KnowledgebaseAttachments> GetListAttachmentsByKnowledgebaseId(long? KnowledgebaseId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _vueTicketDbContext.KnowledgebaseAttachments.AsNoTracking()
                                       where attachments.KnowledgebaseId == KnowledgebaseId
                                       select attachments).ToList();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool KnowledgebaseAttachmentsExistbyknowledgebaseId(long? knowledgebaseId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _vueTicketDbContext.KnowledgebaseAttachments
                                       where attachments.KnowledgebaseId == knowledgebaseId
                                       select attachments.KnowledgebaseId).Any();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public KnowledgebaseAttachments GetAttachments(long knowledgebaseId, long knowledgebaseAttachmentsId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _vueTicketDbContext.KnowledgebaseAttachments.AsNoTracking()
                                       where attachments.KnowledgebaseId == knowledgebaseId && attachments.KnowledgebaseAttachmentsId == knowledgebaseAttachmentsId
                                       select attachments).FirstOrDefault();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public KnowledgebaseAttachmentsDetails GetAttachmentDetailsByAttachmentId(long knowledgebaseId, long knowledgebaseAttachmentsId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _vueTicketDbContext.KnowledgebaseAttachmentsDetails.AsNoTracking()
                                       where attachments.KnowledgebaseId == knowledgebaseId && attachments.KnowledgebaseAttachmentsId == knowledgebaseAttachmentsId
                                       select attachments).FirstOrDefault();
                return attachmentsinfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<KnowledgeSearch> SearchKnowledgebasebydepartmentId(string departmentId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@DepartmentId", departmentId);
                return con.Query<KnowledgeSearch>("Usp_SearchKnowledgebasebyDepartmentId", param, null, false, 0, CommandType.StoredProcedure).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<KnowledgeSearch> SearchKnowledgebase(string search)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@search", search);
                return con.Query<KnowledgeSearch>("Usp_SearchKnowledgebase", param, null, false, 0, CommandType.StoredProcedure).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public KnowledgebaseArticleViewModel GetKnowledgebaseDetailsForArticle(long knowledgebaseId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@KnowledgebaseId", knowledgebaseId);
                return con.Query<KnowledgebaseArticleViewModel>("Usp_GetKnowledgebaseDetailsForArticle", param, null, false, 0, CommandType.StoredProcedure)
                    .FirstOrDefault();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AllKnowledgebaseViewModel> AllKnowledgebase()
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                return con.Query<AllKnowledgebaseViewModel>("Usp_GetAllKnowledgebase", null, null, false, 0, CommandType.StoredProcedure).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}