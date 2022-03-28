using System;
using Microsoft.EntityFrameworkCore;
using TicketCore.Models.CategoryConfigrations;

namespace TicketCore.Data.Department.Command
{
    public class DepartmentConfigrationCommand : IDepartmentConfigrationCommand
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public DepartmentConfigrationCommand(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }

        public int? Add(DepartmentConfigration department)
        {
            try
            {
                int? result = -1;

                if (department != null)
                {
                    department.CreatedOn = DateTime.Now;
                    _vueTicketDbContext.DepartmentConfigration.Add(department);
                    _vueTicketDbContext.SaveChanges();
                    result = department.DepartmentConfigrationId;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int? Update(DepartmentConfigration department)
        {
            try
            {
                int? result = -1;
                if (department != null)
                {
                    department.Modifiedon = DateTime.Now;
                    _vueTicketDbContext.Entry(department).State = EntityState.Modified;
                    _vueTicketDbContext.SaveChanges();
                    result = department.DepartmentId;
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