using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using TicketCore.Common;
using TicketCore.Data.Department.Queries;
using TicketCore.Data.Masters.Queries;
using TicketCore.Data.Reports.Queries;
using TicketCore.Data.Usermaster.Queries;
using TicketCore.ViewModels.Reports;
using TicketCore.ViewModels.Tickets;
using TicketCore.Web.Extensions;
using TicketCore.Web.Filters;
using TicketCore.Web.Messages;

namespace TicketCore.Web.Areas.Administrator.Controllers
{
    [SessionTimeOut]
    [AuthorizeAdministrator]
    [Area("Administrator")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class AllReportController : Controller
    {
        private readonly IReportQueries _reportQueries;
        private readonly ITicketsMastersQueries _ticketsMastersQueries;
        private readonly IPriorityQueries _priorityQueries;
        private readonly IUserMasterQueries _iUserMasterQueries;
        private readonly IDepartmentQueries _departmentQueries;
        private readonly INotificationService _notificationService;

        public AllReportController(IReportQueries reportQueries,
            ITicketsMastersQueries ticketsMastersQueries,
            IPriorityQueries priorityQueries,
            IUserMasterQueries iUserMasterQueries,
            IDepartmentQueries departmentQueries,
            INotificationService notificationService)
        {
            _reportQueries = reportQueries;
            _ticketsMastersQueries = ticketsMastersQueries;
            _priorityQueries = priorityQueries;
            _iUserMasterQueries = iUserMasterQueries;
            _departmentQueries = departmentQueries;
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult Report()
        {
            var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
            var agentAdminReportViewModel = new AgentAdminReportViewModel
            {
                ListofAgent = new List<SelectListItem>(),
                ListofReport = _reportQueries.AgentAdminReportList(),
                ListofOverdueTypes = _ticketsMastersQueries.GetAllActiveOverdueTypes(),
                ListofPriority = _priorityQueries.GetAllPrioritySelectListItem(),
                ListofDepartment = _departmentQueries.GetAssignedDepartmentsofAdministrator(userId)
            };
            return View(agentAdminReportViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Report(AgentAdminReportViewModel reportViewModel)
        {
            if (ModelState.IsValid)
            {
                if (reportViewModel.ReportId == 1)
                {
                    var reportDetailTicketStatusReport = await _reportQueries.GetDetailTicketStatusReport(reportViewModel.Fromdate, reportViewModel.Todate, reportViewModel.AgentId, reportViewModel.DepartmentId);

                    if (reportDetailTicketStatusReport != null && reportDetailTicketStatusReport.Count > 0)
                    {
                        string reportname = $"AgentDetailTicketStatusReport_{Guid.NewGuid():N}.xlsx";
                        var exportbytes = ExporttoExcel(reportDetailTicketStatusReport, reportname);
                        return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    }
                    else
                    {
                        _notificationService.DangerNotification("Alert", "No Data to Export");
                    }
                }
                if (reportViewModel.ReportId == 2)
                {
                    var reportdepartmentWiseTicketStatuReport = await _reportQueries.GetDepartmentWiseTicketStatusReport(reportViewModel.Fromdate, reportViewModel.Todate, reportViewModel.DepartmentId);

                    if (reportdepartmentWiseTicketStatuReport != null && reportdepartmentWiseTicketStatuReport.Count > 0)
                    {
                        string reportname = $"DepartmentWiseTicketStatusReport_{Guid.NewGuid():N}.xlsx";
                        var exportbytes = ExporttoExcel(reportdepartmentWiseTicketStatuReport, reportname);
                        return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    }
                    else
                    {
                        _notificationService.DangerNotification("Alert", "No Data to Export");
                    }
                }

                if (reportViewModel.ReportId == 3)
                {
                    var reportTicketOverduesbydepartmentReport = await _reportQueries.GetTicketOverduesbyDepartmentWiseReport(
                        reportViewModel.Fromdate,
                        reportViewModel.Todate, reportViewModel.OverdueTypeId, reportViewModel.DepartmentId);

                    if (reportTicketOverduesbydepartmentReport != null && reportTicketOverduesbydepartmentReport.Count > 0)
                    {
                        string reportname = $"TicketOverduesReport_{Guid.NewGuid():N}.xlsx";
                        var exportbytes = ExporttoExcel(reportTicketOverduesbydepartmentReport, reportname);
                        return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    }
                    else
                    {
                        _notificationService.DangerNotification("Alert", "No Data to Export");
                    }
                }


                if (reportViewModel.ReportId == 5)
                {
                    var reportEscalationReport = await _reportQueries.GetEscalationbyDepartmentReport(
                        reportViewModel.Fromdate,
                        reportViewModel.Todate, reportViewModel.DepartmentId);

                    if (reportEscalationReport != null && reportEscalationReport.Count > 0)
                    {
                        string reportname = $"EscalationbyCategoryReport_{Guid.NewGuid():N}.xlsx";
                        var exportbytes = ExporttoExcel(reportEscalationReport, reportname);
                        return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    }
                    else
                    {
                        _notificationService.DangerNotification("Alert", "No Data to Export");
                    }
                }

                if (reportViewModel.ReportId == 6)
                {
                    var reportDeletedReport = await _reportQueries.GetDeletedTicketHistoryByDepartmentReport(
                        reportViewModel.Fromdate,
                        reportViewModel.Todate, reportViewModel.DepartmentId);

                    if (reportDeletedReport != null && reportDeletedReport.Count > 0)
                    {
                        string reportname = $"DeletedTicketHistoryByCategoryReport_{Guid.NewGuid():N}.xlsx";
                        var exportbytes = ExporttoExcel(reportDeletedReport, reportname);
                        return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    }
                    else
                    {
                        _notificationService.DangerNotification("Alert", "No Data to Export");
                    }
                }

                if (reportViewModel.ReportId == 7)
                {
                    var reportPriorityReport = await _reportQueries.GetPriorityWiseTicketStatusReport(
                        reportViewModel.Fromdate,
                        reportViewModel.Todate, reportViewModel.PriorityId, reportViewModel.DepartmentId);

                    if (reportPriorityReport != null && reportPriorityReport.Count > 0)
                    {
                        string reportname = $"PriorityWiseTicketStatusReport_{Guid.NewGuid():N}.xlsx";
                        var exportbytes = ExporttoExcel(reportPriorityReport, reportname);
                        return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);

                    }
                    else
                    {
                        _notificationService.DangerNotification("Alert", "No Data to Export");
                    }
                }

                if (reportViewModel.ReportId == 8)
                {
                    var reportUserReport = await _reportQueries.GetUsersDetailsReport(
                        reportViewModel.AgentId
                    );

                    if (reportUserReport != null && reportUserReport.Count > 0)
                    {
                        string reportname = $"UsersDetailsReport_{Guid.NewGuid():N}.xlsx";
                        var exportbytes = ExporttoExcel(reportUserReport, reportname);
                        return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    }
                    else
                    {
                        _notificationService.DangerNotification("Alert", "No Data to Export");
                    }
                }

                if (reportViewModel.ReportId == 9)
                {
                    var checkinCheckOutReport = await _reportQueries.UserWiseCheckinCheckOutReport(
                        reportViewModel.Fromdate,
                        reportViewModel.Todate, reportViewModel.AgentId
                    );

                    if (checkinCheckOutReport != null && checkinCheckOutReport.Count > 0)
                    {
                        string reportname = $"UserWiseCheckinCheckOutReport{Guid.NewGuid():N}.xlsx";
                        var exportbytes = ExporttoExcel(checkinCheckOutReport, reportname);
                        return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    }
                    else
                    {
                        _notificationService.DangerNotification("Alert", "No Data to Export");
                    }
                }

            }


            reportViewModel.ListofAgent = _iUserMasterQueries.GetListofAgents(reportViewModel.DepartmentId);
            reportViewModel.ListofReport = _reportQueries.AgentAdminReportList();
            reportViewModel.ListofOverdueTypes = _ticketsMastersQueries.GetAllActiveOverdueTypes();
            reportViewModel.ListofPriority = _priorityQueries.GetAllPrioritySelectListItem();
            reportViewModel.ListofDepartment = _departmentQueries.GetAllActiveSelectListItemDepartment();

            return View(reportViewModel);
        }

        private byte[] ExporttoExcel<T>(List<T> table, string filename)
        {
            using ExcelPackage pack = new ExcelPackage();
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(filename);
            ws.Cells["A1"].LoadFromCollection(table, true, TableStyles.Light1);
            return pack.GetAsByteArray();
        }

        public IActionResult GetAgentList(UserRequestViewModel userRequestViewModel)
        {
            if (userRequestViewModel.DepartmentId == null)
            {
                var list = new List<SelectListItem>();

                list.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                return Json(list);
            }

            var agentlist = _iUserMasterQueries.GetListofAgents(userRequestViewModel.DepartmentId);
            return Json(new { listofusers = agentlist });
        }
    }
}
