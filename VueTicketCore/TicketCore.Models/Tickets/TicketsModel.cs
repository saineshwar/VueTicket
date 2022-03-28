using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Tickets
{
    [Table("TicketSummary")]
    public class TicketSummary
    {
        [Key]
        public long TicketSummaryId { get; set; }
        [MaxLength(20)]
        public string TrackingId { get; set; }
        public int? PriorityId { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public bool StatusAssigned { get; set; }
        public long? InternalUserId { get; set; }
        public long TicketId { get; set; }
    }
}