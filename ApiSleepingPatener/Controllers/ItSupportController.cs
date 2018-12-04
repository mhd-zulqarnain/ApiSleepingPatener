using System.Collections.Generic;
using ApiSleepingPatener.Models;
using System.Web.Http;
using System.Data;
using System.Linq;
using System;
using System.Net;

namespace ApiSleepingPatener.Controllers
{
    public class ItSupportController : ApiController

    { 
        [HttpPost]
        [Route("inboxitsupport")]
        public IHttpActionResult Inbox(SentAdminMessageModel sentmodel)
        {
            int admin_id = 1;
            var fileImage = sentmodel.MessageImage;
            byte[] img;
            if (fileImage != null)
            {
                img = fileImage;
            }
            else
            {
                img = null;
            }

           // var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            SentAdminMessage sent_msg = new SentAdminMessage();
            sent_msg.Sender = sentmodel.Sender = sentmodel.UserId;
            sent_msg.UserId = sentmodel.UserId = sentmodel.UserId;
            sent_msg.SponserId = admin_id;
            sent_msg.Sender_Name = sentmodel.Sender_Name;
            sent_msg.Message = sentmodel.Message;
            sent_msg.IsRead = sentmodel.IsRead = false;
            sent_msg.CreateDate = sentmodel.CreateDate = DateTime.Today;
            sent_msg.MessageImage = img;
            //sent_msg.MessageImage = fileImage.InputStream; //imageByte;

            db.SentAdminMessages.Add(sent_msg);
            //db.SaveChanges();
            ReceiveAdminMessage Recive_msg = new ReceiveAdminMessage();
            Recive_msg.Sender = sentmodel.Sender = sentmodel.UserId;
            Recive_msg.UserId = sentmodel.UserId = sentmodel.UserId;
            Recive_msg.SponserId = admin_id;
            Recive_msg.Sender_Name = sentmodel.Sender_Name;
            Recive_msg.Message = sentmodel.Message;
            Recive_msg.IsRead = sentmodel.IsRead = true;
            Recive_msg.CreateDate = sentmodel.CreateDate = DateTime.Today;
            Recive_msg.MessageImage = img;
            db.ReceiveAdminMessages.Add(Recive_msg);
            db.SaveChanges();
            //var fcm = db.NewUserRegistrations.Where(x => x.UserId == sentmodel.SponserId).Select(x => x.Fcm).FirstOrDefault();
            //if (fcm != null)
            //{
            //    WebClient client = new WebClient();
            //    client.DownloadString("http://redcodetechnologies.com/MLMAPI/messageNotifyApi.php?send_notification&sname=" +
            //        sentmodel.Sender_Name + "&uid=" + sentmodel.UserId + "&sid=" + sentmodel.SponserId + "&message=" + sentmodel.Message
            //       + "&token=" + fcm);

            //}
            return Ok(new { success = true, message = "messsage sent successfully" });
        }
        [HttpGet]
        [Route("viewallreadmessageitsupport/{userId}")]
        public IHttpActionResult ViewAllReadMessage(SentAdminMessageModel UMM ,int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<ReceiveAdminMessageModel> List = new List<ReceiveAdminMessageModel>();
            List = db.ReceiveAdminMessages.OrderByDescending(id => id.Id).Where(a =>  a.SponserId == userId)
                .Select(x => new ReceiveAdminMessageModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Message = x.Message,
                    Sender_Name = x.Sender_Name,
                    IsRead = x.IsRead,
                    CreateDate = x.CreateDate
                }).ToList();
            return Ok(List);
        }
        [HttpPost]
        [Route("deleteinboxmsgitsupport/{Id}")]
        public IHttpActionResult DeleteInboxMsg(int Id)
        {
            try
            {
                SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
                ReceiveAdminMessage vd1 = db.ReceiveAdminMessages.Where(x => x.Id == Id).FirstOrDefault<ReceiveAdminMessage>();
                db.ReceiveAdminMessages.Remove(vd1);
                db.SaveChanges();
                return Ok(new { success = true, message = "message delete successfully" });

            }
            catch (Exception ex)
            {
                return Ok(new { success = true, message = "unable to delete this field", ex.Message });
            }


        }

        [HttpGet]
        [Route("getsentmessagesitsupport/{userId}")]
        public IHttpActionResult GetSentMessages(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<SentAdminMessageModel> List = new List<SentAdminMessageModel>();
            List = db.SentAdminMessages.Where(a => a.UserId == userId)
                .Select(x => new SentAdminMessageModel
                {
                    Id = x.Id,
                    Message = x.Message,
                    Sender_Name = x.Sender_Name,
                    IsRead = x.IsRead,
                    CreateDate = x.CreateDate
                }).ToList();
            return Ok(List);
        }
        [HttpPost]
        [Route("deletereadmessageitsupport/{Id}")]
        public IHttpActionResult DeleteReadMessage(int Id)
        {
            try

            {             
                SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
                SentAdminMessage sdm = db.SentAdminMessages.Where(x => x.Id == Id).FirstOrDefault<SentAdminMessage>();
                db.SentAdminMessages.Remove(sdm);
                //d.Id = Id;
                //db.Entry(d).State = System.Data.EntityState.Deleted;
                db.SaveChanges();
                return Ok(new { success = true, message = "message delete successfully" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = true, message = "unable to delete this field", ex.Message });
            }
        }

    }
}



