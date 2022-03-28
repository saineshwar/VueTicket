using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketCore.Models.Business;
using TicketCore.ViewModels.Business;


namespace TicketCore.Data.BusinessHours.Queries
{
    public interface IBusinessHoursQueries
    {
        List<SelectListItem> ListofBusinessHoursType();
        BusinessHoursModel GetBusinessHours(int? businessHoursId);
        BusinessHoursDetails GetBusinessHoursDetails(int? businessHoursId);
        IQueryable<BusinessHoursViewModel> GetBusinessList(string sortColumn, string sortColumnDir, string search);
        List<BusinessHoursDetails> DetailsBusinessHours(int? businessHoursId);
        List<SelectListItem> ListofBusinessHours();
        int BusinessHoursCount();
    }
}