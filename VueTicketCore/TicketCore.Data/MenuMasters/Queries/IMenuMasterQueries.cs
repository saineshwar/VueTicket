using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketCore.Models.Menus;
using TicketCore.ViewModels.Menus;
using TicketCore.ViewModels.Ordering;

namespace TicketCore.Data.MenuMasters.Queries
{
    public interface IMenuMasterQueries
    {
        bool CheckMenuNameExists(string menuName);
        bool CheckMenuExists(string menuName, int? roleId, int? categoryId);
        IQueryable<MenuMasterGrid> ShowAllMenus(string sortColumn, string sortColumnDir, string search);
        EditMenuMasterViewModel GetMenuByMenuId(int? menuId);
        MenuMaster GetMenuMasterByMenuId(int? menuId);
        bool EditValidationCheck(int? menuId, EditMenuMasterViewModel editMenu);
        List<SelectListItem> ListofMenusbyRoleId(RequestMenus requestMenus);
        List<MenuMaster> GetMenuByRoleId(int? roleId, int? menuCategoryId);
        List<MenuMasterOrderingVm> GetListofMenu(int roleId, int menuCategoryId);
        List<SelectListItem> ListofMenubyRoleIdSelectListItem(int roleId, int menuCategoryId);
    }
}