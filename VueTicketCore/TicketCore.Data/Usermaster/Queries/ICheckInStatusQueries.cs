using System.Collections.Generic;
using TicketCore.ViewModels.Usermaster;

namespace TicketCore.Data.Usermaster.Queries
{
    public interface ICheckInStatusQueries
    {
        void StatusCheckInCheckOut(long userId);
        bool CheckIsalreadyCheckedIn(long userId);
        List<AgentDailyActivityModel> AgentDailyActivity(long userId);
        bool CheckIsCategoryAssignedtoAgent(long userId);
    }
}