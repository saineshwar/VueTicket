using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text;
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
    public class ConfigureSlaController : Controller
    {
        private readonly ISlaPoliciesCommand _slaPoliciesCommand;
        private readonly IPriorityQueries _priorityQueries;
        private readonly ISlaPoliciesQueries _iSlaPoliciesQueries;
        private readonly INotificationService _notificationService;
        private readonly IBusinessHoursQueries _businessHoursQueries;
        public ConfigureSlaController(ISlaPoliciesCommand slaPoliciesCommand,
            IPriorityQueries priorityQueries,
            ISlaPoliciesQueries slaPoliciesQueries, INotificationService notificationService, IBusinessHoursQueries businessHoursQueries)
        {
            _slaPoliciesCommand = slaPoliciesCommand;
            _priorityQueries = priorityQueries;
            _iSlaPoliciesQueries = slaPoliciesQueries;
            _notificationService = notificationService;
            _businessHoursQueries = businessHoursQueries;
        }

        [HttpGet]
        public IActionResult Add()
        {
            var slaPoliciesViewModel = new SlaPoliciesViewModel()
            {
                ListofPriority = _priorityQueries.GetAllPrioritySelectListItem(),
                ListofBusinessHours = _businessHoursQueries.ListofBusinessHours(),
                Escalation = true
            };

            return View(slaPoliciesViewModel);
        }


        public IActionResult Add(SlaPoliciesViewModel slaPoliciesViewModel)
        {
            if (ModelState.IsValid)
            {
                int stage;
                ValidateSla(slaPoliciesViewModel, out var flag, out stage, out var message);
                if (flag == true)
                {
                    _notificationService.DangerNotification("Message", message.ToString());
                }
                else
                {
                    if (_iSlaPoliciesQueries.CheckPoliciesExists(slaPoliciesViewModel.PriorityId))
                    {
                        ModelState.AddModelError("", "Priority already exists Delete Older One to Add New One");
                    }
                    else
                    {

                        var slaPolicies = new SlaPolicies()
                        {
                            SlaPoliciesId = 0,
                            PriorityId = slaPoliciesViewModel.PriorityId,
                            CreateDate = DateTime.Now,
                            ResolutionResponseMins = slaPoliciesViewModel.ResolutionResponseMins,
                            ResolutionResponseHour = slaPoliciesViewModel.ResolutionResponseHour,
                            FirstResponseHour = slaPoliciesViewModel.FirstResponseHour,
                            NextResponseMins = slaPoliciesViewModel.NextResponseMins,
                            FirstResponseMins = slaPoliciesViewModel.FirstResponseMins,
                            NextResponseHour = slaPoliciesViewModel.NextResponseHour,
                            UserId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId),
                            EscalationStatus = slaPoliciesViewModel.Escalation,
                            BusinessHoursId = Convert.ToInt32(slaPoliciesViewModel.BusinessHoursId)
                        };
                        var result = _slaPoliciesCommand.Add(slaPolicies);

                        _notificationService.SuccessNotification("Message", "Policy Added Successfully");

                    }
                }
            }

            slaPoliciesViewModel.ListofPriority = _priorityQueries.GetAllPrioritySelectListItem();
            slaPoliciesViewModel.ListofBusinessHours = _businessHoursQueries.ListofBusinessHours();
            return View(slaPoliciesViewModel);
        }

        private void ValidateSla(SlaPoliciesViewModel slaPoliciesViewModel, out bool flag, out int stage, out StringBuilder message)
        {
            int counter = 0;
            message = new StringBuilder();
            bool localflag = false;
            // ReSharper disable once ReplaceWithSingleAssignment.False

            if ((slaPoliciesViewModel.FirstResponseHour == null && slaPoliciesViewModel.FirstResponseMins == null))
            {
                localflag = true;
                counter += 1;
                message.Append("First response time | ");
            }

            if ((slaPoliciesViewModel.NextResponseHour == null && slaPoliciesViewModel.NextResponseMins == null))
            {
                localflag = true;
                counter += 1;
                message.Append("Every response time  | ");
            }

            if ((slaPoliciesViewModel.ResolutionResponseHour == null && slaPoliciesViewModel.ResolutionResponseMins == null))
            {
                localflag = true;
                counter += 1;
                message.Append("Resolution time  | ");
            }


            message.Append("Fields Cannot be Empty");
            stage = counter;
            flag = localflag;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult GridAllSlaPolicies()
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
                var records = _iSlaPoliciesQueries.ShowAllSLA(sortColumn, sortColumnDirection, searchValue);
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


        public JsonResult Delete(int slaPoliciesId)
        {
            try
            {
                var result = _slaPoliciesCommand.Delete(slaPoliciesId);
                if (result == 0)
                {
                    return Json(new { Result = "failed" });
                }
                else if (result == -1)
                {
                    return Json(new { Result = "failed", Message = "Cannot Delete" });
                }

                return Json(new { Result = "success" });
            }
            catch (Exception)
            {
                return Json(new { Result = "failed", Message = "Cannot Delete" });
            }
        }
    }
}
