using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace TicketCore.ViewModels.Tickets.Grids
{
    public class UserTicketGrid
    {
        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }
        public List<SelectListItem> ListofDepartment { get; set; }

        [Display(Name = "SearchIn")]
        public int? SearchIn { get; set; }
        public List<SelectListItem> ListofSearch { get; set; }

        [Display(Name = "Priority")]
        public int? PriorityId { get; set; }
        public List<SelectListItem> ListofPriority { get; set; }

        public int? CurrentPage;

        public StaticPagedList<UserTicketGridViewModel> ListofUserTicket { get; set; }

        [Display(Name = "Search")]
        public string Search { get; set; }

        [Display(Name = "Status")]
        public int? StatusId { get; set; }
        public List<SelectListItem> ListofStatus { get; set; }
    }
}