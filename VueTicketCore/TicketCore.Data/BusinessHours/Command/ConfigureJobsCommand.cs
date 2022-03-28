using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TicketCore.Models.CategoryConfigrations;

namespace TicketCore.Data.BusinessHours.Command
{
    public class ConfigureJobsCommand : IConfigureJobsCommand
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        private readonly IConfiguration _configuration;
        public ConfigureJobsCommand(VueTicketDbContext vueTicketDbContext, IConfiguration configuration)
        {
            _vueTicketDbContext = vueTicketDbContext;
            _configuration = configuration;
        }

        public bool Save(ConfigureJobModel configureJobModel)
        {
            try
            {
                _vueTicketDbContext.ConfigureJobModel.Add(configureJobModel);
                var result = _vueTicketDbContext.SaveChanges();

                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public int Update(ConfigureJobModel configureJobModel)
        {
            try
            {
                _vueTicketDbContext.Entry(configureJobModel).State = EntityState.Modified;
                return _vueTicketDbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}