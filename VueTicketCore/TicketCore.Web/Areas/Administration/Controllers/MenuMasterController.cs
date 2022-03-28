using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketCore.Common;
using TicketCore.Data.MenuCategorys.Queries;
using TicketCore.Data.MenuMasters.Command;
using TicketCore.Data.MenuMasters.Queries;
using TicketCore.Data.Rolemasters.Queries;
using TicketCore.Models.Menus;
using TicketCore.ViewModels.MenuCategorys;
using TicketCore.ViewModels.Menus;
using TicketCore.Web.Extensions;
using TicketCore.Web.Filters;
using TicketCore.Web.Messages;


namespace TicketCore.Web.Areas.Administration.Controllers
{
    [SessionTimeOut]
    [AuthorizeSuperAdmin]
    [Area("Administration")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class MenuMasterController : Controller
    {
        private readonly IRoleQueries _roleQueries;
        private readonly IMenuCategoryQueries _menuCategoryQueries;
        private readonly IMenuMasterQueries _iMenuMasterQueries;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IMenuMasterCommand _menuMasterCommand;
        public MenuMasterController(IRoleQueries roleQueries,

            IMenuCategoryQueries menuCategoryQueries,
            IMenuMasterQueries iMenuMasterQueries,
            IMapper mapper, INotificationService notificationService, IMenuMasterCommand menuMasterCommand)
        {
            _roleQueries = roleQueries;
            _menuCategoryQueries = menuCategoryQueries;
            _iMenuMasterQueries = iMenuMasterQueries;
            _mapper = mapper;
            _notificationService = notificationService;
            _menuMasterCommand = menuMasterCommand;
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateMenuMasterViewModel addMenumaster = new CreateMenuMasterViewModel()
            {
                ListofRoles = _roleQueries.ListofRoles(),
                Status = true,
                ListofMenuCategory = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    }
                }
            };
            return View(addMenumaster);
        }

        [HttpPost]
        public IActionResult Create(CreateMenuMasterViewModel createMenu)
        {
            try
            {
                createMenu.ListofRoles = _roleQueries.ListofRoles();
                createMenu.ListofMenuCategory = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    }
                };

                if (ModelState.IsValid)
                {
                    if (_iMenuMasterQueries.CheckMenuExists(createMenu.MenuName, createMenu.RoleId, createMenu.MenuCategoryId))
                    {
                        ModelState.AddModelError("", "Menu Name Already Exists");
                        return View(createMenu);
                    }

                    var mappedobject = _mapper.Map<MenuMaster>(createMenu);
                    mappedobject.CreatedOn = DateTime.Now;
                    mappedobject.CreatedBy = Convert.ToInt64(HttpContext.Session.GetSession<long>(AllSessionKeys.UserId));
                    var result = _menuMasterCommand.Add(mappedobject);
                    if (result > 0)
                    {
                        _notificationService.SuccessNotification("Message", "Menu was added Successfully!");
                        return RedirectToAction("Index", "MenuMaster");
                    }

                }

                return View(createMenu);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GridAllMenu()
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
                var records = _iMenuMasterQueries.ShowAllMenus(sortColumn, sortColumnDirection, searchValue);
                recordsTotal = records.Count();
                var data = records.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult GetCategory(RequestForMenuCategory requestCategory)
        {
            var listofCategory = _menuCategoryQueries.GetCategorybyRoleId(requestCategory.RoleID);
            return Json(listofCategory);
        }

        public IActionResult Edit(int? id)
        {
            try
            {
                var menuMaster = _iMenuMasterQueries.GetMenuByMenuId(id);
                menuMaster.ListofRoles = _roleQueries.ListofRoles();
                menuMaster.ListofMenuCategory = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    }
                };
                return View(menuMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditMenuMasterViewModel editmenuMaster)
        {
            try
            {
                editmenuMaster.ListofRoles = _roleQueries.ListofRoles();
                editmenuMaster.ListofMenuCategory = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    }
                };

                if (ModelState.IsValid)
                {
                    MenuMaster menuMaster = new MenuMaster()
                    {
                        ModifiedBy = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId),
                        MenuName = editmenuMaster.MenuName,
                        Status = editmenuMaster.Status,
                        ActionMethod = editmenuMaster.ActionMethod,
                        ControllerName = editmenuMaster.ControllerName,
                        MenuId = editmenuMaster.MenuId,
                        MenuCategoryId = editmenuMaster.MenuCategoryId,
                        RoleId = editmenuMaster.RoleId,
                        ModifiedOn = DateTime.Now,
                        Area = editmenuMaster.Area
                    };

                    if (_iMenuMasterQueries.EditValidationCheck(editmenuMaster.MenuId, editmenuMaster))
                    {
                        _menuMasterCommand.Update(menuMaster);
                        _notificationService.SuccessNotification("Message", "Menu was updated Successfully!");
                        return RedirectToAction("Index", "MenuMaster");
                    }
                    else if (_iMenuMasterQueries.CheckMenuExists(editmenuMaster.MenuName, editmenuMaster.RoleId, editmenuMaster.MenuCategoryId))
                    {

                        ModelState.AddModelError("", "Menu Name Already Exists");
                        editmenuMaster.ListofRoles = _roleQueries.ListofRoles();
                        editmenuMaster.ListofMenuCategory = new List<SelectListItem>()
                        {
                            new SelectListItem()
                            {
                                Value = "",
                                Text = "-----Select-----"
                            }
                        };
                        return View(editmenuMaster);
                    }
                    else
                    {
                        _menuMasterCommand.Update(menuMaster);
                        _notificationService.SuccessNotification("Message", "Menu was updated Successfully!");
                        return RedirectToAction("Index", "MenuMaster");
                    }

                }

                return View(editmenuMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult DeleteMenu(RequestDeleteMenuMaster requestDeleteMenu)
        {
            try
            {
                var data = _iMenuMasterQueries.GetMenuMasterByMenuId(requestDeleteMenu.MenuId);
                var result = _menuMasterCommand.Delete(data);
                if (result > 0)
                {
                    _notificationService.SuccessNotification("Message", "The Menu Deleted successfully!");
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
