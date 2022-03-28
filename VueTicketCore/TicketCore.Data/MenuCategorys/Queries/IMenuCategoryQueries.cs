using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketCore.Models.MenuCategorys;
using TicketCore.ViewModels.MenuCategorys;
using TicketCore.ViewModels.Ordering;

namespace TicketCore.Data.MenuCategorys.Queries
{
    public interface IMenuCategoryQueries
    {
        MenuCategory GetCategoryByMenuCategoryId(int? menuCategoryId);
        EditMenuCategoriesViewModel GetCategoryByMenuCategoryIdForEdit(int? menuCategoryId);
        List<SelectListItem> GetCategorybyRoleId(int? roleId);
  
        IQueryable<MenuCategoryGridViewModel> ShowAllMenusCategory(string sortColumn, string sortColumnDir,
            string search);
        bool CheckCategoryNameExists(string menuCategoryName, int roleId);
        List<MenuCategory> GetCategoryByRoleId(int? roleId);
        List<MenuCategoryOrderingVm> ListofMenubyRoleCategoryId(int roleId);
    }
}