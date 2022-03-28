using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace TicketCore.ViewModels.Tickets.Grids
{
    public class AgentManagerTicketGrid
    {
        [Display(Name = "Agent")]
        public long? AgentsId { get; set; }
        public List<SelectListItem> ListofAgents { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }
        public List<SelectListItem> ListofDepartment { get; set; }

        [Display(Name = "SearchIn")]
        public int? SearchIn { get; set; }
        public List<SelectListItem> ListofSearch { get; set; }

        [Display(Name = "Priority")]
        public int? PriorityId { get; set; }
        public List<SelectListItem> ListofPriority { get; set; }

        [Display(Name = "Status")]
        public int? StatusId { get; set; }
        public List<SelectListItem> ListofStatus { get; set; }

        public int? CurrentPage;

        public StaticPagedList<AgentManagerTicketGridViewModel> ListofTicketDetails { get; set; }

        [Display(Name = "Search")]
        public string Search { get; set; }
    }

    public class AgentManagerTicketGridViewModel
    {
        public int RowNum { get; set; }
        public long TicketId { get; set; }
        public int? DepartmentId { get; set; }
        public string TrackingId { get; set; }
        public string Subject { get; set; }
        public string DepartmentName { get; set; }
        public string Name { get; set; }
        public string AgentName { get; set; }
        public string Priority { get; set; }
        public int? PriorityId { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public DateTime? FirstResponseDue { get; set; }
        public bool? FirstResponseStatus { get; set; }
        public DateTime? ResolutionDue { get; set; }
        public bool? ResolutionStatus { get; set; }
        public bool? EveryResponseStatus { get; set; }
        public bool? EscalationStage1Status { get; set; }
        public bool? EscalationStage2Status { get; set; }
        public DateTime? TicketAssignedDate { get; set; }
        public DateTime? TicketUpdatedDate { get; set; }
        public bool DeleteStatus { get; set; }
        public string StatusInfo { get; set; }
    }
}