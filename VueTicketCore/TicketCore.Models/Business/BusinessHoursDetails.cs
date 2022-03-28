using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Business
{
    [Table("BusinessHoursDetails")]
    public class BusinessHoursDetails
    {
        [Key]
        public int BusinessHoursDetailsId { get; set; }
        public string Day { get; set; }
        public string MorningTime { get; set; }
        public string MorningPeriods { get; set; }
        public string EveningTime { get; set; }
        public string EveningPeriods { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public int BusinessHoursId { get; set; }
        public string StartTime { get; set; }
        public string CloseTime { get; set; }

    }
}