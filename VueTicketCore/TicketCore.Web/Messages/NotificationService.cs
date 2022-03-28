using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using TicketCore.Common;
using TicketCore.Web.Extensions;

namespace TicketCore.Web.Messages
{
    public class NotificationService : INotificationService
    {


        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NotificationService(ITempDataDictionaryFactory tempDataDictionaryFactory, IHttpContextAccessor httpContextAccessor)
        {
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public void SuccessNotification(string alertTitle, string message, bool encode = true)
        {
            PrepareTempData(alertTitle, NotificationType.success, message, encode);
        }

        public void WarningNotification(string alertTitle, string message, bool encode = true)
        {
            PrepareTempData(alertTitle, NotificationType.warning, message, encode);
        }

        public void DangerNotification(string alertTitle, string message, bool encode = true)
        {
            PrepareTempData(alertTitle, NotificationType.danger, message, encode);
        }

        public void InformationNotification(string alertTitle, NotificationType type, string message, bool encode = true)
        {
            PrepareTempData(alertTitle, NotificationType.info, message, encode);
        }

        private void PrepareTempData(string alertTitle, NotificationType type, string message, bool encode = true)
        {

            string key = "Notification";
            string notificationListKey = "";
            var context = _httpContextAccessor.HttpContext;
            var tempData = _tempDataDictionaryFactory.GetTempData(context);
            if (_httpContextAccessor.HttpContext != null)
            {
                var userId = _httpContextAccessor.HttpContext.Session.GetSession<long>(AllSessionKeys.UserId);
                notificationListKey = $"{key}_{userId}";
            }

            //Messages have stored in a serialized list
            var messages = tempData.ContainsKey(notificationListKey)
                ? JsonConvert.DeserializeObject<IList<NotificationData>>(tempData[notificationListKey].ToString() ?? string.Empty)
                : new List<NotificationData>();

            messages.Add(new NotificationData
            {
                Message = message,
                Type = type,
                Encode = encode,
                AlertTitle = alertTitle
            });

            tempData[notificationListKey] = JsonConvert.SerializeObject(messages);
        }


    }
}