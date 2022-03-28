using System.Linq;
using Microsoft.Extensions.Configuration;
using TicketCore.Models.CategoryConfigrations;
using TicketCore.ViewModels.CategoryConfigrations;

namespace TicketCore.Data.BusinessHours.Queries
{
    public class ConfigureJobsQueries : IConfigureJobsQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        private readonly IConfiguration _configuration;
        public ConfigureJobsQueries(VueTicketDbContext vueTicketDbContext, IConfiguration configuration)
        {
            _vueTicketDbContext = vueTicketDbContext;
            _configuration = configuration;
        }

        public ConfigureJobModel GetConfigureJobDetails()
        {
            var configureJob = (from data in _vueTicketDbContext.ConfigureJobModel
                                select data).FirstOrDefault();

            return configureJob;
        }


        public ConfigureJobViewModel GetConfigureJobDetailsViewModel()
        {
            var configureJob = (from data in _vueTicketDbContext.ConfigureJobModel
                select new ConfigureJobViewModel()
                {
                    AssignTicketsJob = data.AssignTicketsJob,
                    AutoCloseTicketsJob = data.AutoCloseTicketsJob,
                    AutoEscalationJobStage1 = data.AutoEscalationJobStage1,
                    AutoEscalationJobStage2 = data.AutoEscalationJobStage2,
                    OverdueEveryResponsJob = data.OverdueEveryResponsJob,
                    OverdueNotificationJob = data.OverdueNotificationJob,
                    TicketOverdueJob = data.TicketOverdueJob

                }).FirstOrDefault();

            return configureJob;
        }

        public int? GetConfigureJobCount()
        {
            var configureJob = (from data in _vueTicketDbContext.ConfigureJobModel
                select data).Count();

            return configureJob;
        }


    }
}