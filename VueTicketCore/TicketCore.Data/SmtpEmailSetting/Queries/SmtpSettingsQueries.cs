using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using TicketCore.Models.SmtpEmailSettings;
using TicketCore.ViewModels.SmtpEmailSettings;

namespace TicketCore.Data.SmtpEmailSetting.Queries
{
    public class SmtpSettingsQueries : ISmtpSettingsQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        private readonly IConfiguration _configuration;
        public SmtpSettingsQueries(VueTicketDbContext vueTicketDbContext, IConfiguration configuration)
        {
            _vueTicketDbContext = vueTicketDbContext;
            _configuration = configuration;
        }

        public SmtpEmailSettingsViewModel EditSmtpSettings(int? smtpProviderId)
        {
            try
            {
                var queryablesmtp = (from stmp in _vueTicketDbContext.SmtpEmailSettings
                                     where stmp.SmtpProviderId == smtpProviderId
                                     select new SmtpEmailSettingsViewModel()
                                     {
                                         Timeout = stmp.Timeout,
                                         Port = stmp.Port,
                                         TlSProtocol = stmp.TlSProtocol,
                                         SslProtocol = stmp.SslProtocol,
                                         Username = stmp.Username,
                                         SmtpProviderId = stmp.SmtpProviderId,
                                         Host = stmp.Host,
                                         CreatedOn = stmp.CreatedOn,
                                         Name = stmp.Name,
                                         Password = stmp.Password,
                                         EmailTo = stmp.EmailTo,
                                         MailSender = stmp.MailSender
                                     }).FirstOrDefault();
                return queryablesmtp;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SmtpEmailSettings GetEmailSettings(int? smtpProviderId)
        {
            try
            {
                var queryablesmtp = (from stmp in _vueTicketDbContext.SmtpEmailSettings
                                     where stmp.SmtpProviderId == smtpProviderId
                                     select stmp).FirstOrDefault();
                return queryablesmtp;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public SmtpEmailSettings GetDefaultEmailSettings()
        {
            var getdefaultsetting = (from smtp in _vueTicketDbContext.SmtpEmailSettings
                                     where smtp.IsDefault == true
                                     select smtp).FirstOrDefault();

            return getdefaultsetting;
        }

        public IQueryable<SmtpEmailSettingsGrid> ShowAllSmtpSettings(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryable = (from smtp in _vueTicketDbContext.SmtpEmailSettings

                                           select new SmtpEmailSettingsGrid()
                                           {
                                               Timeout = smtp.Timeout,
                                               Port = smtp.Port,
                                               TlSProtocol = smtp.TlSProtocol,
                                               SslProtocol = smtp.SslProtocol,
                                               Username = smtp.Username,
                                               SmtpProviderId = smtp.SmtpProviderId,
                                               Host = smtp.Host,
                                               CreatedOn = smtp.CreatedOn,
                                               Name = smtp.Name,
                                               IsDefault = smtp.IsDefault == true ? "Yes" : "No",
                                               Password = smtp.Password,
                                           }
                    );

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
            catch (Exception)
            {
                throw;
            }
        }


    }
}