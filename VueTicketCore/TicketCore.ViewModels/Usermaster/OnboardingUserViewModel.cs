using System.ComponentModel.DataAnnotations;

namespace TicketCore.ViewModels.Usermaster
{

    public class OnboardingUserViewModel
    {
        [Required(ErrorMessage = "Enter FirstName")]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Please enter a valid FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter LastName")]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Please enter a valid LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Gender Required")]
        public string Gender { get; set; }
    }

    public class VerifyProcess
    {
        [Required(ErrorMessage = "Enter OTP")]
        [RegularExpression(@"^(\d{6})$", ErrorMessage = "Wrong OTP Entered")]
        public string EnterOtp { get; set; }
    }

}