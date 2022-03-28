using System;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using TicketCore.Models.AgentCategoryAssigned;
using TicketCore.Models.Usermaster;
using TicketCore.ViewModels.Usermaster;

namespace TicketCore.Data.Usermaster.Command
{
    public class UserMasterCommand : IUserMasterCommand
    {
        private readonly IConfiguration _configuration;
        private readonly VueTicketDbContext _vueTicketDbContext;
        public UserMasterCommand(VueTicketDbContext vueTicketDbContext, IConfiguration configuration)
        {
            _vueTicketDbContext = vueTicketDbContext;
            _configuration = configuration;
        }

        public long? AddUser(UserMaster usermaster, int? roleId)
        {
            try
            {

                try
                {
                    long userId = -1;

                    if (usermaster != null)
                    {
                        usermaster.Status = true;
                        usermaster.CreatedOn = DateTime.Now;
                        usermaster.IsFirstLogin = true;

                        _vueTicketDbContext.UserMasters.Add(usermaster);
                        _vueTicketDbContext.SaveChanges();
                        userId = usermaster.UserId;

                        if (roleId != null)
                        {
                            var savedAssignedRoles = new AssignedRoles()
                            {
                                RoleId = roleId.Value,
                                UserId = userId,
                                AssignedRoleId = 0,
                                Status = true,
                                CreatedOn = DateTime.Now
                            };
                            _vueTicketDbContext.AssignedRoles.Add(savedAssignedRoles);
                        }

                        _vueTicketDbContext.SaveChanges();
                        _vueTicketDbContext.SaveChanges();

                        return userId;
                    }

                    return userId;
                }
                catch (Exception)
                {

                    return 0;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long? UpdateUser(UserMaster usermaster, AssignedRoles assignedRoles)
        {
            try
            {

                long? result = -1;

                if (usermaster != null)
                {
                    usermaster.ModifiedOn = DateTime.Now;
                    _vueTicketDbContext.Entry(usermaster).State = EntityState.Modified;
                    _vueTicketDbContext.SaveChanges();

                    if (assignedRoles != null)
                    {
                        _vueTicketDbContext.Entry(assignedRoles).State = EntityState.Modified;
                        _vueTicketDbContext.SaveChanges();
                    }

                    result = usermaster.UserId;
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteUser(int? userId)
        {
            try
            {
                UserMaster usermaster = _vueTicketDbContext.UserMasters.Find(userId);
                if (usermaster != null) _vueTicketDbContext.UserMasters.Remove(usermaster);
                _vueTicketDbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdatePasswordandHistory(long? userId, string passwordHash, string processType)
        {
            using SqlConnectionManager sqlDataAccessManager = new SqlConnectionManager(_configuration);
            try
            {

                var (connection, transaction) = sqlDataAccessManager.StartTransaction();
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                param.Add("@PasswordHash", passwordHash);
                param.Add("@ProcessType", processType);
                var result = connection.Execute("Usp_PasswordMaster_UpdatePassword", param, transaction, 0, CommandType.StoredProcedure);

                if (result > 0)
                {
                    sqlDataAccessManager.Commit();
                    return true;
                }
                else
                {
                    sqlDataAccessManager.Rollback();
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public long? AddAgent(UserMaster usermaster, int categoryId, int roleId)
        {
            try
            {
                using var dbContextTransaction = _vueTicketDbContext.Database.BeginTransaction();
                try
                {
                    long userId = -1;
                    var currentdatetime = DateTime.Now;
                    if (usermaster != null)
                    {
                        usermaster.Status = true;
                        usermaster.CreatedOn = currentdatetime;
                        usermaster.IsFirstLogin = true;

                        _vueTicketDbContext.UserMasters.Add(usermaster);
                        _vueTicketDbContext.SaveChanges();
                        userId = usermaster.UserId;

                        var assignedRoles = new AssignedRoles();
                        assignedRoles.UserId = userId;
                        assignedRoles.CreatedOn = currentdatetime;
                        assignedRoles.Status = true;
                        assignedRoles.AssignedRoleId = 0;
                        assignedRoles.RoleId = roleId;

                        _vueTicketDbContext.AssignedRoles.Add(assignedRoles);
                        _vueTicketDbContext.SaveChanges();

                        var agentCategoryAssigned = new AgentDepartmentAssigned()
                        {
                            AgentDepartmentId = 0,
                            DepartmentId = categoryId,
                            UserId = userId,
                            CreatedOn = currentdatetime
                        };

                        _vueTicketDbContext.AgentDepartmentAssigned.Add(agentCategoryAssigned);
                        _vueTicketDbContext.SaveChanges();

                        dbContextTransaction.Commit();
                        return userId;
                    }

                    return userId;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public long? UpdateAgentDetails(UserMaster usermaster)
        {
            try
            {

                long? result = -1;

                if (usermaster != null)
                {
                    usermaster.ModifiedOn = DateTime.Now;
                    _vueTicketDbContext.Entry(usermaster).State = EntityState.Modified;
                    _vueTicketDbContext.SaveChanges();
                    result = usermaster.UserId;
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int UpdateAgentDetailswithCategory(EditAgentViewModel editAgentViewModel)
        {
            using SqlConnectionManager sqlDataAccessManager = new SqlConnectionManager(_configuration);
            var (connection, transaction) = sqlDataAccessManager.StartTransaction();

            try
            {

                var param = new DynamicParameters();
                param.Add("@FirstName", editAgentViewModel.FirstName);
                param.Add("@UserName", editAgentViewModel.UserName);
                param.Add("@LastName", editAgentViewModel.LastName);
                param.Add("@EmailId", editAgentViewModel.EmailId);
                param.Add("@MobileNo", editAgentViewModel.MobileNo);
                param.Add("@Gender", editAgentViewModel.Gender);
                param.Add("@UserId", editAgentViewModel.UserId);
                param.Add("@Status", editAgentViewModel.Status);

                var result = connection.Execute("Usp_UpdateAgentbyUserId", param, transaction, 0, CommandType.StoredProcedure);

                var paramdelete = new DynamicParameters();
                paramdelete.Add("@UserId", editAgentViewModel.UserId);
                var resultdelete = connection.Execute("Usp_DeleteAssignedCategoryUserId", paramdelete, transaction, 0, CommandType.StoredProcedure);


                var paramAgentCategoryAssigned = new DynamicParameters();
                paramAgentCategoryAssigned.Add("@DepartmentId", editAgentViewModel.DepartmentId);
                paramAgentCategoryAssigned.Add("@UserId", editAgentViewModel.UserId);
                var resultAgentCategoryAssigned = connection.Execute("Usp_InsertAgentDepartmentAssigned", paramAgentCategoryAssigned, transaction, 0,
                    CommandType.StoredProcedure);


                if (result > 0 && resultdelete > 0 && resultAgentCategoryAssigned > 0)
                {
                    transaction.Commit();
                    return result;
                }
                else
                {
                    transaction.Rollback();
                    return 0;
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

        }

        public bool UpdateIsFirstLoginStatus(long? userId)
        {
            using SqlConnectionManager sqlDataAccessManager = new SqlConnectionManager(_configuration);
            try
            {

                var (connection, transaction) = sqlDataAccessManager.StartTransaction();
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                var result = connection.Execute("Usp_UpdateIsFirstLoginStatus", param, transaction, 0, CommandType.StoredProcedure);

                if (result > 0)
                {
                    sqlDataAccessManager.Commit();
                    return true;
                }
                else
                {
                    sqlDataAccessManager.Rollback();
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long? UpdateUserDetails(UserMaster usermaster)
        {
            try
            {
                long? result = -1;
                if (usermaster != null)
                {
                    usermaster.ModifiedOn = DateTime.Now;
                    _vueTicketDbContext.Entry(usermaster).State = EntityState.Modified;
                    _vueTicketDbContext.SaveChanges();
                    result = usermaster.UserId;
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}