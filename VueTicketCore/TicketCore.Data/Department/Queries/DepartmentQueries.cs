using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using TicketCore.Data.Department.Queries;
using TicketCore.Models.Department;
using TicketCore.ViewModels.Categorys;

namespace TicketCore.Data.Department.Queries
{
    public class DepartmentQueries : IDepartmentQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public DepartmentQueries(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }
        public List<SelectListItem> GetAllActiveSelectListItemDepartment()
        {
            try
            {
                var departmentList = (from department in _vueTicketDbContext.Department
                                      where department.Status == true
                                      select new SelectListItem()
                                      {
                                          Text = department.DepartmentName,
                                          Value = department.DepartmentId.ToString()
                                      }).ToList();

                departmentList.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                return departmentList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetAllActiveDepartmentWithoutSelect()
        {
            try
            {
                var departmentList = (from department in _vueTicketDbContext.Department
                                      where department.Status == true
                                      select new SelectListItem()
                                      {
                                          Text = department.DepartmentName,
                                          Value = department.DepartmentId.ToString()
                                      }).ToList();

                return departmentList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetAssignedDepartmentsByUserId(long? userId)
        {
            try
            {
                var result = (from agent in _vueTicketDbContext.AgentDepartmentAssigned
                              join department in _vueTicketDbContext.Department on agent.DepartmentId equals department.DepartmentId
                              where agent.UserId == userId && department.Status == true 
                              select new SelectListItem()
                              {
                                  Text = department.DepartmentName,
                                  Value = department.DepartmentId.ToString()
                              }).ToList();
                return result;


            }
            catch (Exception)
            {
                throw;
            }
        }

        public Departments GetDepartmentById(int? departmentId)
        {
            try
            {
                var result = (from department in _vueTicketDbContext.Department
                              where department.DepartmentId == departmentId
                              select department).SingleOrDefault();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

       

        public int GetDepartmentIdsByUserId(long? userId)
        {
            try
            {
                var result = (from agent in _vueTicketDbContext.AgentDepartmentAssigned
                              where agent.UserId == userId 
                              select agent.DepartmentId).FirstOrDefault();
                return result;


            }
            catch (Exception)
            {
                throw;
            }
        }

        public int? GetAgentAdminDepartmentIdsByUserId(long? userId)
        {
            try
            {
                var result = (from agent in _vueTicketDbContext.DepartmentConfigration
                              where agent.AgentAdminUserId == userId
                    select agent.DepartmentId).FirstOrDefault();
                return result;


            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckDepartmentNameExists(string departmentname)
        {
            try
            {
                var result = (from department in _vueTicketDbContext.Department
                              where department.DepartmentName == departmentname
                              select department).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<DepartmentGridViewModel> ShowAllDepartment(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryableMenuMaster = (from department in _vueTicketDbContext.Department

                                           select new DepartmentGridViewModel()
                                           {
                                               Status = department.Status == true ? "Active" : "InActive",
                                               DepartmentId = department.DepartmentId,
                                               DepartmentName = department.DepartmentName,
                                               Code = department.Code
                                           }
                    );

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryableMenuMaster = queryableMenuMaster.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    queryableMenuMaster = queryableMenuMaster.Where(m => m.DepartmentName.Contains(search) || m.DepartmentName.Contains(search));
                }

                return queryableMenuMaster;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetAssignedDepartmentsofAgentManager(long? userId)
        {
            try
            {
                var result = (from agent in _vueTicketDbContext.DepartmentConfigration
                    join department in _vueTicketDbContext.Department on agent.DepartmentId equals department.DepartmentId
                    where agent.AgentAdminUserId == userId && department.Status == true
                    select new SelectListItem()
                    {
                        Text = department.DepartmentName,
                        Value = department.DepartmentId.ToString()
                    }).ToList();
                return result;


            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetAssignedDepartmentsofAdministrator(long? userId)
        {
            try
            {
                var result = (from agent in _vueTicketDbContext.DepartmentConfigration
                    join department in _vueTicketDbContext.Department on agent.DepartmentId equals department.DepartmentId
                    where agent.HodUserId == userId && department.Status == true
                    select new SelectListItem()
                    {
                        Text = department.DepartmentName,
                        Value = department.DepartmentId.ToString()
                    }).ToList();
                return result;


            }
            catch (Exception)
            {
                throw;
            }
        }

       

    }
}