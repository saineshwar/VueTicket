using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketCore.Models.Department;
using TicketCore.ViewModels.Categorys;

namespace TicketCore.Data.Department.Queries
{
    public interface IDepartmentQueries
    {
        List<SelectListItem> GetAllActiveSelectListItemDepartment();
        List<SelectListItem> GetAllActiveDepartmentWithoutSelect();

        Departments GetDepartmentById(int? categoryId);
        int GetDepartmentIdsByUserId(long? userId);
        int? GetAgentAdminDepartmentIdsByUserId(long? userId);
        bool CheckDepartmentNameExists(string departmentname);
        IQueryable<DepartmentGridViewModel> ShowAllDepartment(string sortColumn, string sortColumnDir, string search);
        List<SelectListItem> GetAssignedDepartmentsByUserId(long? userId);
        List<SelectListItem> GetAssignedDepartmentsofAgentManager(long? userId);
        List<SelectListItem> GetAssignedDepartmentsofAdministrator(long? userId);
    }
}