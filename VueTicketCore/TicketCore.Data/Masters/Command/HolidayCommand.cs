using System;
using TicketCore.Models.Masters;

namespace TicketCore.Data.Masters.Command
{
    public class HolidayCommand : IHolidayCommand
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public HolidayCommand(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }

        public int Add(HolidayListModel holiday)
        {
            try
            {

                holiday.CreatedDate = DateTime.Now;
                _vueTicketDbContext.Holidays.Add(holiday);
                return _vueTicketDbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Delete(int? holidayId)
        {
            try
            {
                var holiday = _vueTicketDbContext.Holidays.Find(holidayId);
                if (holiday != null)
                    _vueTicketDbContext.Holidays.Remove(holiday);
                return _vueTicketDbContext.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}