using TicketCore.Models.Masters;

namespace TicketCore.Data.Masters.Command
{
    public interface IHolidayCommand
    {
        int Add(HolidayListModel holiday);
        int Delete(int? holidayId);
    }
}