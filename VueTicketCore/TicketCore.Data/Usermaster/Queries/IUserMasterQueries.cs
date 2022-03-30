using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketCore.Models.AgentCategoryAssigned;
using TicketCore.Models.Usermaster;
using TicketCore.ViewModels.Usermaster;

namespace TicketCore.Data.Usermaster.Queries
{
    public interface IUserMasterQueries
    {
        UserMaster GetUserById(long? userId);
        bool CheckUsernameExists(string username);
        UserMaster GetUserByUsername(string username);
        bool CheckIsCategogryAssignedtoHod(long userId);
        bool CheckIsCategogryAssignedtoAgentAdmin(long userId);
        bool CheckEmailIdExists(string emailid);
        bool CheckMobileNoExists(string mobileno);
        List<SelectListItem> GetListofAgents(int? departmentId);
        List<SelectListItem> GetListofAgentsAdmin();
        List<SelectListItem> GetListofHod();
   
        CommonUserDetailsViewModel GetCommonUserDetailsbyUserName(string username);
        bool CheckIsAlreadyVerifiedRegistration(long userid);
        EditUserViewModel EditUserbyUserId(long? userId);
        bool CheckUserIdExists(long userId);
        IQueryable<UserMasterGrid> ShowAllUsers(string sortColumn, string sortColumnDir, string search);
        IQueryable<AgentUserGrid> ShowAllAgents(string sortColumn, string sortColumnDir, string search);
        EditAgentViewModel EditAgentbyUserId(long? userId);
        bool CheckCanCategorybeChanged(long? userId);
        AgentDepartmentAssigned GetAgentAssignedCategoryId(long userId);
        AssignedRoles GetAssignedRolesByUserId(long? userId);
        List<UserResponseViewModel> GetAutoCompleteUserName(string username, int roleId);

        List<UserResponseViewModel> GetAutoCompleteAgentsUserName(string username, int roleId, long userid,
            int? departmentId);

        List<UserResponseViewModel> GetAutoCompleteAgentandAdminUserName(string username, int roleId, long userid,
            int? departmentId);

        List<SelectListItem> GetListofAgentandAgentsAdmin(int? departmentId);
        UserMaster GetUserdetailsbyEmailId(string emailid);
        UserProfileViewModel GetCommonUserDetailsbyuserId(long userId);
        List<TeamMembers> GetTeam(int? departmentId);
    }
}