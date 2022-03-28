
using TicketCore.Models.Department;

namespace TicketCore.Data.Department.Command
{
    public interface IDepartmentCommand
    {
        int? Add(Departments deparment);
        int? Update(Departments deparment);
        int? Delete(int? departmentId);
    }
}