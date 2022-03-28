namespace TicketCore.ViewModels.Reports
{
    public class EscalationReportViewModel
    {
        public string TrackingId { get; set; }
        public string DepartmentName { get; set; }
        public string TicketAssignedDate { get; set; }
        public string FirstEscalationDate { get; set; }
        public string SecondEscalationDate { get; set; }
        public string Stage1User { get; set; }
        public string Stage2User { get; set; }
    }
}