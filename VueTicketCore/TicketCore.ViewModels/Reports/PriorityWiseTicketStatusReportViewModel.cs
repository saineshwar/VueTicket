namespace TicketCore.ViewModels.Reports
{
    public class PriorityWiseTicketStatusReportViewModel
    {
        public string PriorityName { get; set; }
        public string TicketAssignedDate { get; set; }
        public int? Total { get; set; }
        public int? OpenTicket { get; set; }
        public int? ResolvedTicket { get; set; }
        public int? InProgressTicket { get; set; }
        public int? OnHoldTicket { get; set; }
        public int? RecentlyEditedTicket { get; set; }
        public int? RepliedTicket { get; set; }
        public int? DeletedTicket { get; set; }
        public int? FirstResponseOverdue { get; set; }
        public int? ResolutionOverdue { get; set; }
        public int? EveryResponseOverdue { get; set; }
        public int? EscalationStage1 { get; set; }
        public int? EscalationStage2 { get; set; }
        public int? ClosedTicket { get; set; }
        public int? ReOpened { get; set; }
    }
}