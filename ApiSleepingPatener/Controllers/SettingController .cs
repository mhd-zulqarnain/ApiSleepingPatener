using System.Collections.Generic;
using ApiSleepingPatener.Models;
using System.Web.Http;
using System.Data.Entity;
using System.Data;
using System.Linq;
using System;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using ApiSleepingPatener.Models.Account;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace ApiSleepingPatener.Controllers
{
    public class SettingController : ApiController
    {
        //Packages
        string SendSMSAccountSid = System.Configuration.ConfigurationManager.AppSettings["SendSMSAccountSid"];
        string SendSMSAuthToken = System.Configuration.ConfigurationManager.AppSettings["SendSMSAuthToken"];
        string SendSMSFromNumber = System.Configuration.ConfigurationManager.AppSettings["SendSMSFromNumber"];
        [HttpGet]
        [Route("setting/packages")]
        public IHttpActionResult getPackages()
        {
            try
            {
                List<Package> List = new List<Package>();
                using (SleepingPartnermanagementTestingEntities dce = new SleepingPartnermanagementTestingEntities())
                {

                    List = dce.Packages.ToList();

                    return Ok(List);
                }

            }
            catch (Exception e)
            {
                return Ok(e);
            }

        }
        //Get Ads
        [HttpGet]
        [Route("getAds")]
        public IHttpActionResult ShowAdvertisementData()
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<Advertisement> listadvertisement = db.Advertisements.Where(x => x.IsActive == true).ToList();
            return Ok(listadvertisement);

        }
        //get images from profile images 
        [HttpGet]
        [Route("getprofileimages/{userId}")]
        public IHttpActionResult RetrieveProfileImage(int userId)
        {

            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            var q = from temp in db.NewUserRegistrations where temp.UserId == userId select temp.ProfileImage;
            byte[] cover = q.First();
            if (cover != null)
            {
                // return Ok(new { success = true, message = "User has been saved" });
                //return file
                return Ok(new { success = true, message = cover });
            }

            else
            {
                return Ok(new { success = true, message = "" });
            }

        }
        //get images from Nic images 
        [HttpGet]
        [Route("getnicimages/{userId}")]
        public IHttpActionResult RetrieveNicImage(int userId)
        {

            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            var q = from temp in db.NewUserRegistrations where temp.UserId == userId select temp.NICImage;
            byte[] cover = q.First();
            if (cover != null)
            {
                // return Ok(new { success = true, message = "User has been saved" });
                //return file
                return Ok(new { success = true, message = cover });
            }

            else
            {
                return Ok(new { success = true, message = "" });
            }

        }
      
    }


}
