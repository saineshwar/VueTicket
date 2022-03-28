using Microsoft.AspNetCore.Mvc;
using TicketCore.Common;
using TicketCore.Data.Notifications.Queries;
using TicketCore.Web.Extensions;

namespace TicketCore.Web.Views.Shared.Components.Notification
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly ITicketNotificationQueries _ticketNotificationQueries;
        public NotificationViewComponent(ITicketNotificationQueries ticketNotificationQueries)
        {
            _ticketNotificationQueries = ticketNotificationQueries;
        }

        public IViewComponentResult Invoke()
        {
            var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
            var notificationData = _ticketNotificationQueries.GetTicketNotificationCount(userId);
            ViewBag.TotalNotificationCount = _ticketNotificationQueries.GetTotalNotificationCount(userId);
            return View(notificationData);
        }
    }
}