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

namespace ApiSleepingPatener.Controllers
{
    public class VideoPackController : ApiController
    {
        SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
        [HttpGet]
        [Route("getvideocategories")]
        public IHttpActionResult GetVideoCategory()
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<VideoPackCatTbl> videocategory = db.VideoPackCatTbls.ToList();
           
            return Ok(videocategory);

        }
        [HttpGet]
        [Route("videolist/{userpackageid}/{cat_id}")]
        public IHttpActionResult videolist(int userpackageid,int cat_id)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<VideoPackTblModelCopy> List = new List<VideoPackTblModelCopy>();
            List = db.VideoPackTbls.Where(a => a.VideoPackCatId == cat_id && a.VideoPackPkgId == userpackageid)
                .Select(x => new VideoPackTblModelCopy
                {
                    VideoPackId = x.VideoPackId,
                    VideoPackCatId = x.VideoPackCatId,
                    VideoPackName = x.VideoPackName,
                    VideoPackDesc = x.VideoPackDesc,
                    VideoPackVideos = x.VideoPackVideos,
                    VideoPackImage=x.VideoPackImage
                }).ToList();
            //ViewBag.VideoList = List;
            return Ok(List);
        }
        [HttpGet]
        [Route("viewvideopackuser/{VideoPackId}")]
        public IHttpActionResult ViewVideoPackUser(int VideoPackId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<VideoPackTblModelCopy> List = new List<VideoPackTblModelCopy>();
            List = db.VideoPackTbls.Where(a =>  a.VideoPackId == VideoPackId)
                .Select(x => new VideoPackTblModelCopy
                {
                    VideoPackId = x.VideoPackId,
                    VideoPackCatId = x.VideoPackCatId,
                    VideoPackName = x.VideoPackName,
                    VideoPackDesc = x.VideoPackDesc,
                    VideoPackVideos = x.VideoPackVideos
                }).ToList();
            //ViewBag.VideoList = List;
            //return PartialView("_VideoPackUserView");
            return Ok(List);
        }

    }
}



