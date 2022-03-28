using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TicketCore.Web.Filters
{
    public class SessionTimeOutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (string.IsNullOrEmpty(filterContext.HttpContext.Session.GetString("Portal.UserName")))
            {
                if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    filterContext.Result = new JsonResult(new
                    {
                        error = true,
                        message = "Your session has timed out; please login again."
                    });
                    filterContext.HttpContext.Response.StatusCode = 440;
                }
                else
                {
                    filterContext.HttpContext.Session.SetString("ErrorMessage",
                        "Your Session has timed out; please login again.");
                    filterContext.Result = new RedirectResult("/Error/SessionOut");
                }
            }


        }
    }
}