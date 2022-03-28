using System.Linq;
using TicketCore.Models.CategoryConfigrations;
using TicketCore.ViewModels.CategoryConfigrations;

namespace TicketCore.Data.Department.Queries
{
    public interface IDepartmentConfigrationQueries
    {
        bool CheckDuplicateDepartmentConfigration(long adminuserId, long hoduserId, int categoryId);

        IQueryable<ShowDepartmentConfigration> GetDepartmentConfigrationList(string sortColumn, string sortColumnDir,
            string search);
        DepartmentConfigration GetDepartmentConfigration(int categoryConfigrationId);
    }
}