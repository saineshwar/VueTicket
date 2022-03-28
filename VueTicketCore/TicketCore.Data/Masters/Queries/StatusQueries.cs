using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using TicketCore.Common;

namespace TicketCore.Data.Masters.Queries
{
    public class StatusQueries : IStatusQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        private readonly IMemoryCache _cache;
        public StatusQueries(VueTicketDbContext vueTicketDbContext, IMemoryCache cache)
        {
            _vueTicketDbContext = vueTicketDbContext;
            _cache = cache;
        }

        public List<SelectListItem> GetAllStatusSelectListItem()
        {
            try
            {
                List<SelectListItem> statusList;
                var key = $"{AllMemoryCacheKeys.StatusWithkey}";
                if (_cache.Get(key) == null)
                {
                    statusList = (from status in _vueTicketDbContext.Status

                                  select new SelectListItem()
                                  {
                                      Text = status.StatusText,
                                      Value = status.StatusId.ToString()
                                  }).ToList();


                    statusList.Insert(0, new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    });


                    MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddDays(90),
                        Priority = CacheItemPriority.Normal
                    };

                    _cache.Set<List<SelectListItem>>(key, statusList, cacheExpirationOptions);
                }
                else
                {
                    statusList = _cache.Get(key) as List<SelectListItem>;
                }

                return statusList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SelectListItem> GetAllStatusWithoutInternalStatus()
        {
            try
            {
                List<SelectListItem> statusList;
                var key = $"{AllMemoryCacheKeys.StatusWithoutkey}";
                if (_cache.Get(key) == null)
                {
                    statusList = (from status in _vueTicketDbContext.Status
                                  where status.IsInternalStatus == false
                                  select new SelectListItem()
                                  {
                                      Text = status.StatusText,
                                      Value = status.StatusId.ToString()
                                  }).ToList();


                    statusList.Insert(0, new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    });


                    MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddDays(90),
                        Priority = CacheItemPriority.Normal
                    };

                    _cache.Set<List<SelectListItem>>(key, statusList, cacheExpirationOptions);
                }
                else
                {
                    statusList = _cache.Get(key) as List<SelectListItem>;
                }

                return statusList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SelectListItem> GetAllAgentStatusSelectListItem()
        {
            try
            {
                List<SelectListItem> statusList;
                var key = $"{AllMemoryCacheKeys.StatusAgentWithkey}";
                if (_cache.Get(key) == null)
                {
                    statusList = (from status in _vueTicketDbContext.Status
                                  where status.ShowAgent == true
                                  select new SelectListItem()
                                  {
                                      Text = status.StatusText,
                                      Value = status.StatusId.ToString()
                                  }).ToList();


                    statusList.Insert(0, new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    });


                    MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddDays(90),
                        Priority = CacheItemPriority.Normal
                    };

                    _cache.Set<List<SelectListItem>>(key, statusList, cacheExpirationOptions);
                }
                else
                {
                    statusList = _cache.Get(key) as List<SelectListItem>;
                }

                return statusList;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}