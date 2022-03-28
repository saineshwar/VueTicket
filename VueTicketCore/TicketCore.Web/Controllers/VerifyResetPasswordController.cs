using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TicketCore.Data.EmailVerification.Queries;
using TicketCore.Web.Helpers;

namespace TicketCore.Web.Controllers
{
    public class VerifyResetPasswordController : Controller
    {
        private readonly IVerificationQueries _verificationQueries;
        private readonly ILogger<VerifyResetPasswordController> _logger;
        public VerifyResetPasswordController(IVerificationQueries verificationQueries, ILogger<VerifyResetPasswordController> logger)
        {
            _verificationQueries = verificationQueries;
            _logger = logger;
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
                        // arrayVakue[1] "UserId"

                        var userId = Convert.ToInt32(arrayVakue[1]);
                        var generatedToken = _verificationQueries.GetResetGeneratedTokenbyUnq(userId);
                        if (!string.IsNullOrEmpty(generatedToken))
                        {
                            var result = SecurityTokenHelper.IsTokenValid(arrayVakue, hashtoken, generatedToken);

                            if (result == 1)
                            {
                                TempData["TokenMessage"] = "Sorry Verification Link Expired Please request a new Verification link!";
                                return RedirectToAction("Login", "Portal");
                            }

                            if (result == 2)
                            {
                                TempData["TokenMessage"] = "Sorry Verification Link Expired Please request a new Verification link!";
                                return RedirectToAction("Login", "Portal");
                            }

                            if (result == 0)
                            {
                                HttpContext.Session.SetInt32("VerificationUserId", Convert.ToInt32(arrayVakue[1]));
                                HttpContext.Session.SetString("VerificationGeneratedToken", generatedToken);
                                HttpContext.Session.SetString("ActiveVerification", "1");
                                return RedirectToAction("Reset", "ResetPassword");
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
