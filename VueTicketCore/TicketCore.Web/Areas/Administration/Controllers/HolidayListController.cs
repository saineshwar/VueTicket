using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketCore.Data.Masters.Command;
using TicketCore.Data.Masters.Queries;
using TicketCore.Models.Masters;
using TicketCore.ViewModels.Masters;
using TicketCore.Web.Filters;

namespace TicketCore.Web.Areas.Administration.Controllers
{
    [SessionTimeOut]
    [AuthorizeSuperAdmin]
    [Area("Administration")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class HolidayListController : Controller
    {
        private readonly IHolidayCommand _holidayCommand;
        private readonly IHolidayQueries _holidayQueries;
        public HolidayListController(IHolidayCommand holidayCommand, IHolidayQueries holidayQueries)
        {
            _holidayCommand = holidayCommand;
            _holidayQueries = holidayQueries;
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(HolidayViewModel holidayViewModel)
        {
            if (ModelState.IsValid)
            {
                var list = new HolidayListModel();
                list.HolidayName = holidayViewModel.HolidayName;
                list.HolidayId = 0;
                list.HolidayDate = Convert.ToDateTime(holidayViewModel.HolidayDate);
                _holidayCommand.Add(list);
            }

            return RedirectToAction("Add", "HolidayList");
        }


        public JsonResult Delete(int? holidayId)
        {
            try
            {
                if (holidayId == null)
                {
                    return Json(new { Result = "failed" });
                }
                var result = _holidayCommand.Delete(holidayId);

                return Json(new { Result = "success" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult GridAllHolidays()
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
                var records = _holidayQueries.ShowAllHolidays(sortColumn, sortColumnDirection, searchValue);
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
    }
}
