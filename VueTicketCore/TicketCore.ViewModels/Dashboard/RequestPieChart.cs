using System.Collections.Generic;

namespace TicketCore.ViewModels.Dashboard
{
    public class RequestForCharts
    {
        public int DepartmentId { get; set; }
    }

    public class PieDataset
    {
        public string label { get; set; }
        public List<string> backgroundColor { get; set; }
        public int borderWidth { get; set; }
        public List<int> data { get; set; }
    }

    public class PieRoot
    {
        public List<string> labels { get; set; }
        public List<PieDataset> datasets { get; set; }
    }
}