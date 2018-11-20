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
    
    public partial class UserTableLevel
    {
        public int UserTableLevelId { get; set; }
        public string Username { get; set; }
        public Nullable<int> TableLevel { get; set; }
        public Nullable<int> NoOfUsers { get; set; }
        public Nullable<int> LeftUsers { get; set; }
        public Nullable<int> RightUsers { get; set; }
        public Nullable<int> TableLevelLimit { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<decimal> TotalLeftUserAmount { get; set; }
        public Nullable<decimal> TotalRightUserAmount { get; set; }
        public Nullable<decimal> TotalRemainingLeftUserAmount { get; set; }
        public Nullable<decimal> TotalRemainingRightUserAmount { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
