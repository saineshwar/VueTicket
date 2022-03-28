using System.Collections.Generic;

namespace TicketCore.ViewModels.Dashboard
{
    public class BarCharDataViewModel
    {
        public class Dataset
        {
            public string label { get; set; }
            public string backgroundColor { get; set; }
            public string borderColor { get; set; }
            public bool pointRadius { get; set; }
            public string pointColor { get; set; }
            public string pointStrokeColor { get; set; }
            public string pointHighlightFill { get; set; }
            public string pointHighlightStroke { get; set; }
            public List<int> data { get; set; }
        }

        public class Root
        {
            public List<string> labels { get; set; }
            public List<Dataset> datasets { get; set; }
        }
    }
}