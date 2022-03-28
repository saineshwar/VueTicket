using System.Linq;
using TicketCore.ViewModels.Masters;

namespace TicketCore.Data.Masters.Queries
{
    public interface ISlaPoliciesQueries
    {
        bool CheckPoliciesExists(int? priorityId);
        IQueryable<SlaPoliciesShowViewModel> ShowAllSLA(string sortColumn, string sortColumnDir, string search);
         bool CheckSlaPoliciesReminderExists(int BusinessHoursId);
         IQueryable<SlaPoliciesReminderShowViewModel> ShowAllSLAReminder(string sortColumn, string sortColumnDir,
             string search);
    }
}