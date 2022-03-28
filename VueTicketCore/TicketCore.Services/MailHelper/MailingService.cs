using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using TicketCore.Common;
using TicketCore.Data.ApplicationLog.Command;
using TicketCore.Data.SmtpEmailSetting.Queries;
using TicketCore.Models.ApplicationLog;

namespace TicketCore.Services.MailHelper
{
    public class MailingService : IMailingService
    {
        private readonly ISmtpSettingsQueries _smtpSettingsQueries;
        private readonly ILogger<MailingService> _logger;
        private readonly IEmailLogCommand _emailLogCommand;
        public MailingService(ISmtpSettingsQueries smtpSettingsQueries, ILogger<MailingService> logger, IEmailLogCommand emailLogCommand)
        {
            _smtpSettingsQueries = smtpSettingsQueries;
            _logger = logger;
            _emailLogCommand = emailLogCommand;
        }

        public bool SendEmailAsync(SendingMailRequest mailRequest)
        {
            var senderrequest = _smtpSettingsQueries.GetDefaultEmailSettings();
            var aesAlgorithm = new AesAlgorithm();

            try
            {
                using SmtpClient smtpClient = new SmtpClient();
                using MailMessage myMail = new MailMessage();

                if (!string.IsNullOrEmpty(senderrequest.Username) && !string.IsNullOrEmpty(senderrequest.Password))
                {
                    var password = aesAlgorithm.DecryptFromBase64String(senderrequest.Password);
                    var myCredentials = new NetworkCredential(senderrequest.Username, password);
                    smtpClient.Credentials = myCredentials;
                }

                MailAddress fromAddress = new MailAddress(senderrequest.MailSender);
                smtpClient.Host = senderrequest.Host;
                smtpClient.Port = Convert.ToInt32(senderrequest.Port);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Timeout = (60 * 5 * 1000);
                myMail.From = fromAddress;
                myMail.Subject = mailRequest.Subject;
                myMail.IsBodyHtml = true;
                myMail.Body = mailRequest.Body;
                myMail.To.Add(mailRequest.ToEmail);

                if (senderrequest.SslProtocol == "Y" || senderrequest.TlSProtocol == "Y")
                {
                    smtpClient.EnableSsl = true;
                }
                else
                {
                    smtpClient.EnableSsl = false;
                }


                var emailLogsModel = new EmailLogsModel()
                {
                    CreatedOn = DateTime.Now,
                    EmailId = mailRequest.ToEmail,
                    EmailLogId = 0,
                    TriggeredEvent = mailRequest.TriggeredEvent,
                    CreatedBy = mailRequest.CreatedBy
                };

                _emailLogCommand.InsertEmailLogs(emailLogsModel);

                smtpClient.Send(myMail);

              
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendEmailAsync Failed");
                return false;
            }
        }

    }
}