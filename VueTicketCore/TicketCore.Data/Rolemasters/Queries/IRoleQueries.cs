using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketCore.Models.Rolemasters;
using TicketCore.ViewModels.Rolemasters;

namespace TicketCore.Data.Rolemasters.Queries
{
    public interface IRoleQueries
    {
        bool CheckRoleNameExists(string roleName);
        IQueryable<RoleMasterGrid> ShowAllRoleMaster(string sortColumn, string sortColumnDir, string search);
        RoleMaster GetRoleMasterByroleId(int? roleId);
        EditRoleMasterViewModel GetRoleMasterForEditByroleId(int? roleId);
        List<SelectListItem> ListofRoles();
        List<SelectListItem> GetAllActiveRolesNotAgent();
    }
}