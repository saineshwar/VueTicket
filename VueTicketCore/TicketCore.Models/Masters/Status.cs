using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Masters
{
    [Table("Status")]
    public class Status
    {
        [Key]
        public short StatusId { get; set; }
        public string StatusText { get; set; }
        public bool IsInternalStatus { get; set; }
        public bool ShowAgent { get; set; }
    }
}