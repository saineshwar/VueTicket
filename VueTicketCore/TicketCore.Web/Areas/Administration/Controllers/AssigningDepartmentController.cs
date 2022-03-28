using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TicketCore.Common;
using TicketCore.Data.BusinessHours.Queries;
using TicketCore.Data.Department.Command;
using TicketCore.Data.Department.Queries;
using TicketCore.Data.Usermaster.Queries;
using TicketCore.Models.CategoryConfigrations;
using TicketCore.ViewModels.CategoryConfigrations;
using TicketCore.Web.Extensions;
using TicketCore.Web.Filters;
using TicketCore.Web.Messages;

namespace TicketCore.Web.Areas.Administration.Controllers
{
    [SessionTimeOut]
    [AuthorizeSuperAdmin]
    [Area("Administration")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class AssigningDepartmentController : Controller
    {
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly IBusinessHoursQueries _businessHoursQueries;
        private readonly IDepartmentQueries _departmentQueries;
        private readonly IDepartmentConfigrationCommand _departmentConfigrationCommand;
        private readonly IDepartmentConfigrationQueries _categoryConfigrationQueries;
        private readonly INotificationService _notificationService;
        public AssigningDepartmentController(IUserMasterQueries userMasterQueries,
            IBusinessHoursQueries businessHoursQueries,
            IDepartmentQueries departmentQueries,
            IDepartmentConfigrationCommand departmentConfigrationCommand,
            IDepartmentConfigrationQueries categoryConfigrationQueries,
            INotificationService notificationService)
        {
            _userMasterQueries = userMasterQueries;
            _businessHoursQueries = businessHoursQueries;
            _departmentQueries = departmentQueries;
            _departmentConfigrationCommand = departmentConfigrationCommand;
            _categoryConfigrationQueries = categoryConfigrationQueries;
            _notificationService = notificationService;
        }

        public IActionResult Assign()
        {
            var categoryConfigrationViewModel = new DepartmentConfigrationViewModel()
            {
                BusinessHoursId = 0,
                AgentAdminUserId = 0,
                ListofAdmin = _userMasterQueries.GetListofAgentsAdmin(),
                ListofHod = _userMasterQueries.GetListofHod(),
                ListofBusinessHours = _businessHoursQueries.ListofBusinessHours(),
                DepartmentId = 0,
                ListofDepartment = _departmentQueries.GetAllActiveSelectListItemDepartment(),
                Status = true
            };
            return View(categoryConfigrationViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assign(DepartmentConfigrationViewModel ccViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_categoryConfigrationQueries.CheckDuplicateDepartmentConfigration(ccViewModel.AgentAdminUserId,
                    ccViewModel.HodUserId, ccViewModel.DepartmentId))
                {
                    _notificationService.DangerNotification("Message", "Already Department is Assigned to User");
                }
                else
                {
                    var categoryConfigration = new DepartmentConfigration()
                    {
                        BusinessHoursId = ccViewModel.BusinessHoursId,
                        DepartmentId = ccViewModel.DepartmentId,
                        Status = ccViewModel.Status,
                        AgentAdminUserId = ccViewModel.AgentAdminUserId,
                        HodUserId = ccViewModel.HodUserId,
                        DepartmentConfigrationId = 0,
                        CreatedBy = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId),
                        CreatedOn = DateTime.Now
                    };

                    var result = _departmentConfigrationCommand.Add(categoryConfigration);

                    if (result > 0)
                    {
                        _notificationService.SuccessNotification("Message", "Department Configration Saved Successfully");
                    }
                }
            }

            ccViewModel.ListofAdmin = _userMasterQueries.GetListofAgentsAdmin();
            ccViewModel.ListofBusinessHours = _businessHoursQueries.ListofBusinessHours();
            ccViewModel.ListofDepartment = _departmentQueries.GetAllActiveSelectListItemDepartment();
            ccViewModel.ListofHod = _userMasterQueries.GetListofHod();
            return View(ccViewModel);
        }

        [HttpGet]
        public ActionResult EditAssigned(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var data = _categoryConfigrationQueries.GetDepartmentConfigration(id.Value);

            if (data == null)
            {
                return RedirectToAction("Index");
            }

            var categoryConfigrationViewModel = new EditDepartmentConfigrationViewModel();
            categoryConfigrationViewModel.DepartmentConfigrationId = data.DepartmentConfigrationId;
            categoryConfigrationViewModel.BusinessHoursId = data.BusinessHoursId;
            categoryConfigrationViewModel.AgentAdminUserId = data.AgentAdminUserId;
            categoryConfigrationViewModel.DepartmentId = data.DepartmentId;
            categoryConfigrationViewModel.ListofAdmin = _userMasterQueries.GetListofAgentsAdmin();
            categoryConfigrationViewModel.ListofBusinessHours = _businessHoursQueries.ListofBusinessHours();
            categoryConfigrationViewModel.ListofDepartment = _departmentQueries.GetAllActiveSelectListItemDepartment();
            categoryConfigrationViewModel.Status = data.Status;
            categoryConfigrationViewModel.HodUserId = data.HodUserId;
            categoryConfigrationViewModel.ListofHod = _userMasterQueries.GetListofHod();
         
            return View(categoryConfigrationViewModel);
        }



        [HttpPost]
        public ActionResult EditAssigned(EditDepartmentConfigrationViewModel editCategory)
        {
            var categoryConfigration = new DepartmentConfigration()
            {
                BusinessHoursId = editCategory.BusinessHoursId,
                AgentAdminUserId = editCategory.AgentAdminUserId,
                Modifiedon = DateTime.Now,
                DepartmentId = editCategory.DepartmentId,
                DepartmentConfigrationId = editCategory.DepartmentConfigrationId,
                Status = editCategory.Status,
                HodUserId = editCategory.HodUserId,
                ModifiedBy = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId)
            };

            editCategory.ListofAdmin = _userMasterQueries.GetListofAgentsAdmin();
            editCategory.ListofBusinessHours = _businessHoursQueries.ListofBusinessHours();
            editCategory.ListofDepartment = _departmentQueries.GetAllActiveSelectListItemDepartment();
            editCategory.ListofHod = _userMasterQueries.GetListofHod();
            ;
            var result = _departmentConfigrationCommand.Update(categoryConfigration);
            if (result > 0)
            {
                _notificationService.SuccessNotification("Message", "Department Configration Updated Successfully");
            }
            return View(editCategory);
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
                var configrationList = _categoryConfigrationQueries.GetDepartmentConfigrationList(sortColumn, sortColumnDirection, searchValue);
                recordsTotal = configrationList.Count();
                var data = configrationList.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult Delete(int? categoryConfigrationId)
        {
            try
            {
                if (categoryConfigrationId == null)
                {
                    return Json(new { Result = "failed", Message = "Something Went Wrong Refresh Page and try again!" });
                }
                else
                {
                    var categoryConfigration = _categoryConfigrationQueries.GetDepartmentConfigration(categoryConfigrationId.Value);
                    categoryConfigration.Status = !categoryConfigration.Status;
                    categoryConfigration.ModifiedBy = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                    var result = _departmentConfigrationCommand.Update(categoryConfigration);
                    if (result > 0)
                    {
                        return Json(new { Result = "success" });
                    }
                    else
                    {
                        return Json(new { Result = "failed", Message = "Something Went Wrong Refresh Page and try again!" });
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { Result = "failed", Message = "Cannot Delete" });
            }
        }
    }
}
