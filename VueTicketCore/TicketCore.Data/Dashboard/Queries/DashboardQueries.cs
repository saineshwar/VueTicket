using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TicketCore.ViewModels.Dashboard;

namespace TicketCore.Data.Dashboard.Queries
{
    public class DashboardQueries : IDashboardQueries

    {
        private readonly IConfiguration _configuration;

        public DashboardQueries(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<DisplayTicketCount> GetUserDepartmentWiseUserTicketCountbyUserId(long? userId,
            int? departmentId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@AssignedTo", userId);
            para.Add("@DepartmentId", departmentId);
            var data = con.Query<DisplayTicketCount>(
                    "Usp_GetStatusWiseTicketCount_User", para,
                    commandType: CommandType.StoredProcedure)
                .ToList();
            return data;
        }


        public List<DisplayTicketCount> GetUserDepartmentWiseAgentTicketCountbyUserId(long? userId,
            int? departmentId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@AssignedTo", userId);
            para.Add("@DepartmentId", departmentId);
            var data = con.Query<DisplayTicketCount>(
                    "Usp_GetStatusWiseTicketCount_Agent", para,
                    commandType: CommandType.StoredProcedure)
                .ToList();
            return data;
        }

        public List<DisplayTicketCount> GetUserDepartmentWiseAgentManagerTicketCountbyUserId(int? departmentId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
         
            para.Add("@DepartmentId", departmentId);
            var data = con.Query<DisplayTicketCount>(
                    "Usp_GetStatusWiseTicketCount_AgentManager", para,
                    commandType: CommandType.StoredProcedure)
                .ToList();
            return data;
        }

        public List<DisplayStarPerformer> GetStartAgents(int? departmentId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@DepartmentId", departmentId);
                var data = con.Query<DisplayStarPerformer>("Usp_GetStarAgent", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}