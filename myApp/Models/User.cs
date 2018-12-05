using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiSleepingPatener.Models
{
    public class User
    {
      
        public User(int id, string name, string email, string pass, string uname)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = pass;
            Username = uname;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
      
    }
    public class User1
    {
        public int UId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
   
}