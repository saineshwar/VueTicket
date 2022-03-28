using System;
using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TicketCore.Models.SmtpEmailSettings;

namespace TicketCore.Data.SmtpEmailSetting.Command
{
    public class SmtpSettingsCommand : ISmtpSettingsCommand
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        private readonly IConfiguration _configuration;
        public SmtpSettingsCommand(VueTicketDbContext vueTicketDbContext, IConfiguration configuration)
        {
            _vueTicketDbContext = vueTicketDbContext;
            _configuration = configuration;
        }
        public int SaveSmtpSettings(SmtpEmailSettings smtpEmailSettings)
        {
            try
            {
                _vueTicketDbContext.SmtpEmailSettings.Add(smtpEmailSettings);
                return _vueTicketDbContext.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int UpdateSmtpSettings(SmtpEmailSettings smtpEmailSettings)
        {
            try
            {
                if (smtpEmailSettings != null)
                {
                    _vueTicketDbContext.Entry(smtpEmailSettings).State = EntityState.Modified;
                }
                return _vueTicketDbContext.SaveChanges();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public int SettingDefaultConnection(int? smtpProviderId)
        {
            using SqlConnectionManager sqlDataAccessManager = new SqlConnectionManager(_configuration);
            try
            {
                var (connection, transaction) = sqlDataAccessManager.StartTransaction();
                var param = new DynamicParameters();
                param.Add("@SmtpProviderId", smtpProviderId);
                var result = connection.Execute("Usp_SMTPEmailSettings_SetDefault", param, transaction, 0, CommandType.StoredProcedure);
                if (result > 0)
                {
                    sqlDataAccessManager.Commit();
                    return result;
                }
                else
                {
                    sqlDataAccessManager.Rollback();
                    return 0;
                }
            }
            catch (Exception)
            {
                sqlDataAccessManager.Rollback();
                throw;
            }


        }
    }
}