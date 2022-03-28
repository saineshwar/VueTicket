using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using TicketCore.ViewModels.Reports;

namespace TicketCore.Data.Reports.Queries
{
    public class ReportQueries : IReportQueries
    {
        private readonly IConfiguration _configuration;
        public ReportQueries(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<SelectListItem> AgentAdminReportList()
        {
            List<SelectListItem> listofReport = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "---Select---",Value = "",
                },
                new SelectListItem()
                {
                    Text = "AgentWise Ticket Status Report",Value = "1",
                },
                new SelectListItem()
                {
                    Text = "Department wise Ticket Status Report",Value = "2",
                },
                new SelectListItem()
                {
                    Text = "Ticket Overdues Status Report",Value = "3",
                },
                new SelectListItem()
                {
                    Text = "Ticket Overdues UserWise Report",Value = "4",
                },
                new SelectListItem()
                {
                    Text = "Ticket Escalation Report",Value = "5",
                },
                new SelectListItem()
                {
                    Text = "Ticket Deleted Report",Value = "6",
                },
                new SelectListItem()
                {
                    Text = "PriorityWise Ticket Status Report",Value = "7",
                },
                new SelectListItem()
                {
                    Text = "Agent Detail Report",Value = "8",
                },
                new SelectListItem()
                {
                    Text = "UserWise Checkin CheckOut Report",Value = "9",
                },
            };
            return listofReport;
        }

        public async Task<List<AgentAdminExportReportViewModel>> GetDetailTicketStatusReport(string fromdate, string todate, long? userId, int? departmentId)
        {
            try
            {
                await using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@FromDate", fromdate);
                param.Add("@ToDate", todate);
                param.Add("@UserID", userId);
                param.Add("@DepartmentId", departmentId);
                var result = await con.QueryAsync<AgentAdminExportReportViewModel>("Usp_Report_DetailTicketStatus", param,
                     null, 0, CommandType.StoredProcedure);

                return result.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<AgentAdminExportReportViewModel>> GetDepartmentWiseTicketStatusReport(string fromdate, string todate, int? departmentId)
        {
            try
            {
                await using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@FromDate", fromdate);
                param.Add("@ToDate", todate);
                param.Add("@DepartmentId", departmentId);
                var result = await con.QueryAsync<AgentAdminExportReportViewModel>("Usp_Report_DepartmentWiseTicketStatus", param,
                    null, 0, CommandType.StoredProcedure);

                return result.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<TicketOverduesViewModel>> GetTicketOverduesbyDepartmentWiseReport(string fromdate, string todate, string overdueTypeId, int? departmentId)
        {
            try
            {
                await using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@FromDate", fromdate);
                param.Add("@ToDate", todate);
                param.Add("@DepartmentId", departmentId);
                param.Add("@OverdueTypeId", overdueTypeId);

                var result = await con.QueryAsync<TicketOverduesViewModel>("Usp_Report_TicketOverdues", param,
                    null, 0, CommandType.StoredProcedure);

                return result.ToList();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<EscalationReportViewModel>> GetEscalationbyDepartmentReport(string fromdate, string todate, int? departmentId)
        {
            try
            {
                await using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@FromDate", fromdate);
                param.Add("@ToDate", todate);
                param.Add("@DepartmentId", departmentId);
                var result = await con.QueryAsync<EscalationReportViewModel>("Usp_Report_Escalation", param, null, 0, CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DeletedTicketHistoryReportViewModel>> GetDeletedTicketHistoryByDepartmentReport(string fromdate, string todate, int? departmentId)
        {
            try
            {
                await using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@FromDate", fromdate);
                param.Add("@ToDate", todate);
                param.Add("@DepartmentId", departmentId);
                var result = await con.QueryAsync<DeletedTicketHistoryReportViewModel>("Usp_Report_DeletedTicketHistory", param, null, 0, CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<PriorityWiseTicketStatusReportViewModel>> GetPriorityWiseTicketStatusReport(string fromdate, string todate, string priorityId, int? departmentId)
        {
            try
            {
                await using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@FromDate", fromdate);
                param.Add("@ToDate", todate);
                param.Add("@PriorityID", priorityId);
                param.Add("@DepartmentId", departmentId);
                var result = await con.QueryAsync<PriorityWiseTicketStatusReportViewModel>("Usp_Report_PriorityWiseTicketStatus", param, null, 0, CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<UserDetailsReportViewModel>> GetUsersDetailsReport(long? userId)
        {
            try
            {
                await using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@UserID", userId);
                var result = await con.QueryAsync<UserDetailsReportViewModel>("Usp_Report_UsersDetails", param, null, 0, CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<UserWiseCheckinCheckOutReportViewModel>> UserWiseCheckinCheckOutReport(string fromdate, string todate, long? userId)
        {
            try
            {
                await using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@FromDate", fromdate);
                param.Add("@ToDate", todate);
                param.Add("@UserID", userId);
                var result = await con.QueryAsync<UserWiseCheckinCheckOutReportViewModel>(
                    "Usp_Report_CheckIn_CheckOut_Time_UserWise", param, null, 0, CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}