namespace TicketCore.Core
{
    public interface ITicketHistoryHelper
    {
        string CreateMessage(int? priorityId, int? departmentId);
        string StatusMessage(int? statusId);
        string PriorityMessage(int? priorityId);
        string DepartmentMessage(int? DepartmentId);
        string ReplyMessage(int? statusId);
        string DeleteTicketReplyMessage();
        string EditTicket();
        string EditReplyTicket();
        string DeleteTicketAttachment(string ticketid);
        string DeleteTicketReplyAttachment(string ticketid);
        string TicketDelete();
        string TicketRestore();
        string AssignTickettoUser();
        string TransferMessage(int? fromdepartmentId, int? todepartmentId);
    }
}