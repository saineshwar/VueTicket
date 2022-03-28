using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketCore.Web.Filters;

namespace TicketCore.Web.Areas.Administration.Controllers
{
    [SessionTimeOut]
    [AuthorizeSuperAdmin]
    [Area("Administration")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
