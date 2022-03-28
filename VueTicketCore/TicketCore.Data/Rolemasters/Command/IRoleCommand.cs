using TicketCore.Models.Rolemasters;

namespace TicketCore.Data.Rolemasters.Command
{
    public interface IRoleCommand
    {
        int Delete(RoleMaster roleMaster);
        int Add(RoleMaster roleMaster);
        int Update(RoleMaster roleMaster);
    }
}