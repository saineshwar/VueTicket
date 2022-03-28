using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketCore.Common;
using TicketCore.Data.Usermaster.Command;
using TicketCore.Data.Usermaster.Queries;
using TicketCore.ViewModels.Usermaster;
using TicketCore.Web.Extensions;
using TicketCore.Web.Filters;
using TicketCore.Web.Messages;

namespace TicketCore.Web.Areas.User.Controllers
{
    [SessionTimeOut]
    [AuthorizeUser]
    [ValidateOnboarding]
    [Area("User")]
    public class OnboardingController : Controller
    {
        private readonly IUserMasterQueries _iuserMasterQueries;
        private readonly IUserMasterCommand _iuserMasterCommand;
        private readonly INotificationService _notificationService;
        public OnboardingController(IUserMasterQueries userMasterQueries, IUserMasterCommand userMasterCommand, INotificationService notificationService)
        {
            _iuserMasterQueries = userMasterQueries;
            _iuserMasterCommand = userMasterCommand;
            _notificationService = notificationService;
        }
        [HttpGet]
        public IActionResult Process(int? id)
        {
            if (id != null)
            {
                _notificationService.DangerNotification("Message","Please Complete You Profile to Use Portal.");
            }

            var onboarding = new OnboardingUserViewModel()
            {
                Gender = "M"
            };


            return View(onboarding);
        }


        [HttpPost]
        public IActionResult Process(OnboardingUserViewModel onboardingUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                var userdetails = _iuserMasterQueries.GetUserById(userId);
                userdetails.FirstName = onboardingUserViewModel.FirstName;
                userdetails.LastName = onboardingUserViewModel.LastName;
                userdetails.IsFirstLogin = false;
                var result = _iuserMasterCommand.UpdateUserDetails(userdetails);

                if (result > 0)
                {
                    _notificationService.SuccessNotification("Message","Your Personal Details Updated Successfully.");
                    return RedirectToAction("Dashboard", "Dashboard");
                }
                else
                {
                    _notificationService.DangerNotification("Message", "Something Went Wrong While Updating Personal Details Try Again after Sometime.");
                    return RedirectToAction("Process", "Onboarding");
                }
            }

            return View(onboardingUserViewModel);
        }

        [HttpGet]
        public IActionResult VerifyProcess()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyProcess(OnboardingUserViewModel onboardingUserViewModel)
        {
            if (ModelState.IsValid)
            {

            }

            return View();
        }

    }
}
