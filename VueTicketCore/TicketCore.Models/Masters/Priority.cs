using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Masters
{
    [Table("Priority")]
    public class Priority
    {
        [Key]
        public short PriorityId { get; set; }
        public string PriorityName { get; set; }
    }
}