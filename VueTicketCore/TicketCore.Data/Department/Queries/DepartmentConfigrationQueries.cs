using System;
using System.Linq;
using TicketCore.ViewModels.Business;
using TicketCore.ViewModels.CategoryConfigrations;
using System.Linq.Dynamic.Core;

using TicketCore.Models.CategoryConfigrations;

namespace TicketCore.Data.Department.Queries
{
    public class DepartmentConfigrationQueries : IDepartmentConfigrationQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public DepartmentConfigrationQueries(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }


        public bool CheckDuplicateDepartmentConfigration(long adminuserId, long hoduserId, int categoryId)
        {

            var result = (from categoryconfig in _vueTicketDbContext.DepartmentConfigration
                          where categoryconfig.AgentAdminUserId == adminuserId && categoryconfig.AgentAdminUserId == hoduserId && categoryconfig.DepartmentId == categoryId
                          select categoryconfig).Count();
            return result > 0;

        }

        public DepartmentConfigration GetDepartmentConfigration(int categoryConfigrationId)
        {

            var result = (from categoryconfig in _vueTicketDbContext.DepartmentConfigration
                          where categoryconfig.DepartmentConfigrationId == categoryConfigrationId
                          select categoryconfig).FirstOrDefault();
            return result;

        }

        public IQueryable<ShowDepartmentConfigration> GetDepartmentConfigrationList(string sortColumn, string sortColumnDir, string search)
        {

            try
            {

                var queryable = from categoryconfig in _vueTicketDbContext.DepartmentConfigration
                                join Department in _vueTicketDbContext.Department on categoryconfig.DepartmentId equals Department.DepartmentId
                                join businessHour in _vueTicketDbContext.BusinessHours on categoryconfig.BusinessHoursId equals businessHour.BusinessHoursId
                                join usermaster in _vueTicketDbContext.UserMasters on categoryconfig.AgentAdminUserId equals usermaster.UserId
                                join hodUsermaster in _vueTicketDbContext.UserMasters on categoryconfig.HodUserId equals hodUsermaster.UserId
                                select new ShowDepartmentConfigration()
                                {
                                    DepartmentName = Department.DepartmentName,
                                    Status = categoryconfig.Status == true ? "Active" : "InActive",
                                    Name = businessHour.Name,
                                    DepartmentConfigrationId = categoryconfig.DepartmentConfigrationId,
                                    UserName = usermaster.FirstName + " " + usermaster.LastName,
                                    HodName = hodUsermaster.FirstName + " " + hodUsermaster.LastName,
                                };


                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryable = queryable.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    queryable = queryable.Where(m => m.Name.Contains(search) || m.Name.Contains(search));
                }

                return queryable;

            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}