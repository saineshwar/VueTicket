using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TicketCore.ViewModels.Audit;

namespace TicketCore.Data.Audit.Queries
{
    public class AuditQueries : IAuditQueries
    {
        private readonly IConfiguration _configuration;
        public AuditQueries(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public List<AuditViewModel> GetUserActivity(long? userId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserID", userId);
                var data = con.Query<AuditViewModel>("Usp_GetUserLoggedActivity", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}