using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TicketCore.ViewModels.Profiles
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Old Password Required")]
        [DisplayName("OldPassword")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New Password Required")]
        [DisplayName("NewPassword")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password Required")]
        [DisplayName("ConfirmPassword")]

        public string ConfirmPassword { get; set; }
    }
}