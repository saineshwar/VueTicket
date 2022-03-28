using System.Collections.Generic;
using TicketCore.ViewModels.Notifications;

namespace TicketCore.Data.Notifications.Queries
{
    public interface ITicketNotificationQueries
    {
        List<TicketNotificationViewModel> ListofNotification(long? agentId);
        List<ShowNotificationViewModel> GetTicketNotificationCount(long? agentId);
        int GetTotalNotificationCount(long? agentId);
    }
}