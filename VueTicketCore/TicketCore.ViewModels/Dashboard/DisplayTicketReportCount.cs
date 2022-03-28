namespace TicketCore.ViewModels.Dashboard
{
    public class DisplayTicketReportCount
    {
        public string Names { get; set; }
        public int Open { get; set; }
        public int Resolved { get; set; }
        public int Confirmed { get; set; }
        public int InProgress { get; set; }
        public int OnHold { get; set; }
        public int RecentlyEditedValue { get; set; }
        public int Replied { get; set; }
        public int Deleted { get; set; }
        public int FirstResponseOverdue { get; set; }
        public int ResolutionOverdue { get; set; }
        public int EveryResponseOverdue { get; set; }
        public int EscalationStage1 { get; set; }
        public int EscalationStage2 { get; set; }
        public int Closed { get; set; }
        public int ReOpened { get; set; }
        public int Total { get; set; }
    }
}