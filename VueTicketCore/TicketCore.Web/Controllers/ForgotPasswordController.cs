
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Web;
using TicketCore.Common;
using TicketCore.Data.EmailVerification.Command;
using TicketCore.Data.Usermaster.Queries;
using TicketCore.Models.Usermaster;
using TicketCore.Models.Verification;
using TicketCore.Services.MailHelper;
using TicketCore.ViewModels.LoginVM;
using TicketCore.ViewModels.Usermaster;
using TicketCore.Web.Helpers;

namespace TicketCore.Web.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly IMailingService _mailingService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IVerificationCommand _verificationCommand;
        private readonly ILogger<ForgotPasswordController> _logger;

        public ForgotPasswordController(
            IUserMasterQueries userMasterQueries,
            IMailingService mailingService,
            IWebHostEnvironment webHostEnvironment, IVerificationCommand verificationCommand, ILogger<ForgotPasswordController> logger)
        {
            _userMasterQueries = userMasterQueries;
            _mailingService = mailingService;
            _webHostEnvironment = webHostEnvironment;
            _verificationCommand = verificationCommand;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Recover()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "Please enter valid security code")]
        public IActionResult Recover(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (!_userMasterQueries.CheckEmailIdExists(forgotPasswordViewModel.EmailId))
            {
                TempData["ForgotPasswordErrorMessage"] = "Entered EmailId is Invalid";
            }
            else
            {
                try
                {
                    var verifyuser = _userMasterQueries.CheckEmailIdExists(forgotPasswordViewModel.EmailId);

                    if (verifyuser)
                    {
                        var userdetails = _userMasterQueries.GetUserdetailsbyEmailId(forgotPasswordViewModel.EmailId);

                        var token = HashHelper.CreateHashSHA256((GenerateRandomNumbers.GenerateRandomDigitCode(6)));

                        var commonDatetime = DateTime.Now;
                        var sendingresult = CreateVerificationEmail(userdetails, token, commonDatetime);
                        if (sendingresult == "success")
                        {


                            ResetPasswordVerification resetPasswordVerification = new ResetPasswordVerification()
                            {
                                UserId = userdetails.UserId,
                                GeneratedDate = commonDatetime,
                                GeneratedToken = token
                            };
                            var emailresult = _verificationCommand.InsertResetPasswordVerificationToken(resetPasswordVerification);

                            TempData["ForgotPasswordMessage"] = "An email has been sent to the address you have registered." +
                                                                "Please follow the link in the email to complete your password reset request";
                        }
                        else
                        {
                            TempData["ForgotPasswordErrorMessage"] = "Somthing Went Wrong in Sending Email Please Try after sometime.";
                        }

                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ForgotPasswordController:Recover");
                }

                return RedirectToAction("Recover", "ForgotPassword");
            }

            return View();
        }


        private string CreateVerificationEmail(UserMaster user, string token, DateTime commonDateTime)
        {
            try
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "forgotpassword.html");

                string mailText;
                using (var streamReader = new StreamReader(filePath))
                {
                    mailText = streamReader.ReadToEnd();
                    streamReader?.Close();
                }

                var aesAlgorithm = new AesAlgorithm();
                var key = string.Join(":", new string[] { commonDateTime.Ticks.ToString(), user.UserId.ToString() });
                var encrypt = aesAlgorithm.EncryptToBase64String(key);

                string linktoverify = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/VerifyResetPassword/Verify?key={HttpUtility.UrlEncode(encrypt)}&hashtoken={HttpUtility.UrlEncode(token)}";

                var stringtemplate = new StringBuilder();
                stringtemplate.Append("Please click the following link to reset your password.");
                stringtemplate.Append("<br/>");
                stringtemplate.Append($"Reset password link : <a target='_blank' href={linktoverify}>Reset Password Link</a>");
                stringtemplate.Append("<br/>");
                stringtemplate.Append("<br/>");
                stringtemplate.Append("If the link does not work, copy and paste the URL into a new browser window. The URL will expire in 24 hours for security reasons.");
                stringtemplate.Append("<br/>");

                mailText = mailText.Replace("[XXXXXXXXXXXXXXXXXXXXX]", stringtemplate.ToString());
                mailText = mailText.Replace("[##Name##]", $"{ user.FirstName} { user.LastName}");

                var sendingMailRequest = new SendingMailRequest()
                {
                    Attachments = null,
                    ToEmail = user.EmailId,
                    Body = mailText,
                    Subject = $"Reset Password"
                };


                _mailingService.SendEmailAsync(sendingMailRequest);

                return "success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ForgotPasswordController:CreateVerificationEmail");
                return "failed";
                throw;
            }
        }
    }
}
