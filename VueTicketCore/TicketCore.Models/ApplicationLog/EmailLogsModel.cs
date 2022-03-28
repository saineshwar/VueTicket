using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.ApplicationLog
{
    [Table("EmailLogs")]
    public class EmailLogsModel
    {
        [Key]
        public long EmailLogId { get; set; }
        public string EmailId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public string TriggeredEvent { get; set; }
    }
}