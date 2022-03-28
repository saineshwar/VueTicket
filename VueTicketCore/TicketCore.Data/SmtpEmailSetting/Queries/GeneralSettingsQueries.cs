using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using TicketCore.ViewModels.SmtpEmailSettings;

namespace TicketCore.Data.SmtpEmailSetting.Queries
{
    public class GeneralSettingsQueries : IGeneralSettingsQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        private readonly IConfiguration _configuration;
        public GeneralSettingsQueries(IConfiguration configuration, VueTicketDbContext vueTicketDbContext)
        {
            _configuration = configuration;
            _vueTicketDbContext = vueTicketDbContext;
        }

        public GeneralSettingsViewModel GetGeneralSetting()
        {
            try
            {

                var getsetting = (from general in _vueTicketDbContext.GeneralSettings
                                  select new GeneralSettingsViewModel()
                                  {
                                      Name = general.Name,
                                      Email = general.Email,
                                      GeneralSettingsId = general.GeneralSettingsId,
                                      SupportEmailId = general.SupportEmailId,
                                      WebsiteTitle = general.WebsiteTitle,
                                      WebsiteUrl = general.WebsiteUrl,
                                      EnableSmsFeature = general.EnableSmsFeature,
                                      EnableSignatureFeature = general.EnableSignatureFeature,
                                      EnableEmailFeature = general.EnableEmailFeature
                                  }).FirstOrDefault();

                return getsetting;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}