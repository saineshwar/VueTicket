using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Masters
{
    [Table("OverdueTypes")]
    public class OverdueTypes
    {
        [Key]
        public int OverdueTypeId { get; set; }
        public string OverdueType { get; set; }
    }
}