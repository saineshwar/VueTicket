using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketCore.ViewModels.Reports;

namespace TicketCore.Data.Reports.Queries
{
    public interface IReportQueries
    {
        List<SelectListItem> AgentAdminReportList();

        Task<List<AgentAdminExportReportViewModel>> GetDetailTicketStatusReport(string fromdate, string todate,
            long? userId, int? departmentId);

         Task<List<AgentAdminExportReportViewModel>> GetDepartmentWiseTicketStatusReport(string fromdate,
            string todate, int? departmentId);

         Task<List<TicketOverduesViewModel>> GetTicketOverduesbyDepartmentWiseReport(string fromdate, string todate,
             string overdueTypeId, int? departmentId);

          Task<List<EscalationReportViewModel>> GetEscalationbyDepartmentReport(string fromdate, string todate,
             int? departmentId);

          Task<List<DeletedTicketHistoryReportViewModel>> GetDeletedTicketHistoryByDepartmentReport(string fromdate,
              string todate, int? departmentId);

          Task<List<PriorityWiseTicketStatusReportViewModel>> GetPriorityWiseTicketStatusReport(string fromdate,
              string todate, string priorityId, int? departmentId);

          Task<List<UserDetailsReportViewModel>> GetUsersDetailsReport(long? userId);

           Task<List<UserWiseCheckinCheckOutReportViewModel>> UserWiseCheckinCheckOutReport(string fromdate,
              string todate, long? userId);
    }
}