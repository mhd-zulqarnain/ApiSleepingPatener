using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiSleepingPatener.Models
{
    public class TrainingVideoModel
    {
        public int TrainingVideoId { get; set; }
        public string TrainingVideoName { get; set; }
        public string TrainingVideoURL { get; set; }
        public string TrainingVideoDescription { get; set; }
    }
}