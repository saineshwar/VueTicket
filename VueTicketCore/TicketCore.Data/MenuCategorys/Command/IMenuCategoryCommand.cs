using TicketCore.Models.MenuCategorys;

namespace TicketCore.Data.MenuCategorys.Command
{
    public interface IMenuCategoryCommand
    {
        int Add(MenuCategory category);
        int Update(MenuCategory category);
        int Delete(MenuCategory category);
    }
}