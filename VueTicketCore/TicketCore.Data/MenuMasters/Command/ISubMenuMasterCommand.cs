using TicketCore.Models.Menus;

namespace TicketCore.Data.MenuMasters.Command
{
    public interface ISubMenuMasterCommand
    {
        int Add(SubMenuMaster subMenuMaster);
        int Delete(SubMenuMaster subMenuMaster);
        int Update(SubMenuMaster subMenuMaster);
    }
}