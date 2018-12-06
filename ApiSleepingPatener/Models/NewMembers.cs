using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiSleepingPatener.Models
{
    public class NewMembers
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int? Country { get; set; }
        public string Phone { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public int? SponsorId { get; set; }
        public decimal PaidAmount { get; set; }
        public string SponsorName { get; set; }        
    }
}