using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using TicketCore.Models.Business;
using TicketCore.ViewModels.Business;
using System.Linq.Dynamic.Core;

namespace TicketCore.Data.BusinessHours.Queries
{
    public class BusinessHoursQueries : IBusinessHoursQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;

        public BusinessHoursQueries(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }

        public List<BusinessHoursDetails> DetailsBusinessHours(int? businessHoursId)
        {
            var businessHourslist = (from bh in _vueTicketDbContext.BusinessHoursDetails
                                     where bh.BusinessHoursId == businessHoursId
                                     select bh).ToList();
            return businessHourslist;
        }
        public BusinessHoursModel GetBusinessHours(int? businessHoursId)
        {
            var businessHours = (from bh in _vueTicketDbContext.BusinessHours
                where bh.BusinessHoursId == businessHoursId
                select bh).FirstOrDefault();
            return businessHours;
        }

        public BusinessHoursDetails GetBusinessHoursDetails(int? businessHoursId)
        {
            var businessHours = (from bh in _vueTicketDbContext.BusinessHoursDetails
                where bh.BusinessHoursId == businessHoursId
                select bh).FirstOrDefault();
            return businessHours;
        }


        public int BusinessHoursCount()
        {
            var businessHourscount = (from bh in _vueTicketDbContext.BusinessHours
                where bh.Status == true
                select bh).Count();

            return businessHourscount;
        }

        public List<SelectListItem> ListofBusinessHours()
        {
            var listbt = (from bussinesstype in _vueTicketDbContext.BusinessHours
                          select new SelectListItem()
                          {
                              Text = bussinesstype.Name,
                              Value = bussinesstype.BusinessHoursId.ToString()
                          }).ToList();


            listbt.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-----Select-----"
            });
            return listbt.ToList();
        }

        public List<SelectListItem> ListofBusinessHoursType()
        {
            var listbt = from bussinesstype in _vueTicketDbContext.BusinessHoursType
                         select new SelectListItem()
                         {
                             Text = bussinesstype.BusinessHoursName,
                             Value = bussinesstype.BusinessHoursTypeId.ToString()
                         };

            return listbt.ToList();
        }

        public IQueryable<BusinessHoursViewModel> GetBusinessList(string sortColumn, string sortColumnDir, string search)
        {
       
            try
            {

                var queryable = (from bh in _vueTicketDbContext.BusinessHours
                             join bt in _vueTicketDbContext.BusinessHoursType on bh.HelpdeskHoursType equals bt.BusinessHoursTypeId
                             select new BusinessHoursViewModel()
                             {
                                 Name = bh.Name,
                                 Description = bh.Description,
                                 BusinessHoursName = bt.BusinessHoursName,
                                 CreatedOn = bh.CreatedOn,
                                 BusinessHoursId = bh.BusinessHoursId,
                                 Status =bh.Status == true ? "Active" : "InActive",
                             }).AsQueryable();


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