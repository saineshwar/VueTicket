using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TicketCore.ViewModels.Usermaster
{
    public class RequesterUserViewModel
    {
        [Required(ErrorMessage = "Enter FirstName")]
        [DisplayName("FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter LastName")]
        [DisplayName("LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "EmailID Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail address")]
        [DisplayName("Email")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Mobile-no Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong Mobile-no")]
        [DisplayName("MobileNo")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Gender Required")]
        [DisplayName("Gender")]
        public string Gender { get; set; }
    }
}