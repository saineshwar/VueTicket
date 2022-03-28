using TicketCore.Models.CategoryConfigrations;

namespace TicketCore.Data.Department.Command
{
    public interface IDepartmentConfigrationCommand
    {
        int? Add(DepartmentConfigration department);
        int? Update(DepartmentConfigration department);
    }
}