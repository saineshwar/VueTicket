using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketCore.ViewModels.Reports
{
    public class AgentAdminReportViewModel
    {
        [DisplayName("Agent")]
        public long? AgentId { get; set; }
        public List<SelectListItem> ListofAgent { get; set; }

        [DisplayName("Fromdate")]
        public string Fromdate { get; set; }

        [DisplayName("Todate")]
        public string Todate { get; set; }

      

        [DisplayName("Report")]
        public int ReportId { get; set; }
        public List<SelectListItem> ListofReport { get; set; }

        [DisplayName("OverdueType")]
        public string OverdueTypeId { get; set; }
        public List<SelectListItem> ListofOverdueTypes { get; set; }

        [DisplayName("Priority")]
        public string PriorityId { get; set; }
        public List<SelectListItem> ListofPriority { get; set; }

        [DisplayName("Department")]
        public int? DepartmentId { get; set; }
        public List<SelectListItem> ListofDepartment { get; set; }
    }

}