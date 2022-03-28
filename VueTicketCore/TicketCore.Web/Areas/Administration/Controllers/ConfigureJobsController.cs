using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketCore.Common;
using TicketCore.Data.BusinessHours.Command;
using TicketCore.Data.BusinessHours.Queries;
using TicketCore.Models.CategoryConfigrations;
using TicketCore.ViewModels.CategoryConfigrations;
using TicketCore.Web.Extensions;
using TicketCore.Web.Filters;
using TicketCore.Web.Messages;

namespace TicketCore.Web.Areas.Administration.Controllers
{
    [SessionTimeOut]
    [AuthorizeSuperAdmin]
    [Area("Administration")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class ConfigureJobsController : Controller
    {
        private readonly IConfigureJobsCommand _configureJobsCommand;
        private readonly IConfigureJobsQueries _configureJobsQueries;
        private readonly INotificationService _notificationService;
        public ConfigureJobsController(IConfigureJobsCommand configureJobsCommand, IConfigureJobsQueries configureJobsQueries, INotificationService notificationService)
        {
            _configureJobsCommand = configureJobsCommand;
            _configureJobsQueries = configureJobsQueries;
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult Process()
        {
            var getdata = _configureJobsQueries.GetConfigureJobDetailsViewModel();
            return View(getdata);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Process(ConfigureJobViewModel configureJobViewModel)
        {

            var userid = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
            var totalcount = _configureJobsQueries.GetConfigureJobCount();

            if (totalcount == 0)
            {
                var configureJobModel = new ConfigureJobModel();
                configureJobModel.OverdueNotificationJob = configureJobViewModel.OverdueNotificationJob;
                configureJobModel.OverdueEveryResponsJob = configureJobViewModel.OverdueEveryResponsJob;
                configureJobModel.AutoEscalationJobStage2 = configureJobViewModel.AutoEscalationJobStage2;
                configureJobModel.AutoEscalationJobStage1 = configureJobViewModel.AutoEscalationJobStage1;
                configureJobModel.AssignTicketsJob = configureJobViewModel.AssignTicketsJob;
                configureJobModel.AutoCloseTicketsJob = configureJobViewModel.AutoCloseTicketsJob;
                configureJobModel.TicketOverdueJob = configureJobViewModel.TicketOverdueJob;
                configureJobModel.CreatedOn = DateTime.Now;
                configureJobModel.CreatedBy = userid;
                configureJobModel.ConfigureJobId = 0;

                var result = _configureJobsCommand.Save(configureJobModel);

                _notificationService.SuccessNotification("Message", "Job Configuration Saved Successfully.");
            }
            else
            {
                var getjobdata = _configureJobsQueries.GetConfigureJobDetails();

                getjobdata.OverdueNotificationJob = configureJobViewModel.OverdueNotificationJob;
                getjobdata.OverdueEveryResponsJob = configureJobViewModel.OverdueEveryResponsJob;
                getjobdata.AutoEscalationJobStage2 = configureJobViewModel.AutoEscalationJobStage2;
                getjobdata.AutoEscalationJobStage1 = configureJobViewModel.AutoEscalationJobStage1;
                getjobdata.AssignTicketsJob = configureJobViewModel.AssignTicketsJob;
                getjobdata.AutoCloseTicketsJob = configureJobViewModel.AutoCloseTicketsJob;
                getjobdata.TicketOverdueJob = configureJobViewModel.TicketOverdueJob;
                getjobdata.ModifiedOn = DateTime.Now;
                getjobdata.ModifiedBy = userid;

                var result = _configureJobsCommand.Update(getjobdata);

                _notificationService.SuccessNotification("Message", "Job Configuration Updated Successfully.");
            }

            return View();
        }

    }
}
