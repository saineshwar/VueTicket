using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketCore.ViewModels.Masters
{
    public class SlaPoliciesReminderViewModel
    {
        [Required(ErrorMessage = "Required Business Hours")]
        [DisplayName("Business Hours")]
        public string BusinessHoursId { get; set; }
        public List<SelectListItem> ListofBusinessHours { get; set; }

        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        [DisplayName("First Response Hour")]
        public int? FirstResponseHour { get; set; }

        [DisplayName("First Response Mins")]
        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public int? FirstResponseMins { get; set; }

        [DisplayName("Next Response Hour")]
        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public int? NextResponseHour { get; set; }

        [DisplayName("Next Response Mins")]
        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public int? NextResponseMins { get; set; }

        [DisplayName("Resolution Response Hour")]
        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public int? ResolutionResponseHour { get; set; }

        [DisplayName("Resolution Response Mins")]
        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public int? ResolutionResponseMins { get; set; }

    }
}