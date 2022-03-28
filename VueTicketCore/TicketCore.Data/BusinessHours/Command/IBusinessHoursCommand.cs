using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketCore.Models.Business;


namespace TicketCore.Data.BusinessHours.Command
{
    public interface IBusinessHoursCommand
    {
        int? AddBusinessHours(BusinessHoursModel businessHours, List<BusinessHoursDetails> listBusinessHoursDetails);
        int? AddBusinessHours(BusinessHoursModel businessHours);
        int DeleteBusinessHours(int? businessHoursId);
    }
}