using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketCore.Data.Masters.Queries
{
    public interface ITicketsMastersQueries
    {
        List<SelectListItem> GetAllActiveOverdueTypes();
    }
}