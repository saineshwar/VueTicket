using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Masters
{
    [Table("HolidayList")]
    public class HolidayListModel
    {
        [Key]
        public int HolidayId { get; set; }
        public DateTime HolidayDate { get; set; }
        public string HolidayName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}