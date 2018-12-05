using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiSleepingPatener.Models
{
    public class ProfileSetup
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int? Country { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AccountTitle { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string CNICNumber { get; set; }
        //public byte[] DocumentImage { get; set; }

        public byte[] NICImage { get; set; }
        public byte[] ProfileImage { get; set; }
        public byte[] NICImage1 { get; set; }
        public byte[] DocumentImage { get; set; }
    }
}