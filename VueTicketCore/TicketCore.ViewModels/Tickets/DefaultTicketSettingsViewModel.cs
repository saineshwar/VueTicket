using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TicketCore.ViewModels.Tickets
{
    public class DefaultTicketSettingsViewModel
    {
        public int? DefaultTicketId { get; set; }

        [Required(ErrorMessage = "Enter Min Tickets Count")]
        [DisplayName("Minimum Tickets Count")]
        [RegularExpression("^[0-9]*$")]
        public int? MinTicketsCount { get; set; }

        [Required(ErrorMessage = "Enter Max Tickets Count")]
        [DisplayName("Maximum Tickets Count")]
        [RegularExpression("^[0-9]*$")]
        public int? MaxTicketCount { get; set; }

        [Required(ErrorMessage = "Enter Auto Tickets Close Hour")]
        [DisplayName("Auto Tickets Close Hour")]
        [RegularExpression("^[0-9]*$")]
        public int? AutoTicketsCloseHour { get; set; }

        [Required(ErrorMessage = "Enter Auto Tickets Close Min")]
        [DisplayName("Auto Tickets Close Min")]
        [RegularExpression("^[0-9]*$")]
        public int? AutoTicketsCloseMin { get; set; }
    }
}