using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketCore.ViewModels.Masters
{
    public class SlaPoliciesViewModel
    {
        [DisplayName("Priority")]
        [Required(ErrorMessage = "Required Priority.")]
        public int? PriorityId { get; set; }
        public List<SelectListItem> ListofPriority { get; set; }


        [Display(Name = "First Response Hour")]
        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public int? FirstResponseHour { get; set; }

        [Display(Name = "First Response Mintues")]
        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public int? FirstResponseMins { get; set; }

        [Display(Name = "Next Response Hour")]
        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public int? NextResponseHour { get; set; }


        [Display(Name = "Next Response Mintues")]
        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public int? NextResponseMins { get; set; }


        [Display(Name = "Resolution Response Hour")]
        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public int? ResolutionResponseHour { get; set; }

        [Display(Name = "Resolution Response Mintues")]
        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public int? ResolutionResponseMins { get; set; }

        [Display(Name = "Escalation")]
        public bool Escalation { get; set; }

        [Required(ErrorMessage = "Required Business Hours")]
        [DisplayName("Business Hours")]
        public string BusinessHoursId { get; set; }
        public List<SelectListItem> ListofBusinessHours { get; set; }
    }
}