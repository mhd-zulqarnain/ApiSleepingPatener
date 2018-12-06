using ApiSleepingPatener.Models;
using ApiSleepingPatener.Models.Notification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace ApiSleepingPatener.Controllers
{

    public class NotificationController : ApiController
    {
        [HttpGet]
        [Route("getnotificationlistbyuser/{userId}")]
        public IHttpActionResult GetNotificationListByUser(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<NotificationModel> List = new List<NotificationModel>();
            List = db.UserNotifications.Where(a => a.UserId.Value.Equals(userId) && a.IsSaveByUser.Value.Equals(true))
                .Select(x => new NotificationModel
                {
                    NotificationId = x.NotificationId,
                    NotificationName = x.NotificationName,
                    NotificationDescription = x.NotificationDescription,
                    IsActive = x.IsActive.Value
                }).ToList();
            return Ok(List);
        }
        [HttpPost]
        [Route("notificationsetup/{userId}")]
        public IHttpActionResult NotificationSetup(NotificationModel model)
        {
            SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities();          
                UserNotification newNotificationAdd = new UserNotification();
                newNotificationAdd.NotificationName = model.NotificationName;
                newNotificationAdd.NotificationDescription = model.NotificationDescription;
                newNotificationAdd.IsActive = model.IsActive;
                newNotificationAdd.CreateDate = DateTime.Now;
                newNotificationAdd.UserId = model.UserId;
                newNotificationAdd.IsSaveByUser = true;
                //you should check duplicate registration here 
                dc.UserNotifications.Add(newNotificationAdd);
                dc.SaveChanges();
                ModelState.Clear();
                return Ok(new { success = true, message = "Notification Add Successfully" });
            

        }
    }
}