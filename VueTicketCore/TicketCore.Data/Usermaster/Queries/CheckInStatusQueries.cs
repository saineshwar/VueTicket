using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using TicketCore.ViewModels.Usermaster;

namespace TicketCore.Data.Usermaster.Queries
{
    public class CheckInStatusQueries : ICheckInStatusQueries
    {
        private readonly IConfiguration _configuration;
        private readonly VueTicketDbContext _vueTicketDbContext;
        public CheckInStatusQueries(IConfiguration configuration, VueTicketDbContext vueTicketDbContext)
        {
            _configuration = configuration;
            _vueTicketDbContext = vueTicketDbContext;
        }
        public void StatusCheckInCheckOut(long userId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                var result = con.Execute("USP_AgentCheckInOut", param, transaction, 0, CommandType.StoredProcedure);

                if (result > 0)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool CheckIsalreadyCheckedIn(long userId)
        {
            try
            {
              
                    var checkStatus = (from a in _vueTicketDbContext.AgentCheckInStatusSummary
                        where a.UserId == userId
                        select a.AgentStatus).FirstOrDefault();

                    return checkStatus;
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<AgentDailyActivityModel> AgentDailyActivity(long userId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                con.Open();
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                return con.Query<AgentDailyActivityModel>("Usp_AgentDailyActivity", param, null, false, 0, CommandType.StoredProcedure).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckIsCategoryAssignedtoAgent(long userId)
        {
            try
            {
                var result = (from menu in _vueTicketDbContext.AgentDepartmentAssigned
                    where menu.UserId == userId
                    select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}