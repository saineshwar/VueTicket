using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketCore.Data.Knowledgebase.Queries
{
    public class KnowledgebaseTypeQueries : IKnowledgebaseTypeQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public KnowledgebaseTypeQueries(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }

        public List<SelectListItem> KnowledgebaseTypeList()
        {
            var knowledgebaseTypeList = (from kbt in _vueTicketDbContext.KnowledgebaseType
                                         select new SelectListItem()
                                         {
                                             Text = kbt.KnowledgebaseTypeName,
                                             Value = kbt.KnowledgebaseTypeId.ToString()
                                         }).ToList();

            knowledgebaseTypeList.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-----Select-----"
            });

            return knowledgebaseTypeList;
        }
    }
}