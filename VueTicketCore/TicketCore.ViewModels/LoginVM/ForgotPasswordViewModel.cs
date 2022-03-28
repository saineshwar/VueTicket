using System.ComponentModel.DataAnnotations;

namespace TicketCore.ViewModels.LoginVM
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Enter Email address")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid Email address")]
        public string EmailId { get; set; }
    }
}