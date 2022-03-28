using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Business
{
    [Table("BusinessHoursType")]
    public class BusinessHoursType
    {
        [Key]
        public int BusinessHoursTypeId { get; set; }
        public string BusinessHoursName { get; set; }
    }
}