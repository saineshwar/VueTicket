using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketCore.Data.Knowledgebase.Queries
{
    public interface IKnowledgebaseTypeQueries
    {
        List<SelectListItem> KnowledgebaseTypeList();
    }
}