using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Business
{
    [Table("BusinessHours")]
    public class BusinessHoursModel
    {
        [Key]
        public int BusinessHoursId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int HelpdeskHoursType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool Status { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
    }
}