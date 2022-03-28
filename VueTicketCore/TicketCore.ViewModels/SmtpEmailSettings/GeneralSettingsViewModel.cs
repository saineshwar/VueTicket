using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TicketCore.ViewModels.SmtpEmailSettings
{
    public class GeneralSettingsViewModel
    {
        public int? GeneralSettingsId { get; set; }
        [Required(ErrorMessage = "Required From Email")]
        [DisplayName("From Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Required From Name")]
        [DisplayName("From Name")]
        public string Name { get; set; }

        [DisplayName("Support EmailId")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail address")]
        public string SupportEmailId { get; set; }

        [DisplayName("Website Title")]
        public string WebsiteTitle { get; set; }

        [DisplayName("Website Url")]
        public string WebsiteUrl { get; set; }

        [DisplayName("Enable Email Feature")]
        public bool EnableEmailFeature { get; set; }
        [DisplayName("Enable SMS Feature")]
        public bool EnableSmsFeature { get; set; }
        [DisplayName("Enable Signature Feature")]
        public bool EnableSignatureFeature { get; set; }
    }
}