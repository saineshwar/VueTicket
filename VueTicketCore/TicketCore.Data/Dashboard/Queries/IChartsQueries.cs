using System.Collections.Generic;
using TicketCore.ViewModels.Dashboard;

namespace TicketCore.Data.Dashboard.Queries
{
    public interface IChartsQueries
    {
        List<CommonStatusPieChartViewModel> GetCommonPieChartData(long? userId, int? departmentId, int? roleId);
        List<CommonPriorityPieChartViewModel> CommonPriorityPieChartData(long? userId, int? departmentId, int? roleId);
        List<MonthWiseDataViewModel> GetTicketCreatedMonthWiseChartData(long? userId, int? departmentId, int? roleId);
        List<MonthWiseDataViewModel> GetTicketResolvedMonthWiseChartData(long? userId, int? departmentId, int? roleId);
        List<DisplayTicketReportCount> GetAgentsTeamsticketsDetailCount(int? departmentId);
        List<ResponseResolvedTicketViewModel> GetResolvedTicketAgentWiseCount(int? departmentId);
        List<ResponseResolvedTicketViewModel> GetOpenTicketAgentWiseCount(int? departmentId);
    }
}