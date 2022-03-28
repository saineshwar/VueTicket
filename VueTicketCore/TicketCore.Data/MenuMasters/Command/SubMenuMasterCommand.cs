using Microsoft.EntityFrameworkCore;
using TicketCore.Models.Menus;

namespace TicketCore.Data.MenuMasters.Command
{
    public class SubMenuMasterCommand : ISubMenuMasterCommand
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public SubMenuMasterCommand(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }
        public int Add(SubMenuMaster subMenuMaster)
        {
            _vueTicketDbContext.SubMenuMasters.Add(subMenuMaster);
            return _vueTicketDbContext.SaveChanges();
        }

        public int Delete(SubMenuMaster subMenuMaster)
        {
            _vueTicketDbContext.Entry(subMenuMaster).State = EntityState.Deleted;
            return _vueTicketDbContext.SaveChanges();
        }

        public int Update(SubMenuMaster subMenuMaster)
        {
            _vueTicketDbContext.Entry(subMenuMaster).State = EntityState.Modified;
            return _vueTicketDbContext.SaveChanges();
        }
    }
}