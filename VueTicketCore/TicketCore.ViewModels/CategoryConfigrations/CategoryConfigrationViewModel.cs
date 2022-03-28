using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketCore.ViewModels.CategoryConfigrations
{
    public class DepartmentConfigrationViewModel
    {
        [DisplayName("Department Agent Admin")]
        [Required(ErrorMessage = "Please Select Department Agent Admin")]
        public int AgentAdminUserId { get; set; }
        public List<SelectListItem> ListofAdmin { get; set; }

        [DisplayName("Department HOD")]
        [Required(ErrorMessage = "Please Select Department HOD")]
        public int HodUserId { get; set; }
        public List<SelectListItem> ListofHod { get; set; }


        [DisplayName("BusinessHours")]
        [Required(ErrorMessage = "Please Select BusinessHours")]
        public int? BusinessHoursId { get; set; }
        public List<SelectListItem> ListofBusinessHours { get; set; }

        [DisplayName("Department")]
        [Required(ErrorMessage = "Please Select Department.")]
        public int DepartmentId { get; set; }
        public List<SelectListItem> ListofDepartment { get; set; }

        public bool Status { get; set; }
    }

    public class EditDepartmentConfigrationViewModel
    {
        public int DepartmentConfigrationId { get; set; }

        [DisplayName("Department Agent Admin")]
        [Required(ErrorMessage = "Please Select Department Agent Admin")]
        public long AgentAdminUserId { get; set; }
        public List<SelectListItem> ListofAdmin { get; set; }

        [DisplayName("Department HOD")]
        [Required(ErrorMessage = "Please Select Department HOD")]
        public long HodUserId { get; set; }
        public List<SelectListItem> ListofHod { get; set; }

        [DisplayName("Business Hours")]
        [Required(ErrorMessage = "Please Select BusinessHours")]
        public int? BusinessHoursId { get; set; }
        public List<SelectListItem> ListofBusinessHours { get; set; }

        [DisplayName("Department")]
        [Required(ErrorMessage = "Please Select Department.")]
        public int? DepartmentId { get; set; }
        public List<SelectListItem> ListofDepartment { get; set; }

        public bool Status { get; set; }
    }

    public class ShowDepartmentConfigration
    {
        public int DepartmentConfigrationId { get; set; }
        public string DepartmentName { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string HodName { get; set; }
    }
}