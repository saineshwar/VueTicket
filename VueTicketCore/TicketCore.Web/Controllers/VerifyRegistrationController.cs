using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using TicketCore.Data.EmailVerification.Command;
using TicketCore.Data.EmailVerification.Queries;
using TicketCore.Web.Helpers;

namespace TicketCore.Web.Controllers
{
    public class VerifyRegistrationController : Controller
    {
        private readonly IVerificationQueries _verificationQueries;
        private readonly ILogger<VerifyResetPasswordController> _logger;
        private readonly IVerificationCommand _verificationCommand;
        public VerifyRegistrationController(IVerificationQueries verificationQueries, ILogger<VerifyResetPasswordController> logger, IVerificationCommand verificationCommand)
        {
            _verificationQueries = verificationQueries;
            _logger = logger;
            _verificationCommand = verificationCommand;
        }

        [HttpGet]
        public IActionResult Verify(string key, string hashtoken)
        {
            try
            {
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(hashtoken))
                {
                    var arrayVakue = SecurityTokenHelper.SplitToken(key);
                    if (arrayVakue != null)
                    {
                        var userId = Convert.ToInt32(arrayVakue[1]);
                        var generatedToken = _verificationQueries.GetEmailVerificationCodeGeneratedToken(userId);

                        if (generatedToken != null)
                        {
                            if (generatedToken.Verified)
                            {
                                TempData["TokenErrorMessage"] = "EmailId Is Already Verified!";
                                return RedirectToAction("Login", "Portal");
                            }

                            if (!string.IsNullOrEmpty(generatedToken.VerificationCode))
                            {
                                var result = SecurityTokenHelper.IsTokenValid(arrayVakue, hashtoken, generatedToken.VerificationCode);

                                if (result == 1)
                                {
                                    TempData["TokenErrorMessage"] = "Sorry Verification Link Expired Please request a new Verification link!";
                                    return RedirectToAction("Login", "Portal");
                                }

                                if (result == 2)
                                {
                                    TempData["TokenErrorMessage"] = "Sorry Verification Link Expired Please request a new Verification link!";
                                    return RedirectToAction("Login", "Portal");
                                }

                                if (result == 0)
                                {
                                    var resultemail = _verificationCommand.UpdatedVerificationCode(generatedToken.EmailVerificationId);
                                    if (resultemail)
                                    {
                                        TempData["TokenMessage"] = "Email Verified Successfully!";
                                        return RedirectToAction("Login", "Portal");
                                    }

                                }

                            }
                        }

                        
                        
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "VerifyResetPasswordController:Verify");
                TempData["TokenMessage"] = "Sorry Verification Failed Please request a new Verification link!";
                return RedirectToAction("Login", "Portal");


            }

            TempData["TokenMessage"] = "Sorry Verification Failed Please request a new Verification link!";
            return RedirectToAction("Login", "Portal");
        }
    }
}
