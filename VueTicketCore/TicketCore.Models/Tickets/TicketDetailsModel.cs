using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Tickets
{
    [Table("TicketDetails")]
    public class TicketDetails
    {
        [Key]
        public long TicketDetailsId { get; set; }
        [MaxLength(200)]
        public string Subject { get; set; }
        [MaxLength(2000)]
        public string Message { get; set; }
        public long TicketId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
    }
}