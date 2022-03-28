using TicketCore.Models.Usermaster;
using TicketCore.ViewModels.Usermaster;

namespace TicketCore.Data.Usermaster.Command
{
    public interface IUserMasterCommand
    {
        long? AddUser(UserMaster usermaster,   int? roleId);
        long? UpdateUser(UserMaster usermaster, AssignedRoles assignedRoles);
        bool UpdatePasswordandHistory(long? userId, string passwordHash, string processType);
        void DeleteUser(int? userId);
        long? AddAgent(UserMaster usermaster,  int categoryId, int roleId);
        int UpdateAgentDetailswithCategory(EditAgentViewModel editAgentViewModel);
        bool UpdateIsFirstLoginStatus(long? userId);
        long? UpdateUserDetails(UserMaster usermaster);
    }
}