using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TicketCore.Models.SmtpEmailSettings;

namespace TicketCore.Data.SmtpEmailSetting.Command
{
    public class GeneralSettingsCommand : IGeneralSettingsCommand
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        private readonly IConfiguration _configuration;
        public GeneralSettingsCommand(IConfiguration configuration, VueTicketDbContext vueTicketDbContext)
        {
            _configuration = configuration;
            _vueTicketDbContext = vueTicketDbContext;
        }

        public void InsertGeneralSetting(GeneralSettings generalSettings)
        {
            try
            {
                _vueTicketDbContext.GeneralSettings.Add(generalSettings);
                _vueTicketDbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateGeneralSetting(GeneralSettings generalSettings)
        {
            try
            {
                _vueTicketDbContext.Entry(generalSettings).State = EntityState.Modified;
                _vueTicketDbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}