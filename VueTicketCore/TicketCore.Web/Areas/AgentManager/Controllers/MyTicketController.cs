using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
using TicketCore.Data.Usermaster.Queries;
using TicketCore.Models.Tickets;
using TicketCore.Services.AwsHelper;
using TicketCore.Services.MailHelper;
using TicketCore.ViewModels.Tickets;
using TicketCore.ViewModels.Tickets.Grids;
using TicketCore.Web.Extensions;
using TicketCore.Web.Filters;
using TicketCore.Web.Messages;
using X.PagedList;

namespace TicketCore.Web.Areas.AgentManager.Controllers
{
    [SessionTimeOut]
    [AuthorizeAgentAdmin]
    [Area("AgentManager")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class MyTicketController : Controller
    {
        private readonly ITicketQueries _iTicketQueries;
        private readonly IPriorityQueries _iPriorityQueries;
        private readonly IDepartmentQueries _departmentQueries;
        private readonly IStatusQueries _statusQueries;
        private readonly INotificationService _notificationService;
        private readonly AppSettingsProperties _appsettings;
        private readonly AwsSettings _awsSettings;
        private readonly IAwsS3HelperService _awsS3HelperService;
        private readonly IProfileQueries _profileQueries;
        private readonly ITicketCommand _iTicketCommand;
        private readonly ITicketHistoryHelper _iTicketHistoryHelper;
        private readonly ITicketHistoryCommand _ticketHistoryCommand;
        private readonly IUserMasterQueries _iUserMasterQueries;
        private readonly IGenerateTicketNo _generateTicket;
        private readonly IMapper _mapper;
        private readonly IApplicationMailingService _applicationMailingService;

        public MyTicketController(ITicketQueries iTicketQueries,
            IPriorityQueries iPriorityQueries,
            IDepartmentQueries departmentQueries,
            IStatusQueries statusQueries,
            INotificationService notificationService,
            IOptions<AppSettingsProperties> appsettings,
            IOptions<AwsSettings> awsSettings,
            IAwsS3HelperService awsS3HelperService,
            IProfileQueries profileQueries,
            ITicketCommand iTicketCommand,
            ITicketHistoryHelper iTicketHistoryHelper,
            ITicketHistoryCommand ticketHistoryCommand,
            IUserMasterQueries userMasterQueries,
            IGenerateTicketNo generateTicket, IMapper mapper, IApplicationMailingService iApplicationMailingService)
        {
            _iTicketQueries = iTicketQueries;
            _iPriorityQueries = iPriorityQueries;
            _departmentQueries = departmentQueries;
            _statusQueries = statusQueries;
            _notificationService = notificationService;
            _awsS3HelperService = awsS3HelperService;
            _profileQueries = profileQueries;
            _iTicketCommand = iTicketCommand;
            _iTicketHistoryHelper = iTicketHistoryHelper;
            _ticketHistoryCommand = ticketHistoryCommand;
            _iUserMasterQueries = userMasterQueries;
            _generateTicket = generateTicket;
            _mapper = mapper;
            _applicationMailingService = iApplicationMailingService;
            _awsSettings = awsSettings.Value;
            _appsettings = appsettings.Value;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var ticketsViewModel = new TicketCommonViewModel()
            {
                ListofDepartment = _departmentQueries.GetAllActiveSelectListItemDepartment(),
                ListofPriority = _iPriorityQueries.GetAllPrioritySelectListItem()
            };

            return View(ticketsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketCommonViewModel ticketsViewModel)
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
                    tickets.CreatedBy = ticketsViewModel.HiddenUserId;
                    tickets.InternalUserId = userId;

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

                    return RedirectToAction("Create", "MyTicket");
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

        [HttpGet]
        public IActionResult AllTickets(int? departmentId, string search, int? statusId,
            int? searchIn, int? priorityId, long? agentsId, int? page = 1)
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

            var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);

            if (departmentId == null)
            {
                var defaultdepartment = _departmentQueries.GetAssignedDepartmentsofAgentManager(userId).FirstOrDefault()?.Value;
                departmentId = Convert.ToInt32(defaultdepartment);
            }

            var usergridview = new AgentManagerTicketGrid();
            usergridview.ListofDepartment = _departmentQueries.GetAssignedDepartmentsofAgentManager(userId);
            usergridview.ListofSearch = ListofSearchFields();
            usergridview.ListofPriority = _iPriorityQueries.GetAllPrioritySelectListItem();
            usergridview.ListofStatus = _statusQueries.GetAllStatusSelectListItem();

            var agentlist = _iUserMasterQueries.GetListofAgents(departmentId);
            usergridview.ListofAgents = agentlist;

            var pageIndex = (page ?? 1) - 1;
            var pageSize = 10;
            var totalticketCount = 0;

            totalticketCount =
                _iTicketQueries.GetAgentManagerEndTicketsCount(agentsId, search, statusId, searchIn, departmentId,
                    priorityId);

            var ticketlist = _iTicketQueries
                .GetAgentManagerEndTicketList(agentsId, page, pageSize, departmentId, search, searchIn, statusId,
                    priorityId)
                .ToList();

            ViewBag.totalticketCount = totalticketCount;
            var pagedList =
                new StaticPagedList<AgentManagerTicketGridViewModel>(ticketlist, pageIndex + 1, pageSize,
                    totalticketCount);

            usergridview.ListofTicketDetails = pagedList;
            usergridview.CurrentPage = page;
            usergridview.DepartmentId = departmentId;
            usergridview.PriorityId = priorityId;
            usergridview.Search = search;
            usergridview.SearchIn = searchIn;
            usergridview.AgentsId = agentsId;
            return View(usergridview);
        }


        [HttpGet]
        public IActionResult AllCreatedTickets(int? departmentId, string search, int? statusId,
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

            var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);

            departmentId ??= _departmentQueries.GetAgentAdminDepartmentIdsByUserId(userId);

            var usergridview = new AgentTicketGrid();
            usergridview.ListofDepartment = _departmentQueries.GetAllActiveDepartmentWithoutSelect();
            usergridview.ListofSearch = ListofSearchFields();
            usergridview.ListofPriority = _iPriorityQueries.GetAllPrioritySelectListItem();
            usergridview.ListofStatus = _statusQueries.GetAllStatusSelectListItem();

            var pageIndex = (page ?? 1) - 1;
            var pageSize = 10;
            var totalticketCount = 0;

            totalticketCount =
                _iTicketQueries.GetAgentEndCreatedTicketsCount(userId, search, statusId, searchIn, departmentId,
                    priorityId);
            var ticketlist = _iTicketQueries
                .GetAgentEndCreatedTicketList(userId, page, pageSize, departmentId, search, searchIn, statusId,
                    priorityId)
                .ToList();

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

        [HttpGet]
        public ActionResult Edit(long? ticketId)
        {
            try
            {
                var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                var ticketsViewModel = _iTicketQueries.GetTicketbyTicketId(ticketId);
                ticketsViewModel.HasAttachment = _iTicketQueries.CheckAttachmentsExistbyTicketId(ticketId);
                ticketsViewModel.ListofDepartment = _departmentQueries.GetAssignedDepartmentsofAgentManager(userId);
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

            editTicket.ListofDepartment = _departmentQueries.GetAssignedDepartmentsofAgentManager(userId);
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

                    string url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/User/Ticket/TicketDetails/{editTicket.TicketId}";
                    var fullName = HttpContext.Session.GetString(AllSessionKeys.FullName);
                    _applicationMailingService.TicketEditedTicketEmail(url, "C:MyTicket | A:Edit", editTicket.TicketId, "Edited", fullName, userId);

                    _ticketHistoryCommand.TicketHistory(ticketHistory);
                }

                return RedirectToAction("AllTickets", "MyTicket");
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
                        _notificationService.SuccessNotification("Message", $"Ticket #{editticketReply.TicketId} Reply Details Updated Successfully.");


                        string url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/User/Ticket/TicketDetails/{editticketReply.TicketId}";

                        _applicationMailingService.TicketEditedReplyTicketEmail(url, "C:MyTicket | A:EditReply", editticketReply.TicketId, "EditReply", fullName, userId);


                        return RedirectToAction("TicketDetails", "MyTicketDetails", new { id = editticketReply.TicketId });
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


        public IActionResult GetAgentList(UserRequestViewModel userRequestViewModel)
        {
            if (userRequestViewModel.DepartmentId == null)
            {
                var list = new List<SelectListItem>();

                list.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                return Json(list);
            }

            var agentlist = _iUserMasterQueries.GetListofAgents(userRequestViewModel.DepartmentId);
            return Json(new { listofusers = agentlist });
        }

        public IActionResult GetAllDepartments(string departmentId)
        {
            var departmentlist = _departmentQueries.GetAllActiveSelectListItemDepartment();
            return Json(departmentlist);
        }

        public IActionResult TransferDepartment(int? dId, long? tId)
        {
            if (dId != null && tId != null)
            {
                var getticketdetails = _iTicketQueries.GetTicketbyTicketId(tId);
                if (getticketdetails.DepartmentId != dId)
                {
                    _iTicketCommand.TransferDepartment(dId, tId);
                    var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);

                    var ticketHistory = new TicketHistoryModel
                    {
                        UserId = userId,
                        Message = _iTicketHistoryHelper.TransferMessage(getticketdetails.DepartmentId, dId),
                        DepartmentId = dId,
                        StatusId = null,
                        ProcessDate = DateTime.Now,
                        TicketId = tId,
                        ActivitiesId = Convert.ToInt16(StatusMain.Activities.Transferred_Tickets)
                    };
                    _ticketHistoryCommand.TicketHistory(ticketHistory);

                    return Json(new { status = "success" });

                }
                else
                {
                    return Json(new { status = "validation" });

                }
            }
            return Json(new { status = "failed" });
        }

        public async Task<IActionResult> DeleteTicket(string tid)
        {
            try
            {
                if (!string.IsNullOrEmpty(tid))
                {
                    var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                    var result = await _iTicketCommand.DeleteTicket(userId, Convert.ToInt64(tid), Convert.ToInt16(StatusMain.Status.Deleted));

                    if (result)
                    {

                        var ticketHistory = new TicketHistoryModel
                        {
                            UserId = userId,
                            Message = _iTicketHistoryHelper.TicketDelete(),
                            StatusId = null,
                            ProcessDate = DateTime.Now,
                            TicketId = Convert.ToInt64(tid),
                            ActivitiesId = Convert.ToInt16(StatusMain.Activities.DeleteTicket)
                        };
                        _ticketHistoryCommand.TicketHistory(ticketHistory);

                    }
                    return Json(new { status = "success" });
                }
                else
                {
                    return Json(new { status = "failed" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "failed" });
            }
        }

        public async Task<IActionResult> UndoDeleteTicket(string tid)
        {
            try
            {
                if (!string.IsNullOrEmpty(tid))
                {
                    var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                    var result = await _iTicketCommand.UndoDeleteTicket(Convert.ToInt64(tid));

                    if (result)
                    {

                        var ticketHistory = new TicketHistoryModel
                        {
                            UserId = userId,
                            Message = _iTicketHistoryHelper.TicketRestore(),
                            StatusId = null,
                            ProcessDate = DateTime.Now,
                            TicketId = Convert.ToInt64(tid),
                            ActivitiesId = Convert.ToInt16(StatusMain.Activities.Restored)
                        };
                        _ticketHistoryCommand.TicketHistory(ticketHistory);

                    }
                    return Json(new { status = "success" });
                }
                else
                {
                    return Json(new { status = "failed" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "failed" });
            }
        }

        [HttpGet]
        public IActionResult AllDeletedTickets(int? departmentId, string search,
           int? searchIn, int? priorityId, long? agentsId, int? page = 1)
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

            var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);

            if (departmentId == null)
            {
                var defaultdepartment = _departmentQueries.GetAssignedDepartmentsofAgentManager(userId).FirstOrDefault()?.Value;
                departmentId = Convert.ToInt32(defaultdepartment);
            }

            var usergridview = new AgentManagerTicketGrid();
            usergridview.ListofDepartment = _departmentQueries.GetAssignedDepartmentsofAgentManager(userId);
            usergridview.ListofSearch = ListofSearchFields();
            usergridview.ListofPriority = _iPriorityQueries.GetAllPrioritySelectListItem();
            usergridview.ListofStatus = _statusQueries.GetAllStatusSelectListItem();

            var agentlist = _iUserMasterQueries.GetListofAgents(departmentId);
            usergridview.ListofAgents = agentlist;

            var pageIndex = (page ?? 1) - 1;
            var pageSize = 10;
            var totalticketCount = 0;

            totalticketCount =
                _iTicketQueries.GetAllDeletedAgentManagerEndTicketsCount(agentsId, search, searchIn, departmentId,
                    priorityId);

            var ticketlist = _iTicketQueries
                .GetAllDeletedAgentManagerEndTicketList(agentsId, page, pageSize, departmentId, search, searchIn,
                    priorityId)
                .ToList();

            ViewBag.totalticketCount = totalticketCount;
            var pagedList =
                new StaticPagedList<AgentManagerTicketGridViewModel>(ticketlist, pageIndex + 1, pageSize,
                    totalticketCount);

            usergridview.ListofTicketDetails = pagedList;
            usergridview.CurrentPage = page;
            usergridview.DepartmentId = departmentId;
            usergridview.PriorityId = priorityId;
            usergridview.Search = search;
            usergridview.SearchIn = searchIn;
            usergridview.AgentsId = agentsId;
            return View(usergridview);
        }

        public ActionResult GetUsers(string usernames)
        {
            var userlist = _iUserMasterQueries.GetAutoCompleteUserName(usernames, Convert.ToInt32(StatusMain.Roles.User));
            return Json(userlist);
        }

        [HttpGet]
        public IActionResult AllClosedTickets(int? departmentId, string search,
           int? searchIn, int? priorityId, long? agentsId, int? page = 1)
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

            var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);

            if (departmentId == null)
            {
                var defaultdepartment = _departmentQueries.GetAssignedDepartmentsofAgentManager(userId).FirstOrDefault()?.Value;
                departmentId = Convert.ToInt32(defaultdepartment);
            }

            var usergridview = new AgentManagerTicketGrid();
            usergridview.ListofDepartment = _departmentQueries.GetAssignedDepartmentsofAgentManager(userId);
            usergridview.ListofSearch = ListofSearchFields();
            usergridview.ListofPriority = _iPriorityQueries.GetAllPrioritySelectListItem();
            usergridview.ListofStatus = _statusQueries.GetAllStatusSelectListItem();

            var agentlist = _iUserMasterQueries.GetListofAgents(departmentId);
            usergridview.ListofAgents = agentlist;

            var pageIndex = (page ?? 1) - 1;
            var pageSize = 10;
            var totalticketCount = 0;

            totalticketCount =
                _iTicketQueries.GetAllClosedAgentManagerEndTicketsCount(agentsId, search, searchIn, departmentId,
                    priorityId);

            var ticketlist = _iTicketQueries
                .GetAllClosedAgentManagerEndTicketList(agentsId, page, pageSize, departmentId, search, searchIn,
                    priorityId)
                .ToList();

            ViewBag.totalticketCount = totalticketCount;
            var pagedList =
                new StaticPagedList<AgentManagerTicketGridViewModel>(ticketlist, pageIndex + 1, pageSize,
                    totalticketCount);

            usergridview.ListofTicketDetails = pagedList;
            usergridview.CurrentPage = page;
            usergridview.DepartmentId = departmentId;
            usergridview.PriorityId = priorityId;
            usergridview.Search = search;
            usergridview.SearchIn = searchIn;
            usergridview.AgentsId = agentsId;
            return View(usergridview);
        }
    }
}
