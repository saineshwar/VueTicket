using System.ComponentModel.DataAnnotations;

namespace TicketCore.ViewModels.Rolemasters
{
    public class RoleMasterViewModel
    {
        [Display(Name = "Role")]
        [Required(ErrorMessage = "Enter RoleName")]
        public string RoleName { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Choose Status")]
        public bool Status { get; set; }
    }

    public class EditRoleMasterViewModel
    {
        [Display(Name = "Role")]
        [Required(ErrorMessage = "Enter RoleName")]
        public string RoleName { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Choose Status")]
        public bool Status { get; set; }

        public int RoleId { get; set; }
    }

    public class RequestDeleteRole
    {
        public int RoleId { get; set; }
    }

    public class RoleMasterGrid
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Status { get; set; }
    }
}