﻿using ApiSleepingPatener.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace TokenAuth.Services
{
    public class UserService
    {

        public User ValidateUser(string email, string password)
        {
            // Here you can write the code to validate
            // User from database and return accordingly
            // To test we use dummy list here
            var user = GetUserList(email, password);
            //var user = userList.FirstOrDefault(x => x.Email == email && x.Password == password);
            return user;
        }

        public User GetUserList(String email, String password)
        {

            //  List<NewUserRegistration> list = new List<NewUserRegistration>();
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["remote"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("select * from NewUserRegistration where  UserCode = 'Usr' and Username= '" + email + "' and Password = '" + password + "'", connect);
            try
            {
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                int id = Convert.ToInt32(sdr["UserId"].ToString());
                string uname = sdr["Username"].ToString();
                string pass = sdr["Password"].ToString();
                string Email = sdr["Email"].ToString();
                string name = sdr["Name"].ToString();



                sdr.Close();
                connect.Close();
                return new User(id, name, email, pass, uname);
            }
            catch (Exception n)
            {
                return null;
            }


        }
    }
}


//SleepingPartnermanagementTestingEntities1 dce = new SleepingPartnermanagementTestingEntities1();
////NewUserRegistration newuser = dce.NewUserRegistrations.Where(a => a.UserId.Equals(model.UserId)).FirstOrDefault();
//NewUserRegistration newuser = dce.NewUserRegistrations.SingleOrDefault(x => x.Username == Username && x.Password.Equals(password));
//            if (newuser != null) {
//                return new User(newuser.UserId, newuser.Name, newuser.Email, newuser.Password, newuser.Username);
//            }
//            else
//            { }