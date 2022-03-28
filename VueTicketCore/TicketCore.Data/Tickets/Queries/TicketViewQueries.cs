using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using TicketCore.Models.Tickets;
using TicketCore.ViewModels.Tickets;

namespace TicketCore.Data.Tickets.Queries
{
    public class TicketViewQueries : ITicketViewQueries
    {
        private readonly IConfiguration _configuration;
        private readonly VueTicketDbContext _vueTicketDbContext;
        public TicketViewQueries(IConfiguration configuration, VueTicketDbContext vueTicketDbContext)
        {
            _configuration = configuration;
            _vueTicketDbContext = vueTicketDbContext;
        }

        public TicketDetailsViewModel TicketsDetailsbyticketId(long? ticketId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                var param = new DynamicParameters();
                param.Add("@TicketId", ticketId);
                return con.Query<TicketDetailsViewModel>("Usp_Tickets_TicketsDetailsbyticketId", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckTrackingIdExists(long? ticketId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                var param = new DynamicParameters();
                param.Add("@TicketId", ticketId);
                return con.Query<bool>("Usp_CheckTrackingIdExists", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public EscalatedUserViewModel GetTicketEscalatedToUserNames(long? ticketId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            con.Open();
            var param = new DynamicParameters();
            param.Add("@TicketId", ticketId);
            return con.Query<EscalatedUserViewModel>("Usp_GetTicketEscalatedToUserNames", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();

        }

        public List<ReplyAttachmentModel> GetListReplyAttachmentsByAttachmentId(long? ticketId, long? ticketReplyId)
        {
            try
            {
                var attachmentsinfo = (from attachments in _vueTicketDbContext.ReplyAttachmentModel
                                       where attachments.TicketId == ticketId && attachments.TicketReplyId == ticketReplyId
                                       select attachments).ToList();
                return attachmentsinfo;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<TicketHistoryResponse> ListofTicketHistorybyTicket(long? ticketId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));

                var param = new DynamicParameters();
                param.Add("@TicketId", ticketId);
                return con.Query<TicketHistoryResponse>("Usp_GetTicketHistorybyTicketId", param, null, false, 0, CommandType.StoredProcedure).ToList();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}