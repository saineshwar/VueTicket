using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketCore.Common;
using TicketCore.Data.BusinessHours.Queries;
using TicketCore.Data.Masters.Command;
using TicketCore.Data.Masters.Queries;
using TicketCore.Models.Masters;
using TicketCore.ViewModels.Masters;
using TicketCore.Web.Extensions;
using TicketCore.Web.Filters;
using TicketCore.Web.Messages;

namespace TicketCore.Web.Areas.Administration.Controllers
{
    [SessionTimeOut]
    [AuthorizeSuperAdmin]
    [Area("Administration")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class ConfigureSlaReminderController : Controller
    {
        private readonly IBusinessHoursQueries _businessHoursQueries;
        private readonly ISlaPoliciesCommand _slaPoliciesCommand;
        private readonly ISlaPoliciesQueries _slaPoliciesQueries;
        private readonly INotificationService _notificationService;
        public ConfigureSlaReminderController(IBusinessHoursQueries businessHoursQueries,
            ISlaPoliciesCommand slaPoliciesCommand,
            ISlaPoliciesQueries slaPoliciesQueries, 
            INotificationService notificationService)
        {
            _businessHoursQueries = businessHoursQueries;
            _slaPoliciesCommand = slaPoliciesCommand;
            _slaPoliciesQueries = slaPoliciesQueries;
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            var slaPoliciesViewModel = new SlaPoliciesReminderViewModel()
            {
                ListofBusinessHours = _businessHoursQueries.ListofBusinessHours()
            };
            return View(slaPoliciesViewModel);
        }


        [HttpPost]
        public IActionResult Add(SlaPoliciesReminderViewModel slaPoliciesViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_slaPoliciesQueries.CheckSlaPoliciesReminderExists(Convert.ToInt32(slaPoliciesViewModel.BusinessHoursId)))
                {
                    _notificationService.DangerNotification("Validation Message", "Sla Policies Reminder already exists for Selected Bussiness Hour");
                }
                else
                {
                    var slaPolicies = new SlaPoliciesReminder()
                    {
                        SlaPoliciesReminderId = 0,
                        CreateDate = DateTime.Now,
                        ResolutionResponseMins = slaPoliciesViewModel.ResolutionResponseMins,
                        ResolutionResponseHour = slaPoliciesViewModel.ResolutionResponseHour,
                        FirstResponseHour = slaPoliciesViewModel.FirstResponseHour,
                        NextResponseMins = slaPoliciesViewModel.NextResponseMins,
                        FirstResponseMins = slaPoliciesViewModel.FirstResponseMins,
                        NextResponseHour = slaPoliciesViewModel.NextResponseHour,
                        UserId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId),
                        BusinessHoursId = Convert.ToInt32(slaPoliciesViewModel.BusinessHoursId)
                    };

                    var result = _slaPoliciesCommand.AddReminder(slaPolicies);
                    _notificationService.SuccessNotification("Message", "Policy Reminder Added Successfully");
                }
            }

            slaPoliciesViewModel.ListofBusinessHours = _businessHoursQueries.ListofBusinessHours();
            return View(slaPoliciesViewModel);
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult GridAllSlaPoliciesReminder()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var records = _slaPoliciesQueries.ShowAllSLAReminder(sortColumn, sortColumnDirection, searchValue);
                recordsTotal = records.Count();
                var data = records.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
