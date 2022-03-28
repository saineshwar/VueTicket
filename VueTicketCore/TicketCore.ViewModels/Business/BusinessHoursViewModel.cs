using System;

namespace TicketCore.ViewModels.Business
{
    public class BusinessHoursViewModel
    {
        public int BusinessHoursId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BusinessHoursName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Status { get; set; }
    }
}