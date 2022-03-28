using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Masters
{
    [Table("SLAPolicies")]
    public class SlaPolicies
    {
        [Key]
        public int SlaPoliciesId { get; set; }
        public int? PriorityId { get; set; }
        public int? FirstResponseHour { get; set; }
        public int? FirstResponseMins { get; set; }
        public int? NextResponseHour { get; set; }
        public int? NextResponseMins { get; set; }
        public int? ResolutionResponseHour { get; set; }
        public int? ResolutionResponseMins { get; set; }
        public DateTime? CreateDate { get; set; }
        public long UserId { get; set; }
        public bool EscalationStatus { get; set; }
        public int BusinessHoursId { get; set; }
    }
}