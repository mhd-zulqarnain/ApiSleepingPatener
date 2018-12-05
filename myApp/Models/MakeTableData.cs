using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApiSleepingPatener.Models
{
    public class MakeTableData
    {
        public string totalLeftUsers  { get; set; }
        public string totalRightUsers  { get; set; }
        public string totalAmountLeftUsers { get; set; }
        public string totalAmountRightUsers { get; set; }
        public string rightRemaingAmount { get; set; }
        public string leftRemaingAmount { get; set; }
        public string getalltotalearningamount { get; set; }
        public string usertablebalance { get; set; }
    }
}