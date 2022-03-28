using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TicketCore.Common;
using TicketCore.Web.Helpers;

namespace TicketCore.Web.Filters
{
    public class OnboardingCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetString("OnboardingUser"))))
            {
                var onboardingValue = Convert.ToString(context.HttpContext.Session.GetString("OnboardingUser"));

                if (onboardingValue == "1")
                {
                    context.Result = new RedirectResult("/User/Onboarding/Process/1");
                }

            }
        }
    }
}