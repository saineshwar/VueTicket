using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketCore.ViewModels.Masters
{
    public class BusinessHoursDetailsViewModel
    {
        public IList<string> SelectedDays { get; set; }
        public IList<SelectListItem> ListofDays { get; set; }

        [Display(Name = "Morning Hour")]
        public string MorningHour { get; set; }
        [Display(Name = "Evening Hour")]
        public string EveningHour { get; set; }
        public IList<SelectListItem> ListofHour { get; set; }

        [Display(Name = "Morning Period")]
        public string MorningPeriod { get; set; }

        [Display(Name = "Evening Period")]
        public string EveningPeriod { get; set; }
        public IList<SelectListItem> ListofPeriod { get; set; }

        [Required(ErrorMessage = "Enter Selected Business Hours Type")]
        [Display(Name = "Business Hours Type")]
        public string SelectedBusinessHoursType { get; set; }
        public IList<SelectListItem> ListofBusinessHoursType { get; set; }

        [Required(ErrorMessage = "Enter Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Select Week")]
        public string SelectWeek { get; set; }
        public IList<SelectListItem> Listofdd { get; set; }


        [Required(ErrorMessage = "Status Required")]
        public bool Status { get; set; }

    }
}