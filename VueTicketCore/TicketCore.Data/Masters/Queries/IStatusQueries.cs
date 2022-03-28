using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketCore.Data.Masters.Queries
{
    public interface IStatusQueries
    {
        List<SelectListItem> GetAllStatusSelectListItem();
        List<SelectListItem> GetAllStatusWithoutInternalStatus();
        List<SelectListItem> GetAllAgentStatusSelectListItem();
    }
}