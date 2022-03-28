using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.ViewModels.Tickets
{
    [Table("TicketStatus")]
    public class TicketStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TicketStatusId { get; set; }
        public long? TicketId { get; set; }
        public long? AssignedTicketUserId { get; set; }
        public long? CreatedTicketUserId { get; set; }
        public DateTime? TicketAssignedDate { get; set; }
        public DateTime? TicketUpdatedDate { get; set; }
        public int? StatusId { get; set; }
        public bool? DeleteStatus { get; set; }
        public bool IsActive { get; set; }
    }
}