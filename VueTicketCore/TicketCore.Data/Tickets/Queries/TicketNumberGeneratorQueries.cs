using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TicketCore.Data.Tickets.Queries
{
    public class TicketNumberGeneratorQueries : ITicketNumberGeneratorQueries
    {
        private readonly IConfiguration _configuration;
        public TicketNumberGeneratorQueries(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int GetLatestTicketNo()
        {
            using var con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            con.Open();
            using var transcation = con.BeginTransaction();
            try
            {
                int value = 0;
                var para = new DynamicParameters();
                para.Add(name: "@RetVal", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                var returnCode = con.Execute("Usp_TicketIdentity", para, transcation, commandType: CommandType.StoredProcedure);
                if (returnCode > 0)
                {
                    value = para.Get<int>("RetVal");
                    transcation.Commit();
                }
                else
                {
                    transcation.Rollback();
                }
                 
                return value;
            }
            catch (System.Exception)
            {
                transcation.Rollback();
                throw;
            }

        }
    }
}