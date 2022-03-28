using System.ComponentModel.DataAnnotations;

namespace TicketCore.ViewModels.Signup
{
    public class SignupFormViewModel
    {
        [MinLength(6, ErrorMessage = "Minimum Username must be 6 in characters")]
        [Required(ErrorMessage = "Username Required")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "Email Id")]
        [Required(ErrorMessage = "Email Id Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail address")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Mobile-no Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong Mobile-no")]
        [MaxLength(10)]
        [Display(Name = "Mobile No.")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password Required")]
        [Compare("Password", ErrorMessage = "Enter Valid Password")]
        public string ConfirmPassword { get; set; }
        public bool agreeTerms { get; set; }
    }
}