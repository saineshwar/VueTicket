using System.Collections.Generic;
using TicketCore.ViewModels.Ordering;

namespace TicketCore.Data.MenuMasters.Command
{
    public interface IOrderingCommand
    {
        void UpdateMenuCategoryOrder(List<MenuCategoryStoringOrder> menuCategorylist);
        void UpdateMenuOrder(List<MenuStoringOrder> menuStoringOrder);
        void UpdateSubMenuOrder(List<SubMenuStoringOrder> submenuStoringOrder);
    }
}