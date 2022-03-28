using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TicketCore.Common;
using TicketCore.Data.Usermaster.Queries;
using TicketCore.Web.Extensions;
using TicketCore.Web.Filters;
using TicketCore.Web.Messages;

namespace TicketCore.Web.Areas.User.Controllers
{
    [Area("User")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class CheckInController : Controller
    {
        private readonly ICheckInStatusQueries _checkInStatusQueries;
        private readonly INotificationService _notificationService;
        public CheckInController(ICheckInStatusQueries checkInStatusQueries, INotificationService notificationService)
        {
            _checkInStatusQueries = checkInStatusQueries;
            _notificationService = notificationService;
        }

        public ActionResult Process()
        {
            try
            {
                var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                if (!_checkInStatusQueries.CheckIsCategoryAssignedtoAgent(userId))
                {
                    _notificationService.DangerNotification("Message", "Category is not Assigned, Please contact your administrator");
                }

                ViewBag.Data = _checkInStatusQueries.AgentDailyActivity(userId);
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: Process
        [HttpPost]
        public ActionResult InProcess(string process)
        {
            try
            {
                var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                _checkInStatusQueries.StatusCheckInCheckOut(userId);
                HttpContext.Session.SetString("CheckInSession", "N");
                return Json("OK");
            }
            catch (Exception)
            {
                return Json("Failed");
            }
        }

        [HttpPost]
        public ActionResult OutProcess(string process)
        {
            try
            {
                var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                _checkInStatusQueries.StatusCheckInCheckOut(userId);
                HttpContext.Session.SetString("CheckInSession", "Y");
                return Json("OK");
            }
            catch (Exception)
            {
                return Json("Failed");
            }
        }

        [HttpGet]
        public ActionResult AutoOutProcess()
        {
            try
            {
                var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                _checkInStatusQueries.StatusCheckInCheckOut(userId);
                return RedirectToAction("Process", "CheckIn", new { Area = "User" });
            }
            catch (Exception)
            {
                return RedirectToAction("Logout", "Portal");
            }
        }
    }
}
