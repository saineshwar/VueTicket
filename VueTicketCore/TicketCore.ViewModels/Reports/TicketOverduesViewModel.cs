namespace TicketCore.ViewModels.Reports
{
    public class TicketOverduesViewModel
    {
        public string TrackingId { get; set; }
        public string DepartmentName { get; set; }
        public string AgentName { get; set; }
        public string OverdueType { get; set; }
        public string TicketAssignedDate { get; set; }
        public string OverdueDate { get; set; }
        public int? Days { get; set; }
        public int? Hours { get; set; }
    }
}