using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TicketCore.Web.Views.Shared.Components.Sidebar
{
    public class SidebarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}
