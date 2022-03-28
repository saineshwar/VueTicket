using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TicketCore.Models.Business;


namespace TicketCore.Data.BusinessHours.Command
{
    public class BusinessHoursCommand : IBusinessHoursCommand
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        private readonly IConfiguration _configuration;
        public BusinessHoursCommand(VueTicketDbContext vueTicketDbContext, IConfiguration configuration)
        {
            _vueTicketDbContext = vueTicketDbContext;
            _configuration = configuration;
        }

        public int? AddBusinessHours(BusinessHoursModel businessHours, List<BusinessHoursDetails> listBusinessHoursDetails)
        {

            try
            {
                _vueTicketDbContext.BusinessHours.Add(businessHours);
                _vueTicketDbContext.SaveChanges();

                var businessHoursId = businessHours.BusinessHoursId;

                foreach (var businessHoursDetail in listBusinessHoursDetails)
                {
                    businessHoursDetail.BusinessHoursId = businessHoursId;
                    _vueTicketDbContext.BusinessHoursDetails.Add(businessHoursDetail);
                    _vueTicketDbContext.SaveChanges();
                }

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int? AddBusinessHours(BusinessHoursModel businessHours)
        {
            try
            {
                _vueTicketDbContext.BusinessHours.Add(businessHours);
                _vueTicketDbContext.SaveChanges();

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int DeleteBusinessHours(int? businessHoursId)
        {
            try
            {
                var businessHours = _vueTicketDbContext.BusinessHours.Find(businessHoursId);
                businessHours.Status = false;
                _vueTicketDbContext.Entry(businessHours).State = EntityState.Modified;
                return _vueTicketDbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}