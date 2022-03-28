using TicketCore.Models.Menus;

namespace TicketCore.Data.MenuMasters.Command
{
    public interface IMenuMasterCommand
    {
        int Add(MenuMaster menuMaster);
        int Delete(MenuMaster menuMaster);
        int Update(MenuMaster menuMaster);
    }
}