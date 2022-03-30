using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using TicketCore.Common;
using TicketCore.Data.Profiles.Command;
using TicketCore.Data.Profiles.Queries;
using TicketCore.Data.Usermaster.Command;
using TicketCore.Data.Usermaster.Queries;
using TicketCore.Models.Profiles;
using TicketCore.ViewModels.Profiles;
using TicketCore.ViewModels.Usermaster;
using TicketCore.Web.Extensions;
using TicketCore.Web.Filters;
using TicketCore.Web.Helpers;
using TicketCore.Web.Messages;

namespace TicketCore.Web.Areas.Administration.Controllers
{
    [SessionTimeOut]
    [AuthorizeSuperAdmin]
    [Area("Administration")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class ProfileController : Controller
    {
        private readonly IProfileQueries _profileQueries;
        private readonly IProfileCommand _profileCommand;
        private readonly IUserMasterCommand _userMasterCommand;
      
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly INotificationService _notificationService;

        public ProfileController(IProfileQueries profileQueries,
            IProfileCommand profileCommand,
            IUserMasterCommand userMasterCommand,
            IUserMasterQueries userMasterQueries, INotificationService notificationService)
        {
            _profileQueries = profileQueries;
            _profileCommand = profileCommand;
            _userMasterCommand = userMasterCommand;
         
            _userMasterQueries = userMasterQueries;
            _notificationService = notificationService;
        }

        public IActionResult AdminProfile()
        {
            try
            {
                var userid = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                var adminprofile = _profileQueries.GetprofileById(userid);
                return View(adminprofile);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult AdminProfile(UsermasterEditView usermasterEditView)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userid = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                    var result = _profileCommand.UpdateUserMasterDetails(userid, usermasterEditView);

                    if (result != 0)
                    {
                        TempData["MessageProfileUpdate"] = CommonMessages.UserDetailsUpdateSuccessMessages;
                        return RedirectToAction("AdminProfile");
                    }
                    else
                    {
                        return View(usermasterEditView);
                    }
                }
                else
                {
                    return View(usermasterEditView);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        public IActionResult Profileimage(IFormFile files)
        {
            var userid = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);

            try
            {

                ProfileImage profileImage = new ProfileImage { ProfileImageId = 0 };
                string data = string.Empty;
                var fileContent = files;
                if (fileContent != null && fileContent.Length > 0)
                {
                    // and optionally write the file to disk
                    var fileName = Path.GetFileName(fileContent.FileName);
                    var fileextension = Path.GetExtension(fileContent.FileName);

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        using var target = new MemoryStream();
                        files.CopyTo(target);
                        data = Convert.ToBase64String(target.ToArray());
                    }
                }

                profileImage.ProfileImageBase64String = data;
                profileImage.CreatedDate = DateTime.Now;
                profileImage.UserId = userid;
                _profileCommand.UpdateProfileImage(profileImage);


            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }

            return Json("File uploaded successfully");
        }

        [HttpPost]
        public IActionResult CheckIsProfileImageExists()
        {
            try
            {
                var userid = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                var result = _profileQueries.IsProfileImageExists(userid);
                if (result)
                {
                    var imageBase64String = _profileQueries.GetProfileImageBase64String(userid);
                    var tempimageBase64String = string.Concat("data:image/png;base64,", imageBase64String);
                    return Json(new { result = true, imagepath = tempimageBase64String });
                }
                else
                {

                    return Json(new { result = false, imagepath = "/img/user.png" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult DeleteProfilepicture()
        {
            try
            {
                var userid = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                var isexists = _profileQueries.IsProfileImageExists(userid);
                if (isexists)
                {
                    var result = _profileCommand.DeleteProfileImage(userid);
                    if (result > 0)
                    {
                        return Json(new { result = true });
                    }
                    else
                    {
                        return Json(new { result = false });
                    }
                }
                else
                {
                    return Json(new { result = false });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        public IActionResult UpdateSignature(ViewSignatureRequestViewModel viewSignatureRequestViewModel)
        {
            try
            {
                var userid = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                if (!string.IsNullOrEmpty(viewSignatureRequestViewModel.Signature))
                {
                    if (_profileQueries.CheckSignatureAlreadyExists(userid))
                    {
                        var signature = _profileQueries.GetSignatureDetails(userid);
                        int result = _profileCommand.DeleteSignature(signature);

                        if (result != 0)
                        {
                            Signatures signatures = new Signatures()
                            {
                                UserId = userid,
                                Signature = viewSignatureRequestViewModel.Signature,
                                SignatureId = 0
                            };

                            _profileCommand.UpdateSignature(signatures);

                            return Json(true);
                        }

                        return Json(false);
                    }
                    else
                    {
                        Signatures signatures = new Signatures()
                        {
                            UserId = userid,
                            Signature = viewSignatureRequestViewModel.Signature,
                            SignatureId = 0
                        };

                        _profileCommand.UpdateSignature(signatures);

                        return Json(true);
                    }
                }

                return Json(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult GetSignature()
        {
            try
            {
                var userid = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                var result = _profileQueries.CheckSignatureAlreadyExists(userid);
                if (result)
                {
                    var signature = _profileQueries.GetSignature(userid);
                    return Json(new { result = true, values = signature });
                }
                else
                {
                    return Json(new { result = false, values = "" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

       
    }
}
