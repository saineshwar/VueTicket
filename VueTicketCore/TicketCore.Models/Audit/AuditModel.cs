using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Audit
{
    [Table("Audit")]
    public class AuditModel
    {
        [Key]
        public long AuditId { get; set; }
        [MaxLength(50)]
        public string Area { get; set; }
        [MaxLength(50)]
        public string ControllerName { get; set; }
        [MaxLength(50)]
        public string ActionName { get; set; }
        [MaxLength(1)]
        public string LoginStatus { get; set; }
        [MaxLength(23)]
        public string LoggedInAt { get; set; }
        [MaxLength(23)]
        public string LoggedOutAt { get; set; }
        [MaxLength(500)]
        public string PageAccessed { get; set; }
        [MaxLength(50)]
        public string IPAddress { get; set; }
        [MaxLength(50)]
        public string SessionID { get; set; }
        [MaxLength(50)]
        public long? UserID { get; set; }
        [MaxLength(2)]
        public int? RoleId { get; set; }
        [MaxLength(2)]
        public int? LangId { get; set; }
        [MaxLength(2)]
        public bool IsFirstLogin { get; set; }
        [MaxLength(23)]
        public DateTime CurrentDatetime { get; set; }
        [MaxLength(50)]
        public string PortalToken { get; set; }
        public bool Logged { get; set; }
    }
}