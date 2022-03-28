using System;
using System.Linq;
using TicketCore.ViewModels.Menus;
using System.Linq.Dynamic.Core;
using TicketCore.Models.Masters;

namespace TicketCore.Data.Masters.Queries
{
    public class HolidayQueries : IHolidayQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public HolidayQueries(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }

        public IQueryable<HolidayListModel> ShowAllHolidays(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryableMenuMaster = (from holiday in _vueTicketDbContext.Holidays
                                           orderby holiday.HolidayDate descending
                                           select new HolidayListModel()
                                           {
                                               HolidayDate = holiday.HolidayDate,
                                               HolidayName = holiday.HolidayName,
                                               CreatedDate = holiday.CreatedDate,
                                               HolidayId = holiday.HolidayId
                                           });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryableMenuMaster = queryableMenuMaster.OrderBy(sortColumn + " " + sortColumnDir);
                }


                if (!string.IsNullOrEmpty(search))
                {
                    queryableMenuMaster = queryableMenuMaster.Where(m => m.HolidayName.Contains(search));
                }

                return queryableMenuMaster;

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}