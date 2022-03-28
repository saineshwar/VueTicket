using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TicketCore.Common;
using TicketCore.Data.SmtpEmailSetting.Command;
using TicketCore.Data.SmtpEmailSetting.Queries;
using TicketCore.Models.SmtpEmailSettings;

using TicketCore.ViewModels.Messages;
using TicketCore.ViewModels.SmtpEmailSettings;
using TicketCore.Web.Filters;
using TicketCore.Web.Messages;

namespace TicketCore.Web.Areas.Administration.Controllers
{
    [SessionTimeOut]
    [AuthorizeSuperAdmin]
    [Area("Administration")]
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class ApplicationSettingController : Controller
    {
       
        private readonly IMapper _mapper;
       
        private readonly IGeneralSettingsQueries _generalSettingsQueries;
        private readonly IGeneralSettingsCommand _generalSettingsCommand;
        private readonly INotificationService _notificationService;
        public ApplicationSettingController(
            IMapper mapper,
            IGeneralSettingsQueries generalSettingsQueries,
            IGeneralSettingsCommand generalSettingsCommand,
            INotificationService notificationService)
        {
    
            _mapper = mapper;
            
      
            _generalSettingsQueries = generalSettingsQueries;
            _generalSettingsCommand = generalSettingsCommand;
            _notificationService = notificationService;
        }
      

        [HttpGet]
        public ActionResult General()
        {
            try
            {
                var getsettings = _generalSettingsQueries.GetGeneralSetting();
                return View(getsettings);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult General(GeneralSettingsViewModel generalSettingsViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                }

                var generalSettings = _mapper.Map<GeneralSettings>(generalSettingsViewModel);
                var getsettings = _generalSettingsQueries.GetGeneralSetting();

                if (getsettings == null)
                {
                    _generalSettingsCommand.InsertGeneralSetting(generalSettings);
                    _notificationService.SuccessNotification("Message", "General Settings Saved Successfully!");
                }
                else
                {
                    _generalSettingsCommand.UpdateGeneralSetting(generalSettings);
                    _notificationService.SuccessNotification("Message", "General Settings Updated Successfully!");
                }

                return RedirectToAction("General", "ApplicationSetting");
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
