using System.ComponentModel.DataAnnotations;

namespace TicketCore.ViewModels.Masters
{
    public class HolidayViewModel
    {
        [Required(ErrorMessage = "Holiday Date")]
        public string HolidayDate { get; set; }
        [Required(ErrorMessage = "Holiday Name")]
        public string HolidayName { get; set; }
    }
}