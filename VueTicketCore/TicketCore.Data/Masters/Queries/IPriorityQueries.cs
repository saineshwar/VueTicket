using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketCore.Models.Masters;

namespace TicketCore.Data.Masters.Queries
{
    public interface IPriorityQueries
    {
        List<SelectListItem> GetAllPrioritySelectListItem();
        string GetPriorityNameBypriorityId(int? priorityId);
    }
}