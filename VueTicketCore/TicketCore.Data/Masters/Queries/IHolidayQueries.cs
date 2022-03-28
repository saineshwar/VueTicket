using System.Linq;
using TicketCore.Models.Masters;

namespace TicketCore.Data.Masters.Queries
{
    public interface IHolidayQueries
    {
        IQueryable<HolidayListModel> ShowAllHolidays(string sortColumn, string sortColumnDir, string search);
    }
}