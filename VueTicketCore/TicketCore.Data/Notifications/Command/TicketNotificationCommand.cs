using System;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TicketCore.Data.Notifications.Command
{
    public class TicketNotificationCommand : ITicketNotificationCommand
    {
        private readonly IConfiguration _configuration;
        public TicketNotificationCommand(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void UpdateTicketNotificationasRead(long? agentId, long? notificationId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@NotificationId", notificationId);
                    param.Add("@AgentId", agentId);
                    var result = con.Execute("Usp_UpdateTicketNotificationasRead", param, transaction, 0,
                        CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}