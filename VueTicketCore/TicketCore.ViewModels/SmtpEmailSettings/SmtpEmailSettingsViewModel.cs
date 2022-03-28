using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TicketCore.ViewModels.SmtpEmailSettings
{
    public class SmtpEmailSettingsViewModel
    {
        [Required(ErrorMessage = "Required Host")]
        [DisplayName("Host")]
        public string Host { get; set; }

        [DisplayName("Port")]
        [Required(ErrorMessage = "Required Port")]
        public string Port { get; set; }

        [DisplayName("Timeout")]
        [Required(ErrorMessage = "Required Timeout")]
        public int? Timeout { get; set; }

        [DisplayName("SSL Protocol")]
        [Required(ErrorMessage = "Required SSL Protocol")]
        public string SslProtocol { get; set; }

        [DisplayName("TLS Protocol")]
        [Required(ErrorMessage = "Required TLS Protocol")]
        public string TlSProtocol { get; set; }

        [DisplayName("Username")]
        [Required(ErrorMessage = "Required Username")]
        public string Username { get; set; }

        [DisplayName("Password")]
        public string Password { get; set; }

        public DateTime? CreatedOn { get; set; }
        [Required(ErrorMessage = "Required Setting Name")]

        [DisplayName("Setting Name")]
        public string Name { get; set; }

        [DisplayName("Sender EmailId")]
        [Required(ErrorMessage = "Required Sender EmailId")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please Enter a Valid Sender EmailId address")]
        public string MailSender { get; set; }
        public int? SmtpProviderId { get; set; }

        [DisplayName("Send EmailId to Test")]
        [Required(ErrorMessage = "Required EmailTo")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please Enter a Valid Sender EmailId address")]
        public string EmailTo { get; set; }
    }

    public class RequestSmtp
    {
        public int? SmtpProviderId { get; set; }
    }
}