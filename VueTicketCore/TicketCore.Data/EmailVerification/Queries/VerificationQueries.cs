using System;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TicketCore.Models.Masters;
using TicketCore.Models.Verification;

namespace TicketCore.Data.EmailVerification.Queries
{
    public class VerificationQueries : IVerificationQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        private readonly IConfiguration _configuration;
        public VerificationQueries(VueTicketDbContext vueTicketDbContext, IConfiguration configuration)
        {
            _vueTicketDbContext = vueTicketDbContext;
            _configuration = configuration;
        }
     


        public string GetResetGeneratedTokenbyUnq(int? unq)
        {
            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var param = new DynamicParameters();
            param.Add("@UserId", unq);
            var result = connection.Query<string>("USP_GetResetGeneratedToken", param, null, true, 0, CommandType.StoredProcedure).FirstOrDefault();
            return result;
        }

        public EmailVerificationModel GetEmailVerificationCodeGeneratedToken(long userid)
        {
            var registerVerification = (from rv in _vueTicketDbContext.EmailVerification
                orderby rv.EmailVerificationId descending
                where rv.UserId == userid
                select rv).FirstOrDefault();

            return registerVerification;
        }

       
    }
}