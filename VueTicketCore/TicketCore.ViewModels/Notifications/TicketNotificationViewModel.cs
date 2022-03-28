namespace TicketCore.ViewModels.Notifications
{
    public class TicketNotificationViewModel
    {
        public long? TicketId { get; set; }
        public long? NotificationId { get; set; }
        public string TrackingId { get; set; }
        public string TicketAssignedDate { get; set; }
        public string NotificationDate { get; set; }
        public string NotificationType { get; set; }
        public string Message { get; set; }
    }

    public class RequestNotificationViewModel
    {
        public long? NotificationId { get; set; }
    }

    public class ShowNotificationViewModel
    {
        public int? NotificationCount { get; set; }
        public string NotificationType { get; set; }
        public int? TotalHours { get; set; }
        public int? TotalMinutes { get; set; }
        public int? Totaldays { get; set; }
    }

}