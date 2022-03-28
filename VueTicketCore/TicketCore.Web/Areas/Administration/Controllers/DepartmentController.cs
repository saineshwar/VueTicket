using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketCore.Common;
using TicketCore.Data.Department.Command;
using TicketCore.Data.Department.Queries;
using TicketCore.Models.Department;
using TicketCore.Web.Extensions;
using TicketCore.Web.Filters;
using TicketCore.Web.Messages;


namespace TicketCore.Web.Areas.Administration.Controllers
{
    [SessionTimeOut]
    [AuthorizeSuperAdmin]
    [Area("Administration")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentQueries _departmentQueries;
        private readonly IDepartmentCommand _departmentCommand;
        private readonly INotificationService _notificationService;
        public DepartmentController(IDepartmentQueries departmentQueries, IDepartmentCommand departmentCommand, INotificationService notificationService)
        {
            _departmentQueries = departmentQueries;
            _departmentCommand = departmentCommand;
            _notificationService = notificationService;
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Departments departments)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_departmentQueries.CheckDepartmentNameExists(departments.DepartmentName))
                    {
                        ModelState.AddModelError("", CommonMessages.DepartmentAlreadyExistsMessages);
                        return View(departments);
                    }
                    else
                    {
                        departments.CreatedOn = DateTime.Now;
                        departments.UserId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                        _departmentCommand.Add(departments);
                        _notificationService.SuccessNotification("Message", CommonMessages.DepartmentSuccessMessages);
                        return RedirectToAction("Create", "Department");
                    }
                }

                return View(departments);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var editdata = _departmentQueries.GetDepartmentById(id);
                return View(editdata);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Edit(int departmentId, Departments departments)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    departments.CreatedOn = DateTime.Now;
                    departments.UserId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                    _departmentCommand.Update(departments);
                    _notificationService.SuccessNotification("Message", CommonMessages.DepartmentSuccessMessages);
                    return RedirectToAction("Edit", "Department", new { id = departmentId });

                }

                return View(departments);
            }
            catch
            {
                return View();
            }
        }

        public JsonResult CheckDepartmentName(string departmentName)
        {
            try
            {
                if (!string.IsNullOrEmpty(departmentName))
                {
                    var result = _departmentQueries.CheckDepartmentNameExists(departmentName);
                    return Json(result);
                }

                return Json("");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult Delete(int departmentId)
        {
            try
            {
                var result = _departmentCommand.Delete(departmentId);
                return Json(new { Result = "OK" });
            }
            catch (Exception)
            {
                return Json(new { Result = "ERROR", Message = "Cannot Delete" });
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GridAllDepartment()
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
                var rolesdata = _departmentQueries.ShowAllDepartment(sortColumn, sortColumnDirection, searchValue);
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
    }
}
