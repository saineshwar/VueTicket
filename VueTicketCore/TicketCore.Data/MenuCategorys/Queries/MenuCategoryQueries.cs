using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TicketCore.Common;
using TicketCore.Models.MenuCategorys;
using TicketCore.ViewModels.MenuCategorys;
using TicketCore.ViewModels.Ordering;

namespace TicketCore.Data.MenuCategorys.Queries
{
    public class MenuCategoryQueries : IMenuCategoryQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        private readonly IMemoryCache _cache;
        public MenuCategoryQueries(VueTicketDbContext vueTicketDbContext, IMemoryCache cache)
        {
            _vueTicketDbContext = vueTicketDbContext;
            _cache = cache;
        }

        public EditMenuCategoriesViewModel GetCategoryByMenuCategoryIdForEdit(int? menuCategoryId)
        {
            try
            {
                var result = (from category in _vueTicketDbContext.MenuCategorys.AsNoTracking()
                              where category.MenuCategoryId == menuCategoryId
                              select new EditMenuCategoriesViewModel()
                              {
                                  RoleId = category.RoleId,
                                  Status = category.Status,
                                  MenuCategoryId = category.MenuCategoryId,
                                  MenuCategoryName = category.MenuCategoryName

                              }).SingleOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MenuCategory GetCategoryByMenuCategoryId(int? menuCategoryId)
        {
            var result = (from category in _vueTicketDbContext.MenuCategorys.AsNoTracking()
                          where category.MenuCategoryId == menuCategoryId
                          select category).SingleOrDefault();

            return result;
        }

        public List<SelectListItem> GetCategorybyRoleId(int? roleId)
        {
            var categoryList = (from cat in _vueTicketDbContext.MenuCategorys
                                where cat.Status == true && cat.RoleId == roleId
                                select new SelectListItem()
                                {
                                    Text = cat.MenuCategoryName,
                                    Value = cat.MenuCategoryId.ToString()
                                }).ToList();

            categoryList.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-----Select-----"
            });

            return categoryList;
        }


        public IQueryable<MenuCategoryGridViewModel> ShowAllMenusCategory(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryableMenuMaster = (from menuCategory in _vueTicketDbContext.MenuCategorys
                                           join roleMaster in _vueTicketDbContext.RoleMasters on menuCategory.RoleId equals roleMaster.RoleId
                                           select new MenuCategoryGridViewModel()
                                           {
                                               Status = menuCategory.Status == true ? "Active" : "InActive",
                                               MenuCategoryId = menuCategory.MenuCategoryId,
                                               MenuCategoryName = menuCategory.MenuCategoryName,
                                               RoleName = roleMaster.RoleName
                                           }
                    );

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryableMenuMaster = queryableMenuMaster.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    queryableMenuMaster = queryableMenuMaster.Where(m => m.MenuCategoryName.Contains(search) || m.MenuCategoryName.Contains(search));
                }

                return queryableMenuMaster;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckCategoryNameExists(string menuCategoryName, int roleId)
        {
            try
            {
                var result = (from category in _vueTicketDbContext.MenuCategorys.AsNoTracking()
                              where category.MenuCategoryName == menuCategoryName && category.RoleId == roleId
                              select category).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<MenuCategory> GetCategoryByRoleId(int? roleId)
        {

            var key = $"{AllMemoryCacheKeys.MenuCategoryKey}_{roleId}";
            List<MenuCategory> menuCategory;
            if (_cache.Get(key) == null)
            {
                var result = (from category in _vueTicketDbContext.MenuCategorys.AsNoTracking()
                              orderby category.SortingOrder ascending
                              where category.RoleId == roleId && category.Status == true
                              select category).ToList();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(7),
                    Priority = CacheItemPriority.Normal
                };

                menuCategory = _cache.Set<List<MenuCategory>>(key, result, cacheExpirationOptions);
            }
            else
            {
                menuCategory = _cache.Get(key) as List<MenuCategory>;
            }

            return menuCategory;
        }

        public List<MenuCategoryOrderingVm> ListofMenubyRoleCategoryId(int roleId)
        {
            var listofmenucategories = (from tempmenu in _vueTicketDbContext.MenuCategorys
                                        where tempmenu.Status == true && tempmenu.RoleId == roleId
                                        orderby tempmenu.SortingOrder ascending
                                        select new MenuCategoryOrderingVm
                                        {
                                            MenuCategoryId = tempmenu.MenuCategoryId,
                                            MenuCategoryName = tempmenu.MenuCategoryName
                                        }).ToList();

            return listofmenucategories;
        }
    }
}