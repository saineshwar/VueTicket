using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using TicketCore.Common;
using TicketCore.Core;
using TicketCore.Data.Masters.Queries;
using TicketCore.Data.Profiles.Queries;
using TicketCore.Data.Tickets.Command;
using TicketCore.Data.Tickets.Queries;
using TicketCore.Data.Usermaster.Queries;
using TicketCore.Models.Tickets;
using TicketCore.Services.AwsHelper;
using TicketCore.Services.MailHelper;
using TicketCore.ViewModels.Tickets;
using TicketCore.Web.Extensions;
using TicketCore.Web.Filters;
using TicketCore.Web.Messages;

namespace TicketCore.Web.Areas.User.Controllers
{
    [SessionTimeOut]
    [AuthorizeAgent]
    [Area("User")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class MyTicketDetailsController : Controller
    {
        private readonly ITicketViewQueries _ticketViewQueries;
        private readonly INotificationService _notificationService;
        private readonly ITicketsReplyQueries _ticketsReplyQueries;
        private readonly IProfileQueries _profileQueries;
        private readonly ITicketCommand _iTicketCommand;
        private readonly ITicketHistoryHelper _iTicketHistoryHelper;
        private readonly ITicketHistoryCommand _ticketHistoryCommand;
        private readonly ITicketQueries _iTicketQueries;
        private readonly IStatusQueries _statusQueries;
        private readonly IPriorityQueries _iPriorityQueries;
        private readonly AppSettingsProperties _appsettings;
        private readonly AwsSettings _awsSettings;
        private readonly IAwsS3HelperService _awsS3HelperService;
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly IApplicationMailingService _iApplicationMailingService;
        public MyTicketDetailsController(ITicketViewQueries ticketViewQueries,
            INotificationService notificationService,
            ITicketsReplyQueries ticketsReplyQueries,
            IProfileQueries profileQueries,
            ITicketCommand iTicketCommand,
            ITicketHistoryHelper iTicketHistoryHelper,
            ITicketHistoryCommand ticketHistoryCommand,
            ITicketQueries iTicketQueries,
            IStatusQueries statusQueries,
            IPriorityQueries iPriorityQueries,
            IOptions<AppSettingsProperties> appsettings,
            IAwsS3HelperService awsS3HelperService,
            IOptions<AwsSettings> awsSettings, IUserMasterQueries userMasterQueries, IApplicationMailingService iApplicationMailingService)
        {
            _ticketViewQueries = ticketViewQueries;
            _notificationService = notificationService;
            _ticketsReplyQueries = ticketsReplyQueries;
            _profileQueries = profileQueries;
            _iTicketCommand = iTicketCommand;
            _iTicketHistoryHelper = iTicketHistoryHelper;
            _ticketHistoryCommand = ticketHistoryCommand;
            _iTicketQueries = iTicketQueries;
            _statusQueries = statusQueries;
            _iPriorityQueries = iPriorityQueries;
            _awsSettings = awsSettings.Value;
            _appsettings = appsettings.Value;
            _awsS3HelperService = awsS3HelperService;
            _userMasterQueries = userMasterQueries;
            _iApplicationMailingService = iApplicationMailingService;
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
                    ListofStatus = _statusQueries.GetAllAgentStatusSelectListItem()
                };

                displayticketViewModel.ViewMainModel = new ViewTicketReplyMainModel()
                {
                    ListofReplyAttachment = new List<ReplyAttachmentModel>(),
                    ListofTicketreply = _ticketsReplyQueries.ListofHistoryTicketReplies(id)
                };

                var expressChangesViewModel = new ExpressChangesTicketViewModel();
                expressChangesViewModel.ListofPriority = _iPriorityQueries.GetAllPrioritySelectListItem();
                expressChangesViewModel.ListofStatus = _statusQueries.GetAllAgentStatusSelectListItem();
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

        [HttpPost]
        public async Task<IActionResult> TicketReply(TicketReplyViewModel ticketReplyVm)
        {
            var statusId = ticketReplyVm.StatusId;
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
                            CreatedOn = DateTime.Now,

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
                CreatedBy = userId,
                TicketReplyId = 0,
                SystemUser = userId,
                TicketUser = 0
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
            latestreply.StatusInfo = "Agent responded";

            var result = await _iTicketCommand.AddTicketReply(userId, ticketReplyModel, ticketReplyDetails,
                listofattachments, currentticketstatus, latestreply);
            if (result)
            {
                var responsestatus = _iTicketQueries.GetCurrentStatusResponse(ticketid);

                if (responsestatus.FirstResponseStatus)
                {
                    await _iTicketCommand.UpdateResponseStatus(userId, ticketReplyVm.TicketId, (int)StatusMain.Status.First_Response_Overdue);
                }
                else if (responsestatus.EveryResponseStatus)
                {
                    await _iTicketCommand.UpdateResponseStatus(userId, ticketReplyVm.TicketId, (int)StatusMain.Status.Every_Response_Overdue);
                }
                else if (responsestatus.ResolutionStatus)
                {
                    await _iTicketCommand.UpdateResponseStatus(userId, ticketReplyVm.TicketId, (int)StatusMain.Status.Resolution_Overdue);
                }
                else if (responsestatus.EscalationStage1Status)
                {
                    await _iTicketCommand.UpdateResponseStatus(userId, ticketReplyVm.TicketId, (int)StatusMain.Status.Escalation_Stage_1);
                }
                else if (responsestatus.EscalationStage2Status)
                {
                    await _iTicketCommand.UpdateResponseStatus(userId, ticketReplyVm.TicketId, (int)StatusMain.Status.Escalation_Stage_2);
                }

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

                string url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/User/Ticket/TicketDetails/{ticketReplyVm.TicketId}";

                var fullName = HttpContext.Session.GetString(AllSessionKeys.FullName);

                _iApplicationMailingService.TicketAgentReplyEmail(url, "C:MyTicketDetails | A:TicketReply", ticketReplyVm.TicketId, "Replied", fullName, userId);


                return Json(new { Result = "success", Message = "Replied on Ticket Successfully!" });

            }
            else
            {
                return Json(new
                { Result = "failed", Message = "Something went wrong while Replied on Ticket. Please Try again!" });
            }
        }

        public IActionResult GetTicketActivities(TicketHistoryRequest requestBug)
        {
            var listofticketHistory = _ticketViewQueries.ListofTicketHistorybyTicket(requestBug.Ticketid);
            return PartialView("_TicketActivities", listofticketHistory);
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
                            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet,
                                document.GenerateAttachmentName);
                        }
                        else if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                        {
                            var response =
                                await _awsS3HelperService.ReadFileAsync(document.GenerateAttachmentName, "documents");
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
                var document =
                    _iTicketQueries.GetAttachmentsByticketId(requestAttachments.TicketId,
                        requestAttachments.AttachmentsId);
                var documentdetail = _iTicketQueries.GetAttachmentDetailsByAttachmentId(requestAttachments.TicketId,
                    requestAttachments.AttachmentsId);

                if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                {
                    var status =
                        await _awsS3HelperService.RemoveFileAsync(document.GenerateAttachmentName, "documents");
                }

                var result = _iTicketCommand.DeleteAttachmentByAttachmentId(document, documentdetail);

                if (result)
                {
                    var fullName = HttpContext.Session.GetString(AllSessionKeys.FullName);
                    var user = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);

                    var ticketHistory = new TicketHistoryModel
                    {
                        UserId = user,
                        Message = _iTicketHistoryHelper.DeleteTicketAttachment(
                            Convert.ToString(requestAttachments.TicketId)),
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
                            var documentdetail =
                                _iTicketQueries.GetReplyAttachmentDetailsByAttachmentId(ticketId,
                                    document.ReplyAttachmentId);
                            byte[] bytes = System.Convert.FromBase64String(documentdetail.AttachmentBase64);
                            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet,
                                document.GenerateAttachmentName);
                        }
                        else if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                        {
                            var response =
                                await _awsS3HelperService.ReadFileAsync(document.GenerateAttachmentName, "documents");
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
                var document = _iTicketQueries.GetReplyAttachmentsByTicketId(requestAttachments.TicketId,
                    requestAttachments.AttachmentsId);
                var documentdetail =
                    _iTicketQueries.GetReplyAttachmentDetailsByAttachmentId(requestAttachments.TicketId,
                        requestAttachments.AttachmentsId);

                if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                {
                    var status =
                        await _awsS3HelperService.RemoveFileAsync(document.GenerateAttachmentName, "documents");
                }

                var result = _iTicketCommand.DeleteReplyAttachmentByAttachmentId(document, documentdetail);
                if (result)
                {
                    var fullName = HttpContext.Session.GetString(AllSessionKeys.FullName);
                    var user = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);


                    var ticketHistory = new TicketHistoryModel
                    {
                        UserId = user,
                        Message = _iTicketHistoryHelper.DeleteTicketReplyAttachment(
                            Convert.ToString(requestAttachments.TicketId)),
                        ProcessDate = DateTime.Now,
                        TicketId = requestAttachments.TicketId,
                        PriorityId = null,
                        StatusId = null,
                        DepartmentId = null,
                        ActivitiesId = Convert.ToInt16(StatusMain.Activities.DeleteReplyAttachment),
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
        public JsonResult ChangePriority(RequestChangePriority requestChangePriority)
        {
            try
            {
                var result = _iTicketCommand.ChangeTicketPriority(requestChangePriority);
                if (result)
                {

                    var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                  
                    var ticketHistory = new TicketHistoryModel
                    {
                        UserId = userId,
                        Message = _iTicketHistoryHelper.PriorityMessage(requestChangePriority.PriorityId),
                        ProcessDate = DateTime.Now,
                        TicketId = requestChangePriority.TicketId,
                        PriorityId = requestChangePriority.PriorityId,
                        DepartmentId = null,
                        StatusId = null,
                        ActivitiesId = Convert.ToInt16(StatusMain.Activities.PriorityChanged)
                    };
                    _ticketHistoryCommand.TicketHistory(ticketHistory);


                    string url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/User/Ticket/TicketDetails/{requestChangePriority.TicketId}";
                    var priority = _iPriorityQueries.GetPriorityNameBypriorityId(requestChangePriority.PriorityId);
                    var fullName = HttpContext.Session.GetString(AllSessionKeys.FullName);
                    _iApplicationMailingService.ChangeTicketPriorityEmail(url, "C:MyTicketDetails | A:ChangePriority", requestChangePriority.TicketId, priority, fullName, userId);

                    return Json(new { Result = "success", Message = "Changed Ticket Priority Successfully!" });
                }
                else
                {
                    return Json(new
                    { Result = "failed", Message = "Something went wrong while Replied on Ticket. Please Try again!" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> ChangeStatus(RequestTicketDetails requestTicket)
        {
            try
            {
                var currentticketstatus = await _iTicketQueries.GetTicketStatusbyTicketId(requestTicket.TicketId);
                var result = await _iTicketCommand.ReOpenTicket(requestTicket.TicketId);

                if (result)
                {
                    var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);

                    var ticketHistory = new TicketHistoryModel
                    {
                        UserId = userId,
                        Message = _iTicketHistoryHelper.StatusMessage((int)StatusMain.Status.ReOpened),
                        ProcessDate = DateTime.Now,
                        TicketId = requestTicket.TicketId,
                        PriorityId = null,
                        DepartmentId = null,
                        StatusId = (int)StatusMain.Status.ReOpened,
                        ActivitiesId = Convert.ToInt16(StatusMain.Activities.StatusChanged)
                    };

                    _ticketHistoryCommand.TicketHistory(ticketHistory);

                    return Json(new { Result = "success", Message = "Changed Ticket Status Successfully!" });
                }
                else
                {
                    return Json(new
                    { Result = "failed", Message = "Something went wrong while Replied on Ticket. Please Try again!" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        public async Task<IActionResult> AssignTickettoUser(string ticketId, long userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(ticketId))
                {
                    var currentticketAssignedUser = await _iTicketQueries.GetTicketStatusbyTicketId(Convert.ToInt64(ticketId));

                    if (currentticketAssignedUser.AssignedTicketUserId == userId)
                    {
                        return Json(new { status = "validation" });
                    }


                    var result = await _iTicketCommand.UpdateAssignTickettoUser(userId, Convert.ToInt64(ticketId));
                    if (result)
                    {
                        var ticketnameassignedUserName = _userMasterQueries.GetUserById(userId);

                        var ticketHistory = new TicketHistoryModel
                        {
                            UserId = userId,
                            Message = $"{HistoryTicketCommonMessages.Message6} {ticketnameassignedUserName.FirstName} {ticketnameassignedUserName.LastName}",
                            TicketId = Convert.ToInt64(ticketId),
                            ActivitiesId = Convert.ToInt16(StatusMain.Activities.ManuallyAssigedTicket),
                            ProcessDate = DateTime.Now
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
            catch (Exception)
            {
                return Json(new { status = "failed" });
            }
        }


        public ActionResult GetAllAgentUsers(string usernames, int? departmentId)
        {
            var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
            var userlist = _userMasterQueries.GetAutoCompleteAgentandAdminUserName(usernames, Convert.ToInt32(StatusMain.Roles.Agent), userId, departmentId);
            return Json(userlist);
        }

        [HttpGet]
        public IActionResult TicketDetailsView(long? id)
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
                    ListofStatus = _statusQueries.GetAllAgentStatusSelectListItem()
                };

                displayticketViewModel.ViewMainModel = new ViewTicketReplyMainModel()
                {
                    ListofReplyAttachment = new List<ReplyAttachmentModel>(),
                    ListofTicketreply = _ticketsReplyQueries.ListofHistoryTicketReplies(id)
                };

                var expressChangesViewModel = new ExpressChangesTicketViewModel();
                expressChangesViewModel.ListofPriority = _iPriorityQueries.GetAllPrioritySelectListItem();
                expressChangesViewModel.ListofStatus = _statusQueries.GetAllAgentStatusSelectListItem();
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
    }
}
