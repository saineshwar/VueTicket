namespace TicketCore.ViewModels.Reports
{
    public class DeletedTicketHistoryReportViewModel
    {
        public string TrackingId { get; set; }
        public string CreatedUser { get; set; }
        public string DeletedUser { get; set; }
        public string DepartmentName { get; set; }
        public string AssignedDate { get; set; }
        public string DeletedDate { get; set; }
    }
}