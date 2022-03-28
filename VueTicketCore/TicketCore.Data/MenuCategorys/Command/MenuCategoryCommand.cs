using Microsoft.EntityFrameworkCore;
using TicketCore.Models.MenuCategorys;

namespace TicketCore.Data.MenuCategorys.Command
{
    public class MenuCategoryCommand : IMenuCategoryCommand
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public MenuCategoryCommand(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }
        public int Add(MenuCategory category)
        {
            _vueTicketDbContext.MenuCategorys.Add(category);
            return _vueTicketDbContext.SaveChanges();
        }

        public int Delete(MenuCategory category)
        {
            _vueTicketDbContext.Entry(category).State = EntityState.Deleted;
            return _vueTicketDbContext.SaveChanges();
        }

        public int Update(MenuCategory category)
        {
            _vueTicketDbContext.Entry(category).State = EntityState.Modified;
            return _vueTicketDbContext.SaveChanges();
        }
    }
}