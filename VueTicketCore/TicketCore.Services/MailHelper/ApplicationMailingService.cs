using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TicketCore.Common;
using TicketCore.Data.Tickets.Queries;

namespace TicketCore.Services.MailHelper
{
    public class ApplicationMailingService : IApplicationMailingService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMailingService _mailingService;
        private readonly ITicketQueries _iTicketQueries;
        private readonly ILogger<ApplicationMailingService> _logger;
        public ApplicationMailingService(IWebHostEnvironment webHostEnvironment, IMailingService mailingService, ITicketQueries ticketQueries, ILogger<ApplicationMailingService> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _mailingService = mailingService;
            _iTicketQueries = ticketQueries;
            _logger = logger;
        }

        public void TicketAgentReplyEmail(string templateUrl, string onevent, long? ticketId, string statusname, string repliedby, long? createdBy)
        {
            try
            {

                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "ticketreply.html");
                var recipientdetails = _iTicketQueries.GetAssignedAgentDetails(ticketId);

                string mailText;
                using (var streamReader = new StreamReader(filePath))
                {
                    mailText = streamReader.ReadToEnd();
                    streamReader?.Close();
                }

                var str = new StringBuilder();
                str.Append("<table style='border-collapse: collapse; border:1px solid #ddd; font-size:14px;' class='one-column' cellpadding='0'  cellspacing='0' width='100%'>  <tbody>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd; '>Ticket Id</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'> <a href='{templateUrl}'>{ticketId}</a> </td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Status</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{statusname}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Replied by</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{repliedby}</td>");
                str.Append("</tr>");
                str.Append("</tbody></table>");

                mailText = mailText.Replace("[XXXXXXXXXXXXXXXXXXXXX]", str.ToString());
                mailText = mailText.Replace("[##Name##]", recipientdetails.RecipientFullName);

                var sendingMailRequest = new SendingMailRequest()
                {
                    Attachments = null,
                    ToEmail = recipientdetails.RecipientEmailId,
                    Body = mailText,
                    Subject = $"[Ticket #{ticketId}] [Replied] by {repliedby}",
                    CreatedBy = createdBy,
                    TriggeredEvent = onevent
                };


                _mailingService.SendEmailAsync(sendingMailRequest);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TicketAgentReplyEmail Failed");
            }
        }

        public void TicketUserReplyEmail(string templateUrl,string onevent, long? ticketId, string statusname, string repliedby, long? createdBy)
        {
            try
            {

                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "ticketreply.html");
                var recipientdetails = _iTicketQueries.GetCreatedTicketUserDetails(ticketId);

                string mailText;
                using (var streamReader = new StreamReader(filePath))
                {
                    mailText = streamReader.ReadToEnd();
                    streamReader?.Close();
                }

                var str = new StringBuilder();
                str.Append("<table style='border-collapse: collapse; border:1px solid #ddd; font-size:14px;' class='one-column' cellpadding='0'  cellspacing='0' width='100%'>  <tbody>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd; '>Ticket Id</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'> <a href='{templateUrl}'>{ticketId}</a> </td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Status</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{statusname}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Replied by</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{repliedby}</td>");
                str.Append("</tr>");
                str.Append("</tbody></table>");

                mailText = mailText.Replace("[XXXXXXXXXXXXXXXXXXXXX]", str.ToString());
                mailText = mailText.Replace("[##Name##]", recipientdetails.RecipientFullName);

                var sendingMailRequest = new SendingMailRequest()
                {
                    Attachments = null,
                    ToEmail = recipientdetails.RecipientEmailId,
                    Body = mailText,
                    Subject = $"[Ticket #{ticketId}] [Replied] by {repliedby}",
                    CreatedBy = createdBy,
                    TriggeredEvent = onevent
                };


                _mailingService.SendEmailAsync(sendingMailRequest);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TicketUserReplyEmail Failed");
            }
        }

        public void TicketEditedTicketEmail(string templateUrl, string onevent, long? ticketId, string statusname, string repliedby, long? createdBy)
        {
            try
            {

                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "ticketreply.html");
                var recipientdetails = _iTicketQueries.GetCreatedTicketUserDetails(ticketId);

                string mailText;
                using (var streamReader = new StreamReader(filePath))
                {
                    mailText = streamReader.ReadToEnd();
                    streamReader?.Close();
                }

                var str = new StringBuilder();
                str.Append("<table style='border-collapse: collapse; border:1px solid #ddd; font-size:14px;' class='one-column' cellpadding='0'  cellspacing='0' width='100%'>  <tbody>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd; '>Ticket Id</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'> <a href='{templateUrl}'>{ticketId}</a> </td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Status</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{statusname}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Edited by</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{repliedby}</td>");
                str.Append("</tr>");
                str.Append("</tbody></table>");

                mailText = mailText.Replace("[XXXXXXXXXXXXXXXXXXXXX]", str.ToString());
                mailText = mailText.Replace("[##Name##]", recipientdetails.RecipientFullName);

                var sendingMailRequest = new SendingMailRequest()
                {
                    Attachments = null,
                    ToEmail = recipientdetails.RecipientEmailId,
                    Body = mailText,
                    Subject = $"[Ticket #{ticketId}] [Edited Ticket] by {repliedby}",
                    CreatedBy = createdBy,
                    TriggeredEvent = onevent 
                };


                _mailingService.SendEmailAsync(sendingMailRequest);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TicketEditedTicketEmail Failed");
            }
        }

        public void TicketEditedReplyTicketEmail(string templateUrl, string onevent, long? ticketId, string statusname, string repliedby, long? createdBy)
        {
            try
            {

                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "ticketreply.html");
                var recipientdetails = _iTicketQueries.GetCreatedTicketUserDetails(ticketId);

                string mailText;
                using (var streamReader = new StreamReader(filePath))
                {
                    mailText = streamReader.ReadToEnd();
                    streamReader?.Close();
                }

                var str = new StringBuilder();
                str.Append("<table style='border-collapse: collapse; border:1px solid #ddd; font-size:14px;' class='one-column' cellpadding='0'  cellspacing='0' width='100%'>  <tbody>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd; '>Ticket Id</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'> <a href='{templateUrl}'>{ticketId}</a> </td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Status</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{statusname}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Edited Reply by</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{repliedby}</td>");
                str.Append("</tr>");
                str.Append("</tbody></table>");

                mailText = mailText.Replace("[XXXXXXXXXXXXXXXXXXXXX]", str.ToString());
                mailText = mailText.Replace("[##Name##]", recipientdetails.RecipientFullName);

                var sendingMailRequest = new SendingMailRequest()
                {
                    Attachments = null,
                    ToEmail = recipientdetails.RecipientEmailId,
                    Body = mailText,
                    Subject = $"[Ticket #{ticketId}] [Edited Ticket Reply] by {repliedby}",
                    CreatedBy = createdBy,
                    TriggeredEvent = onevent
                };


                _mailingService.SendEmailAsync(sendingMailRequest);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TicketEditedReplyTicketEmail Failed");
            }
        }

        public void ChangeTicketPriorityEmail(string templateUrl, string onevent, long? ticketId, string priority, string repliedby, long? createdBy)
        {
            try
            {

                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "ticketreply.html");
                var recipientdetails = _iTicketQueries.GetCreatedTicketUserDetails(ticketId);

                string mailText;
                using (var streamReader = new StreamReader(filePath))
                {
                    mailText = streamReader.ReadToEnd();
                    streamReader?.Close();
                }

                var str = new StringBuilder();
                str.Append("<table style='border-collapse: collapse; border:1px solid #ddd; font-size:14px;' class='one-column' cellpadding='0'  cellspacing='0' width='100%'>  <tbody>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd; '>Ticket Id</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'> <a href='{templateUrl}'>{ticketId}</a> </td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Priority</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{priority}</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th align='left' style='padding:10px;  border:1px solid #ddd;'>Changed by</th>");
                str.Append($"<td align='left' style='padding:10px;  border:1px solid #ddd;'>{repliedby}</td>");
                str.Append("</tr>");
                str.Append("</tbody></table>");

                mailText = mailText.Replace("[XXXXXXXXXXXXXXXXXXXXX]", str.ToString());
                mailText = mailText.Replace("[##Name##]", recipientdetails.RecipientFullName);

                var sendingMailRequest = new SendingMailRequest()
                {
                    Attachments = null,
                    ToEmail = recipientdetails.RecipientEmailId,
                    Body = mailText,
                    Subject = $"[Changed Ticket Priority #{ticketId}] [Changed by]  {repliedby}",
                    CreatedBy = createdBy,
                    TriggeredEvent = onevent
                };


                _mailingService.SendEmailAsync(sendingMailRequest);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ChangeTicketPriorityEmail Failed");
            }
        }


    }
}