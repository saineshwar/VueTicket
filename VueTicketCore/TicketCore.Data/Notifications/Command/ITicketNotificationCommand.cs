namespace TicketCore.Data.Notifications.Command
{
    public interface ITicketNotificationCommand
    {
        void UpdateTicketNotificationasRead(long? agentId, long? notificationId);
    }
}