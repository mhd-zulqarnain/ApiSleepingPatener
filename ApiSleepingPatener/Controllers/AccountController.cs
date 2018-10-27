﻿using ApiSleepingPatener.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace ApiSleepingPatener.Controllers
{
    [Authorize]
    public class AccountController : ApiController
    {
        [HttpGet]
        [Route("api/account/getuser/{id}")]
        public IHttpActionResult GetUFirstUser(int id)
        {
            // Get user from dummy list

            //var userList = new List<User>(){
            //     new User(   2,
            //       "twt",
            //        "t@g.com",
            //        "test",
            //        "test") {

            //    },
            //      new User(  1,
            //       "twt",
            //        "t@g.com",
            //        "test",
            //        "test") {

            //    }
                
            //};

            List<NewUserRegistration> list = new List<NewUserRegistration>();
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["remote"].ConnectionString);

            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("select * from NewUserRegistration where UserId = '" + id+"'", connect);
            try
            {
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                int uid = Convert.ToInt32(sdr["UserId"].ToString());
                string uname = sdr["Username"].ToString();
                string pass = sdr["Password"].ToString();
                string Email = sdr["Email"].ToString();
               sdr.Close();
                connect.Close();
                return Ok(new NewUserRegistration(id, uname, Email, pass));
            }
            catch (Exception n)
            {
                return Ok(0);
            }
            
           
        }
    }
}