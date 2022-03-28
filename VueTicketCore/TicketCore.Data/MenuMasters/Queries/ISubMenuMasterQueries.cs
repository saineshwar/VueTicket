using System.Collections.Generic;
using System.Linq;
using TicketCore.Models.Menus;
using TicketCore.ViewModels.Menus;
using TicketCore.ViewModels.Ordering;

namespace TicketCore.Data.MenuMasters.Queries
{
    public interface ISubMenuMasterQueries
    {
        bool CheckSubMenuNameExists(string subMenuName, int menuId);
        EditSubMenuMasterViewModel GetSubMenuById(int? subMenuId);
        bool CheckSubMenuNameExists(string subMenuName, int? menuId, int? roleId, int? categoryId);
        IQueryable<SubMenuMasterViewModel> ShowAllSubMenus(string sortColumn, string sortColumnDir, string search);
        bool EditValidationCheck(int? subMenuId, EditSubMenuMasterViewModel editsubMenu);
        SubMenuMaster GetSubMenuBySubMenuId(int? subMenuId);
        List<SubMenuMaster> GetSubMenuByRoleId(int? roleId, int? menuCategoryId, int menuid);
        List<SubMenuMasterOrderingVm> ListofSubMenubyRoleId(int roleId, int menuid);
    }
}