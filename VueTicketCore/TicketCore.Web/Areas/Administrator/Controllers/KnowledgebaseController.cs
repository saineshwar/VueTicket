using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using TicketCore.Common;
using TicketCore.Data.Department.Queries;
using TicketCore.Data.Knowledgebase.Command;
using TicketCore.Data.Knowledgebase.Queries;
using TicketCore.Models.Knowledgebase;
using TicketCore.Models.Tickets;
using TicketCore.Services.AwsHelper;
using TicketCore.ViewModels.Knowledgebase;
using TicketCore.ViewModels.Tickets;
using TicketCore.Web.Extensions;
using TicketCore.Web.Filters;
using TicketCore.Web.Messages;

namespace TicketCore.Web.Areas.Administrator.Controllers
{
    [SessionTimeOut]
    [AuthorizeAdministrator]
    [Area("Administrator")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class KnowledgebaseController : Controller
    {
        private readonly IDepartmentQueries _iDepartmentQueries;
        private readonly IKnowledgebaseQueries _iKnowledgebaseQueries;
        private readonly AppSettingsProperties _appsettings;
        private readonly AwsSettings _awsSettings;
        private readonly IAwsS3HelperService _awsS3HelperService;
        private readonly IKnowledgebaseCommand _iKnowledgebaseCommand;
        private readonly IKnowledgebaseTypeQueries _iKnowledgebaseTypeQueries;
        private readonly INotificationService _notificationService;

        public KnowledgebaseController(IDepartmentQueries departmentQueries, IKnowledgebaseQueries knowledgebaseQueries,
            IOptions<AppSettingsProperties> appsettings,
            IOptions<AwsSettings> awsSettings,
            IAwsS3HelperService awsS3HelperService, IKnowledgebaseCommand knowledgebaseCommand, IKnowledgebaseTypeQueries knowledgebaseTypeQueries, INotificationService notificationService)
        {
            _iDepartmentQueries = departmentQueries;
            _iKnowledgebaseQueries = knowledgebaseQueries;
            _awsS3HelperService = awsS3HelperService;
            _iKnowledgebaseCommand = knowledgebaseCommand;
            _iKnowledgebaseTypeQueries = knowledgebaseTypeQueries;
            _notificationService = notificationService;
            _awsSettings = awsSettings.Value;
            _appsettings = appsettings.Value;
        }

        [HttpGet]
        public ActionResult Add()
        {
            try
            {
                var knowledgebaseViewModel = new KnowledgebaseViewModel()
                {
                    ListofDepartment = _iDepartmentQueries.GetAllActiveSelectListItemDepartment(),
                    ListofKnowledgebaseType = _iKnowledgebaseTypeQueries.KnowledgebaseTypeList()
                };
                return View(knowledgebaseViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(KnowledgebaseViewModel knowledgebaseViewModel)
        {
            try
            {
                var knowledgebase = new KnowledgebaseModel();
                var knowledgebaseDetails = new KnowledgebaseDetails();

                if (ModelState.IsValid)
                {
                    var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                    knowledgebase.Subject = knowledgebaseViewModel.Subject;
                    knowledgebase.Status = knowledgebaseViewModel.Status;
                    knowledgebase.KnowledgebaseTypeId = knowledgebaseViewModel.KnowledgebaseTypeId;
                    knowledgebase.CreatedOn = DateTime.Now;
                    knowledgebase.KnowledgebaseId = 0;
                    knowledgebase.CreatedBy = userId;
                    knowledgebase.DepartmentId = knowledgebaseViewModel.DepartmentId;
                    knowledgebaseDetails.Contents = WebUtility.HtmlDecode(knowledgebaseViewModel.Contents);
                    knowledgebaseDetails.Keywords = knowledgebaseViewModel.Keywords;
                    knowledgebaseDetails.KnowledgebaseDetailsId = 0;

                    // ReSharper disable once CollectionNeverQueried.Local
                    var knowledgebaseAttachmentsDetailsList = new List<KnowledgebaseAttachmentViewModel>();

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

                                var attachments = new KnowledgebaseAttachmentViewModel();
                                attachments.KnowledgebaseId = 1;
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

                                knowledgebaseAttachmentsDetailsList.Add(attachments);

                                if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                                {
                                    bool status =
                                        await _awsS3HelperService.UploadFileAsync(target, newFileName, "documents");
                                }

                            }
                        }
                    }

                    var result = await _iKnowledgebaseCommand.AddKnowledgebase(knowledgebase, knowledgebaseAttachmentsDetailsList, knowledgebaseDetails);

                    if (result)
                    {
                        _notificationService.SuccessNotification("Message", "Your Article Added Successfully");
                        return RedirectToAction("Index");
                    }

                }

                knowledgebaseViewModel.ListofDepartment = _iDepartmentQueries.GetAllActiveSelectListItemDepartment();
                knowledgebaseViewModel.ListofKnowledgebaseType = _iKnowledgebaseTypeQueries.KnowledgebaseTypeList();
                return View(knowledgebaseViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GridAllKnowledgebase()
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

                var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                var usersdata = _iKnowledgebaseQueries.ShowAllKnowledgebase(sortColumn, sortColumnDirection, searchValue);
                recordsTotal = usersdata.Count();
                var data = usersdata.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var knowledgebaseViewModel = _iKnowledgebaseQueries.GetKnowledgeBaseDetailsbyId(id);
                knowledgebaseViewModel.ListofDepartment = _iDepartmentQueries.GetAllActiveSelectListItemDepartment();
                knowledgebaseViewModel.ListofKnowledgebaseType = _iKnowledgebaseTypeQueries.KnowledgebaseTypeList();
                knowledgebaseViewModel.ListofAttachments = _iKnowledgebaseQueries.GetListAttachmentsByKnowledgebaseId(id);
                return View(knowledgebaseViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditKnowledgebaseViewModel editKnowledgebaseViewModel)
        {
            editKnowledgebaseViewModel.ListofDepartment = _iDepartmentQueries.GetAllActiveSelectListItemDepartment();
            editKnowledgebaseViewModel.ListofKnowledgebaseType = _iKnowledgebaseTypeQueries.KnowledgebaseTypeList();
            var files = HttpContext.Request.Form.Files;
            var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
            if (ModelState.IsValid)
            {

                if (files.Any() &&
                    (_iKnowledgebaseQueries.KnowledgebaseAttachmentsExistbyknowledgebaseId(editKnowledgebaseViewModel.KnowledgebaseId)))
                {
                    _notificationService.DangerNotification("Message",
                        "All Delete Pervious uploaded Attachments for Uploading New Attachments");
                    return RedirectToAction("Edit", "MyTicket",
                        new { id = editKnowledgebaseViewModel.KnowledgebaseId });
                }

                var knowledgebase = new KnowledgebaseModel();
                var knowledgebaseDetails = new KnowledgebaseDetails();


                knowledgebase.Subject = editKnowledgebaseViewModel.Subject;
                knowledgebase.Status = editKnowledgebaseViewModel.Status;
                knowledgebase.KnowledgebaseTypeId = editKnowledgebaseViewModel.KnowledgebaseTypeId;
                knowledgebase.CreatedOn = DateTime.Now;
                knowledgebase.CreatedBy = userId;
                knowledgebase.DepartmentId = editKnowledgebaseViewModel.DepartmentId;
                knowledgebase.KnowledgebaseId = editKnowledgebaseViewModel.KnowledgebaseId;


                knowledgebaseDetails.Contents = WebUtility.HtmlDecode(editKnowledgebaseViewModel.Contents);
                knowledgebaseDetails.Keywords = editKnowledgebaseViewModel.Keywords;
                knowledgebaseDetails.KnowledgebaseDetailsId = editKnowledgebaseViewModel.KnowledgebaseDetailsId;
                knowledgebaseDetails.KnowledgebaseId = editKnowledgebaseViewModel.KnowledgebaseId;

                // ReSharper disable once CollectionNeverQueried.Local
                var knowledgebaseAttachmentsDetailsList = new List<KnowledgebaseAttachmentViewModel>();


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

                            var attachments = new KnowledgebaseAttachmentViewModel();
                            attachments.KnowledgebaseId = editKnowledgebaseViewModel.KnowledgebaseId;
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

                            knowledgebaseAttachmentsDetailsList.Add(attachments);

                            if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                            {
                                bool status =
                                    await _awsS3HelperService.UploadFileAsync(target, newFileName, "documents");
                            }

                        }
                    }
                }

                var result = await _iKnowledgebaseCommand.Update(knowledgebase, knowledgebaseAttachmentsDetailsList, knowledgebaseDetails);

                if (result)
                {
                    _notificationService.SuccessNotification("Message", "Your Article Updated Successfully");
                    return RedirectToAction("Index");
                }

            }



            return View(editKnowledgebaseViewModel);
        }

        public async Task<IActionResult> DownloadAttachment(long knowledgeId, long attachmentId)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(attachmentId)))
                {
                    var document = _iKnowledgebaseQueries.GetAttachments(knowledgeId, attachmentId);
                    if (document != null)
                    {
                        if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Database)
                        {
                            var documentdetail =
                                _iKnowledgebaseQueries.GetAttachmentDetailsByAttachmentId(knowledgeId, document.KnowledgebaseAttachmentsId);
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

                    return RedirectToAction("Index", "Knowledgebase");
                }
                else
                {
                    return RedirectToAction("Index", "Knowledgebase");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> DeleteAttachment(RequestKnowledgebaseAttachments requestAttachments)
        {
            try
            {
                var document =
                    _iKnowledgebaseQueries.GetAttachments(requestAttachments.KnowledgebaseId,
                        requestAttachments.AttachmentsId);
                var documentdetail = _iKnowledgebaseQueries.GetAttachmentDetailsByAttachmentId(requestAttachments.KnowledgebaseId, document.KnowledgebaseAttachmentsId);

                if (_appsettings.DocumentStorage == (int)StoragePropertiesHelper.Aws)
                {
                    var status =
                        await _awsS3HelperService.RemoveFileAsync(document.GenerateAttachmentName, "documents");
                }

                var result = await _iKnowledgebaseCommand.DeleteAttachmentByAttachmentId(document, documentdetail);

                if (result)
                {
                    var fullName = HttpContext.Session.GetString(AllSessionKeys.FullName);
                    var user = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);

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

        public async Task<IActionResult> ChangeStatus(RequestKnowledgebaseChange requestchange)
        {
            if (requestchange.Status)
            {
                var userId = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                var result = await _iKnowledgebaseCommand.UpdateStatus(requestchange, userId);

                if (result)
                {
                    return Json(new { Result = "success" });
                }
                else
                {
                    return Json(new { Result = "failed" });
                }
            }

            return Json(new { Status = true });
        }

       
    }
}
