using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using TicketCore.Common;
using TicketCore.Core;

using TicketCore.Data.Department.Queries;
using TicketCore.Data.Masters.Queries;
using TicketCore.Data.Profiles.Queries;
using TicketCore.Data.Tickets.Command;
using TicketCore.Data.Tickets.Queries;
using TicketCore.Models.Tickets;
using TicketCore.Services.AwsHelper;
using TicketCore.Services.MailHelper;
using TicketCore.ViewModels.Tickets;
using TicketCore.ViewModels.Tickets.Grids;
using TicketCore.Web.Extensions;
using TicketCore.Web.Filters;
using TicketCore.Web.Messages;
using X.PagedList;

namespace TicketCore.Web.Areas.User.Controllers
{
    [SessionTimeOut]
    [AuthorizeUser]
    [Area("User")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    [OnboardingCheckAttribute]
    public class TicketController : Controller
    {
        private readonly IPriorityQueries _iPriorityQueries;
        private readonly IDepartmentQueries _departmentQueries;
        private readonly IGenerateTicketNo _generateTicket;
        private readonly IMapper _mapper;
        private readonly AppSettingsProperties _appsettings;
        private readonly AwsSettings _awsSettings;
        private readonly IAwsS3HelperService _awsS3HelperService;
        private readonly ITicketHistoryHelper _iTicketHistoryHelper;
        private readonly ITicketCommand _iTicketCommand;
        private readonly ITicketHistoryCommand _ticketHistoryCommand;
        private readonly INotificationService _notificationService;
        private readonly IProfileQueries _profileQueries;
        private readonly ITicketQueries _iTicketQueries;
        private readonly ITicketViewQueries _ticketViewQueries;
        private readonly IStatusQueries _statusQueries;
        private readonly ITicketsReplyQueries _ticketsReplyQueries;
        private readonly IApplicationMailingService _iApplicationMailingService;

        public TicketController(IPriorityQueries priorityQueries,
            IDepartmentQueries idepartmentQueries,
            IGenerateTicketNo generateTicketNo, IMapper mapper,
            ITicketHistoryHelper ticketHistoryHelper,
            IOptions<AppSettingsProperties> appsettings,
            IOptions<AwsSettings> awsSettings,
            IAwsS3HelperService awsS3HelperService,
            ITicketCommand ticketCommand,
            ITicketHistoryCommand ticketHistoryCommand,
            INotificationService notificationService,
            IProfileQueries profileQueries,
            ITicketQueries ticketQueries,
            ITicketViewQueries ticketViewQueries,
            IStatusQueries statusQueries,
            ITicketsReplyQueries ticketsReplyQueries, IApplicationMailingService applicationMailingService)
        {
            _iPriorityQueries = priorityQueries;
            _departmentQueries = idepartmentQueries;
            _generateTicket = generateTicketNo;
            _mapper = mapper;
            _iTicketHistoryHelper = ticketHistoryHelper;
            _awsS3HelperService = awsS3HelperService;
            _iTicketCommand = ticketCommand;
            _ticketHistoryCommand = ticketHistoryCommand;
            _notificationService = notificationService;
            _profileQueries = profileQueries;
            _iTicketQueries = ticketQueries;
            _ticketViewQueries = ticketViewQueries;
            _statusQueries = statusQueries;
            _ticketsReplyQueries = ticketsReplyQueries;
            _iApplicationMailingService = applicationMailingService;
            _awsSettings = awsSettings.Value;
            _appsettings = appsettings.Value;
        }

        public IActionResult Create()
        {
            var ticketsViewModel = new TicketsUserViewModel()
            {
                ListofDepartment = _departmentQueries.GetAllActiveSelectListItemDepartment(),
                ListofPriority = _iPriorityQueries.GetAllPrioritySelectListItem()
            };

            return View(ticketsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketsUserViewModel ticketsViewModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                    var applicationNo = _generateTicket.ApplicationNo(out var ticketId);
                    var tickets = _mapper.Map<TicketSummary>(ticketsViewModel);
                    tickets.TicketId = ticketId;
                    tickets.TicketSummaryId = 0;
                    tickets.CreatedOn = DateTime.Now;
                    tickets.TrackingId = applicationNo;
                    tickets.StatusAssigned = false;
                    tickets.CreatedBy = userId;

                    var message = AppendString(ticketsViewModel.Message);
                    var ticketDetails = new TicketDetails()
                    {
                        Subject = ticketsViewModel.Subject,
                        Message = message,
                        TicketDetailsId = 0,
                        CreatedBy = userId
                    };


                    // ReSharper disable once CollectionNeverQueried.Local
                    var listofattachments = new List<TicketAttachmentsViewModel>();

                    var files = HttpContext.Request.Form.Files;
                    if (files.Any())
                    {
                        foreach (var file in files)
                        {
                            if (file.Length > 0)
                            {
                                //Getting FileName
                                var fileName = Path.GetFileName(file.FileName);
                                //Assigning Unique Filename (Guid)
                                var myUniqueFileName = Convert.ToString(Guid.NewGuid().ToString("N"));
                                //Getting file Extension
                                var fileExtension = Path.GetExtension(fileName);
                                // concatenating  FileName + FileExtension
                                var newFileName = String.Concat(myUniqueFileName, fileExtension);

                                await using var target = new MemoryStream();
                                await file.CopyToAsync(target);

                                var attachments = new TicketAttachmentsViewModel();
                                attachments.TicketId = 1;
                                attachments.CreatedBy = userId;
                                attachments.GenerateAttachmentName = newFileName;
                                attachments.OriginalAttachmentName = file.FileName;
                                attachments.AttachmentType = fileExtension;
                                attachments.CreatedOn = DateTime.Now;

                                if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Database)
                                {
                                    attachments.AttachmentBase64 = Convert.ToBase64String(target.ToArray());
                                }
                                else if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                                {
                                    attachments.AttachmentBase64 = null;
                                    attachments.BucketName = _awsSettings.BucketName;
                                    attachments.DirectoryName = "documents";
                                }

                                listofattachments.Add(attachments);

                                if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                                {
                                    bool status =
                                        await _awsS3HelperService.UploadFileAsync(target, newFileName, "documents");
                                }

                            }
                        }
                    }

                    var ticketresult =
                        await _iTicketCommand.AddTickets(userId, tickets, ticketDetails, listofattachments);

                    if (ticketresult)
                    {
                        _notificationService.SuccessNotification("Message",
                            $"{applicationNo}{' '}{CommonMessages.TicketSuccessMessages}");

                        var ticketHistory = new TicketHistoryModel
                        {
                            UserId = userId,
                            Message = _iTicketHistoryHelper.CreateMessage(tickets.PriorityId, tickets.DepartmentId),
                            DepartmentId = tickets.DepartmentId,
                            PriorityId = tickets.PriorityId,
                            StatusId = Convert.ToInt16(StatusMain.Status.Open),
                            ProcessDate = DateTime.Now,
                            TicketId = ticketId,
                            ActivitiesId = Convert.ToInt16(StatusMain.Activities.Created)
                        };
                        _ticketHistoryCommand.TicketHistory(ticketHistory);

                    }
                    else
                    {
                        TempData["ErrorMessageTicket"] = CommonMessages.TicketErrorMessages;
                    }

                    return RedirectToAction("Create", "Ticket");
                }
                else
                {
                    ticketsViewModel.ListofDepartment = _departmentQueries.GetAllActiveSelectListItemDepartment();
                    ticketsViewModel.ListofPriority = _iPriorityQueries.GetAllPrioritySelectListItem();
                    return View(ticketsViewModel);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private string AppendString(string message)
        {
            try
            {
                var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                var signature = _profileQueries.GetSignature(userId);
                var appendedmessage = "";
                if (string.IsNullOrEmpty(signature))
                {
                    appendedmessage = WebUtility.HtmlDecode(message);
                }
                else
                {
                    appendedmessage = WebUtility.HtmlDecode(message)
                                      + Environment.NewLine +
                                      Environment.NewLine +
                                      _profileQueries.GetSignature(userId);
                }

                return appendedmessage;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public IActionResult AllTickets(int? departmentId, string search, int? statusId,
            int? searchIn, int? priorityId, int? page = 1)
        {

            if (searchIn == 1 && !string.IsNullOrEmpty(search))
            {
                var searchtrim = Regex.Replace(search, @"\s+", "");
                search = searchtrim;
            }

            if (searchIn == 2 && !string.IsNullOrEmpty(search))
            {
                var isNumeric = int.TryParse(search, out int n);
                if (isNumeric)
                {

                }
                else
                {
                    _notificationService.DangerNotification("Message", $"Enter Valid TicketId");
                    search = string.Empty;
                }
            }

            if (searchIn == 3 && string.IsNullOrEmpty(search))
            {
                _notificationService.DangerNotification("Message", $"Enter Subject");
            }

            var usergridview = new UserTicketGrid();
            usergridview.ListofDepartment = _departmentQueries.GetAllActiveDepartmentWithoutSelect();
            usergridview.ListofSearch = ListofSearchFields();
            usergridview.ListofPriority = _iPriorityQueries.GetAllPrioritySelectListItem();
            usergridview.ListofStatus = _statusQueries.GetAllStatusSelectListItem();

            var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
            var pageIndex = (page ?? 1) - 1;
            var pageSize = 10;
            var totalticketCount = 0;

            totalticketCount = _iTicketQueries.GetUserEndTicketsCount(userId, search, statusId, searchIn, departmentId, priorityId);
            var ticketlist = _iTicketQueries
                .GetUserEndTicketList(userId, page, pageSize, departmentId, search, searchIn, statusId, priorityId).ToList();

            ViewBag.totalticketCount = totalticketCount;
            var pagedList =
                new StaticPagedList<UserTicketGridViewModel>(ticketlist, pageIndex + 1, pageSize, totalticketCount);

            usergridview.ListofUserTicket = pagedList;
            usergridview.CurrentPage = page;
            usergridview.DepartmentId = departmentId;
            usergridview.PriorityId = priorityId;
            usergridview.Search = search;
            usergridview.SearchIn = searchIn;
            return View(usergridview);
        }


        private List<SelectListItem> ListofSearchFields()
        {
            var result = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "TrackingId",
                    Value = "1"
                },
                new SelectListItem()
                {
                    Text = "TicketId",
                    Value = "2"
                },
                new SelectListItem()
                {
                    Text = "Subject",
                    Value = "3"
                }

            };

            return result;
        }

        [HttpGet]
        public IActionResult TicketDetails(long? id)
        {
            if (id != null)
            {
                var currentrole = HttpContext.Session.GetInt32(AllSessionKeys.RoleId);
                ViewBag.Currentrole = currentrole;

                if (!_ticketViewQueries.CheckTrackingIdExists(id))
                {
                    _notificationService.DangerNotification("Message", "TicketId does not exists");
                    return RedirectToAction("Dashboard", "MyDashboard", new { Area = "User" });
                }

                var displayticketViewModel = new DisplayTicketViewModel();

                var ticketdetailvm = _ticketViewQueries.TicketsDetailsbyticketId(id);

                displayticketViewModel.TicketDetailViewModel = ticketdetailvm;
                displayticketViewModel.ListofAttachments = _iTicketQueries.GetListAttachmentsByticketId(id);

                displayticketViewModel.TicketReply = new TicketReplyViewModel()
                {
                    Message = string.Empty,
                    TicketId = displayticketViewModel.TicketDetailViewModel.TicketId,
                    ListofStatus = _statusQueries.GetAllStatusWithoutInternalStatus()
                };

                displayticketViewModel.ViewMainModel = new ViewTicketReplyMainModel()
                {
                    ListofReplyAttachment = new List<ReplyAttachmentModel>(),
                    ListofTicketreply = _ticketsReplyQueries.ListofHistoryTicketReplies(id)
                };

                var expressChangesViewModel = new ExpressChangesTicketViewModel();
                expressChangesViewModel.ListofPriority = _iPriorityQueries.GetAllPrioritySelectListItem();
                expressChangesViewModel.ListofStatus = _statusQueries.GetAllStatusWithoutInternalStatus();
                expressChangesViewModel.StatusId = ticketdetailvm.StatusId;
                expressChangesViewModel.PriorityId = ticketdetailvm.PriorityId;
                displayticketViewModel.ExpressChangesViewModel = expressChangesViewModel;
                displayticketViewModel.EscalatedUser = _ticketViewQueries.GetTicketEscalatedToUserNames(id);
                ViewBag.CurrentTicketStatus = ticketdetailvm.StatusId;
                ViewBag.CurrentDeleteTicketStatus = ticketdetailvm.DeleteStatus;

                return View(displayticketViewModel);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TicketReply(TicketUserReplyViewModel ticketReplyVm)
        {
            var statusId = (int)StatusMain.Status.Replied;
            var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
            var ticketid = ticketReplyVm.TicketId;

            #region Attachment
            var listofattachments = new List<TicketAttachmentsViewModel>();
            var files = HttpContext.Request.Form.Files;
            if (files.Any())
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        //Getting FileName

                        var fileName = Path.GetFileName(file.FileName);
                        //Assigning Unique Filename (Guid)
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid().ToString("N"));
                        //Getting file Extension
                        var fileExtension = Path.GetExtension(fileName);
                        // concatenating  FileName + FileExtension
                        var newFileName = String.Concat(myUniqueFileName, fileExtension);


                        await using var target = new MemoryStream();
                        await file.CopyToAsync(target);

                        var attachments = new TicketAttachmentsViewModel
                        {
                            TicketId = ticketid,
                            CreatedBy = userId,
                            GenerateAttachmentName = newFileName,
                            OriginalAttachmentName = file.FileName,
                            AttachmentType = fileExtension,
                            CreatedOn = DateTime.Now
                        };

                        if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Database)
                        {
                            attachments.AttachmentBase64 = Convert.ToBase64String(target.ToArray());
                        }
                        else if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                        {
                            attachments.AttachmentBase64 = null;
                            attachments.BucketName = _awsSettings.BucketName;
                            attachments.DirectoryName = "documents";
                        }
                        listofattachments.Add(attachments);

                        if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                        {
                            bool status = await _awsS3HelperService.UploadFileAsync(target, newFileName, "documents");
                        }

                    }
                }
            }
            #endregion


            var message = AppendString(ticketReplyVm.Message);

            var ticketReplyModel = new TicketReplyModel()
            {
                TicketId = ticketid,
                CreatedOn = DateTime.Now,
                CreatedDateDisplay = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                DeleteStatus = false,
                TicketReplyId = 0,
                CreatedBy = userId,
                TicketUser = userId,
                SystemUser = 0
            };

            var ticketReplyDetails = new TicketReplyDetailsModel()
            {
                Message = message,
                Note = ticketReplyVm.Note,
                TicketReplyId = 0,
                TicketReplyDetailsId = 0
            };

            var currentticketstatus = await _iTicketQueries.GetTicketStatusbyTicketId(ticketid);
            currentticketstatus.StatusId = statusId;
            currentticketstatus.TicketUpdatedDate = DateTime.Now;

            var latestreply = await _iTicketQueries.GetLatestTicketReplybyId(ticketid);
            latestreply.StatusInfo = "User responded";

            var result = await _iTicketCommand.AddTicketReply(userId, ticketReplyModel, ticketReplyDetails, listofattachments, currentticketstatus, latestreply);

            if (result)
            {
                var ticketHistory = new TicketHistoryModel
                {
                    UserId = userId,
                    Message = _iTicketHistoryHelper.ReplyMessage(statusId),
                    ProcessDate = DateTime.Now,
                    TicketId = ticketReplyVm.TicketId,
                    PriorityId = null,
                    StatusId = statusId,
                    DepartmentId = null,
                    ActivitiesId = Convert.ToInt16(StatusMain.Activities.Replied),
                    TicketReplyId = 0,
                    TicketHistoryId = 0
                };

                _ticketHistoryCommand.TicketHistory(ticketHistory);

                string url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/User/MyTicketDetails/TicketDetails/{ticketReplyVm.TicketId}";
                var fullName = HttpContext.Session.GetString(AllSessionKeys.FullName);
                _iApplicationMailingService.TicketUserReplyEmail(url, "C:Ticket| A:TicketReply", ticketReplyVm.TicketId, "Replied", fullName, userId);

                return Json(new { Result = "success", Message = "Replied on Ticket Successfully!" });
            }
            else
            {
                return Json(new { Result = "failed", Message = "Something went wrong while Replied on Ticket. Please Try again!" });
            }


        }

        public IActionResult GetTicketActivities(TicketHistoryRequest requestBug)
        {
            var listofticketHistory = _ticketViewQueries.ListofTicketHistorybyTicket(requestBug.Ticketid);
            return PartialView("_TicketActivities", listofticketHistory);
        }

        public async Task<IActionResult> DownloadAttachment(long ticketId, long attachmentId)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(attachmentId)))
                {
                    var document = _iTicketQueries.GetAttachmentsByticketId(ticketId, attachmentId);
                    if (document != null)
                    {
                        if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Database)
                        {
                            var documentdetail =
                                _iTicketQueries.GetAttachmentDetailsByAttachmentId(ticketId, document.AttachmentId);
                            byte[] bytes = System.Convert.FromBase64String(documentdetail.AttachmentBase64);
                            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, document.GenerateAttachmentName);
                        }
                        else if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                        {
                            var response = await _awsS3HelperService.ReadFileAsync(document.GenerateAttachmentName, "documents");
                            return File(response.FileStream, response.ContentType, document.GenerateAttachmentName);
                        }
                    }

                    return RedirectToAction("AllTickets", "Ticket");
                }
                else
                {
                    return RedirectToAction("AllTickets", "Ticket");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> DeleteAttachment(RequestAttachments requestAttachments)
        {
            try
            {
                var document = _iTicketQueries.GetAttachmentsByticketId(requestAttachments.TicketId, requestAttachments.AttachmentsId);
                var documentdetail = _iTicketQueries.GetAttachmentDetailsByAttachmentId(requestAttachments.TicketId, requestAttachments.AttachmentsId);

                if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                {
                    var status =
                        await _awsS3HelperService.RemoveFileAsync(document.GenerateAttachmentName, "documents");
                }

                var result = _iTicketCommand.DeleteAttachmentByAttachmentId(document, documentdetail);

                if (result)
                {
                    var fullName = HttpContext.Session.GetString(AllSessionKeys.FullName);
                    var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);

                    var ticketHistory = new TicketHistoryModel
                    {
                        UserId = userId,
                        Message = _iTicketHistoryHelper.DeleteTicketAttachment(Convert.ToString(requestAttachments.TicketId)),
                        ProcessDate = DateTime.Now,
                        TicketId = requestAttachments.TicketId,
                        PriorityId = null,
                        StatusId = null,
                        DepartmentId = null,
                        ActivitiesId = Convert.ToInt16(StatusMain.Activities.DeletedAttachment),
                        TicketReplyId = null,
                        TicketHistoryId = 0
                    };

                    _ticketHistoryCommand.TicketHistory(ticketHistory);

                    return Json(new { Status = true });
                }
                else
                {
                    return Json(new { Status = false });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> DownloadReplyAttachment(long ticketId, long attachmentId)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(attachmentId)))
                {
                    var document = _iTicketQueries.GetReplyAttachmentsByTicketId(ticketId, attachmentId);
                    if (document != null)
                    {
                        if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Database)
                        {
                            var documentdetail = _iTicketQueries.GetReplyAttachmentDetailsByAttachmentId(ticketId, document.ReplyAttachmentId);
                            byte[] bytes = System.Convert.FromBase64String(documentdetail.AttachmentBase64);
                            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, document.GenerateAttachmentName);
                        }
                        else if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                        {
                            var response = await _awsS3HelperService.ReadFileAsync(document.GenerateAttachmentName, "documents");
                            return File(response.FileStream, response.ContentType, document.GenerateAttachmentName);
                        }
                    }

                    return RedirectToAction("AllTickets", "Ticket");
                }
                else
                {
                    return RedirectToAction("AllTickets", "Ticket");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> DeleteReplyAttachment(RequestReplyAttachments requestAttachments)
        {
            try
            {
                var document = _iTicketQueries.GetReplyAttachmentsByTicketId(requestAttachments.TicketId, requestAttachments.AttachmentsId);
                var documentdetail = _iTicketQueries.GetReplyAttachmentDetailsByAttachmentId(requestAttachments.TicketId, requestAttachments.AttachmentsId);

                if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                {
                    var status =
                        await _awsS3HelperService.RemoveFileAsync(document.GenerateAttachmentName, "documents");
                }

                var result = _iTicketCommand.DeleteReplyAttachmentByAttachmentId(document, documentdetail);
                if (result)
                {
                    var fullName = HttpContext.Session.GetString(AllSessionKeys.FullName);
                    var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);


                    var ticketHistory = new TicketHistoryModel
                    {
                        UserId = userId,
                        Message = _iTicketHistoryHelper.DeleteTicketReplyAttachment(Convert.ToString(requestAttachments.TicketId)),
                        ProcessDate = DateTime.Now,
                        TicketId = requestAttachments.TicketId,
                        PriorityId = null,
                        StatusId = null,
                        DepartmentId = null,
                        ActivitiesId = Convert.ToInt16(StatusMain.Activities.DeletedAttachment),
                        TicketReplyId = requestAttachments.TicketReplyId,
                        TicketHistoryId = 0
                    };

                    _ticketHistoryCommand.TicketHistory(ticketHistory);

                    return Json(new { Status = true });
                }
                else
                {
                    return Json(new { Status = false });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult Edit(long? id)
        {
            try
            {
                var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                var ticketsViewModel = _iTicketQueries.GetTicketbyTicketId(id);
                ticketsViewModel.HasAttachment = _iTicketQueries.CheckAttachmentsExistbyTicketId(id);
                ticketsViewModel.ListofDepartment = _departmentQueries.GetAllActiveSelectListItemDepartment();
                ticketsViewModel.ListofPriority = _iPriorityQueries.GetAllPrioritySelectListItem();
                ticketsViewModel.ListofAttachments =
                    _iTicketQueries.GetListAttachmentsByticketId(ticketsViewModel.TicketId);
                return View(ticketsViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EditTicketViewModel editTicket)
        {
            var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
            var currentdate = DateTime.Now;

            editTicket.ListofDepartment = _departmentQueries.GetAllActiveSelectListItemDepartment();
            editTicket.ListofPriority = _iPriorityQueries.GetAllPrioritySelectListItem();

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Any() && (_iTicketQueries.CheckAttachmentsExistbyTicketId(editTicket.TicketId)))
                {
                    _notificationService.DangerNotification("Message",
                        "All Delete Pervious uploaded Attachments for Uploading New Attachments");
                    return View(editTicket);
                }

                var message = AppendString(editTicket.Message);

                var ticketsUserViewModel = new TicketsUserViewModel()
                {
                    DepartmentId = editTicket.DepartmentId,
                    TicketId = editTicket.TicketId,
                    Message = message,
                    Subject = editTicket.Subject,
                    PriorityId = editTicket.PriorityId
                };

                var listofattachments = new List<TicketAttachmentsViewModel>();

                if (files.Any())
                {
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            //Getting FileName

                            var fileName = Path.GetFileName(file.FileName);
                            //Assigning Unique Filename (Guid)
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid().ToString("N"));
                            //Getting file Extension
                            var fileExtension = Path.GetExtension(fileName);
                            // concatenating  FileName + FileExtension
                            var newFileName = String.Concat(myUniqueFileName, fileExtension);


                            await using var target = new MemoryStream();
                            await file.CopyToAsync(target);

                            var attachments = new TicketAttachmentsViewModel();
                            attachments.TicketId = editTicket.TicketId;
                            attachments.CreatedBy = userId;
                            attachments.GenerateAttachmentName = newFileName;
                            attachments.OriginalAttachmentName = file.FileName;
                            attachments.AttachmentType = fileExtension;
                            attachments.CreatedOn = DateTime.Now;

                            if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Database)
                            {
                                attachments.AttachmentBase64 = Convert.ToBase64String(target.ToArray());
                            }
                            else if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                            {
                                attachments.AttachmentBase64 = null;
                                attachments.BucketName = _awsSettings.BucketName;
                                attachments.DirectoryName = "documents";
                            }
                            listofattachments.Add(attachments);

                            if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                            {
                                bool status = await _awsS3HelperService.UploadFileAsync(target, newFileName, "documents");
                            }

                        }
                    }
                }

                _notificationService.SuccessNotification("Message", $"Ticket #{editTicket.TicketId} Updated Successfully.");
                var result = await _iTicketCommand.UpdateTicket(userId, ticketsUserViewModel, listofattachments);


                if (result)
                {
                    var ticketHistory = new TicketHistoryModel
                    {
                        UserId = userId,
                        Message = _iTicketHistoryHelper.EditReplyTicket(),
                        ProcessDate = DateTime.Now,
                        TicketId = editTicket.TicketId,
                        PriorityId = null,
                        StatusId = null,
                        ActivitiesId = Convert.ToInt16(StatusMain.Activities.EditedTicket),
                    };

                    _ticketHistoryCommand.TicketHistory(ticketHistory);

                    string url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/User/MyTicketDetails/TicketDetails/{editTicket.TicketId}";
                    var fullName = HttpContext.Session.GetString(AllSessionKeys.FullName);
                    _iApplicationMailingService.TicketEditedTicketEmail(url, "C:Ticket | A:Edit", editTicket.TicketId, "Edited Ticket", fullName, userId);

                }

                return RedirectToAction("AllTickets", "Ticket");
            }


            return View(editTicket);
        }

        [HttpGet]
        public ActionResult EditReply(long? tid, long? rid)
        {
            try
            {
                var currentrole = HttpContext.Session.GetInt32(AllSessionKeys.RoleId);
                ViewBag.Currentrole = currentrole;
                var ticketsViewModel = _iTicketQueries.GetTicketReplyDetailsbyTicketId(tid, rid);
                ticketsViewModel.ListofAttachments = _iTicketQueries.GetReplyAttachmentsListByTicketId(tid, rid);
                return View(ticketsViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditReply(EditTicketReplyViewModel editticketReply)
        {
            try
            {
                var fullName = HttpContext.Session.GetString(AllSessionKeys.FullName);
                if (ModelState.IsValid)
                {
                    var files = HttpContext.Request.Form.Files;
                    if (files.Any() && (_iTicketQueries.ReplyAttachmentsExistbyTicketId(editticketReply.TicketId, editticketReply.TicketReplyId)))
                    {
                        _notificationService.DangerNotification("Message",
                            "All Delete Pervious uploaded Attachments for Uploading New Attachments");
                        return RedirectToAction("EditReply", "MyTicket", new { tid = editticketReply.TicketId, rid = editticketReply.TicketReplyId });
                    }


                    var message = AppendString(editticketReply.Message);
                    editticketReply.Message = message;

                    var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                    var listofattachments = new List<TicketAttachmentsViewModel>();

                    if (files.Any())
                    {
                        foreach (var file in files)
                        {
                            if (file.Length > 0)
                            {
                                //Getting FileName

                                var fileName = Path.GetFileName(file.FileName);
                                //Assigning Unique Filename (Guid)
                                var myUniqueFileName = Convert.ToString(Guid.NewGuid().ToString("N"));
                                //Getting file Extension
                                var fileExtension = Path.GetExtension(fileName);
                                // concatenating  FileName + FileExtension
                                var newFileName = String.Concat(myUniqueFileName, fileExtension);


                                await using var target = new MemoryStream();
                                await file.CopyToAsync(target);

                                var attachments = new TicketAttachmentsViewModel();
                                attachments.TicketId = editticketReply.TicketId;
                                attachments.CreatedBy = userId;
                                attachments.GenerateAttachmentName = newFileName;
                                attachments.OriginalAttachmentName = file.FileName;
                                attachments.AttachmentType = fileExtension;
                                attachments.CreatedOn = DateTime.Now;

                                if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Database)
                                {
                                    attachments.AttachmentBase64 = Convert.ToBase64String(target.ToArray());
                                }
                                else if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                                {
                                    attachments.AttachmentBase64 = null;
                                    attachments.BucketName = _awsSettings.BucketName;
                                    attachments.DirectoryName = "documents";
                                }
                                listofattachments.Add(attachments);

                                if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                                {
                                    bool status = await _awsS3HelperService.UploadFileAsync(target, newFileName, "documents");
                                }

                            }
                        }
                    }

                    var result = await _iTicketCommand.UpdateTicketReply(editticketReply, listofattachments, userId, null);

                    if (result)
                    {
                        var ticketHistory = new TicketHistoryModel
                        {
                            UserId = userId,
                            Message = _iTicketHistoryHelper.EditReplyTicket(),
                            ProcessDate = DateTime.Now,
                            TicketId = editticketReply.TicketId,
                            PriorityId = null,
                            StatusId = null,
                            ActivitiesId = Convert.ToInt16(StatusMain.Activities.EditedTicketReply),
                        };

                        _ticketHistoryCommand.TicketHistory(ticketHistory);


                        string url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/User/MyTicketDetails/TicketDetails/{editticketReply.TicketId}";

                        _iApplicationMailingService.TicketEditedReplyTicketEmail(url, "C:Ticket | A:EditReply", editticketReply.TicketId, "Edited Reply", fullName, userId);

                        _notificationService.SuccessNotification("Message", $"Ticket #{editticketReply.TicketId} Reply Details Updated Successfully.");

                        return RedirectToAction("TicketDetails", "Ticket", new { id = editticketReply.TicketId });
                    }
                }

                var currentrole = HttpContext.Session.GetInt32(AllSessionKeys.RoleId);
                ViewBag.Currentrole = currentrole;
                editticketReply.ListofAttachments = _iTicketQueries.GetReplyAttachmentsListByTicketId(editticketReply.TicketId, editticketReply.TicketReplyId);


                return View(editticketReply);
            }
            catch (Exception)
            {
                throw;
            }

        }


    }
}
