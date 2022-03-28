using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketCore.Common;
using TicketCore.Data.BusinessHours.Command;
using TicketCore.Data.BusinessHours.Queries;
using TicketCore.Models.Business;
using TicketCore.ViewModels.Business;
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
    public class BusinessHoursController : Controller
    {
        private readonly IBusinessHoursQueries _businessHoursQueries;
        private readonly IBusinessHoursCommand _businessHoursCommand;
        private readonly INotificationService _notificationService;
        public BusinessHoursController(
            IBusinessHoursQueries businessHoursQueries,
            IBusinessHoursCommand businessHoursCommand, 
            INotificationService notificationService)
        {
            _businessHoursQueries = businessHoursQueries;
            _businessHoursCommand = businessHoursCommand;
            _notificationService = notificationService;
        }

        public IActionResult Add()
        {
            var businessHoursModel = new BusinessHoursDetailsViewModel()
            {
                ListofDays = Dayslist(),
                ListofHour = Hourlist(),
                ListofPeriod = Periodslist(),
                SelectedDays = new List<string>(),
                ListofBusinessHoursType = _businessHoursQueries.ListofBusinessHoursType()
            };
            return View(businessHoursModel);
        }


        [HttpPost]
        public IActionResult Add(
            BusinessHoursDetailsViewModel model,
           List<string> morningHour,
           List<string> morningPeriod,
           List<string> eveningHour,
           List<string> eveningPeriod)
        {

            if (model.SelectedBusinessHoursType == "1")
            {
                BusinessHoursModel businessHours = new BusinessHoursModel()
                {
                    BusinessHoursId = 0,
                    Description = model.Description,
                    HelpdeskHoursType = Convert.ToInt32(model.SelectedBusinessHoursType),
                    Name = model.Name
                };

                _businessHoursCommand.AddBusinessHours(businessHours);
                _notificationService.SuccessNotification("Message", "BusinessHours Saved Successfully");
            }
            else
            {
                if (model.SelectedDays == null)
                {
                    _notificationService.DangerNotification("Message", "Please Selected Days");
                }
                else
                {
                  var userId  = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);

                    BusinessHoursModel businessHours = new BusinessHoursModel
                    {
                        BusinessHoursId = 0,
                        Description = model.Description,
                        HelpdeskHoursType = Convert.ToInt32(model.SelectedBusinessHoursType),
                        Name = model.Name,
                        CreatedBy = userId,
                        CreatedOn = DateTime.Now,
                        Status = model.Status
                    };

                    List<BusinessHoursDetails> listBusinessHoursDetails = new List<BusinessHoursDetails>();

                    for (int i = 0; i < model.SelectedDays.Count(); i++)
                    {
                        var currentMorningHour = morningHour[i];
                        var currentMorningPeriod = morningPeriod[i];
                        var currentEveningHour = eveningHour[i];
                        var currentEveningPeriod = eveningPeriod[i];
                        var currentday = model.SelectedDays[i];

                        var starthours = DateTime.Parse($"{currentMorningHour + ":00" + " " + currentMorningPeriod.ToUpper()}").ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                        var endhours = DateTime.Parse($"{currentEveningHour + ":00" + " " + currentEveningPeriod.ToUpper()}").ToString("HH:mm:ss", CultureInfo.InvariantCulture);


                        BusinessHoursDetails businessHoursDetails = new BusinessHoursDetails()
                        {
                            BusinessHoursId = 0,
                            BusinessHoursDetailsId = 0,
                            CreatedOn = DateTime.Now,
                            Day = currentday,
                            MorningTime = $"{currentMorningHour + ":00" + " " + currentMorningPeriod.ToUpper()}",
                            EveningTime = $"{currentEveningHour + ":00" + " " + currentEveningPeriod.ToUpper()}",
                            MorningPeriods = currentMorningPeriod,
                            EveningPeriods = currentEveningPeriod,
                            StartTime = starthours,
                            CloseTime = endhours,
                            CreatedBy = userId
                        };

                        listBusinessHoursDetails.Add(businessHoursDetails);
                    }

                    var result = _businessHoursCommand.AddBusinessHours(businessHours, listBusinessHoursDetails);

                    if (result> 0)
                    {
                        _notificationService.SuccessNotification("Message", "BusinessHours Saved Successfully");
                    }
                    else
                    {
                        _notificationService.DangerNotification("Message", "BusinessHours Failed to Save. Please Try Again.");
                    }

                    return RedirectToAction("Add", "BusinessHours");
                }
            }

            model.ListofDays = Dayslist();
            model.ListofHour = Hourlist();
            model.ListofPeriod = Periodslist();
            model.SelectedDays = new List<string>();
            model.ListofBusinessHoursType = _businessHoursQueries.ListofBusinessHoursType();
            return View(model);
        }


        private List<SelectListItem> Hourlist()
        {
            var hourlist = new List<SelectListItem>
            {
                new SelectListItem() { Text = "1:00", Value = "1:00" },
                new SelectListItem() { Text = "1:30", Value = "1:30" },
                new SelectListItem() { Text = "2:00", Value = "2:00" },
                new SelectListItem() { Text = "2:30", Value = "2:30" },
                new SelectListItem() { Text = "3:00", Value = "3:00" },
                new SelectListItem() { Text = "3:30", Value = "3:30" },
                new SelectListItem() { Text = "4:00", Value = "4:00" },
                new SelectListItem() { Text = "4:30", Value = "4:30" },
                new SelectListItem() { Text = "5:00", Value = "5:00" },
                new SelectListItem() { Text = "5:30", Value = "5:30" },
                new SelectListItem() { Text = "6:00", Value = "6:00" },
                new SelectListItem() { Text = "6:30", Value = "6:30" },
                new SelectListItem() { Text = "7:00", Value = "7:00" },
                new SelectListItem() { Text = "7:30", Value = "7:30" },
                new SelectListItem() { Text = "8:00", Value = "8:00" },
                new SelectListItem() { Text = "8:30", Value = "8:30" },
                new SelectListItem() { Text = "9:00", Value = "9:00" },
                new SelectListItem() { Text = "9:30", Value = "9:30" },
                new SelectListItem() { Text = "10:00", Value = "10:00" },
                new SelectListItem() { Text = "10:30", Value = "10:30" },
                new SelectListItem() { Text = "11:00", Value = "11:00" },
                new SelectListItem() { Text = "11:30", Value = "11:30" },
                new SelectListItem() { Text = "12:00", Value = "12:00" },
                new SelectListItem() { Text = "12:30", Value = "12:30" }
            };
            return hourlist;
        }

        private List<SelectListItem> Periodslist()
        {
            var periodslist = new List<SelectListItem>
            {
                new SelectListItem() { Text = "am", Value = "am" },
                new SelectListItem() { Text = "pm", Value = "pm" }
            };
            return periodslist;
        }

        private List<SelectListItem> Dayslist()
        {
            var dayslist = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Monday", Value = "Monday" },
                new SelectListItem() { Text = "Tuesday", Value = "Tuesday" },
                new SelectListItem() { Text = "Wednesday", Value = "Wednesday" },
                new SelectListItem() { Text = "Thursday", Value = "Thursday" },
                new SelectListItem() { Text = "Friday", Value = "Friday" },
                new SelectListItem() { Text = "Saturday", Value = "Saturday" },
                new SelectListItem() { Text = "Sunday", Value = "Sunday" }
            };
            return dayslist;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GridAllBusinessHours()
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
                var rolesdata = _businessHoursQueries.GetBusinessList(sortColumn, sortColumnDirection, searchValue);
                recordsTotal = rolesdata.Count();
                var data = rolesdata.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult Deactivate(int businessHoursId)
        {
            try
            {
                var result = _businessHoursQueries.BusinessHoursCount();
                if (result == 1)
                {
                    return Json(new { Result = "validation", Message = "Configure Another Business Hours Before InActivating Default One." });
                }

                var businessHours = _businessHoursCommand.DeleteBusinessHours(businessHoursId);

                if (businessHours > 0)
                {
                    return Json(new { Result = "success", Message = "Business Hours InActivated Successfully" });
                }
                else
                {
                    return Json(new { Result = "failed", Message = "Somthing Went Wrong While Processing! Try after Sometime." });
                }
            }
            catch (Exception)
            {
                return Json(new { Result = "failed", Message = "Cannot Delete" });
            }
        }

        public ActionResult Details(int? id)
        {
            var businessHoursDisplayViewModel = new BusinessHoursDisplayViewModel();
            businessHoursDisplayViewModel.BusinessHours = _businessHoursQueries.GetBusinessHours(id);
            businessHoursDisplayViewModel.ListofBusinessHoursDetails = _businessHoursQueries.DetailsBusinessHours(id);
            return View(businessHoursDisplayViewModel);
        }
    }
}
