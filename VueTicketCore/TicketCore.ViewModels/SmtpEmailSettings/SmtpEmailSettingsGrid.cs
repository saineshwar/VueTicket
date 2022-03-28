using System;

namespace TicketCore.ViewModels.SmtpEmailSettings
{
    public class SmtpEmailSettingsGrid
    {
        public long SmtpProviderId { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public int? Timeout { get; set; }
        public string SslProtocol { get; set; }
        public string TlSProtocol { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Name { get; set; }
        public string IsDefault { get; set; }
    }
}