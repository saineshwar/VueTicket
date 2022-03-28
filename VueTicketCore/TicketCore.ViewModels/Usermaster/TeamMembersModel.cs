using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketCore.ViewModels.Usermaster
{
    public class TeamMembersModel
    {
        [Display(Name = "Department")]
        [Required(ErrorMessage = "Department Required")]
        public int? DepartmentId { get; set; }
        public List<SelectListItem> ListofDepartments { get; set; }
    }

    public class RequestTeamViewModel
    {
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
    }

    public class TeamMembers
    {
        public int UserId { get; set; }

        [DisplayName("FullName")]
        public string FullName { get; set; }
        public string AssignedRole { get; set; }

        [DisplayName("Gender")]
        public string Gender { get; set; }
        public string AssignedOn { get; set; }
    }
}