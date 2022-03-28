namespace TicketCore.ViewModels.Reports
{
    public class UserWiseCheckinCheckOutReportViewModel
    {
        public string AgentName { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public string TotalHours { get; set; }
    }
}