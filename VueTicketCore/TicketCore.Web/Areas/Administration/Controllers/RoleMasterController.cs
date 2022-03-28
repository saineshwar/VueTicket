using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TicketCore.Common;
using TicketCore.Data.Rolemasters.Command;
using TicketCore.Data.Rolemasters.Queries;
using TicketCore.Models.Rolemasters;
using TicketCore.ViewModels.Rolemasters;
using TicketCore.Web.Filters;
using TicketCore.Web.Messages;

namespace TicketCore.Web.Areas.Administration.Controllers
{
    [SessionTimeOut]
    [AuthorizeSuperAdmin]
    [Area("Administration")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class RoleMasterController : Controller
    {
        private readonly IRoleCommand _roleCommand;
        private readonly IRoleCommand _roleCommandRepository;
        private readonly IRoleQueries _roleQueries;
        private readonly INotificationService _notificationService;
        public RoleMasterController(IRoleCommand roleCommand,
            IRoleCommand roleCommandRepository, IRoleQueries roleQueries, INotificationService notificationService)
        {
            _roleCommand = roleCommand;
            _roleCommandRepository = roleCommandRepository;
            _roleQueries = roleQueries;
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            RoleMasterViewModel roleMasterVm = new RoleMasterViewModel()
            {
                Status = true
            };
            return View(roleMasterVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoleMasterViewModel roleMasterVm)
        {
            if (ModelState.IsValid)
            {
                RoleMaster roleMaster = new RoleMaster()
                {
                    RoleName = roleMasterVm.RoleName,
                    Status = roleMasterVm.Status,
                    CreatedOn = DateTime.Now,
                    RoleId = 0,
                    CreatedBy = Convert.ToInt32(HttpContext.Session.GetString(AllSessionKeys.UserId))
                };

                var result = _roleCommand.Add(roleMaster);
                if (result > 0)
                {
                    _notificationService.SuccessNotification("Message", "The Role was added successfully!");
                    return RedirectToAction("Index", "RoleMaster");
                }
            }

            return View(roleMasterVm);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GridAllRoles()
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
                var rolesdata = _roleQueries.ShowAllRoleMaster(sortColumn, sortColumnDirection, searchValue);
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

        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index", "RoleMaster");
                }
                var roleMaster = _roleQueries.GetRoleMasterForEditByroleId(id);
                return View(roleMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditRoleMasterViewModel roleMasterVm)
        {
            if (ModelState.IsValid)
            {
                var roleMaster = _roleQueries.GetRoleMasterByroleId(roleMasterVm.RoleId);

                if (roleMasterVm.RoleName != roleMaster.RoleName)
                {
                    if (_roleQueries.CheckRoleNameExists(roleMasterVm.RoleName))
                    {
                        ModelState.AddModelError("", "RoleName Already exists");
                        return View(roleMasterVm);
                    }
                }

                roleMaster.Status = roleMasterVm.Status;
                roleMaster.RoleName = roleMasterVm.RoleName;
                roleMaster.ModifiedOn = DateTime.Now;
                roleMaster.ModifiedBy = Convert.ToInt32(HttpContext.Session.GetString(AllSessionKeys.UserId));

                var result = _roleCommand.Update(roleMaster);
                if (result > 0)
                {
                    _notificationService.SuccessNotification("Message", "The Role was Updated successfully!");
                    return RedirectToAction("Index", "RoleMaster");
                }
            }

            return View(roleMasterVm);
        }

        public JsonResult DeleteRole(RequestDeleteRole requestrole)
        {
            try
            {
                var roleMaster = _roleQueries.GetRoleMasterByroleId(requestrole.RoleId);
                var result = _roleCommandRepository.Delete(roleMaster);
                if (result > 0)
                {
                    _notificationService.SuccessNotification("Message", "The Role was Deleted successfully!");
                    return Json(new { Result = "success" });
                }
                else
                {
                    return Json(new { Result = "failed", Message = "Cannot Delete" });
                }
            }
            catch (Exception)
            {
                return Json(new { Result = "failed", Message = "Cannot Delete" });
            }
        }
    }
}
