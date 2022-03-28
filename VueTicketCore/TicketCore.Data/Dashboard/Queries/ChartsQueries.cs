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
    public class ChartsQueries : IChartsQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        private readonly IConfiguration _configuration;
        public ChartsQueries(VueTicketDbContext vueTicketDbContext, IConfiguration configuration)
        {
            _vueTicketDbContext = vueTicketDbContext;
            _configuration = configuration;
        }

        public List<CommonStatusPieChartViewModel> GetCommonPieChartData(long? userId, int? departmentId, int?roleId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                para.Add("@DepartmentId", departmentId);
                para.Add("@RoleId", roleId);
                var data = con.Query<CommonStatusPieChartViewModel>("Usp_CommonPieChartData", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CommonPriorityPieChartViewModel> CommonPriorityPieChartData(long? userId, int? departmentId, int? roleId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                para.Add("@DepartmentId", departmentId);
                para.Add("@RoleId", roleId);
                var data = con.Query<CommonPriorityPieChartViewModel>("Usp_CommonPriorityPieChartData", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<MonthWiseDataViewModel> GetTicketResolvedMonthWiseChartData(long? userId, int? departmentId, int? roleId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                para.Add("@DepartmentId", departmentId);
                para.Add("@RoleId", roleId);
                var data = con.Query<MonthWiseDataViewModel>("Usp_GetResolvedTicketMonthWiseByAgent", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<MonthWiseDataViewModel> GetTicketCreatedMonthWiseChartData(long? userId, int? departmentId, int? roleId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@UserId", userId);
                para.Add("@DepartmentId", departmentId);
                para.Add("@RoleId", roleId);
                var data = con.Query<MonthWiseDataViewModel>("Usp_GetCreatedTicketMonthWiseByAgent", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<DisplayTicketReportCount> GetAgentsTeamsticketsDetailCount(int? departmentId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@DepartmentId", departmentId);
                var data = con.Query<DisplayTicketReportCount>("Usp_GetAgentTeamWiseStatusCount_AgentManager", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ResponseResolvedTicketViewModel> GetResolvedTicketAgentWiseCount(int? departmentId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@DepartmentId", departmentId);
                var data = con.Query<ResponseResolvedTicketViewModel>("Usp_GetResolvedTicketAgentWise", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ResponseResolvedTicketViewModel> GetOpenTicketAgentWiseCount(int? departmentId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var para = new DynamicParameters();
                para.Add("@DepartmentId", departmentId);
                var data = con.Query<ResponseResolvedTicketViewModel>("Usp_GetOpenTicketAgentWise", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}