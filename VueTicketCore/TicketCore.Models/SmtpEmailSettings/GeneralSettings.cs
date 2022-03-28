using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.SmtpEmailSettings
{
    [Table("GeneralSettings")]
    public class GeneralSettings
    {
        [Key]
        public int? GeneralSettingsId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string SupportEmailId { get; set; }
        public string WebsiteTitle { get; set; }
        public string WebsiteUrl { get; set; }
        public bool EnableEmailFeature { get; set; }
        public bool EnableSmsFeature { get; set; }
        public bool EnableSignatureFeature { get; set; }
    }
}