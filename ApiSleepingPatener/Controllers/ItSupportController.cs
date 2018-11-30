using System.Collections.Generic;
using ApiSleepingPatener.Models;
using System.Web.Http;
using System.Data;
using System.Linq;
using System;


namespace ApiSleepingPatener.Controllers
{
    public class ItSupportController : ApiController

    { 
        [HttpPost]
        [Route("inboxitsupport/{userId}/{username}")]
        public IHttpActionResult Inbox(SentAdminMessageModel sentmodel,int userId,string username)
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
            sent_msg.Sender = sentmodel.Sender = userId;
            sent_msg.UserId = sentmodel.UserId = userId;
            sent_msg.SponserId = admin_id;
            sent_msg.Sender_Name = username;
            sent_msg.Message = sentmodel.Message;
            sent_msg.IsRead = sentmodel.IsRead = true;
            sent_msg.CreateDate = sentmodel.CreateDate = DateTime.Today;
            sent_msg.MessageImage = img;
            //sent_msg.MessageImage = fileImage.InputStream; //imageByte;

            db.SentAdminMessages.Add(sent_msg);
            //db.SaveChanges();
            ReceiveAdminMessage Recive_msg = new ReceiveAdminMessage();
            Recive_msg.Sender = sentmodel.Sender = userId;
            Recive_msg.UserId = sentmodel.UserId = userId;
            Recive_msg.SponserId = admin_id;
            Recive_msg.Sender_Name = username;
            Recive_msg.Message = sentmodel.Message;
            Recive_msg.IsRead = sentmodel.IsRead = true;
            Recive_msg.CreateDate = sentmodel.CreateDate = DateTime.Today;
            Recive_msg.MessageImage = img;
            db.ReceiveAdminMessages.Add(Recive_msg);
            db.SaveChanges();
            return Ok(new { success = true, message = "messsage sent successfully" });
        }
        [HttpGet]
        [Route("viewallreadmessageitsupport/{userId}")]
        public IHttpActionResult ViewAllReadMessage(SentAdminMessageModel UMM ,int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<ReceiveAdminMessageModel> List = new List<ReceiveAdminMessageModel>();
            List = db.ReceiveAdminMessages.Where(a =>  a.SponserId == userId)
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
        [Route("getsentmessagessponsorit/{userId}")]
        public IHttpActionResult getsentmessagessponsorit(SentAdminMessageModel UMM, int userId)
        {
            //to be implement
            List<ReceiveAdminMessageModel> List = new List<ReceiveAdminMessageModel>();
            return Ok(List);
        }


    }
}



