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
using myApp;

namespace ApiSleepingPatener.Controllers
{
    public class TrainingVideoController : ApiController
    {
        SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
        [HttpGet]
        [Route("viewusertrainingvideos")]
        public IHttpActionResult ViewUserTrainingVideo()
        {
            List<TrainingVideoModel> listVideo = db.TrainingVideos
                .Select(x => new TrainingVideoModel
                {
                    TrainingVideoId = x.TrainingVideoId,
                    TrainingVideoName = x.TrainingVideoName,
                    TrainingVideoDescription = x.TrainingVideoDescription,
                    TrainingVideoURL = x.TrainingVideoURL
                }).ToList();
            return Ok(listVideo);
            // ViewBag.VideoList = listVideo;


        }

    }
}



