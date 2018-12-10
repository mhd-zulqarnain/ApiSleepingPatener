using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApiSleepingPatener.Models.Packages
{
    public class PackageModel
    {
        public int PackageId { get; set; }
        [Required(ErrorMessage = "enter package name")]
        public string PackageName { get; set; }
        [Required(ErrorMessage = "enter package percent")]
        public int? PackagePercent { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }
        [Required(ErrorMessage = "enter package price")]        
        public decimal? PackagePrice { get; set; }
        [Required(ErrorMessage = "enter package validity")]        
        public string PackageValidity { get; set; }
        [Required(ErrorMessage = "enter package minimum withdrawal amount")]
        public decimal? PackageMinWithdrawalAmount { get; set; }
        [Required(ErrorMessage = "enter package maximum withdrawal amount")]
        public decimal? PackageMaxWithdrawalAmount { get; set; }
        [Required(ErrorMessage = "enter member list")]
        public int? AddMemberLimit { get; set; }
        [Required(ErrorMessage = "enter amount")]
        public decimal? MaximumMatchingBonusAmount { get; set; }
    }
}