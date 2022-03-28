using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.SmtpEmailSettings
{
    [Table("SMTPEmailSettings")]
    public class SmtpEmailSettings
    {
        [Key]
        public int SmtpProviderId { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public int? Timeout { get; set; }
        public string SslProtocol { get; set; }
        public string TlSProtocol { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Status { get; set; }
        public string Name { get; set; }
        public long UserId { get; set; }
        public bool IsDefault { get; set; }
        public string MailSender { get; set; }
        public string EmailTo { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}