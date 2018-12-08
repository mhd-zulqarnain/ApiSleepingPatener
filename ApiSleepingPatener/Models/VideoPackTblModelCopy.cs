using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiSleepingPatener.Models
{
    public class VideoPackTblModelCopy
    {
        public int VideoPackId { get; set; }
        public int VideoPackPkgId { get; set; }
        public int VideoPackCatId { get; set; }
        public string VideoPackName { get; set; }
        public string VideoPackDesc { get; set; }
        public string VideoPackVideos { get; set; }
        public System.DateTime CreateDate { get; set; }
        public byte[] VideoPackImage { get; set; }
    }
}