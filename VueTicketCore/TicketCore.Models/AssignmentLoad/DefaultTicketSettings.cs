using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.AssignmentLoad
{
    [Table("DefaultTicketSettings")]
    public class DefaultTicketSettings
    {
        [Key]
        public int DefaultTicketId { get; set; }
        public int? MinTicketsCount { get; set; }
        public int? MaxTicketCount { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long UserId { get; set; }
        public int? AutoTicketsCloseHour { get; set; }
        public int? AutoTicketsCloseMin { get; set; }
    }
}