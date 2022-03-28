using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using TicketCore.ViewModels.Notifications;

namespace TicketCore.Data.Notifications.Queries
{
    public class TicketNotificationQueries : ITicketNotificationQueries
    {
        private readonly IConfiguration _configuration;
        public TicketNotificationQueries(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<TicketNotificationViewModel> ListofNotification(long? agentId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@AgentId", agentId);
                return con.Query<TicketNotificationViewModel>("Usp_GetTicketNotificationbyAgentId", param, null, false, 0, CommandType.StoredProcedure).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ShowNotificationViewModel> GetTicketNotificationCount(long? agentId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@AgentId", agentId);
                return con.Query<ShowNotificationViewModel>("Usp_GetTicketNotificationbyAgentIdCount", param, null, false, 0, CommandType.StoredProcedure).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetTotalNotificationCount(long? agentId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@AgentId", agentId);
                return con.Query<int>("Usp_GetTicketNotificationbyAgentIdTotalCount", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}