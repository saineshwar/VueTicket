using System;
using Microsoft.EntityFrameworkCore;
using TicketCore.Models.Department;

namespace TicketCore.Data.Department.Command
{
    public class DepartmentCommand : IDepartmentCommand
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public DepartmentCommand(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }

        public int? Add(Departments departments)
        {
            try
            {
                int? result = -1;

                if (departments != null)
                {
                    departments.CreatedOn = DateTime.Now;
                    _vueTicketDbContext.Department.Add(departments);
                    _vueTicketDbContext.SaveChanges();
                    result = departments.DepartmentId;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int? Update(Departments departments)
        {
            try
            {
                int? result = -1;
                if (departments != null)
                {
                    departments.CreatedOn = DateTime.Now;
                    _vueTicketDbContext.Entry(departments).State = EntityState.Modified;
                    _vueTicketDbContext.SaveChanges();
                    result = departments.DepartmentId;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int? Delete(int? departmentId)
        {
            try
            {
                var departments = _vueTicketDbContext.Department.Find(departmentId);
                departments.Status = false;
                _vueTicketDbContext.Entry(departments).State = EntityState.Modified;
                return _vueTicketDbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}