using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using TicketCore.Models.Usermaster;
using TicketCore.ViewModels.Usermaster;
using System.Linq.Dynamic.Core;
using TicketCore.Models.AgentCategoryAssigned;

namespace TicketCore.Data.Usermaster.Queries
{
    public class UserMasterQueries : IUserMasterQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        private readonly IConfiguration _configuration;
        public UserMasterQueries(VueTicketDbContext vueTicketDbContext, IConfiguration configuration)
        {
            _vueTicketDbContext = vueTicketDbContext;
            _configuration = configuration;
        }

        public UserMaster GetUserById(long? userId)
        {
            try
            {
                return _vueTicketDbContext.UserMasters.Find(userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckUsernameExists(string username)
        {
            try
            {
                var result = (from menu in _vueTicketDbContext.UserMasters
                              where menu.UserName == username
                              select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserMaster GetUserByUsername(string username)
        {
            try
            {
                var result = (from usermaster in _vueTicketDbContext.UserMasters
                              where usermaster.UserName == username
                              select usermaster).FirstOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckIsCategogryAssignedtoHod(long userId)
        {
            try
            {
                var result = (from departmentconfig in _vueTicketDbContext.DepartmentConfigration
                              where departmentconfig.HodUserId == userId && departmentconfig.Status == true
                              select departmentconfig).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckIsCategogryAssignedtoAgentAdmin(long userId)
        {
            try
            {
                var result = (from departmentconfig in _vueTicketDbContext.DepartmentConfigration
                              where departmentconfig.AgentAdminUserId == userId && departmentconfig.Status == true
                              select departmentconfig).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckEmailIdExists(string emailid)
        {
            try
            {
                var result = (from userMaster in _vueTicketDbContext.UserMasters
                              where userMaster.EmailId == emailid
                              select userMaster).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckMobileNoExists(string mobileno)
        {
            try
            {
                var result = (from userMaster in _vueTicketDbContext.UserMasters
                              where userMaster.MobileNo == mobileno
                              select userMaster).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetListofAgents(int? departmentId)
        {
            try
            {

                //RoleId RoleName
                //1   SuperAdmin
                //2   User
                //3   Admin
                //4   Agent

                var adminlist = (from agentDepartment in _vueTicketDbContext.AgentDepartmentAssigned
                                 join usermaster in _vueTicketDbContext.UserMasters on agentDepartment.UserId equals usermaster.UserId
                                 where agentDepartment.DepartmentId == departmentId
                                 select new SelectListItem()
                                 {
                                     Text = usermaster.FirstName + " " + usermaster.LastName,
                                     Value = usermaster.UserId.ToString()
                                 }).ToList();

                adminlist.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                return adminlist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetListofAgentsAdmin()
        {
            try
            {

                //RoleId RoleName
                //1   SuperAdmin
                //2   User
                //3   Admin
                //4   Agent
                //4   AgentAdmin

                var adminlist = (from usermaster in _vueTicketDbContext.UserMasters
                                 join savedroles in _vueTicketDbContext.AssignedRoles on usermaster.UserId equals savedroles.UserId
                                 where usermaster.Status == true && savedroles.RoleId == 5
                                 select new SelectListItem()
                                 {
                                     Text = usermaster.FirstName + " " + usermaster.LastName,
                                     Value = usermaster.UserId.ToString()
                                 }).ToList();

                adminlist.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                return adminlist;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public List<SelectListItem> GetListofHod()
        {
            try
            {

                //RoleId RoleName
                //1   SuperAdmin
                //2   User
                //3   Admin
                //4   Agent
                //5   AgentAdmin
                //6   HOD

                var adminlist = (from usermaster in _vueTicketDbContext.UserMasters
                                 join savedroles in _vueTicketDbContext.AssignedRoles on usermaster.UserId equals savedroles.UserId
                                 where usermaster.Status == true && savedroles.RoleId == 6
                                 select new SelectListItem()
                                 {
                                     Text = usermaster.FirstName + " " + usermaster.LastName,
                                     Value = usermaster.UserId.ToString()
                                 }).ToList();

                adminlist.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                return adminlist;
            }
            catch (Exception)
            {
                throw;
            }
        }

   

        public CommonUserDetailsViewModel GetCommonUserDetailsbyUserName(string username)
        {
            var userdata = (from tempuser in _vueTicketDbContext.UserMasters
                            join assignedRoles in _vueTicketDbContext.AssignedRoles on tempuser.UserId equals assignedRoles.UserId
                            join roleMaster in _vueTicketDbContext.RoleMasters on assignedRoles.RoleId equals roleMaster.RoleId
                            where tempuser.UserName == username
                            select new CommonUserDetailsViewModel()
                            {
                                FirstName = tempuser.FirstName,
                                EmailId = tempuser.EmailId,
                                LastName = tempuser.LastName,
                                RoleId = roleMaster.RoleId,
                                UserId = tempuser.UserId,
                                RoleName = roleMaster.RoleName,
                                Status = tempuser.Status,
                                UserName = tempuser.UserName,
                                PasswordHash = tempuser.PasswordHash,
                                MobileNo = tempuser.MobileNo,
                                IsFirstLogin = tempuser.IsFirstLogin
                            }).FirstOrDefault();

            return userdata;
        }

        public UserProfileViewModel GetCommonUserDetailsbyuserId(long userId)
        {
            var userdata = (from tempuser in _vueTicketDbContext.UserMasters
                            join assignedRoles in _vueTicketDbContext.AssignedRoles on tempuser.UserId equals assignedRoles.UserId
                            join roleMaster in _vueTicketDbContext.RoleMasters on assignedRoles.RoleId equals roleMaster.RoleId
                            where tempuser.UserId == userId
                            select new UserProfileViewModel()
                            {
                                FullName = $"{tempuser.FirstName} {tempuser.LastName}",
                                EmailId = tempuser.EmailId,
                                MobileNo = tempuser.MobileNo,
                                FirstLoginDate = tempuser.IsFirstLoginDate,
                                Gender = tempuser.Gender,
                                RoleName = roleMaster.RoleName
                            }).FirstOrDefault();

            return userdata;
        }

        public bool CheckIsAlreadyVerifiedRegistration(long userid)
        {
            var registerVerification = (from rv in _vueTicketDbContext.EmailVerification
                                        where rv.UserId == userid && rv.Verified == true
                                        select rv).Any();

            return registerVerification;
        }

        public EditUserViewModel EditUserbyUserId(long? userId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                return con.Query<EditUserViewModel>("Usp_GetUsersbyUserId", param, null, false, 0,
                    CommandType.StoredProcedure).SingleOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public EditAgentViewModel EditAgentbyUserId(long? userId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                return con.Query<EditAgentViewModel>("Usp_GetAgentbyUserId", param, null, false, 0,
                    CommandType.StoredProcedure).SingleOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckUserIdExists(long userId)
        {
            try
            {
                var result = (from usermaster in _vueTicketDbContext.UserMasters
                              where usermaster.UserId == userId
                              select usermaster).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<UserMasterGrid> ShowAllUsers(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryablesUserMasters = (from userMaster in _vueTicketDbContext.UserMasters
                                             join assignedRole in _vueTicketDbContext.AssignedRoles on userMaster.UserId equals assignedRole.UserId
                                             join roles in _vueTicketDbContext.RoleMasters on assignedRole.RoleId equals roles.RoleId

                                             select new UserMasterGrid()
                                             {
                                                 CreatedOn = userMaster.CreatedOn,
                                                 EmailId = userMaster.EmailId,
                                                 FirstName = string.IsNullOrEmpty(userMaster.FirstName) ? "-" : userMaster.FirstName,
                                                 Gender = string.IsNullOrEmpty(userMaster.Gender) ? "-" : userMaster.Gender == "M" ? "Male" : "Female",
                                                 LastName = string.IsNullOrEmpty(userMaster.LastName) ? "-" : userMaster.LastName,
                                                 MobileNo = userMaster.MobileNo,
                                                 RoleName = roles.RoleName,
                                                 UserId = userMaster.UserId,
                                                 UserName = userMaster.UserName,
                                                 Status = userMaster.Status == true ? "Active" : "InActive",
                                                 RoleId = roles.RoleId,
                                                 IsFirstLogin = userMaster.IsFirstLogin == true ? "Yes" : "No"
                                             }).AsQueryable();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryablesUserMasters = queryablesUserMasters.OrderBy(sortColumn + " " + sortColumnDir);
                }
                else
                {
                    queryablesUserMasters = queryablesUserMasters.OrderByDescending(x => x.UserId);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    queryablesUserMasters = queryablesUserMasters.Where(m => m.UserName.Contains(search) || m.UserName.Contains(search));
                }

                return queryablesUserMasters;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<AgentUserGrid> ShowAllAgents(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryablesUserMasters = (from userMaster in _vueTicketDbContext.UserMasters
                                             join assignedRole in _vueTicketDbContext.AssignedRoles on userMaster.UserId equals assignedRole.UserId
                                             join assignedcategory in _vueTicketDbContext.AgentDepartmentAssigned on userMaster.UserId equals assignedcategory.UserId
                                             join department in _vueTicketDbContext.Department on assignedcategory.DepartmentId equals department.DepartmentId
                                             join roles in _vueTicketDbContext.RoleMasters on assignedRole.RoleId equals roles.RoleId
                                             where roles.RoleId == 4
                                             select new AgentUserGrid()
                                             {
                                                 CreatedOn = userMaster.CreatedOn,
                                                 EmailId = userMaster.EmailId,
                                                 FirstName = string.IsNullOrEmpty(userMaster.FirstName) ? "-" : userMaster.FirstName,
                                                 Gender = string.IsNullOrEmpty(userMaster.Gender) ? "-" : userMaster.Gender == "M" ? "Male" : "Female",
                                                 LastName = string.IsNullOrEmpty(userMaster.LastName) ? "-" : userMaster.LastName,
                                                 MobileNo = userMaster.MobileNo,
                                                 RoleName = roles.RoleName,
                                                 UserId = userMaster.UserId,
                                                 UserName = userMaster.UserName,
                                                 Status = userMaster.Status == true ? "Active" : "InActive",
                                                 RoleId = roles.RoleId,
                                                 IsFirstLogin = userMaster.IsFirstLogin == true ? "Yes" : "No",
                                                 Department = department.DepartmentName
                                             }).AsQueryable();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryablesUserMasters = queryablesUserMasters.OrderBy(sortColumn + " " + sortColumnDir);
                }
                else
                {
                    queryablesUserMasters = queryablesUserMasters.OrderByDescending(x => x.UserId);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    queryablesUserMasters = queryablesUserMasters.Where(m => m.UserName.Contains(search) || m.UserName.Contains(search));
                }

                return queryablesUserMasters;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool CheckCanCategorybeChanged(long? userId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                return con.Query<bool>("Usp_CheckCanCategorybeChanged", param, null, false, 0,
                    CommandType.StoredProcedure).SingleOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AgentDepartmentAssigned GetAgentAssignedCategoryId(long userId)
        {
            try
            {
                var result = (from departmentconfig in _vueTicketDbContext.AgentDepartmentAssigned
                              where departmentconfig.UserId == userId
                              select departmentconfig).FirstOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AssignedRoles GetAssignedRolesByUserId(long? userId)
        {
            try
            {

                var assignedRolesdetails = (from assignedRole in _vueTicketDbContext.AssignedRoles
                                            where assignedRole.UserId == userId
                                            select assignedRole).FirstOrDefault();

                return assignedRolesdetails;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<UserResponseViewModel> GetAutoCompleteUserName(string username, int roleId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@Username", username);
                param.Add("@RoleId", roleId);
                return con.Query<UserResponseViewModel>("Usp_GetActiveUsers", param, null, false, 0,
                    CommandType.StoredProcedure).ToList();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<UserResponseViewModel> GetAutoCompleteAgentsUserName(string username, int roleId, long userid, int? departmentId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@Username", username);
                param.Add("@RoleId", roleId);
                param.Add("@UserId", userid);
                param.Add("@DepartmentId", departmentId);
                return con.Query<UserResponseViewModel>("Usp_GetActiveAgents", param, null, false, 0,
                    CommandType.StoredProcedure).ToList();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<UserResponseViewModel> GetAutoCompleteAgentandAdminUserName(string username, int roleId, long userid, int? departmentId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@Username", username);
                param.Add("@RoleId", roleId);
                param.Add("@UserId", userid);
                param.Add("@DepartmentId", departmentId);
                return con.Query<UserResponseViewModel>("Usp_GetActiveAgentsandAdmin", param, null, false, 0,
                    CommandType.StoredProcedure).ToList();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SelectListItem> GetListofAgentandAgentsAdmin(int? departmentId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                var param = new DynamicParameters();
                param.Add("@DepartmentId", departmentId);
                var adminlist = con.Query<SelectListItem>("Usp_GetAllActiveAgentsandAdmin", param, null, false, 0,
                     CommandType.StoredProcedure).ToList();


                adminlist.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                return adminlist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserMaster GetUserdetailsbyEmailId(string emailid)
        {
            try
            {
                var result = (from user in _vueTicketDbContext.UserMasters
                              where user.EmailId == emailid
                              select user).FirstOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TeamMembers> GetTeam(int? departmentId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            var para = new DynamicParameters();
            para.Add("@DepartmentId", departmentId);
            var listofteam = con.Query<TeamMembers>("Usp_TeamDetailsbyDepartmentId", para, commandType: CommandType.StoredProcedure).ToList();
            return listofteam;
        }
    }
}