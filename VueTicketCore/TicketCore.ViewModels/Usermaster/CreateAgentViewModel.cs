using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketCore.ViewModels.Usermaster
{
    public class CreateAgentViewModel
    {
        [MinLength(6, ErrorMessage = "Minimum Username must be 6 in characters")]
        [Required(ErrorMessage = "Username Required")]

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Enter FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Enter LastName")]
        public string LastName { get; set; }

        [Display(Name = "EmailId")]
        [Required(ErrorMessage = "EmailID Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail address")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Mobile-no Required")]
        [Display(Name = "MobileNo")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong Mobile-no")]
        public string MobileNo { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender Required")]
        public string Gender { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Password")]
        [MinLength(7, ErrorMessage = "Minimum Password must be 7 in characters")]
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }

        [Display(Name = "ConfirmPassword")]
        [Required(ErrorMessage = "Confirm Password Required")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Enter Valid Password")]
        public string ConfirmPassword { get; set; }


        [DisplayName("Department")]
        [Required(ErrorMessage = "Please Select Department.")]
        public int? DepartmentId { get; set; }
        public List<SelectListItem> ListofDepartment { get; set; }

    }

    public class EditAgentViewModel
    {
        public long UserId { get; set; }

        [MinLength(6, ErrorMessage = "Minimum Username must be 6 in characters")]
        [Required(ErrorMessage = "Username Required")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Enter FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Enter LastName")]
        public string LastName { get; set; }

        [Display(Name = "EmailId")]
        [Required(ErrorMessage = "EmailID Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string EmailId { get; set; }

        [Display(Name = "MobileNo")]
        [Required(ErrorMessage = "Mobileno Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong Mobileno")]
        public string MobileNo { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender Required")]
        public string Gender { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [DisplayName("Department")]
        [Required(ErrorMessage = "Please Select Department.")]
        public int DepartmentId { get; set; }
        public List<SelectListItem> ListofDepartment { get; set; }

    }
}