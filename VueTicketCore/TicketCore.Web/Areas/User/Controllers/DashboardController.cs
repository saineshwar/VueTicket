using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketCore.Common;
using TicketCore.Data.Dashboard.Queries;
using TicketCore.Data.Department.Queries;
using TicketCore.ViewModels.Dashboard;
using TicketCore.Web.Extensions;
using TicketCore.Web.Filters;

namespace TicketCore.Web.Areas.User.Controllers
{
    [SessionTimeOut]
    [AuthorizeUser]
    [Area("User")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    [OnboardingCheckAttribute]
    public class DashboardController : Controller
    {
        private readonly IDepartmentQueries _departmentQueries;
        private readonly IDashboardQueries _dashboardQueries;
        public DashboardController(IDepartmentQueries departmentQueries, IDashboardQueries dashboardQueries)
        {
            _departmentQueries = departmentQueries;
            _dashboardQueries = dashboardQueries;
        }
        public IActionResult Dashboard()
        {
            var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
            var userDashboardViewModel = new UserDashboardViewModel()
            {
                ListofDepartments = _departmentQueries.GetAllActiveDepartmentWithoutSelect()
        };
            return View(userDashboardViewModel);
        }

        public IActionResult GetDepartmentWiseCount(RequestDepartmentWiseReport requestDepartment)
        {
            var user = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
            var listofStatus = _dashboardQueries.GetUserDepartmentWiseUserTicketCountbyUserId(user, requestDepartment.DepartmentId);
            return Json(listofStatus);
        }
    }
}
