using System.Collections.Generic;
using TicketCore.Models.Business;

namespace TicketCore.ViewModels.Business
{
    public class BusinessHoursDisplayViewModel
    {
        public BusinessHoursModel BusinessHours { get; set; }
        public List<BusinessHoursDetails> ListofBusinessHoursDetails { get; set; }
    }
}