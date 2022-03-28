using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using TicketCore.Common;
using TicketCore.Models.Masters;
using TicketCore.Models.Menus;

namespace TicketCore.Data.Masters.Queries
{
    public class PriorityQueries : IPriorityQueries
    {

        private readonly VueTicketDbContext _vueTicketDbContext;
        private readonly IMemoryCache _cache;
        public PriorityQueries(VueTicketDbContext vueTicketDbContext, IMemoryCache cache)
        {
            _vueTicketDbContext = vueTicketDbContext;
            _cache = cache;
        }

        

        public List<SelectListItem> GetAllPrioritySelectListItem()
        {
            
            try
            {
                List<SelectListItem> priorityList;
                var key = $"{AllMemoryCacheKeys.PriorityKey}";
                if (_cache.Get(key) == null)
                {
                    priorityList = (from priority in _vueTicketDbContext.Priority
                        select new SelectListItem()
                        {
                            Text = priority.PriorityName,
                            Value = priority.PriorityId.ToString()
                        }).ToList();


                    priorityList.Insert(0, new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    });

                    MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddDays(7),
                        Priority = CacheItemPriority.Normal
                    };

                    _cache.Set<List<SelectListItem>>(key, priorityList, cacheExpirationOptions);

                }
                else
                {
                    priorityList = _cache.Get(key) as List<SelectListItem>;
                }

                return priorityList;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public string GetPriorityNameBypriorityId(int? priorityId)
        {
            var priorityList = (from priority in _vueTicketDbContext.Priority
                where priority.PriorityId == priorityId
                select priority.PriorityName).FirstOrDefault();
            return priorityList;
        }

    }
}