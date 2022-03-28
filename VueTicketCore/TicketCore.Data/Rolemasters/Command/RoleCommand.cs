using Microsoft.EntityFrameworkCore;
using TicketCore.Models.Rolemasters;

namespace TicketCore.Data.Rolemasters.Command
{
    public class RoleCommand : IRoleCommand
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public RoleCommand(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }

        public int Delete(RoleMaster roleMaster)
        {
            _vueTicketDbContext.Entry(roleMaster).State = EntityState.Deleted;
            return _vueTicketDbContext.SaveChanges();
        }

        public int Add(RoleMaster roleMaster)
        {
            _vueTicketDbContext.RoleMasters.Add(roleMaster);
            return _vueTicketDbContext.SaveChanges();
        }

        public int Update(RoleMaster roleMaster)
        {
            _vueTicketDbContext.Entry(roleMaster).State = EntityState.Modified;
            return _vueTicketDbContext.SaveChanges();
        }
    }
}