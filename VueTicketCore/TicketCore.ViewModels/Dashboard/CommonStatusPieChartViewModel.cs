namespace TicketCore.ViewModels.Dashboard
{
    public class CommonStatusPieChartViewModel
    {
        public int TotalCount { get; set; }
        public string StatusName { get; set; }
        public int StatusId { get; set; }
        
    }

    public class CommonPriorityPieChartViewModel
    {
        public int TotalCount { get; set; }
        public string PriorityName { get; set; }
        public int PriorityId { get; set; }
    }

}