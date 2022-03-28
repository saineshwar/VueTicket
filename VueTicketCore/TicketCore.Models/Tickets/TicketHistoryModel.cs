using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Tickets
{
    [Table("TicketHistory")]
    public class TicketHistoryModel
    {
        [Key]
        public long TicketHistoryId { get; set; }
        public string Message { get; set; }
        public DateTime? ProcessDate { get; set; }
        public long? UserId { get; set; }
        public long? TicketId { get; set; }
        public int? StatusId { get; set; }
        public int? PriorityId { get; set; }
        public int? DepartmentId { get; set; }
        public long? TicketReplyId { get; set; }
        public int? ActivitiesId { get; set; }
    }
}