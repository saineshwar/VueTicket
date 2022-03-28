namespace TicketCore.ViewModels.Dashboard
{
    public class DisplayTicketCount
    {
        public int OpenCount { get; set; }
        public int ResolvedCount { get; set; }
        public int InProgressCount { get; set; }
        public int OnHoldCount { get; set; }
        public int RecentlyEditedCount { get; set; }
        public int RepliedCount { get; set; }
        public int DeletedCount { get; set; }
        public int FirstResponseOverdueCount { get; set; }
        public int ResolutionOverdueCount { get; set; }
        public int EveryResponseOverdueCount { get; set; }
        public int Escalation1Count { get; set; }
        public int Escalation2Count { get; set; }
        public int ClosedCount { get; set; }
        public int ReOpenedCount { get; set; }
    }
}