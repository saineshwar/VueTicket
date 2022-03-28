using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using TicketCore.Models.Rolemasters;
using TicketCore.ViewModels.Rolemasters;
using System.Linq.Dynamic.Core;
namespace TicketCore.Data.Rolemasters.Queries
{
    public class RoleQueries : IRoleQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public RoleQueries(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }

        public bool CheckRoleNameExists(string roleName)
        {
            try
            {
                var result = (from role in _vueTicketDbContext.RoleMasters
                              where role.RoleName == roleName
                              select role).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<RoleMasterGrid> ShowAllRoleMaster(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryablesRoleMasters = (from roleMaster in _vueTicketDbContext.RoleMasters
                                             select new RoleMasterGrid()
                                             {
                                                 RoleId = roleMaster.RoleId,
                                                 RoleName = roleMaster.RoleName,
                                                 Status = roleMaster.Status == true ? "Active" : "InActive",
                                             }).AsQueryable();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryablesRoleMasters = queryablesRoleMasters.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    queryablesRoleMasters = queryablesRoleMasters.Where(m => m.RoleName.Contains(search) || m.RoleName.Contains(search));
                }

                return queryablesRoleMasters;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public RoleMaster GetRoleMasterByroleId(int? roleId)
        {
            try
            {
                var rolesdata = _vueTicketDbContext.RoleMasters.FirstOrDefault(s => s.RoleId == roleId);
                return rolesdata;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EditRoleMasterViewModel GetRoleMasterForEditByroleId(int? roleId)
        {
            try
            {
                var role = (from roles in _vueTicketDbContext.RoleMasters
                            where roles.RoleId == roleId
                            select new EditRoleMasterViewModel()
                            {
                                RoleName = roles.RoleName,
                                Status = roles.Status,
                                RoleId = roles.RoleId
                            }).FirstOrDefault();
                return role;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> ListofRoles()
        {
            var listofrolesdata = (from roles in _vueTicketDbContext.RoleMasters
                                   where roles.Status == true
                                   select new SelectListItem()
                                   {
                                       Text = roles.RoleName,
                                       Value = roles.RoleId.ToString()
                                   }).ToList();

            listofrolesdata.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "---Select---"
            });
            return listofrolesdata;
        }

        public List<SelectListItem> GetAllActiveRolesNotAgent()
        {
            try
            {
                var listofActiveMenu = (from roleMaster in _vueTicketDbContext.RoleMasters
                    where roleMaster.Status == true && roleMaster.RoleId != 4
                    select new SelectListItem
                    {
                        Value = roleMaster.RoleId.ToString(),
                        Text = roleMaster.RoleName
                    }).ToList();

                listofActiveMenu.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "---Select---"
                });

                return listofActiveMenu;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}