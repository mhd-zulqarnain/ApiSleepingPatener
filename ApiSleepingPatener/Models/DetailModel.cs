using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiSleepingPatener.Models
{
 
     public partial class DetailModel
    {
        public int TransactionId { get; set; }
        public string TransactionSource { get; set; }
        public string TransactionName { get; set; }
        public Nullable<int> AsscociatedMember { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public Nullable<bool> Credit { get; set; }
        public Nullable<bool> Debit { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<bool> IsPackageBonus { get; set; }
        public Nullable<int> PackageId { get; set; }
        public Nullable<bool> IsMatchingBonus { get; set; }
        public Nullable<bool> IsParentBonus { get; set; }
        public Nullable<bool> IsWithdrawlRequestByUser { get; set; }
        public Nullable<bool> IsWithdrawlPaidByAdmin { get; set; }
        public Nullable<bool> AdminCredit { get; set; }
        public Nullable<bool> AdminDebit { get; set; }
        public string AdminTransactionName { get; set; }
        public Nullable<bool> IsWithdrawlRequestApproved { get; set; }
    }
}