using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using TicketCore.Common;
using TicketCore.Web.Extensions;

namespace TicketCore.Web.Areas.AgentManager.Controllers
{
    [Area("AgentManager")]
    public class CultureController : Controller
    {
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            HttpContext.Session.SetString(AllSessionKeys.Culture, culture);

            return LocalRedirect(returnUrl);
        }
    }
}
