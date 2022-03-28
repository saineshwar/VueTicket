using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using TicketCore.Common;
using TicketCore.Data.Audit.Command;
using TicketCore.Data.Usermaster.Queries;
using TicketCore.Models.Audit;
using TicketCore.Web.Extensions;

namespace TicketCore.Web.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuditCommand _auditCommand;
        private readonly ICheckInStatusQueries _checkInStatusQueries;
        public ErrorController(IHttpContextAccessor httpContextAccessor, IAuditCommand auditCommand, ICheckInStatusQueries checkInStatusQueries)
        {
            _httpContextAccessor = httpContextAccessor;
            _auditCommand = auditCommand;
            _checkInStatusQueries = checkInStatusQueries;
        }

        [Route("Error/{StatusCode}")]
        public IActionResult Error(int statusCode)
        {
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (isAjax)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { responseText = "Ajax Error" });
            }

            if (statusCode == 404)
            {
                ViewBag.ErrorId = "404";
            }

            if (statusCode == 500)
            {
                ViewBag.ErrorId = "500";
            }

            CookieOptions option = new CookieOptions();

            if (Request.Cookies[AllSessionKeys.AuthenticationToken] != null)
            {
                option.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Append(AllSessionKeys.AuthenticationToken, "", option);
            }

            HttpContext.Session.Remove(AllSessionKeys.UserId);
            HttpContext.Session.Clear();


            return View();
        }

        [HttpGet]
        [Route("Error/SessionOut/{StatusCode}")]
        public IActionResult SessionOut(int statusCode)
        {
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (isAjax)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { responseText = "Ajax Error" });
            }
            ViewBag.ErrorId = "440";

            var userId = HttpContext.Session.GetSession<long?>(AllSessionKeys.UserId);

            if (userId != null)
            {
                if (_checkInStatusQueries.CheckIsalreadyCheckedIn(userId.Value))
                {
                    _checkInStatusQueries.StatusCheckInCheckOut(userId.Value);
                }
            }


            CookieOptions option = new CookieOptions();

            if (Request.Cookies[AllSessionKeys.AuthenticationToken] != null)
            {
                option.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Append(AllSessionKeys.AuthenticationToken, "", option);
            }

            HttpContext.Session.Remove(AllSessionKeys.RoleId);
            HttpContext.Session.Clear();

            var objaudit = new AuditModel();
            objaudit.AuditId = 0;
            objaudit.RoleId = HttpContext.Session.GetInt32(AllSessionKeys.RoleId);
            objaudit.ControllerName = "Error";
            objaudit.ActionName = "SessionOut";
            objaudit.Area = "";
            objaudit.LoggedOutAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            if (_httpContextAccessor.HttpContext != null)
                objaudit.IPAddress = Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            objaudit.UserID = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
            objaudit.PortalToken = "";
            objaudit.PageAccessed = "";
            objaudit.SessionID = HttpContext.Session.Id;
            objaudit.CurrentDatetime = DateTime.Now;
            objaudit.Logged = true;
            _auditCommand.InsertAuditData(objaudit);

            return View();
        }
    }
}
