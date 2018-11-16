using System.Collections.Generic;
using ApiSleepingPatener.Models;
using System.Web.Http;
using System.Data.Entity;
using System.Data;
using System.Linq;
using System;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using ApiSleepingPatener.Models.Account;

namespace ApiSleepingPatener.Controllers
{

    public class DashboardController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("maketabledetails/{userId}")]
        public IHttpActionResult getMaketableData(int userId)
        {
            MakeTableData obj = new MakeTableData();
            decimal TotalLeftUsers = GetTotalLeftUsers(userId);
            decimal TotalRightUsers = GetTotalRightUsers(userId);

            decimal TotalAmountLeftUsers = GetTotalAmountLeftUsers(userId);
            decimal TotalAmountRightUsers = GetTotalAmountRightUsers(userId);
            decimal RightRemaingAmount = GetRightRemaingAmount(userId);
            decimal LeftRemaingAmount = GetLeftRemaingAmount(userId);

            obj.totalLeftUsers = TotalLeftUsers;
            obj.totalRightUsers = TotalRightUsers;
            obj.totalAmountLeftUsers = TotalAmountLeftUsers;
            obj.totalAmountRightUsers = TotalAmountRightUsers;
            obj.rightRemaingAmount = RightRemaingAmount;
            obj.leftRemaingAmount = LeftRemaingAmount;

            return Ok(obj);

        }

        /// [Authorize]
        [HttpGet]
        [Route("getAllDownlineMembersLeft/{userId}")]
        public IHttpActionResult AllGetUserDownlineMembersLeft(int userId)
        {
            sleepingtestEntities db = new sleepingtestEntities();
            SleepingTestTreeEntities dbTree = new SleepingTestTreeEntities();
            IEnumerable<UserModel> usrmodel = new List<UserModel>();  
            string UserTypeUser = Common.Enum.UserType.User.ToString();

            List<GetParentChildsSP_Result> List = new List<GetParentChildsSP_Result>();          
            //List = db.NewUserRegistrations.Where(a => a.UserCode.Equals(UserTypeUser)
                //    && a.DownlineMemberId.Equals(userId))
                usrmodel = (from n in db.GetParentChildsLeftSP(userId)
                            join c in db.NewUserRegistrations on n.SponsorId equals c.UserId
                            where n.IsUserActive.Value == true
                            //&& n.IsPaidMember.Value == true
                            select new UserModel
                            {
                                UserId = n.UserId.Value,
                                UserName = n.Username,
                                Country = n.Country,
                                Phone = n.Phone,
                                AccountNumber = n.AccountNumber,
                                BankName = n.BankName,
                                SponsorId = n.SponsorId,
                                PaidAmount = n.PaidAmount.Value,
                                SponsorName = c.Username
                            }).ToList();

          
            return Ok(usrmodel);

        }
        [HttpGet]
        [Route("getAllDownlineMembersRight/{userId}")]
        public IHttpActionResult AllGetUserDownlineMembersRight(int userId)
        {
            sleepingtestEntities db = new sleepingtestEntities();
            IEnumerable<UserModel> usrmodel = new List<UserModel>();
            string UserTypeUser = Common.Enum.UserType.User.ToString();

            List<GetParentChildsSP_Result> List = new List<GetParentChildsSP_Result>();
            //List = db.NewUserRegistrations.Where(a => a.UserCode.Equals(UserTypeUser)
            //    && a.DownlineMemberId.Equals(userId))
            //List = db.NewUserRegistrations.Select(x => new UserModel

            //List = db.GetParentChildsRightSP(userId)
            //    .Select(x => new GetParentChildsSP_Result

            usrmodel = (from n in db.GetParentChildsRightSP(userId)
                        join c in db.NewUserRegistrations on n.SponsorId equals c.UserId
                        where n.IsPaidMember.Value == true
                        select new UserModel
                        {
                            UserId = n.UserId.Value,
                            UserName = n.Username,
                            Country = n.Country,
                            Phone = n.Phone,
                            AccountNumber = n.AccountNumber,
                            BankName = n.BankName,
                            SponsorId = n.SponsorId,
                            PaidAmount = n.PaidAmount.Value,
                            SponsorName = c.Username
                        }).ToList();
            return Ok(usrmodel);
        }
    


        //  [Authorize]
        //[HttpGet]
        //[Route("getAllDownlineMembersRight/{userId}")]
        //public IHttpActionResult AllGetUserDownlineMembersRight(int userId)
        //{
        //    sleepingtestEntities db = new sleepingtestEntities();
        //    IEnumerable<UserModel> usrmodel = new List<UserModel>();            
        //        usrmodel = (from n in db.GetParentChildsRightSP(userId)
        //                    join c in db.NewUserRegistrations on n.SponsorId equals c.UserId
        //                    where n.IsPaidMember.Value == true
        //                    select new UserModel
        //                    {
        //                        UserId = n.UserId.Value,
        //                        UserName = n.Username,
        //                        Country = n.Country,
        //                        Phone = n.Phone,
        //                        AccountNumber = n.AccountNumber,
        //                        BankName = n.BankName,
        //                        SponsorId = n.SponsorId,
        //                        PaidAmount = n.PaidAmount.Value,
        //                        SponsorName = c.Username
        //                    }).ToList();
            
        //    return Ok(usrmodel);
        //}

        public decimal GetLeftRemaingAmount(int userId)
        {
            decimal LeftPaidAmountShow = 0;
            decimal RightPaidAmountShow = 0;
            decimal amountLeft = 0;
            string UserTypeAdmin = ApiSleepingPatener.Common.Enum.UserType.Admin.ToString();
            string UserTypeUser = ApiSleepingPatener.Common.Enum.UserType.User.ToString();
            List<GetParentChildsLeftSP_Result> ListLeft = new List<GetParentChildsLeftSP_Result>();
            List<GetParentChildsRightSP_Result> ListRight = new List<GetParentChildsRightSP_Result>();
            using (sleepingtestEntities dc = new sleepingtestEntities())
            {
                ListLeft = dc.GetParentChildsLeftSP(userId).ToList();
                ListRight = dc.GetParentChildsRightSP(userId).ToList();
                List<UserGenealogyTableLeft> usersLeft = new List<UserGenealogyTableLeft>();
                List<NewUserRegistration> newUsersLeft = new List<NewUserRegistration>();
                List<UserGenealogyTableRight> usersRight = new List<UserGenealogyTableRight>();
                List<NewUserRegistration> newUsersRight = new List<NewUserRegistration>();
                
                List<NewUserRegistration> listDownlineMemberLeft = new List<NewUserRegistration>();
                List<NewUserRegistration> listDownlineMemberRight = new List<NewUserRegistration>();

                foreach (var itemLeft in ListLeft)
                {
                    var userIdChild = Convert.ToInt32(itemLeft.UserId);
                    usersLeft = dc.UserGenealogyTableLefts.Where(a => a.UserId.Value.Equals(userIdChild)
                            && a.MatchingCommision.Value.Equals(false)).ToList();
                    foreach (var itemUser in usersLeft)
                    {
                        var userIdChildLeft = Convert.ToInt32(itemUser.UserId);
                        newUsersLeft = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userIdChildLeft)
                            && a.IsUserActive.Value.Equals(true)).ToList();
                        if (newUsersLeft != null)
                        {
                            listDownlineMemberLeft.Add(new NewUserRegistration()
                            {
                                UserId = itemLeft.UserId.Value,
                                Username = itemLeft.Username,
                                Country = itemLeft.Country,
                                Phone = itemLeft.Phone,
                                PaidAmount = itemLeft.PaidAmount.Value
                            });
                            LeftPaidAmountShow = listDownlineMemberLeft.Sum(x => x.PaidAmount.Value);
                        }

                    }

                }
                foreach (var itemRight in ListRight)
                {
                    var userIdChild = Convert.ToInt32(itemRight.UserId);
                    usersRight = dc.UserGenealogyTableRights.Where(a => a.UserId.Value.Equals(userIdChild)
                            && a.MatchingCommision.Value.Equals(false)).ToList();
                    foreach (var itemUser in usersRight)
                    {
                        var userIdChildRight = Convert.ToInt32(itemUser.UserId);
                        newUsersRight = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userIdChildRight)
                            && a.IsUserActive.Value.Equals(true)).ToList();
                        if (newUsersRight != null)
                        {
                            listDownlineMemberRight.Add(new NewUserRegistration()
                            {
                                UserId = itemRight.UserId.Value,
                                Username = itemRight.Username,
                                Country = itemRight.Country,
                                Phone = itemRight.Phone,
                                PaidAmount = itemRight.PaidAmount.Value
                            });
                            RightPaidAmountShow = listDownlineMemberRight.Sum(x => x.PaidAmount.Value);
                        }

                    }

                }
                //var LeftPaidAmount = dc.GetParentChildsLeftSP(userId).ToList();
                //var RightPaidAmount = dc.GetParentChildsRightSP(userId).ToList();
                //decimal LeftPaidAmountShow = LeftPaidAmount.Sum(x => x.PaidAmount.Value);
                //decimal RightPaidAmountShow = RightPaidAmount.Sum(x => x.PaidAmount.Value);



                //decimal minimumAmount = Math.Min(LeftPaidAmountShow, RightPaidAmountShow);
                decimal maximumAmount = Math.Max(LeftPaidAmountShow, RightPaidAmountShow);
                decimal showAmount = maximumAmount - LeftPaidAmountShow;

                if (showAmount != 0)
                {
                    amountLeft = showAmount;
                    
                }
                
            }
            return amountLeft;

        }

        
        public decimal GetTotalLeftUsers(int userId)
        {
            int TotalLeftUsersShow = 0;
            decimal TotalLeftUsers = 0;
            List<GetParentChildsLeftSP_Result> List = new List<GetParentChildsLeftSP_Result>();
            using (sleepingtestEntities dc = new sleepingtestEntities())
            {
                List = dc.GetParentChildsLeftSP(userId).ToList();
                List<NewUserRegistration> listDownlineMember = new List<NewUserRegistration>();
                UserGenealogyTableLeft usersLeft = new UserGenealogyTableLeft();
                foreach (var item in List)
                {
                    var userIdChild = Convert.ToInt32(item.UserId);
                    usersLeft = dc.UserGenealogyTableLefts.Where(a => a.UserId.Value.Equals(userIdChild)
                            && a.MatchingCommision.Value.Equals(false)).FirstOrDefault();
                    if (usersLeft != null)
                    {
                        listDownlineMember.Add(new NewUserRegistration()
                        {
                            UserId = item.UserId.Value,
                            Username = item.Username,
                            Country = item.Country,
                            Phone = item.Phone,
                            AccountNumber = item.AccountNumber,
                            BankName = item.BankName,
                            SponsorId = item.SponsorId.Value,
                            PaidAmount = item.PaidAmount.Value,
                            UserCode = item.UserCode
                        });
                        TotalLeftUsersShow = listDownlineMember.Count();
                    }

                }

                if (TotalLeftUsersShow != 0)
                {
                    TotalLeftUsers = TotalLeftUsersShow;
                    
                }
               
            }
            return TotalLeftUsersShow;

        }

        
        public decimal GetTotalAmountLeftUsers(int userId)
        {
            decimal TotalAmountLeftUsersShow = 0;
           
            List<GetParentChildsLeftSP_Result> List = new List<GetParentChildsLeftSP_Result>();
            using (sleepingtestEntities dc = new sleepingtestEntities())
            {
                List = dc.GetParentChildsLeftSP(userId).ToList();
                List<UserGenealogyTableLeft> usersLeft = new List<UserGenealogyTableLeft>();
                List<NewUserRegistration> newUsersLeft = new List<NewUserRegistration>();
                List<NewUserRegistration> listDownlineMember = new List<NewUserRegistration>();
                foreach (var item in List)
                {
                    var userIdChild = Convert.ToInt32(item.UserId);
                    usersLeft = dc.UserGenealogyTableLefts.Where(a => a.UserId.Value.Equals(userIdChild)
                            && a.MatchingCommision.Value.Equals(false)).ToList();
                    foreach (var itemUser in usersLeft)
                    {
                        var userIdChildLeft = Convert.ToInt32(itemUser.UserId);
                        newUsersLeft = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userIdChildLeft)
                            && a.IsUserActive.Value.Equals(true)).ToList();
                        if (newUsersLeft != null)
                        {
                            listDownlineMember.Add(new NewUserRegistration()
                            {
                                UserId = item.UserId.Value,
                                Username = item.Username,
                                Country = item.Country,
                                Phone = item.Phone,
                                AccountNumber = item.AccountNumber,
                                BankName = item.BankName,
                                SponsorId = item.SponsorId.Value,
                                PaidAmount = item.PaidAmount.Value,
                                UserCode = item.UserCode
                            });
                            TotalAmountLeftUsersShow = listDownlineMember.Sum(x => x.PaidAmount.Value);
                        }

                    }

                }

              
                
            }
            return TotalAmountLeftUsersShow;

        }

       
        [HttpPost]
        public decimal GetRightRemaingAmount(int userId)
        {
            decimal LeftPaidAmountShow = 0;
            decimal RightPaidAmountShow = 0;
            decimal RightPaidAmount = 0;
            string UserTypeAdmin = Common.Enum.UserType.Admin.ToString();
            string UserTypeUser = Common.Enum.UserType.User.ToString();
            List<GetParentChildsLeftSP_Result> ListLeft = new List<GetParentChildsLeftSP_Result>();
            List<GetParentChildsRightSP_Result> ListRight = new List<GetParentChildsRightSP_Result>();
            using (sleepingtestEntities dc = new sleepingtestEntities())
            {
                ListLeft = dc.GetParentChildsLeftSP(userId).ToList();
                ListRight = dc.GetParentChildsRightSP(userId).ToList();
                List<UserGenealogyTableLeft> usersLeft = new List<UserGenealogyTableLeft>();
                List<NewUserRegistration> newUsersLeft = new List<NewUserRegistration>();
                List<UserGenealogyTableRight> usersRight = new List<UserGenealogyTableRight>();
                List<NewUserRegistration> newUsersRight = new List<NewUserRegistration>();

                List<NewUserRegistration> listDownlineMemberLeft = new List<NewUserRegistration>();
                List<NewUserRegistration> listDownlineMemberRight = new List<NewUserRegistration>();

                foreach (var itemLeft in ListLeft)
                {
                    var userIdChild = Convert.ToInt32(itemLeft.UserId);
                    usersLeft = dc.UserGenealogyTableLefts.Where(a => a.UserId.Value.Equals(userIdChild)
                            && a.MatchingCommision.Value.Equals(false)).ToList();
                    foreach (var itemUser in usersLeft)
                    {
                        var userIdChildLeft = Convert.ToInt32(itemUser.UserId);
                        newUsersLeft = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userIdChildLeft)
                            && a.IsUserActive.Value.Equals(true)).ToList();
                        if (newUsersLeft != null)
                        {
                            listDownlineMemberLeft.Add(new NewUserRegistration()
                            {
                                UserId = itemLeft.UserId.Value,
                                Username = itemLeft.Username,
                                Country = itemLeft.Country,
                                Phone = itemLeft.Phone,
                                PaidAmount = itemLeft.PaidAmount.Value
                            });
                            LeftPaidAmountShow = listDownlineMemberLeft.Sum(x => x.PaidAmount.Value);
                        }

                    }

                }
                foreach (var itemRight in ListRight)
                {
                    var userIdChild = Convert.ToInt32(itemRight.UserId);
                    usersRight = dc.UserGenealogyTableRights.Where(a => a.UserId.Value.Equals(userIdChild)
                            && a.MatchingCommision.Value.Equals(false)).ToList();
                    foreach (var itemUser in usersRight)
                    {
                        var userIdChildRight = Convert.ToInt32(itemUser.UserId);
                        newUsersRight = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userIdChildRight)
                            && a.IsUserActive.Value.Equals(true)).ToList();
                        if (newUsersRight != null)
                        {
                            listDownlineMemberRight.Add(new NewUserRegistration()
                            {
                                UserId = itemRight.UserId.Value,
                                Username = itemRight.Username,
                                Country = itemRight.Country,
                                Phone = itemRight.Phone,
                                PaidAmount = itemRight.PaidAmount.Value
                            });
                            RightPaidAmountShow = listDownlineMemberRight.Sum(x => x.PaidAmount.Value);
                        }

                    }

                }
                //var LeftPaidAmount = dc.GetParentChildsLeftSP(userId).ToList();
                //var RightPaidAmount = dc.GetParentChildsRightSP(userId).ToList();
                //decimal LeftPaidAmountShow = LeftPaidAmount.Sum(x => x.PaidAmount.Value);
                //decimal RightPaidAmountShow = RightPaidAmount.Sum(x => x.PaidAmount.Value);



                //decimal minimumAmount = Math.Min(LeftPaidAmountShow, RightPaidAmountShow);
                decimal maximumAmount = Math.Max(LeftPaidAmountShow, RightPaidAmountShow);
                decimal showAmount = maximumAmount - RightPaidAmountShow;

                if (showAmount != 0)
                {
                    RightPaidAmount = showAmount;
                    
                }
               
            }
            return RightPaidAmount;

        }

        [HttpPost]
        public decimal GetTotalRightUsers(int userId)
        {
            int TotalRightUsersShow = 0;
            decimal TotalRightUsers = 0;

            List<GetParentChildsRightSP_Result> List = new List<GetParentChildsRightSP_Result>();
            using (sleepingtestEntities dc = new sleepingtestEntities())
            {
                List = dc.GetParentChildsRightSP(userId).ToList();
                List<NewUserRegistration> listDownlineMember = new List<NewUserRegistration>();
                UserGenealogyTableRight usersRight = new UserGenealogyTableRight();
                foreach (var item in List)
                {
                    var userIdChild = Convert.ToInt32(item.UserId);
                    usersRight = dc.UserGenealogyTableRights.Where(a => a.UserId.Value.Equals(userIdChild)
                            && a.MatchingCommision.Value.Equals(false)).FirstOrDefault();
                    if (usersRight != null)
                    {
                        listDownlineMember.Add(new NewUserRegistration()
                        {
                            UserId = item.UserId.Value,
                            Username = item.Username,
                            Country = item.Country,
                            Phone = item.Phone,
                            AccountNumber = item.AccountNumber,
                            BankName = item.BankName,
                            SponsorId = item.SponsorId.Value,
                            PaidAmount = item.PaidAmount.Value,
                            UserCode = item.UserCode
                        });
                        TotalRightUsersShow = listDownlineMember.Count();
                    }

                }

                if (TotalRightUsersShow != 0)
                {
                    TotalRightUsers = TotalRightUsersShow;
                }
               
            }
            return TotalRightUsers;

        }

        [HttpPost]
        public decimal GetTotalAmountRightUsers(int userId)
        {
            decimal TotalAmountRightUsersShow = 0;
            decimal TotalAmountRightUsers = 0;
            List<GetParentChildsRightSP_Result> List = new List<GetParentChildsRightSP_Result>();
            using (sleepingtestEntities dc = new sleepingtestEntities())
            {
                List = dc.GetParentChildsRightSP(userId).ToList();
                List<UserGenealogyTableRight> usersRight = new List<UserGenealogyTableRight>();
                List<NewUserRegistration> newUsersRight = new List<NewUserRegistration>();
                List<NewUserRegistration> listDownlineMember = new List<NewUserRegistration>();
                foreach (var item in List)
                {
                    var userIdChild = Convert.ToInt32(item.UserId);
                    usersRight = dc.UserGenealogyTableRights.Where(a => a.UserId.Value.Equals(userIdChild)
                            && a.MatchingCommision.Value.Equals(false)).ToList();
                    foreach (var itemUser in usersRight)
                    {
                        var userIdChildRight = Convert.ToInt32(itemUser.UserId);
                        newUsersRight = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userIdChildRight)
                            && a.IsUserActive.Value.Equals(true)).ToList();
                        if (newUsersRight != null)
                        {
                            listDownlineMember.Add(new NewUserRegistration()
                            {
                                UserId = item.UserId.Value,
                                Username = item.Username,
                                Country = item.Country,
                                Phone = item.Phone,
                                AccountNumber = item.AccountNumber,
                                BankName = item.BankName,
                                SponsorId = item.SponsorId.Value,
                                PaidAmount = item.PaidAmount.Value,
                                UserCode = item.UserCode
                            });
                            TotalAmountRightUsersShow = listDownlineMember.Sum(x => x.PaidAmount.Value);
                        }

                    }

                }

                if (TotalAmountRightUsersShow != 0)
                {
                    TotalAmountRightUsers = TotalAmountRightUsersShow;
                }
                
            }
            return TotalAmountRightUsersShow;

        }

        [HttpPost]
        [Route("addleftmembers/{userId}")]
        public IHttpActionResult AddNewMemeber(UserModel model,int userId)
        {
            return Ok(0);

        }
        [Authorize]
        [HttpGet]
        [Route("dropdownleft/{userId}")]
        public IHttpActionResult GetUserForDownlineMemberByUserOnlyLeft(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());

            sleepingtestEntities db = new sleepingtestEntities();

            List<GetParentChildsLeftSP_Result> List = new List<GetParentChildsLeftSP_Result>();
            List = db.GetParentChildsLeftSP(userId).ToList();

            List<DropDownMembers> listDownlineMember = new List<DropDownMembers>();

            UserGenealogyTableLeft leftUsers = new UserGenealogyTableLeft();

            foreach (var item in List)
            {
                var userIdChild = Convert.ToInt32(item.UserId);
                if (item.UserCode == Common.Enum.UserType.User)
                {
                    leftUsers = db.UserGenealogyTableLefts.Where(a => a.DownlineMemberId.Value.Equals(userIdChild)).FirstOrDefault();
                    
                    if (leftUsers == null)
                    {
                        listDownlineMember.Add(new DropDownMembers() { UserId = item.UserId.Value, Username = item.Username,UserPosition = null });
                    }
                }
            }

            return Ok(listDownlineMember);
               // return Ok(listDownlineMember);
          
           // ViewBag.DownlineMemberList = listDownlineMember;
        }
        [Authorize]
        [HttpGet]
        [Route("dropdownright/{userId}")]
        public IHttpActionResult GetUserForDownlineMemberByUserOnlyRight(int userId)
        {

            sleepingtestEntities db = new sleepingtestEntities();

            List<GetParentChildsRightSP_Result> List = new List<GetParentChildsRightSP_Result>();
            List = db.GetParentChildsRightSP(userId).ToList();

            List<DropDownMembers> listDownlineMember = new List<DropDownMembers>();

            UserGenealogyTableRight rightUsers = new UserGenealogyTableRight();

            foreach (var item in List)
            {
                var userIdChild = Convert.ToInt32(item.UserId);
                if (item.UserCode == Common.Enum.UserType.User)
                {
                    rightUsers = db.UserGenealogyTableRights.Where(a => a.DownlineMemberId.Value.Equals(userIdChild)).FirstOrDefault();
                    if (rightUsers == null)
                    {
                        listDownlineMember.Add(new DropDownMembers() { UserId = item.UserId.Value, Username = item.Username, UserPosition = null });
                    }
                   
                }
            }
            return Ok(listDownlineMember);
        }
        [Authorize]
        [HttpGet]
        [Route("maketablemembersleft/{userId}")]
        public IHttpActionResult GetUserDownlineMembersLeft(int userId)
        {
            sleepingtestEntities db = new sleepingtestEntities();
       //     TreeDataTbl dbTree = new TreeDataTbl();
            UserModel usrmodel = new UserModel();
            List<GetParentChildsLeftSP_Result> List = new List<GetParentChildsLeftSP_Result>();          
                List = db.GetParentChildsLeftSP(userId).ToList();

                List<NewUserRegistration> listDownlineMember = new List<NewUserRegistration>();
                UserGenealogyTableLeft usersLeft = new UserGenealogyTableLeft();

                foreach (var item in List)
                {
                    var userIdChild = Convert.ToInt32(item.UserId);
                    if (item.UserCode == Common.Enum.UserType.User)
                    {
                        usersLeft = db.UserGenealogyTableLefts.Where(a => a.UserId.Value.Equals(userIdChild)
                            && a.MatchingCommision.Value.Equals(false)).FirstOrDefault();
                        if (usersLeft != null)
                        {
                            listDownlineMember.Add(new NewUserRegistration()
                            {
                                UserId = item.UserId.Value,
                                Username = item.Username,
                                Country = item.Country,
                                Phone = item.Phone,
                                AccountNumber = item.AccountNumber,
                                BankName = item.BankName,
                                SponsorId = item.SponsorId.Value,
                                PaidAmount = item.PaidAmount.Value,
                                UserCode = item.UserCode
                            });
                        }
                    }
                }
            return Ok(listDownlineMember);
                

            }
        [HttpGet]
        [Route("maketablemembersright/{userId}")]
        public IHttpActionResult GetUserDownlineMembersRight(int userId)
        {
            sleepingtestEntities db = new sleepingtestEntities();
            UserModel usrmodel = new UserModel();

            List<GetParentChildsRightSP_Result> List = new List<GetParentChildsRightSP_Result>();

            List = db.GetParentChildsRightSP(userId)
                .Select(x => new GetParentChildsRightSP_Result
                    //List = db.NewUserRegistrations.Select(x => new UserModel
                    {
                    UserId = x.UserId.Value,
                    Username = x.Username,
                    Country = x.Country,
                    Phone = x.Phone,
                    AccountNumber = x.AccountNumber,
                    BankName = x.BankName,
                    SponsorId = x.SponsorId,
                    PaidAmount = x.PaidAmount.Value,
                    UserCode = x.UserCode
                }).ToList();
            //return Json(new { data = List }, JsonRequestBehavior.AllowGet);
            return Ok(List);


            List = db.GetParentChildsRightSP(userId).ToList();

            List<NewUserRegistration> listDownlineMember = new List<NewUserRegistration>();
            UserGenealogyTableRight usersRight = new UserGenealogyTableRight();

            foreach (var item in List)
            {
                var userIdChild = Convert.ToInt32(item.UserId);
                if (item.UserCode == Common.Enum.UserType.User)
                {
                    usersRight = db.UserGenealogyTableRights.Where(a => a.UserId.Value.Equals(userIdChild)
                        && a.MatchingCommision.Value.Equals(false)).FirstOrDefault();
                    if (usersRight != null) //both null
                    {
                        listDownlineMember.Add(new NewUserRegistration()
                        {
                            UserId = item.UserId.Value,
                            Username = item.Username,
                            Country = item.Country,
                            Phone = item.Phone,
                            AccountNumber = item.AccountNumber,
                            BankName = item.BankName,
                            SponsorId = item.SponsorId.Value,
                            PaidAmount = item.PaidAmount.Value,
                            UserCode = item.UserCode
                        });
                    }
                }
            }
            return Ok(listDownlineMember);
           // return Json(new { data = listDownlineMember }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Route("GetUserPaidMembersList/{userId}")]
        public IHttpActionResult GetUserPaidMembersList(int userId)
        {
            sleepingtestEntities db = new sleepingtestEntities();

           // var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            IEnumerable<UserModel> usrmodel = new List<UserModel>();
            usrmodel = (from n in db.GetParentChildsSP(userId)
                        join c in db.NewUserRegistrations on n.SponsorId equals c.UserId
                        where n.IsPaidMember.Value == true
                        select new UserModel
                        {
                            UserId = n.UserId.Value,
                            UserName = n.Username,
                            Country = n.Country,
                            Phone = n.Phone,
                            AccountNumber = n.AccountNumber,
                            BankName = n.BankName,
                            SponsorId = n.SponsorId,
                            PaidAmount = n.PaidAmount.Value,
                            SponsorName = c.Username
                        }).ToList();

            return Ok(usrmodel);
        }

        [HttpGet]
        [Route("GetUserUnPaidMembersList/{userId}")]
        public IHttpActionResult GetUserUnPaidMembersList(int userId)
        {
            sleepingtestEntities db = new sleepingtestEntities();

            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            IEnumerable<UserModel> usrmodel = new List<UserModel>();
            usrmodel = (from n in db.GetParentChildsSP(userId)
                        join c in db.NewUserRegistrations on n.SponsorId equals c.UserId
                        where n.IsPaidMember.Value == false
                        select new UserModel
                        {
                            UserId = n.UserId.Value,
                            UserName = n.Username,
                            Country = n.Country,
                            Phone = n.Phone,
                            AccountNumber = n.AccountNumber,
                            BankName = n.BankName,
                            SponsorId = n.SponsorId,
                            PaidAmount = n.PaidAmount.Value,
                            SponsorName = c.Username
                        }).ToList();
            return Ok(usrmodel);
        }
        //refered members
        [HttpGet]
        [Route("GetUserReferedMembers/{userId}")]
        public IHttpActionResult GetUserReferedMembers(int userId)
        {
            sleepingtestEntities db = new sleepingtestEntities();
            string UserTypeUser = Common.Enum.UserType.User.ToString();
            UserModel usrmodel = new UserModel();
            List<UserModel> List = new List<UserModel>();           
                List = db.NewUserRegistrations.Where(a => a.UserCode.Equals(UserTypeUser)
                    && a.SponsorId.Equals(userId))
                    .Select(x => new UserModel
                    //List = db.NewUserRegistrations.Select(x => new UserModel
                    {
                        UserId = x.UserId,
                        UserName = x.Username,
                        Country = x.Country,
                        Phone = x.Phone,
                        AccountNumber = x.AccountNumber,
                        BankName = x.BankName,
                        SponsorId = x.SponsorId,
                        PaidAmount = x.PaidAmount.Value
                    }).ToList();
            return Ok(List);
        }
    }

}


