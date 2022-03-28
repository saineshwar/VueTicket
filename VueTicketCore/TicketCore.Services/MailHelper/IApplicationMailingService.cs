namespace TicketCore.Services.MailHelper
{
    public interface IApplicationMailingService
    {
        void TicketAgentReplyEmail(string templateUrl, string onevent, long? ticketId, string statusname, string repliedby,
            long? createdBy);

        void TicketUserReplyEmail(string templateUrl, string onevent, long? ticketId, string statusname, string repliedby,
            long? createdBy);

        void TicketEditedTicketEmail(string templateUrl, string onevent, long? ticketId, string statusname,
            string repliedby, long? createdBy);

        void TicketEditedReplyTicketEmail(string templateUrl, string onevent, long? ticketId, string statusname,
            string repliedby, long? createdBy);

        void ChangeTicketPriorityEmail(string templateUrl, string onevent, long? ticketId, string priority,
            string repliedby, long? createdBy);
    }
}