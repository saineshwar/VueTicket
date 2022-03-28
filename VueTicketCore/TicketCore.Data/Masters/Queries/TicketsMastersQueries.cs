using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketCore.Data.Masters.Queries
{
    public class TicketsMastersQueries : ITicketsMastersQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public TicketsMastersQueries(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }

        public List<SelectListItem> GetAllActiveOverdueTypes()
        {
            try
            {
                var overdueTypeList = (from cat in _vueTicketDbContext.OverdueTypes
                    select new SelectListItem()
                    {
                        Text = cat.OverdueType,
                        Value = cat.OverdueTypeId.ToString()
                    }).ToList();

                overdueTypeList.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                return overdueTypeList;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}