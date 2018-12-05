using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiSleepingPatener.Models
{
    public class EwalletWithDrawObjectModel
    {
        public string GetUserPackageCommissionAmount { get; set; }
        public string GetUserPackageAmountLimitForWithdrawal { get; set; }
        public string GetUserEWalletAmountInProcess { get; set; }
        public string PayoutChargesPercent { get; set; }
        public string MinimumPayout { get; set; }
        public string PackageName { get; set; }
        //public string EWalletWithdrawalFund { get; set; }
    }
}