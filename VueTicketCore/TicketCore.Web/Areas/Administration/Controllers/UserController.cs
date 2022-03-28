using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using TicketCore.Common;
using TicketCore.Models.Usermaster;
using TicketCore.Web.Extensions;
using TicketCore.Web.Helpers;
using TicketCore.Data.Rolemasters.Queries;
using TicketCore.Data.Usermaster.Command;
using TicketCore.Data.Usermaster.Queries;
using TicketCore.ViewModels.Usermaster;
using TicketCore.Web.Filters;
using TicketCore.Web.Messages;

namespace TicketCore.Web.Areas.Administration.Controllers
{
    [SessionTimeOut]
    [AuthorizeSuperAdmin]
    [Area("Administration")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class UserController : Controller
    {
        private readonly IRoleQueries _roleQueries;
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly IMapper _mapper;
        private readonly IUserMasterCommand _userMasterCommand;
        private readonly INotificationService _notificationService;
        public UserController(IRoleQueries roleQueries,
            IUserMasterQueries userMasterQueries,
            IMapper mapper,
            IUserMasterCommand userMasterCommand, INotificationService notificationService)
        {

            _roleQueries = roleQueries;
            _userMasterQueries = userMasterQueries;
            _mapper = mapper;
            _userMasterCommand = userMasterCommand;
            _notificationService = notificationService;
        }

        public IActionResult Create()
        {
            try
            {
                var createUserViewModel = new CreateUserViewModel()
                {
                    ListofRoles = _roleQueries.GetAllActiveRolesNotAgent(),
                    Status = true
                };
                return View(createUserViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Create(CreateUserViewModel createUserViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_userMasterQueries.CheckEmailIdExists(createUserViewModel.EmailId))
                    {
                        _notificationService.DangerNotification("Message", "EmailId Already Exists");
                    }
                    else if (_userMasterQueries.CheckMobileNoExists(createUserViewModel.MobileNo))
                    {
                        _notificationService.DangerNotification("Message", "MobileNo Already Exists");
                    }
                    else if (_userMasterQueries.CheckUsernameExists(createUserViewModel.UserName))
                    {
                        _notificationService.DangerNotification("Message", "Username Already Exists");
                    }
                    else
                    {

                        if (!string.Equals(createUserViewModel.Password, createUserViewModel.ConfirmPassword,
                                StringComparison.Ordinal))
                        {
                            _notificationService.DangerNotification("Message", "Password Does not Match!");
                            return View(createUserViewModel);
                        }
                        else
                        {
                            createUserViewModel.FirstName =
                                UppercaseFirstHelper.UppercaseFirst(createUserViewModel.FirstName);
                            createUserViewModel.LastName =
                                UppercaseFirstHelper.UppercaseFirst(createUserViewModel.LastName);

                            var usermaster = _mapper.Map<UserMaster>(createUserViewModel);
                            usermaster.Status = true;

                            usermaster.CreatedOn = DateTime.Now;
                            usermaster.UserId = 0;
                            usermaster.CreatedBy = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                            usermaster.PasswordHash = createUserViewModel.Password;

                            var userId = _userMasterCommand.AddUser(usermaster, createUserViewModel.RoleId);
                            if (userId != -1)
                            {
                                _notificationService.SuccessNotification("Message", "User Created Successfully");
                            }

                            return RedirectToAction("Create", "User");
                        }
                    }

                    createUserViewModel.ListofRoles = _roleQueries.GetAllActiveRolesNotAgent();
                    return View("Create", createUserViewModel);
                }
                else
                {
                    createUserViewModel.ListofRoles = _roleQueries.GetAllActiveRolesNotAgent();
                    return View("Create", createUserViewModel);
                }
            }
            catch
            {
                throw;
            }
        }


        public ActionResult Edit(long? id)
        {
            try
            {
                if (id != null)
                {
                    var createUserViewModel = _userMasterQueries.EditUserbyUserId(id);
                    createUserViewModel.ListofRoles = _roleQueries.GetAllActiveRolesNotAgent();
                    return View(createUserViewModel);
                }
                else
                {
                    return RedirectToAction("Index", "User");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        public ActionResult Edit(EditUserViewModel editUserViewModel)
        {
            try
            {
                editUserViewModel.ListofRoles = _roleQueries.GetAllActiveRolesNotAgent();

                if (ModelState.IsValid)
                {

                    var isUser = _userMasterQueries.CheckUserIdExists(editUserViewModel.UserId);
                    if (isUser)
                    {
                        var createUserViewModel = _userMasterQueries.GetUserById(editUserViewModel.UserId);

                        if (createUserViewModel.EmailId != editUserViewModel.EmailId)
                        {
                            if (_userMasterQueries.CheckEmailIdExists(editUserViewModel.EmailId))
                            {
                                _notificationService.DangerNotification("Message", "EmailId Already Exists");
                                return View(editUserViewModel);
                            }
                        }

                        if (createUserViewModel.MobileNo != editUserViewModel.MobileNo)
                        {
                            if (_userMasterQueries.CheckMobileNoExists(editUserViewModel.MobileNo))
                            {
                                _notificationService.DangerNotification("Message", "MobileNo Already Exists");
                                return View(editUserViewModel);
                            }
                        }

                        createUserViewModel.FirstName = UppercaseFirstHelper.UppercaseFirst(createUserViewModel.FirstName);
                        createUserViewModel.LastName = UppercaseFirstHelper.UppercaseFirst(createUserViewModel.LastName);
                        createUserViewModel.EmailId = editUserViewModel.EmailId;
                        createUserViewModel.MobileNo = editUserViewModel.MobileNo;
                        createUserViewModel.Gender = editUserViewModel.Gender;
                        createUserViewModel.Status = editUserViewModel.Status;

                        var assignedrole = _userMasterQueries.GetAssignedRolesByUserId(editUserViewModel.UserId);

                        var result = _userMasterCommand.UpdateUser(createUserViewModel, assignedrole);

                        if (result > 0)
                        {
                            _notificationService.SuccessNotification("Message", "User Details Updated Successfully");
                        }
                        
                        return View(editUserViewModel);
                    }
                    else
                    {

                        _notificationService.DangerNotification("Message", "User Details doesn't exist");
                        return View(editUserViewModel);
                    }
                }
                else
                {
                    return View("Edit", editUserViewModel);
                }
            }
            catch
            {
                throw;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GridAllUser()
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

                var currentrole = HttpContext.Session.GetInt32(AllSessionKeys.RoleId);
                var usersdata = _userMasterQueries.ShowAllUsers(sortColumn, sortColumnDirection, searchValue);
                recordsTotal = usersdata.Count();
                var data = usersdata.Skip(skip).Take(pageSize).ToList();
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
