using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using TicketCore.Common;
using TicketCore.Data.Notices.Command;
using TicketCore.Data.Notices.Queries;
using TicketCore.Models.Notices;
using TicketCore.ViewModels.Notices;
using TicketCore.Web.Filters;
using TicketCore.Web.Messages;

namespace TicketCore.Web.Areas.Administration.Controllers
{
    [SessionTimeOut]
    [AuthorizeSuperAdmin]
    [Area("Administration")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class NoticeController : Controller
    {
        private readonly INoticeQueries _noticeQueries;
        private readonly IMapper _mapper;
        private readonly INoticeCommand _noticeCommand;
        private readonly INotificationService _notificationService;
        public NoticeController(INoticeQueries noticeQueries, IMapper mapper, INoticeCommand noticeCommand, INotificationService notificationService)
        {
            _noticeQueries = noticeQueries;
            _mapper = mapper;
            _noticeCommand = noticeCommand;
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GridAllNotice()
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
                var records = _noticeQueries.ShowAllNotice(sortColumn, sortColumnDirection, searchValue);
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateNoticeViewModel noticeViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_noticeQueries.ValidateNotice(noticeViewModel.NoticeStart))
                {
                    ModelState.AddModelError("", "Notice Already Exits Between Selected Dates");
                    return View(noticeViewModel);
                }

                var noticeMappedobject = _mapper.Map<Notice>(noticeViewModel);
                noticeMappedobject.Status = true;
                noticeMappedobject.CreatedOn = DateTime.Now;
                noticeMappedobject.CreatedBy = HttpContext.Session.GetInt32(AllSessionKeys.UserId);



                var noticeDetails = new NoticeDetails()
                {
                    Notice = noticeMappedobject,
                    NoticeBody = HttpUtility.HtmlDecode(noticeViewModel.NoticeBody),
                    NoticeDetailsId = 0,
                    NoticeId = noticeMappedobject.NoticeId
                };


                var result = _noticeCommand.AddNotice(noticeMappedobject, noticeDetails);

                if (result > 0)
                {
                    _notificationService.SuccessNotification("Message", "The Notice was added Successfully!");
                }

            }

            return RedirectToAction("Create");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            try
            {
                var notice = _noticeQueries.GetNoticeDetailsForEdit(id);
                return View(notice);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditNoticeViewModel editNotice)
        {
            if (ModelState.IsValid)
            {
                var notice = _noticeQueries.GetNoticeByNoticeId(editNotice.NoticeId);
                notice.NoticeStart = Convert.ToDateTime(notice.NoticeStart);
                notice.NoticeEnd = Convert.ToDateTime(notice.NoticeEnd);

                notice.NoticeTitle = editNotice.NoticeTitle;
                notice.Status = editNotice.Status;
                notice.ModifiedOn = DateTime.Now;
                notice.ModifiedBy = HttpContext.Session.GetInt32(AllSessionKeys.UserId);


                var noticedetails = _noticeQueries.GetNoticeDetailsByNoticeId(editNotice.NoticeId);

                noticedetails.Notice = notice;
                noticedetails.NoticeBody = HttpUtility.HtmlDecode(editNotice.NoticeBody);
                noticedetails.NoticeId = editNotice.NoticeId;

                var result = _noticeCommand.UpdateNotice(notice, noticedetails);

                if (result > 0)
                {
                    _notificationService.SuccessNotification("Message", "The Notice was updated Successfully!");
                    return RedirectToAction("Index");
                }
            }

            return View(editNotice);
        }


        public JsonResult DeleteNotice(RequestDeleteNotice requestDeleteNotice)
        {
            try
            {
                var notice = _noticeQueries.GetNoticeByNoticeId(requestDeleteNotice.NoticeId);

                var noticedetails = _noticeQueries.GetNoticeDetailsByNoticeId(requestDeleteNotice.NoticeId);
                notice.Status = !notice.Status;
                var result = _noticeCommand.AddNotice(notice, noticedetails);

                if (result > 0)
                {
                    _notificationService.SuccessNotification("Message", "The Notice Deleted successfully!");
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
