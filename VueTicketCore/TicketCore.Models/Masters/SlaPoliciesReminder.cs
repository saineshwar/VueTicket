using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Masters
{
    [Table("SLAPoliciesReminder")]
    public class SlaPoliciesReminder
    {
        public int SlaPoliciesReminderId { get; set; }
        public int? FirstResponseHour { get; set; }
        public int? FirstResponseMins { get; set; }
        public int? NextResponseHour { get; set; }
        public int? NextResponseMins { get; set; }
        public int? ResolutionResponseHour { get; set; }
        public int? ResolutionResponseMins { get; set; }
        public DateTime? CreateDate { get; set; }
        public long UserId { get; set; }
        public int BusinessHoursId { get; set; }
    }
}