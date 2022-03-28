using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using TicketCore.ViewModels.Tickets;

namespace TicketCore.Data.Tickets.Queries
{
    public class TicketsReplyQueries : ITicketsReplyQueries
    {
        private readonly IConfiguration _configuration;
        public TicketsReplyQueries(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<ViewTicketReplyHistoryModel> ListofHistoryTicketReplies(long? ticketId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                
                    var param = new DynamicParameters();
                    param.Add("@TicketId", ticketId);
                    return con.Query<ViewTicketReplyHistoryModel>("Usp_Tickets_HistoryTicketRepliesbytrackingId", param, null, false, 0, CommandType.StoredProcedure).ToList();
                
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}