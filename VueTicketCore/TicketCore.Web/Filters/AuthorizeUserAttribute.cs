using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TicketCore.Common;
using TicketCore.Web.Helpers;

namespace TicketCore.Web.Filters
{
    /// <summary>
    /// User
    /// </summary>
    public class AuthorizeUserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));

                if (roleValue != Convert.ToInt32(RolesHelper.Roles.User))
                {
                    if (context.Controller is Controller controller)
                    {
                        controller.ViewData["ErrorMessage"] = "Invalid User";
                        controller.HttpContext.Session.Clear();
                    }

                    context.Result = new RedirectResult("/Error/Error");
                }

            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectResult("/Error/Error");

            }
        }
    }

    /// <summary>
    /// Agent
    /// </summary>
    public class AuthorizeAgentAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));

                if (roleValue != Convert.ToInt32(RolesHelper.Roles.Agent))
                {
                    if (context.Controller is Controller controller)
                    {
                        controller.ViewData["ErrorMessage"] = "Invalid User";
                        controller.HttpContext.Session.Clear();
                    }

                    context.Result = new RedirectResult("/Error/Error");
                }

            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectResult("/Error/Error");

            }
        }
    }


    /// <summary>
    /// Agent Admin
    /// </summary>
    public class AuthorizeAgentAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));

                if (roleValue != Convert.ToInt32(RolesHelper.Roles.AgentAdmin))
                {
                    if (context.Controller is Controller controller)
                    {
                        controller.ViewData["ErrorMessage"] = "Invalid User";
                        controller.HttpContext.Session.Clear();
                    }

                    context.Result = new RedirectResult("/Error/Error");
                }

            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectResult("/Error/Error");

            }
        }
    }


    /// <summary>
    /// Administrator
    /// </summary>
    public class AuthorizeAdministratorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));

                if (roleValue != Convert.ToInt32(RolesHelper.Roles.Hod))
                {
                    if (context.Controller is Controller controller)
                    {
                        controller.ViewData["ErrorMessage"] = "Invalid User";
                        controller.HttpContext.Session.Clear();
                    }

                    context.Result = new RedirectResult("/Error/Error");
                }

            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectResult("/Error/Error");

            }
        }
    }

    /// <summary>
    /// SuperAdmin
    /// </summary>
    public class AuthorizeSuperAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));

                if (roleValue != Convert.ToInt32(RolesHelper.Roles.SuperAdmin))
                {
                    if (context.Controller is Controller controller)
                    {
                        controller.ViewData["ErrorMessage"] = "Invalid User";
                        controller.HttpContext.Session.Clear();
                    }

                    context.Result = new RedirectResult("/Error/Error");
                }

            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectResult("/Error/Error");

            }
        }
    }


    public class ValidateOnboardingAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));
                var onboardingUser = Convert.ToString(context.HttpContext.Session.GetString("OnboardingUser"));


                if (roleValue != Convert.ToInt32(RolesHelper.Roles.User))
                {
                    if (context.Controller is Controller controller)
                    {
                        controller.ViewData["ErrorMessage"] = "Invalid User";
                        controller.HttpContext.Session.Clear();
                    }

                    context.Result = new RedirectResult("/Error/Error");
                }

                if (onboardingUser != "1")
                {
                    if (context.Controller is Controller controller)
                    {
                        controller.ViewData["ErrorMessage"] = "Invalid User";
                        controller.HttpContext.Session.Clear();
                    }

                    context.Result = new RedirectResult("/Error/Error");
                }

            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectResult("/Error/Error");

            }
        }
    }

    public class AuthorizeCommonAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));

                if (roleValue == Convert.ToInt32(RolesHelper.Roles.SuperAdmin) && roleValue == Convert.ToInt32(RolesHelper.Roles.User)
                                                                               && roleValue == Convert.ToInt32(RolesHelper.Roles.Hod)
                                                                               && roleValue == Convert.ToInt32(RolesHelper.Roles.Agent)
                                                                               && roleValue == Convert.ToInt32(RolesHelper.Roles.AgentAdmin))
                {
                    if (context.Controller is Controller controller)
                    {
                        controller.ViewData["ErrorMessage"] = "Invalid User";
                        controller.HttpContext.Session.Clear();
                    }

                    context.Result = new RedirectResult("/Error/Error");
                }

            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectResult("/Error/Error");

            }
        }
    }

}