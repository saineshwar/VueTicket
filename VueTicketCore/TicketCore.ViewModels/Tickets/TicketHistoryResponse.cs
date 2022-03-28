namespace TicketCore.ViewModels.Tickets
{
    public class TicketHistoryResponse
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public string Activities { get; set; }
        public string StatusText { get; set; }
        public string PriorityName { get; set; }
        public string CategoryName { get; set; }
        public string ProcessDate { get; set; }
        public string Viewcolor { get; set; }
        public string Nameinitial { get; set; }
        public string Avatar { get; set; }
    }

    public class TicketHistoryRequest
    {
        public long? Ticketid { get; set; }
    }
}