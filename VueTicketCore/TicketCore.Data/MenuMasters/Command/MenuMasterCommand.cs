using Microsoft.EntityFrameworkCore;
using TicketCore.Models.Menus;

namespace TicketCore.Data.MenuMasters.Command
{
    public class MenuMasterCommand : IMenuMasterCommand
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public MenuMasterCommand(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }

        public int Add(MenuMaster category)
        {
            _vueTicketDbContext.MenuMasters.Add(category);
            return _vueTicketDbContext.SaveChanges();
        }

        public int Delete(MenuMaster category)
        {
            _vueTicketDbContext.Entry(category).State = EntityState.Deleted;
            return _vueTicketDbContext.SaveChanges();
        }

        public int Update(MenuMaster category)
        {
            _vueTicketDbContext.Entry(category).State = EntityState.Modified;
            return _vueTicketDbContext.SaveChanges();
        }
    }
}