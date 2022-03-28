using System.Collections.Generic;
using TicketCore.ViewModels.Dashboard;

namespace TicketCore.Data.Dashboard.Queries
{
    public interface IDashboardQueries
    {
        List<DisplayTicketCount> GetUserDepartmentWiseUserTicketCountbyUserId(long? userId,
            int? departmentId);

        List<DisplayTicketCount> GetUserDepartmentWiseAgentTicketCountbyUserId(long? userId,
            int? departmentId);

        List<DisplayTicketCount> GetUserDepartmentWiseAgentManagerTicketCountbyUserId(int? departmentId);
        List<DisplayStarPerformer> GetStartAgents(int? departmentId);
    }
}