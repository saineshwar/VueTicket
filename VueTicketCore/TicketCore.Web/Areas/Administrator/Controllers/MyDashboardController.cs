using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TicketCore.Common;
using TicketCore.Data.Dashboard.Queries;
using TicketCore.Data.Department.Queries;
using TicketCore.Data.Notifications.Command;
using TicketCore.Data.Notifications.Queries;
using TicketCore.ViewModels.Dashboard;
using TicketCore.Web.Extensions;
using TicketCore.Web.Filters;
using TicketCore.Web.Messages;

namespace TicketCore.Web.Areas.Administrator.Controllers
{
    [SessionTimeOut]
    [AuthorizeAdministrator]
    [Area("Administrator")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class MyDashboardController : Controller
    {
        private readonly IDepartmentQueries _departmentQueries;
        private readonly IDashboardQueries _dashboardQueries;
        private readonly ITicketNotificationQueries _iTicketNotificationQueries;
        private readonly ITicketNotificationCommand _iTicketNotificationCommand;
        private readonly IChartsQueries _chartsQueries;
        private readonly INotificationService _notificationService;
        public MyDashboardController(IDepartmentQueries departmentQueries,
            IDashboardQueries dashboardQueries,
            ITicketNotificationQueries iTicketNotificationQueries,
            ITicketNotificationCommand iTicketNotificationCommand, 
            IChartsQueries chartsQueries, INotificationService notificationService)
        {
            _departmentQueries = departmentQueries;
            _dashboardQueries = dashboardQueries;
            _iTicketNotificationQueries = iTicketNotificationQueries;
            _iTicketNotificationCommand = iTicketNotificationCommand;
            _chartsQueries = chartsQueries;
            _notificationService = notificationService;
        }

        public IActionResult Dashboard()
        {
            var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
            var userDashboardViewModel = new UserDashboardViewModel()
            {
                ListofDepartments = _departmentQueries.GetAssignedDepartmentsofAdministrator(userId)
            };
            return View(userDashboardViewModel);
        }

        public IActionResult GetDepartmentWiseCount(RequestDepartmentWiseReport requestDepartment)
        {
            var user = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
            var listofStatus = _dashboardQueries.GetUserDepartmentWiseAgentManagerTicketCountbyUserId( requestDepartment.DepartmentId);
            return Json(listofStatus);
        }

        public IActionResult GetPieChartData(RequestForCharts requestPieChart)
        {
            var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
            var roleId = HttpContext.Session.GetInt32(AllSessionKeys.RoleId);

            var chartsdata = _chartsQueries.GetCommonPieChartData(userId, requestPieChart.DepartmentId, roleId);

            var ticketcount = (from temp in chartsdata
                            select temp.TotalCount).ToList();

            var status = (from temp in chartsdata
                          select temp.StatusName).ToList();

            var listofcolors = new List<string>();

            var listcolortoshow = new List<string>();

            foreach (var value in chartsdata)
            {
                if (value.StatusId == 1)
                {
                    listcolortoshow.Add("#99ccff");
                }
                else if (value.StatusId == 2)
                {
                    listcolortoshow.Add("#83e7a8");
                }
                else if (value.StatusId == 2)
                {
                    listcolortoshow.Add("#f96987");
                }
                else if (value.StatusId == 4)
                {
                    listcolortoshow.Add("#FFDF7D");
                }
                else if (value.StatusId == 5)
                {
                    listcolortoshow.Add("#83e7a8");
                }
                else if (value.StatusId == 6)
                {
                    listcolortoshow.Add("#43e8d87d");
                }
                else if (value.StatusId == 7)
                {
                    listcolortoshow.Add("#ff7b7b");
                }
                else if (value.StatusId == 8)
                {
                    listcolortoshow.Add("#FF9966");
                }
                else if (value.StatusId == 9)
                {
                    listcolortoshow.Add("#cc7a52");
                }
                else if (value.StatusId == 10)
                {
                    listcolortoshow.Add("#ffe0d1");
                }
                else if (value.StatusId == 11)
                {
                    listcolortoshow.Add("#5848b0");
                }
                else if (value.StatusId == 12)
                {
                    listcolortoshow.Add("#5848b0");
                }
                else if (value.StatusId == 13)
                {
                    listcolortoshow.Add("#9ccc34");
                }
                else if (value.StatusId == 14)
                {
                    listcolortoshow.Add("#ffdc73");
                }
            }

            ViewBag.BugCount_List = string.Join(",", ticketcount);
            ViewBag.Statusname_List = string.Join(", ", status.Select(item => $"\"{item}\""));
            var joined = string.Join(", ", listcolortoshow.Select(item => $"\"{item}\""));
            ViewBag.Color_List = joined;

            var pieRoot = new PieRoot()
            {
                labels = status,
                datasets = new List<PieDataset>()
                {
                    new PieDataset()
                    {
                        backgroundColor = listcolortoshow,
                        borderWidth = 2,
                        data = ticketcount,

                    }
                }
            };

            return Json(pieRoot);
        }

        public IActionResult GetPriorityChartData(RequestForCharts requestProject)
        {
            var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
            var roleId = HttpContext.Session.GetInt32(AllSessionKeys.RoleId);

            var chartsPrioritydata =
                _chartsQueries.CommonPriorityPieChartData(userId, requestProject.DepartmentId, roleId);

            var ticketPrioritycount = (from temp in chartsPrioritydata
                                    select temp.TotalCount).ToList();

            var prioritylist = (from temp in chartsPrioritydata
                                select temp.PriorityName).ToList();


            var listPrioritycolortoshow = new List<string>();

            foreach (var value in chartsPrioritydata)
            {
                if (value.PriorityId == 1)
                {
                    listPrioritycolortoshow.Add("#f07676");
                }

                if (value.PriorityId == 2)
                {
                    listPrioritycolortoshow.Add("#ff8100");
                }

                if (value.PriorityId == 3)
                {
                    listPrioritycolortoshow.Add("#ffd700");
                }

                if (value.PriorityId == 4)
                {
                    listPrioritycolortoshow.Add("#cceaff");
                }
            }

            var pieRoot = new PieRoot()
            {
                labels = prioritylist,
                datasets = new List<PieDataset>()
                {
                    new PieDataset()
                    {
                        backgroundColor = listPrioritycolortoshow,
                        borderWidth = 2,
                        data = ticketPrioritycount,

                    }
                }
            };
            return Json(pieRoot);
        }

        public IActionResult GetBarChartData(RequestForCharts requestProject)
        {
            var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
            var roleId = HttpContext.Session.GetInt32(AllSessionKeys.RoleId);

            var createdticket = _chartsQueries.GetTicketCreatedMonthWiseChartData(userId, requestProject.DepartmentId, roleId);
            var resolvedticket = _chartsQueries.GetTicketResolvedMonthWiseChartData(userId, requestProject.DepartmentId, roleId);

            var listofcreated = new List<int>();

            foreach (var data in createdticket)
            {
                listofcreated.Add(data.January);
                listofcreated.Add(data.February);
                listofcreated.Add(data.March);
                listofcreated.Add(data.April);
                listofcreated.Add(data.May);
                listofcreated.Add(data.June);
                listofcreated.Add(data.July);
                listofcreated.Add(data.August);
                listofcreated.Add(data.September);
                listofcreated.Add(data.October);
                listofcreated.Add(data.November);
                listofcreated.Add(data.December);
            }


            var listofresolved = new List<int>();
            foreach (var data in resolvedticket)
            {
                listofresolved.Add(data.January);
                listofresolved.Add(data.February);
                listofresolved.Add(data.March);
                listofresolved.Add(data.April);
                listofresolved.Add(data.May);
                listofresolved.Add(data.June);
                listofresolved.Add(data.July);
                listofresolved.Add(data.August);
                listofresolved.Add(data.September);
                listofresolved.Add(data.October);
                listofresolved.Add(data.November);
                listofresolved.Add(data.December);
            }

            var Root = new BarCharDataViewModel.Root
            {
                labels = new List<string>()
                {
                    "January",
                    "February",
                    "March",
                    "April",
                    "May",
                    "June",
                    "July",
                    "August",
                    "September",
                    "October",
                    "November",
                    "December"
                },
                datasets = new List<BarCharDataViewModel.Dataset>()
                {

                    new BarCharDataViewModel.Dataset()
                    {
                        label = "Resolved Ticket",
                        backgroundColor = "#83e7a8",
                        borderColor = "rgb(54, 162, 235)",
                        pointRadius = false,
                        pointColor = "rgba(54, 162, 235, 0.2)",
                        pointStrokeColor = "#c1c7d1",
                        pointHighlightFill = "#fff",
                        pointHighlightStroke = "rgba(54, 162, 235, 0.2)",
                        data = listofresolved
                    },
                    new BarCharDataViewModel.Dataset()
                    {
                        label = "Created Ticket",
                        backgroundColor = "#99ccff",
                        borderColor = "rgb(75, 192, 192)",
                        pointRadius = false,
                        pointColor = "#3b8bba",
                        pointStrokeColor = "rgba(75, 192, 192, 0.2)",
                        pointHighlightFill = "#fff",
                        pointHighlightStroke = "rgba(75, 192, 192, 0.2)",
                        data = listofcreated
                    }
                }
            };


            return Json(Root);
        }

        public IActionResult GetBarChartTicketVolumeData(RequestForCharts requestProject)
        {
            var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
            var roleId = HttpContext.Session.GetInt32(AllSessionKeys.RoleId);

            var createdticket = _chartsQueries.GetTicketCreatedMonthWiseChartData(userId, requestProject.DepartmentId, roleId);

            var listofcreated = new List<int>();

            foreach (var data in createdticket)
            {
                listofcreated.Add(data.January);
                listofcreated.Add(data.February);
                listofcreated.Add(data.March);
                listofcreated.Add(data.April);
                listofcreated.Add(data.May);
                listofcreated.Add(data.June);
                listofcreated.Add(data.July);
                listofcreated.Add(data.August);
                listofcreated.Add(data.September);
                listofcreated.Add(data.October);
                listofcreated.Add(data.November);
                listofcreated.Add(data.December);
            }



            var Root = new BarCharDataViewModel.Root
            {
                labels = new List<string>()
                {
                    "January",
                    "February",
                    "March",
                    "April",
                    "May",
                    "June",
                    "July",
                    "August",
                    "September",
                    "October",
                    "November",
                    "December"
                },
                datasets = new List<BarCharDataViewModel.Dataset>()
                {
                    new BarCharDataViewModel.Dataset()
                    {
                        label = "Tickets",
                        backgroundColor = "#99ccff",
                        borderColor = "rgb(75, 192, 192)",
                        pointRadius = false,
                        pointColor = "#3b8bba",
                        pointStrokeColor = "rgba(75, 192, 192, 0.2)",
                        pointHighlightFill = "#fff",
                        pointHighlightStroke = "rgba(75, 192, 192, 0.2)",
                        data = listofcreated
                    }
                }
            };


            return Json(Root);
        }

        public IActionResult GetAgentTeamTicketCount(RequestForCharts requestdata)
        {
            var dataCount = _chartsQueries.GetAgentsTeamsticketsDetailCount(requestdata.DepartmentId);
            return PartialView("_CommonStatusTables", dataCount);
        }

        public IActionResult GetStarAgentCount(RequestForCharts requestdata)
        {
            var dataCount = _dashboardQueries.GetStartAgents(requestdata.DepartmentId);
            return PartialView("_CommonStar", dataCount);
        }

        public IActionResult GetResolvedTicketAgentWise(RequestForCharts requestdata)
        {
            var dataCount = _chartsQueries.GetResolvedTicketAgentWiseCount(requestdata.DepartmentId);
            return PartialView("_ResponseResolvedTable", dataCount);
        }

        public IActionResult GetOpenTicketAgentWise(RequestForCharts requestdata)
        {
            var dataCount = _chartsQueries.GetOpenTicketAgentWiseCount(requestdata.DepartmentId);
            return PartialView("_ResponseResolvedTable", dataCount);
        }

        [HttpPost]
        public IActionResult Search(UserDashboardViewModel userDashboard)
        {
            if (userDashboard.TicketIdSearch != null)
            {
                return RedirectToAction("TicketDetailsView", "MyTicketDetails", new { id = userDashboard.TicketIdSearch });
            }
            else
            {
                _notificationService.DangerNotification("Message", "Something Went Wrong Please try to refesh page and try Again.");
                return RedirectToAction("Dashboard", "MyDashboard");
            }

        }
    }
}
