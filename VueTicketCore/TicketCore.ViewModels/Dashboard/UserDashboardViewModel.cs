using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketCore.ViewModels.Dashboard
{
    public class UserDashboardViewModel
    {
        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }
        public List<SelectListItem> ListofDepartments { get; set; }

        [Required(ErrorMessage = "TicketId Required")]
        public int? TicketIdSearch { get; set; }
    }
}