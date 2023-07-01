using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TicketCore.Common;
using TicketCore.Data.EmailVerification.Command;
using TicketCore.Data.Usermaster.Command;
using TicketCore.Data.Usermaster.Queries;
using TicketCore.Models.Usermaster;
using TicketCore.Services.MailHelper;
using TicketCore.ViewModels.Signup;
using TicketCore.Web.Extensions;
using TicketCore.Web.Helpers;
using TicketCore.Web.Messages;

namespace TicketCore.Web.Controllers
{
    public class SignupController : Controller
    {
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly IUserMasterCommand _userMasterCommand;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMailingService _mailingService;
        private readonly IVerificationCommand _verificationCommand;
        public SignupController(IUserMasterQueries userMasterQueries,
            IUserMasterCommand userMasterCommand,
            IWebHostEnvironment webHostEnvironment,
            IMailingService mailingService, IVerificationCommand verificationCommand)
        {
            _userMasterQueries = userMasterQueries;
            _userMasterCommand = userMasterCommand;

            _webHostEnvironment = webHostEnvironment;
            _mailingService = mailingService;
            _verificationCommand = verificationCommand;
        }
        [HttpGet]
        public IActionResult Form()
        {
            return View();
        }

        [HttpPost]
        [ValidateDNTCaptcha(ErrorMessage = "Please enter the Valid security code.")]
        public IActionResult Form(SignupFormViewModel signupFormViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_userMasterQueries.CheckUsernameExists(signupFormViewModel.UserName))
                {
                    TempData["Signup_Message_Error"] = "Username Already Exists";
                }
                else if(_userMasterQueries.CheckEmailIdExists(signupFormViewModel.EmailId))
                {
                    TempData["Signup_Message_Error"] = "EmailId Already Exists";
                }
                else if (_userMasterQueries.CheckMobileNoExists(signupFormViewModel.MobileNo))
                {
                    TempData["Signup_Message_Error"] = "MobileNo Already Exists";
                }
                else
                {
                    if (!string.Equals(signupFormViewModel.Password, signupFormViewModel.ConfirmPassword,
                            StringComparison.Ordinal))
                    {
                        TempData["Signup_Message_Error"] = "Password Does not Match!";
                        return View(signupFormViewModel);
                    }
                    else
                    {
                        var usermaster = new UserMaster
                        {
                            UserName = signupFormViewModel.UserName,
                            EmailId = signupFormViewModel.EmailId,
                            PasswordHash = signupFormViewModel.Password,
                            MobileNo = signupFormViewModel.MobileNo,
                            Status = true,
                            CreatedOn = DateTime.Now,
                            UserId = 0,
                            CreatedBy = 0
                        };


                        var userId = _userMasterCommand.AddUser(usermaster, (int)RolesHelper.Roles.User);
                        if (userId != -1)
                        {
                            var token = GenerateRandomNumbers.GenerateRandomDigitCode(6);
                            var hashtoken = GenerateHashSha256.ComputeSha256Hash(token);

                            var resultemail = _verificationCommand.InsertEmailVerification(userId, signupFormViewModel.EmailId, hashtoken);
                           
                            if (resultemail > 0)
                            {
                                var result = CreateVerificationEmail(usermaster, hashtoken, DateTime.Now);
                                TempData["Signup_Message_Success"] = "Your Account Has been Created Successfully";
                            }
                            
                            
                            return RedirectToAction("Form");
                        }
                    }
                }
            }

            return View(signupFormViewModel);
        }


        private string CreateVerificationEmail(UserMaster user, string token, DateTime commonDateTime)
        {
            try
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "newUser.html");

                string mailText;
                using (var streamReader = new StreamReader(filePath))
                {
                    mailText = streamReader.ReadToEnd();
                    streamReader?.Close();
                }

                var aesAlgorithm = new AesAlgorithm();
                var key = string.Join(":", new string[] { commonDateTime.Ticks.ToString(), user.UserId.ToString() });
                var encrypt = aesAlgorithm.EncryptToBase64String(key);

                string linktoverify = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/VerifyRegistration/Verify?key={HttpUtility.UrlEncode(encrypt)}&hashtoken={HttpUtility.UrlEncode(token)}";

                var stringtemplate = new StringBuilder();
                stringtemplate.Append("Your account has been successfully created!");
                stringtemplate.Append("<br/>");
                stringtemplate.Append("<br/>");
                stringtemplate.Append($"Your Username: {user.UserName}");
                stringtemplate.Append("<br/>");
                stringtemplate.Append("<br/>");
                stringtemplate.Append("Please click the following link to Verify Your EmailId.");
                stringtemplate.Append("<br/>");
                stringtemplate.Append($"Link : <a target='_blank' href={linktoverify}>Click here to Verify</a>");
                stringtemplate.Append("<br/>");
                stringtemplate.Append("<br/>");
                stringtemplate.Append("If the link does not work, copy and paste the URL into a new browser window. The URL will expire in 24 hours for security reasons.");
                stringtemplate.Append("<br/>");

                mailText = mailText.Replace("[XXXXXXXXXXXXXXXXXXXXX]", stringtemplate.ToString());
                mailText = mailText.Replace("[##Name##]", $"{ user.UserName}");

                var sendingMailRequest = new SendingMailRequest()
                {
                    Attachments = null,
                    ToEmail = user.EmailId,
                    Body = mailText,
                    Subject = $"Welcome to VueTicket",
                    CreatedBy = HttpContext.Session.GetSession<long>(AllSessionKeys.UserId),
                TriggeredEvent = "SignUp:CreateVerificationEmail"
                };


                _mailingService.SendEmailAsync(sendingMailRequest);

                return "success";
            }
            catch (Exception)
            {
                return "failed";
                throw;
            }
        }
    }
}
