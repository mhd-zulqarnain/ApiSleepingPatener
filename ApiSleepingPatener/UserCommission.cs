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
    
    public partial class UserCommission
    {
        public int UserCommissionId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string UserName { get; set; }
        public Nullable<int> DownlineMemberId { get; set; }
        public Nullable<int> MatchingCommUserId { get; set; }
        public string MatchingCommUserPosition { get; set; }
        public string MatchingCommUserName { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    }
}
