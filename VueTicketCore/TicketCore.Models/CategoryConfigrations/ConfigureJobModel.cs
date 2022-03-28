using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace TicketCore.Models.CategoryConfigrations
{
    [Table("ConfigureJobs")]
    public class ConfigureJobModel
    {
        [Key]
        public int ConfigureJobId { get; set; }
        public bool AssignTicketsJob { get; set; }
        public bool TicketOverdueJob { get; set; }
        public bool OverdueNotificationJob { get; set; }
        public bool OverdueEveryResponsJob { get; set; }
        public bool AutoEscalationJobStage1 { get; set; }
        public bool AutoEscalationJobStage2 { get; set; }
        public bool AutoCloseTicketsJob { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
    }
}