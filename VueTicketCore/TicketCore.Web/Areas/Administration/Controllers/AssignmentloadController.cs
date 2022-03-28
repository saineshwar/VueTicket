using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketCore.Common;
using TicketCore.Data.AssignmentLoad.Command;
using TicketCore.Data.AssignmentLoad.Queries;
using TicketCore.Models.AssignmentLoad;
using TicketCore.ViewModels.Tickets;
using TicketCore.Web.Extensions;
using TicketCore.Web.Filters;
using TicketCore.Web.Messages;

namespace TicketCore.Web.Areas.Administration.Controllers
{
    [SessionTimeOut]
    [AuthorizeSuperAdmin]
    [Area("Administration")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class AssignmentloadController : Controller
    {
        private readonly IAssignmentloadQueries _assignmentloadQueries;
        private readonly IAssignmentLoadCommand _assignmentLoadCommand;
        private readonly INotificationService _notificationService;
        public AssignmentloadController(IAssignmentloadQueries assignmentloadQueries,
            IAssignmentLoadCommand assignmentLoadCommand,
            INotificationService notificationService)
        {
            _assignmentloadQueries = assignmentloadQueries;
            _assignmentLoadCommand = assignmentLoadCommand;
            _notificationService = notificationService;
        }

        
        public ActionResult Assign()
        {
            DefaultTicketSettingsViewModel defaultTicketSettingsViewModel = new DefaultTicketSettingsViewModel();
            var defaultModel = _assignmentloadQueries.GetDefaultTicketCount();
            if (defaultModel != null)
            {
                defaultTicketSettingsViewModel.MaxTicketCount = defaultModel.MaxTicketCount;
                defaultTicketSettingsViewModel.MinTicketsCount = defaultModel.MinTicketsCount;
                defaultTicketSettingsViewModel.AutoTicketsCloseMin = defaultModel.AutoTicketsCloseMin;
                defaultTicketSettingsViewModel.AutoTicketsCloseHour = defaultModel.AutoTicketsCloseHour;
                defaultTicketSettingsViewModel.DefaultTicketId = defaultModel.DefaultTicketId;
            }
            return View(defaultTicketSettingsViewModel);
        }

        [HttpPost]
        public ActionResult Assign(DefaultTicketSettingsViewModel defaultTicket)
        {
           
            if (ModelState.IsValid)
            {

                var defaultTicketSettings = new DefaultTicketSettings();

                if (defaultTicket.MinTicketsCount > defaultTicket.MaxTicketCount)
                {
                    _notificationService.DangerNotification("Message", "Minimum Ticket Count Cannot be Greater then Max Ticket Count.");
                }
                else
                {
                    defaultTicketSettings.DefaultTicketId = defaultTicket.DefaultTicketId ?? 0;
                    defaultTicketSettings.UserId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                    defaultTicketSettings.MaxTicketCount = defaultTicket.MaxTicketCount;
                    defaultTicketSettings.MinTicketsCount = defaultTicket.MinTicketsCount;
                    defaultTicketSettings.AutoTicketsCloseMin = defaultTicket.AutoTicketsCloseMin;
                    defaultTicketSettings.AutoTicketsCloseHour = defaultTicket.AutoTicketsCloseHour;
                    _assignmentLoadCommand.AddDefaultTicketCount(defaultTicketSettings);
                    _notificationService.SuccessNotification("Message", "DefaultTicket Saved Successfully");
                }
             
            }

            return RedirectToAction("Assign", "Assignmentload");
        }
    }
}
