//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiSleepingPatener
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserReward
    {
        public int UserRewardId { get; set; }
        public string UserRewardName { get; set; }
        public string UserRewardDesignation { get; set; }
        public Nullable<decimal> UserLeftAmount { get; set; }
        public Nullable<decimal> UserRightAmount { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Username { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> RewardId { get; set; }
        public Nullable<bool> IsClaimByUser { get; set; }
        public Nullable<bool> IsRewardedByAdmin { get; set; }
        public Nullable<System.DateTime> ClaimDate { get; set; }
        public Nullable<System.DateTime> RewardedDate { get; set; }
    }
}
