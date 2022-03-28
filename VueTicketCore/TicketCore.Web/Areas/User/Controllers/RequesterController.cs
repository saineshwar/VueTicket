using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using TicketCore.Common;
using TicketCore.Data.Rolemasters.Queries;
using TicketCore.Data.Usermaster.Command;
using TicketCore.Data.Usermaster.Queries;
using TicketCore.Models.Usermaster;
using TicketCore.ViewModels.Usermaster;
using TicketCore.Web.Extensions;
using TicketCore.Web.Filters;
using TicketCore.Web.Helpers;
using TicketCore.Web.Messages;

namespace TicketCore.Web.Areas.User.Controllers
{
    [SessionTimeOut]
    [AuthorizeAgent]
    [Area("User")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class RequesterController : Controller
    {
        private readonly IRoleQueries _roleQueries;
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly IMapper _mapper;
        private readonly IUserMasterCommand _userMasterCommand;
        private readonly INotificationService _notificationService;
        public RequesterController(IUserMasterQueries userMasterQueries, 
            IMapper mapper, 
            IUserMasterCommand userMasterCommand, 
            INotificationService notificationService,
            IRoleQueries roleQueries)
        {
            _userMasterQueries = userMasterQueries;
            _mapper = mapper;
            _userMasterCommand = userMasterCommand;
            _notificationService = notificationService;
            _roleQueries = roleQueries;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(RequesterUserViewModel requesterUserView)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_userMasterQueries.CheckEmailIdExists(requesterUserView.EmailId))
                    {
                        _notificationService.DangerNotification("Message", "EmailId Already Exists");
                    }
                    else if (_userMasterQueries.CheckMobileNoExists(requesterUserView.MobileNo))
                    {
                        _notificationService.DangerNotification("Message", "MobileNo Already Exists");
                    }
                    else
                    {
                        var usermaster = new UserMaster
                        {
                            UserId = 0,
                            FirstName = UppercaseFirstHelper.UppercaseFirst(requesterUserView.FirstName),
                            LastName = UppercaseFirstHelper.UppercaseFirst(requesterUserView.LastName),
                            Status = true,
                            CreatedOn = DateTime.Now,
                            CreatedBy = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId),
                            PasswordHash = GeneratePassword(requesterUserView),
                            EmailId = requesterUserView.EmailId,
                            MobileNo = requesterUserView.MobileNo,
                            Gender = requesterUserView.Gender,
                            UserName = GenerateUserName(requesterUserView),
                            IsFirstLogin = true
                        };

                        var userId = _userMasterCommand.AddUser(usermaster, (int)RolesHelper.Roles.User);
                        if (userId != -1)
                        {
                            _notificationService.SuccessNotification("Message", "Requester Created Successfully");
                        }

                        return RedirectToAction("Create", "Requester");

                    }

                    return View("Create", requesterUserView);
                }
                else
                {

                    return View("Create", requesterUserView);
                }
            }
            catch
            {
                throw;
            }
        }

        private string GenerateUserName(RequesterUserViewModel createUserViewModel)
        {
            try
            {
                string mobileno = createUserViewModel.MobileNo;
                string username = string.Concat(createUserViewModel.FirstName, mobileno.Substring(6, 4)).Trim();
                if (_userMasterQueries.CheckUsernameExists(username))
                {
                    username = string.Concat(createUserViewModel.LastName, mobileno.Substring(6, 4)).Trim();
                    if (_userMasterQueries.CheckUsernameExists(username))
                    {
                        username = string.Concat(createUserViewModel.FirstName, mobileno.Substring(3, 6)).Trim();
                        if (_userMasterQueries.CheckUsernameExists(username))
                        {

                        }
                    }
                }
                return username;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GeneratePassword(RequesterUserViewModel createUserViewModel)
        {
            try
            {
                string onecharfirstName = createUserViewModel.FirstName.Substring(0, 1);
                string onecharlastName = createUserViewModel.LastName.Substring(0, 1);
                string mobileno = createUserViewModel.MobileNo.Substring(5, 5);
                string password = string.Concat("tkts", onecharfirstName.ToLower(), onecharlastName.ToLower(), mobileno);
                var saltedpassword = GenerateHashSha256.ComputeSha256Hash(password);
                return saltedpassword;
            }
            catch (Exception)
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

                        createUserViewModel.FirstName = UppercaseFirstHelper.UppercaseFirst(editUserViewModel.FirstName);
                        createUserViewModel.LastName = UppercaseFirstHelper.UppercaseFirst(editUserViewModel.LastName);
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
    }
}
