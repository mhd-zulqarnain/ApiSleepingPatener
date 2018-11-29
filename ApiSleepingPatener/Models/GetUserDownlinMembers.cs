using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiSleepingPatener.Models
{
    public class GetUserDownlinMembers
    {
        public string GetTotalLeftUsers { get; set; }
        public string totalRightUsers { get; set; }
        public string totalAmountLeftUsers { get; set; }
        public string totalAmountRightUsers { get; set; }
        public string rightRemaingAmount { get; set; }
        public string leftRemaingAmount { get; set; }
    }
}