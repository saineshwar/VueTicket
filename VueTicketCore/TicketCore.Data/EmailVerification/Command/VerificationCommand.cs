using System;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using TicketCore.Models.Verification;
using TicketCore.ViewModels.Usermaster;
using ResetPasswordVerification = TicketCore.Models.Verification.ResetPasswordVerification;

namespace TicketCore.Data.EmailVerification.Command
{
    public class VerificationCommand : IVerificationCommand
    {
        private readonly IConfiguration _configuration;
        private readonly VueTicketDbContext _vueTicketDbContext;
        public VerificationCommand(VueTicketDbContext vueTicketDbContext, IConfiguration configuration)
        {
            _vueTicketDbContext = vueTicketDbContext;
            _configuration = configuration;
        }
   

        public string InsertResetPasswordVerificationToken(ResetPasswordVerification resetPassword)
        {
            using SqlConnectionManager sqlConnectionManager = new SqlConnectionManager(_configuration);
            try
            {
                var (connection, transaction) = sqlConnectionManager.StartTransaction();
                var param = new DynamicParameters();
                param.Add("@UserId", resetPassword.UserId);
                param.Add("@GeneratedToken", resetPassword.GeneratedToken);
                param.Add("@GeneratedDate", resetPassword.GeneratedDate);

                var result = connection.Execute("USP_InsertResetPasswordVerificationToken", param, transaction, 0, CommandType.StoredProcedure);
                if (result > 0)
                {
                    sqlConnectionManager.Commit();
                    return "Success";
                }
                else
                {
                    sqlConnectionManager.Rollback();
                    return "Failed";
                }
            }
            catch (Exception ex)
            {
                sqlConnectionManager.Rollback();
                throw;
            }
        }

        public string UpdatePasswordandVerificationStatus(UpdateResetPasswordVerification resetPassword)
        {
            using SqlConnectionManager sqlConnectionManager = new SqlConnectionManager(_configuration);
            try
            {
                var (connection, transaction) = sqlConnectionManager.StartTransaction();
                var param = new DynamicParameters();
                param.Add("@UserId", resetPassword.UserId);
                param.Add("@GeneratedToken", resetPassword.GeneratedToken);
                param.Add("@Password", resetPassword.Password);

                var result = connection.Execute("USP_UpdatePasswordandVerificationStatus", param, transaction, 0, CommandType.StoredProcedure);
                if (result > 0)
                {
                    sqlConnectionManager.Commit();
                    return "Success";
                }
                else
                {
                    sqlConnectionManager.Rollback();
                    return "Failed";
                }
            }
            catch (Exception ex)
            {
                sqlConnectionManager.Rollback();
                throw;
            }
        }


        public int InsertEmailVerification(long? userId, string emailId, string verificationCode)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                param.Add("@EmailId", emailId);
                param.Add("@VerificationCode", verificationCode);
                var result = con.Execute("Usp_InsertEmailVerification", param, transaction, 0, CommandType.StoredProcedure);

                if (result > 0)
                {
                    transaction.Commit();
                    return result;
                }
                else
                {
                    transaction.Rollback();
                    return 0;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

      

        public bool UpdatedVerificationCode(long emailVerificationId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                var param = new DynamicParameters();
                param.Add("@EmailVerificationId", emailVerificationId);
                param.Add("@IsVerified", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                var result = con.Execute("Usp_UpdatedVerificationCode", param, transaction, 0, CommandType.StoredProcedure);
                bool isVerified = param.Get<bool>("@IsVerified");

                if (result > 0)
                {
                    transaction.Commit();
                    return isVerified;
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