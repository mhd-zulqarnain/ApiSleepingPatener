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
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using ApiSleepingPatener.Models.Reports;

namespace ApiSleepingPatener.Controllers
{

    public class DashboardController : ApiController
    {

        string SendSMSAccountSid = System.Configuration.ConfigurationManager.AppSettings["SendSMSAccountSid"];
        string SendSMSAuthToken = System.Configuration.ConfigurationManager.AppSettings["SendSMSAuthToken"];
        string SendSMSFromNumber = System.Configuration.ConfigurationManager.AppSettings["SendSMSFromNumber"];
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
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            SleepingPartnermanagementTreeTestingEntities dbTree = new SleepingPartnermanagementTreeTestingEntities();
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
                            Username = n.Username,
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
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
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
                        where n.IsUserActive.Value == true
                        //&& n.IsPaidMember.Value == true
                        select new UserModel
                        {
                            UserId = n.UserId.Value,
                            Username = n.Username,
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
        //    SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
        //    IEnumerable<UserModel> usrmodel = new List<UserModel>();            
        //        usrmodel = (from n in db.GetParentChildsRightSP(userId)
        //                    join c in db.NewUserRegistrations on n.SponsorId equals c.UserId
        //                    where n.IsPaidMember.Value == true
        //                    select new UserModel
        //                    {
        //                        UserId = n.UserId.Value,
        //                        Username = n.Username,
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
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
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
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
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
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
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
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
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
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
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
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
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
        //add left members in network page
        [HttpPost]
        [Route("addleftmembers/{userId}")]
        public IHttpActionResult AddNewMemeberLeft(UserModel model, int userId)
        {
            try
            {
                // var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
                SleepingPartnermanagementTreeTestingEntities dbTree = new SleepingPartnermanagementTreeTestingEntities();
                using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
                {
                    var usercheckEmail = dc.NewUserRegistrations.Where(a => a.Email.Equals(model.Email)).FirstOrDefault();
                    var usercheckPhone = dc.NewUserRegistrations.Where(a => a.Phone.Equals(model.Phone)).FirstOrDefault();
                    //var usercheckAccountNumber = dc.NewUserRegistrations.Where(a => a.AccountNumber.Equals(model.AccountNumber)).FirstOrDefault();
                    //var usercheckCNIC = dc.NewUserRegistrations.Where(a => a.CNIC.Equals(model.CNICNumber)).FirstOrDefault();
                    if (usercheckEmail != null)
                    {
                        return Ok(new { success = false, message = "User email already exist" });
                    }
                    else if (usercheckPhone != null)
                    {
                        return Ok(new { success = false, message = "User phone number already exist" });
                    }
                    //else if (usercheckAccountNumber != null)
                    //{
                    //    return Json(new { error = true, message = "User Account Number already exist" }, JsonRequestBehavior.AllowGet);
                    //}
                    //else if (usercheckCNIC != null)
                    //{
                    //    return Json(new { error = true, message = "User CNIC already exist" }, JsonRequestBehavior.AllowGet);
                    //}
                    else
                    {
                        decimal pckgePrice = 0;

                        Package package = dc.Packages.Where(a => a.PackagePrice.Value.Equals(pckgePrice)).FirstOrDefault();
                        UserPackage userpackage = new UserPackage();
                        UserTableLevel userTableLevel = new UserTableLevel();
                        NewUserRegistration newuser = new NewUserRegistration();

                        newuser.Name = model.Name;
                        newuser.Username = model.Username;
                        newuser.Password = model.Password;
                        newuser.Country = model.Country;
                        //newuser.Address = model.Address;
                        newuser.Phone = model.Phone;
                        newuser.Email = model.Email;
                        //newuser.AccountNumber = model.AccountNumber;
                        //newuser.BankName = model.BankName;
                        //newuser.CNIC = model.CNICNumber;
                        newuser.IsThisFirstUser = model.IsThisFirstUser;
                        if (model.DownlineMemberId == 0 || model.DownlineMemberId == null)
                        {
                            newuser.DownlineMemberId = userId;
                        }
                        else
                        {
                            newuser.DownlineMemberId = model.DownlineMemberId.Value;
                        }
                        newuser.UserPosition = Common.Enum.UserPosition.Left;
                        newuser.IsUserActive = false;
                        newuser.IsNewRequest = true;
                        newuser.SponsorId = userId;
                        newuser.UpperId = model.UpperId;
                        newuser.PaidAmount = package.PackagePrice.Value;
                        newuser.CreateDate = DateTime.Now;
                        newuser.UserCode = Common.Enum.UserType.User.ToString();
                        newuser.IsPaidMember = false;
                        //newuser.UserPackage = model.UserPackage;
                        newuser.UserPackage = package.PackageId;
                        //file = Request.Files["AddNewMemberLeftImageData"];
                        var fileImage = model.DocumentImage;
                        if (fileImage != null)
                        {
                            // byte[] img = ConvertToBytes(fileImage);
                            newuser.DocumentImage = fileImage;
                        }
                        newuser.IsSleepingPartner = false;
                        newuser.IsSalesExecutive = false;
                        newuser.IsWithdrawalOpen = false;
                        newuser.IsBlock = false;
                        newuser.IsVerify = false;
                        dc.NewUserRegistrations.Add(newuser);
                        dc.SaveChanges();


                        userpackage.PackageId = package.PackageId;
                        userpackage.PackageName = package.PackageName;
                        userpackage.PackagePercent = package.PackagePercent;
                        userpackage.PackagePrice = package.PackagePrice;
                        userpackage.PackageValidity = package.PackageValidity;
                        userpackage.PackageMinWithdrawalAmount = package.PackageMinWithdrawalAmount;
                        userpackage.PackageMaxWithdrawalAmount = package.PackageMaxWithdrawalAmount;
                        userpackage.UserId = newuser.UserId;
                        userpackage.IsInCurrentUse = true;
                        userpackage.PurchaseDate = DateTime.Now;
                        dc.UserPackages.Add(userpackage);


                        userTableLevel.UserName = model.Username;
                        userTableLevel.TableLevel = 1;
                        userTableLevel.NoOfUsers = 0;
                        userTableLevel.RightUsers = 0;
                        userTableLevel.LeftUsers = 0;
                        userTableLevel.TableLevelLimit = 2;
                        userTableLevel.UserId = newuser.UserId;
                        userTableLevel.LastModifiedDate = DateTime.Now;
                        dc.UserTableLevels.Add(userTableLevel);

                        dc.SaveChanges();

                        #region creating first user tree

                        TreeDataTbl userTree = dbTree.TreeDataTbls.Where(a => a.UserId.Value.Equals(newuser.DownlineMemberId)).FirstOrDefault();

                        if (userTree == null)
                        {
                            if (newuser.IsThisFirstUser == true)
                            {
                                dbTree.insert_tree_node(newuser.Username, 0, newuser.UserId, newuser.DownlineMemberId, newuser.UserPosition);
                            }
                            else
                            {
                                dbTree.insert_tree_node(newuser.Username, userTree.Tree_ID, newuser.UserId, newuser.DownlineMemberId, newuser.UserPosition);
                            }
                        }



                        #endregion

                        #region send sms


                        TwilioClient.Init(SendSMSAccountSid, SendSMSAuthToken);

                        var message = MessageResource.Create(
                            body: "Welcome to Sleeping partner portal. "
                            + " Please make sure to pay your amount with in 5 bussiness days"
                            + " to avoid your account deactivation. "
                            + " Your Username is : " + model.Username
                            + " and password is : " + model.Password + "."
                            + " Click on http://sleepingpartnermanagementportalrct.com ",
                            from: new Twilio.Types.PhoneNumber(SendSMSFromNumber),
                            to: new Twilio.Types.PhoneNumber(model.Phone)
                        );


                        #endregion

                        #region user email

                       // System.Net.Mail.MailMessage mail1 = new System.Net.Mail.MailMessage();
                        //mail1.From = new MailAddress("noreply@sleepingpartnermanagementportalrct.com");
                        //mail1.To.Add(model.Email);
                        //mail1.Subject = "Sleeping partner management portal";
                        //mail1.Body = "User accept by admin. " +
                        //    " Your Username is " + model.Username + " and password : " + model.Password + "</br></br>" +
                        //    "<table style='font-family:Verdana, Helvetica, sans-serif;' cellpadding='0' cellspacing='0'><tbody><tr><td style='font-family:Verdana; border-right:2px solid #BD272D; padding-right:15px; text-align: right; vertical-align:top; ' valign='top'><table style='font-family:Verdana; margin-right:0; margin-left:auto;' cellpadding='0' cellspacing='0'><tbody><tr><td style='font-family:Verdana; height:55px; vertical-align:top; text-align:right;' valign='top' align='right'><span style='font-family:Verdana; font-size:14pt; font-weight:bold'>Sleeping partner management<span><br></span></span></td></tr><tr><td style='font-family:Verdana; height:40px; vertical-align:top; padding:0; text-align:right;' valign='top' align='right'><span style='font-family:Verdana; font-size:10pt;'>phone: 123456<span><br></span></span><span style='font-family:Verdana; font-size:10pt;'>mobile: 0123456</span></td></tr><tr><td><a href='http://sleepingpartnermanagementportalrct.com'>sleepingpartnermanagementportal</a></td></tr></tbody></table></td><td style='padding-left:15px;font-size:1pt; vertical-align:top; font-family:Verdana;' valign='top'><table style='font-family:Verdana;' cellpadding='0' cellspacing='0'><tbody><tr><td style='height:55px; font-family:Verdana; vertical-align:top;' valign='top'><a href='{Logo URL}' target='_blank'><img alt='Logo' style='height:40px; width:auto; border:0; ' height='40' border='0'  src='~/Content/images/newsleepinglogo.png'></a></td></tr><tr><td style='height:40px; font-family:Verdana; vertical-align:top; padding:0;' valign='top'><span style='font-family:Verdana; font-size:10pt;'>{Address 1}<span><br></span></span> <span style='font-family:Verdana; font-size:10pt;'>{Address 2}</span> </td></tr><tr><td style='height:20px; font-family:Verdana; vertical-align:middle;' valign='middle'><a href='http://{Web page}' target='_blank' style='color:#BD272D; font-size:10pt; font-family:Verdana;'>{Web page}</a></td></tr></tbody></table></td></tr></tbody></table>";
                        //mail1.IsBodyHtml = true;
                        //SmtpClient smtp1 = new SmtpClient();
                        //smtp1.Host = "sleepingpartnermanagementportalrct.com";
                        //smtp1.EnableSsl = true;
                        //smtp1.UseDefaultCredentials = false;
                        //smtp1.Credentials = new NetworkCredential("noreply@sleepingpartnermanagementportalrct.com", "Yly21#p8");
                        //smtp1.DeliveryMethod = SmtpDeliveryMethod.Network;
                        //smtp1.Port = 25;
                        //ServicePointManager.ServerCertificateValidationCallback =
                        //delegate (object s, X509Certificate certificate,
                        //         X509Chain chain, SslPolicyErrors sslPolicyErrors)
                        //{ return true; };
                        //smtp1.Send(mail1);
                        //await SendEmailToSponsor(newuserdata.Email, "sleeping patners", Body);

                        #endregion

                        ModelState.Clear();
                        model = null;
                        // ViewBag.MessageAddNewMemeberLeft = "Successfully Registration Done";
                    }

                }
                return Ok(new { success = true, message = "User has been saved" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }

        }
        //add right members in network page 
        [HttpPost]
        [Route("addrightmembers/{userId}")]
        public IHttpActionResult AddNewMemeberRight(UserModel model,int userId)
        {
            try
            {
                SleepingPartnermanagementTreeTestingEntities dbTree = new SleepingPartnermanagementTreeTestingEntities();
                using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
                {
                    var usercheckEmail = dc.NewUserRegistrations.Where(a => a.Email.Equals(model.Email)).FirstOrDefault();
                    var usercheckPhone = dc.NewUserRegistrations.Where(a => a.Phone.Equals(model.Phone)).FirstOrDefault();
                    //var usercheckAccountNumber = dc.NewUserRegistrations.Where(a => a.AccountNumber.Equals(model.AccountNumber)).FirstOrDefault();
                    //var usercheckCNIC = dc.NewUserRegistrations.Where(a => a.CNIC.Equals(model.CNICNumber)).FirstOrDefault();
                    if (usercheckEmail != null)
                    {
                        return Ok(new { success = false, message = "User email already exist" });
                    }
                    else if (usercheckPhone != null)
                    {
                        return Ok(new { success = false, message = "User phone number already exist" });
                    }
                    //else if (usercheckAccountNumber != null)
                    //{
                    //    return Json(new { error = true, message = "User Account Number already exist" }, JsonRequestBehavior.AllowGet);
                    //}
                    //else if (usercheckCNIC != null)
                    //{
                    //    return Json(new { error = true, message = "User CNIC already exist" }, JsonRequestBehavior.AllowGet);
                    //}
                    else
                    {
                        decimal pckgePrice = 0;

                        Package package = dc.Packages.Where(a => a.PackagePrice.Value.Equals(pckgePrice)).FirstOrDefault();
                        //Package package = dc.Packages.Where(a => a.PackageId.Equals(model.UserPackage)).FirstOrDefault();
                        UserPackage userpackage = new UserPackage();
                        UserTableLevel userTableLevel = new UserTableLevel();
                        NewUserRegistration newuser = new NewUserRegistration();

                        newuser.Name = model.Name;
                        newuser.Username = model.Username;
                        newuser.Password = model.Password;
                        newuser.Country = model.Country;
                        //newuser.Address = model.Address;
                        newuser.Phone = model.Phone;
                        newuser.Email = model.Email;
                        //newuser.AccountNumber = model.AccountNumber;
                        //newuser.BankName = model.BankName;
                        //newuser.CNIC = model.CNICNumber;
                        newuser.IsThisFirstUser = model.IsThisFirstUser;
                        if (model.DownlineMemberId == 0 || model.DownlineMemberId == null)
                        {
                            newuser.DownlineMemberId = userId;
                        }
                        else
                        {
                            newuser.DownlineMemberId = model.DownlineMemberId.Value;
                        }
                        newuser.UserPosition = Common.Enum.UserPosition.Right;
                        newuser.IsUserActive = false;
                        newuser.IsNewRequest = true;
                        newuser.SponsorId = userId;
                        newuser.UpperId = model.UpperId;
                        newuser.PaidAmount = package.PackagePrice;
                        newuser.CreateDate = DateTime.Now;
                        newuser.UserCode =Common.Enum.UserType.User.ToString();
                        newuser.IsPaidMember = false;
                        //newuser.UserPackage = model.UserPackage;
                        newuser.UserPackage = package.PackageId;
                        //file = Request.Files["AddNewMemberRightImageData"];
                        var fileImage = model.DocumentImage;
                        if (fileImage != null)
                        {
                            //byte[] img = ConvertToBytes(fileImage);
                            newuser.DocumentImage = fileImage;
                        }
                        newuser.IsSleepingPartner = false;
                        newuser.IsSalesExecutive = false;
                        newuser.IsWithdrawalOpen = false;
                        newuser.IsBlock = false;
                        newuser.IsVerify = false;
                        dc.NewUserRegistrations.Add(newuser);
                        dc.SaveChanges();


                        userpackage.PackageId = package.PackageId;
                        userpackage.PackageName = package.PackageName;
                        userpackage.PackagePercent = package.PackagePercent;
                        userpackage.PackagePrice = package.PackagePrice;
                        userpackage.PackageValidity = package.PackageValidity;
                        userpackage.PackageMinWithdrawalAmount = package.PackageMinWithdrawalAmount;
                        userpackage.PackageMaxWithdrawalAmount = package.PackageMaxWithdrawalAmount;
                        userpackage.UserId = newuser.UserId;
                        userpackage.IsInCurrentUse = true;
                        userpackage.PurchaseDate = DateTime.Now;

                        dc.UserPackages.Add(userpackage);


                        userTableLevel.UserName = model.Username;
                        userTableLevel.TableLevel = 1;
                        userTableLevel.NoOfUsers = 0;
                        userTableLevel.RightUsers = 0;
                        userTableLevel.LeftUsers = 0;
                        userTableLevel.TableLevelLimit = 2;
                        userTableLevel.UserId = newuser.UserId;
                        userTableLevel.LastModifiedDate = DateTime.Now;
                        dc.UserTableLevels.Add(userTableLevel);

                        dc.SaveChanges();

                        #region creating first user tree

                        TreeDataTbl userTree = dbTree.TreeDataTbls.Where(a => a.UserId.Value.Equals(newuser.DownlineMemberId)).FirstOrDefault();

                        if (userTree == null)
                        {
                            if (newuser.IsThisFirstUser == true)
                            {
                                dbTree.insert_tree_node(newuser.Username, 0, newuser.UserId, newuser.DownlineMemberId, newuser.UserPosition);
                            }
                            else
                            {
                                dbTree.insert_tree_node(newuser.Username, userTree.Tree_ID, newuser.UserId, newuser.DownlineMemberId, newuser.UserPosition);
                            }
                        }



                        #endregion

                        #region send sms


                        TwilioClient.Init(SendSMSAccountSid, SendSMSAuthToken);

                        var message = MessageResource.Create(
                            body: "Welcome to Sleeping partner portal. "
                            + " Please make sure to pay your amount with in 5 bussiness days"
                            + " to avoid your account deactivation. "
                            + " Your Username is : " + model.Username
                            + " and password is : " + model.Password + "."
                            + " Click on http://sleepingpartnermanagementportalrct.com ",
                            from: new Twilio.Types.PhoneNumber(SendSMSFromNumber),
                            to: new Twilio.Types.PhoneNumber(model.Phone)
                        );


                        #endregion

                        #region user email

                        System.Net.Mail.MailMessage mail1 = new System.Net.Mail.MailMessage();
                        mail1.From = new MailAddress("noreply@sleepingpartnermanagementportalrct.com");
                        mail1.To.Add(model.Email);
                        mail1.Subject = "Sleeping partner management portal";
                        mail1.Body = "User accept by admin. " +
                            " Your Username is " + model.Username + " and password : " + model.Password + "</br></br>" +
                            "<table style='font-family:Verdana, Helvetica, sans-serif;' cellpadding='0' cellspacing='0'><tbody><tr><td style='font-family:Verdana; border-right:2px solid #BD272D; padding-right:15px; text-align: right; vertical-align:top; ' valign='top'><table style='font-family:Verdana; margin-right:0; margin-left:auto;' cellpadding='0' cellspacing='0'><tbody><tr><td style='font-family:Verdana; height:55px; vertical-align:top; text-align:right;' valign='top' align='right'><span style='font-family:Verdana; font-size:14pt; font-weight:bold'>Sleeping partner management<span><br></span></span></td></tr><tr><td style='font-family:Verdana; height:40px; vertical-align:top; padding:0; text-align:right;' valign='top' align='right'><span style='font-family:Verdana; font-size:10pt;'>phone: 123456<span><br></span></span><span style='font-family:Verdana; font-size:10pt;'>mobile: 0123456</span></td></tr><tr><td><a href='http://sleepingpartnermanagementportalrct.com'>sleepingpartnermanagementportal</a></td></tr></tbody></table></td><td style='padding-left:15px;font-size:1pt; vertical-align:top; font-family:Verdana;' valign='top'><table style='font-family:Verdana;' cellpadding='0' cellspacing='0'><tbody><tr><td style='height:55px; font-family:Verdana; vertical-align:top;' valign='top'><a href='{Logo URL}' target='_blank'><img alt='Logo' style='height:40px; width:auto; border:0; ' height='40' border='0'  src='~/Content/images/newsleepinglogo.png'></a></td></tr><tr><td style='height:40px; font-family:Verdana; vertical-align:top; padding:0;' valign='top'><span style='font-family:Verdana; font-size:10pt;'>{Address 1}<span><br></span></span> <span style='font-family:Verdana; font-size:10pt;'>{Address 2}</span> </td></tr><tr><td style='height:20px; font-family:Verdana; vertical-align:middle;' valign='middle'><a href='http://{Web page}' target='_blank' style='color:#BD272D; font-size:10pt; font-family:Verdana;'>{Web page}</a></td></tr></tbody></table></td></tr></tbody></table>";
                        mail1.IsBodyHtml = true;
                        SmtpClient smtp1 = new SmtpClient();
                        smtp1.Host = "sleepingpartnermanagementportalrct.com";
                        smtp1.EnableSsl = true;
                        smtp1.UseDefaultCredentials = false;
                        smtp1.Credentials = new NetworkCredential("noreply@sleepingpartnermanagementportalrct.com", "Yly21#p8");
                        smtp1.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp1.Port = 25;
                        ServicePointManager.ServerCertificateValidationCallback =
                        delegate (object s, X509Certificate certificate,
                                 X509Chain chain, SslPolicyErrors sslPolicyErrors)
                        { return true; };
                        smtp1.Send(mail1);
                        //await SendEmailToSponsor(newuserdata.Email, "sleeping patners", Body);

                        #endregion

                        ModelState.Clear();
                        model = null;
                        //ViewBag.MessageAddNewMemeberRight = "Successfully Registration Done";

                    }
                }
                return Ok(new { success = true, message = "User has been saved" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }

        }
        //dropdown for left downliner memebers
      //  [Authorize]
        [HttpGet]
        [Route("dropdownleft/{userId}")]
        public IHttpActionResult GetUserForDownlineMemberByUserOnlyLeft(int userId)
        {
           // var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            string UserTypeUser = Common.Enum.UserType.User.ToString();

            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();

            List<GetParentChildsOuterLeftSP_Result> List = new List<GetParentChildsOuterLeftSP_Result>();
            List = db.GetParentChildsOuterLeftSP(userId).ToList();

            List<NewUserRegistration> listDownlineMember = new List<NewUserRegistration>();

            UserGenealogyTableLeft leftUsers = new UserGenealogyTableLeft();

            foreach (var item in List)
            {
                var userIdChild = Convert.ToInt32(item.UserId);
                if (item.UserCode == Common.Enum.UserType.User)
                {
                    leftUsers = db.UserGenealogyTableLefts.Where(a => a.DownlineMemberId.Value.Equals(userIdChild)).FirstOrDefault();
                    if (leftUsers == null)
                    {
                        listDownlineMember.Add(new NewUserRegistration() { UserId = item.UserId.Value, Username = item.Username, UserPosition = null });
                    }
                }
            }
            return Ok(listDownlineMember);           

            // ViewBag.DownlineMemberList = listDownlineMember;
        }
        // dropdown for right downliner memebers
       // [Authorize]
        [HttpGet]
        [Route("dropdownright/{userId}")]
        public IHttpActionResult GetUserForDownlineMemberByUserOnlyRight(int userId)
        {

         //   var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            string UserTypeUser = Common.Enum.UserType.User.ToString();

            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();

            List<GetParentChildsOuterRightSP_Result> List = new List<GetParentChildsOuterRightSP_Result>();
            List = db.GetParentChildsOuterRightSP(userId).ToList();

            List<NewUserRegistration> listDownlineMember = new List<NewUserRegistration>();

            UserGenealogyTableRight rightUsers = new UserGenealogyTableRight();

            foreach (var item in List)
            {
                var userIdChild = Convert.ToInt32(item.UserId);
                if (item.UserCode == Common.Enum.UserType.User)
                {
                    rightUsers = db.UserGenealogyTableRights.Where(a => a.DownlineMemberId.Value.Equals(userIdChild)).FirstOrDefault();
                    if (rightUsers == null)
                    {
                        listDownlineMember.Add(new NewUserRegistration() { UserId = item.UserId.Value, Username = item.Username, UserPosition = null });
                    }
                }
            }
            return Ok(listDownlineMember);
        }
        // [Authorize]
        [HttpGet]
        [Route("maketablemembersleft/{userId}")]
        public IHttpActionResult GetUserDownlineMembersLeft(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            SleepingPartnermanagementTreeTestingEntities dbTree = new SleepingPartnermanagementTreeTestingEntities();
            UserModel usrmodel = new UserModel();
            //string UserTypeAdmin = Common.Enum.UserType.Admin.ToString();
            string UserTypeUser = Common.Enum.UserType.User.ToString();       
            List<GetParentChildsLeftSP_Result> List = new List<GetParentChildsLeftSP_Result>();
            List = db.GetParentChildsLeftSP(userId).Where(a => a.IsPaidMember.Value == true).ToList();

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
                return Ok(List);           
        }
        [HttpGet]
        [Route("maketablemembersright/{userId}")]
        public IHttpActionResult GetUserDownlineMembersRight(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
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
        [Route("getuserpaidmembersleftlist/{userId}")]
        public IHttpActionResult GetUserPaidMembersLeftList(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            IEnumerable<UserModel> usrmodel = new List<UserModel>();
            usrmodel = (from n in db.GetParentChildsLeftSP(userId)
                        join c in db.NewUserRegistrations on n.SponsorId equals c.UserId
                        where n.IsPaidMember.Value == true
                        select new UserModel
                        {
                            UserId = n.UserId.Value,
                            Username = n.Username,
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

        //get paid and unpaid memebers on nework page
        [HttpGet]
        [Route("getuserunpaidmembersleftlist/{userId}")]
        public IHttpActionResult GetUserUnPaidMembersLeftList(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            IEnumerable<UserModel> usrmodel = new List<UserModel>();
            usrmodel = (from n in db.GetParentChildsLeftSP(userId)
                        join c in db.NewUserRegistrations on n.SponsorId equals c.UserId
                        where n.IsPaidMember.Value == false
                        select new UserModel
                        {
                            UserId = n.UserId.Value,
                            Username = n.Username,
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

        //get paid members right list
        [HttpGet]
        [Route("getuserPaidmembersrightlist/{userId}")]
        public IHttpActionResult GetUserPaidMembersRightList(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            IEnumerable<UserModel> usrmodel = new List<UserModel>();
            usrmodel = (from n in db.GetParentChildsRightSP(userId)
                        join c in db.NewUserRegistrations on n.SponsorId equals c.UserId
                        where n.IsPaidMember.Value == true
                        select new UserModel
                        {
                            UserId = n.UserId.Value,
                            Username = n.Username,
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
        //get Unpaid members right list
        [HttpGet]
        [Route("getuserunpaidmembersrightlist/{userId}")]
        public IHttpActionResult GetUserUnPaidMembersRightList(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            IEnumerable<UserModel> usrmodel = new List<UserModel>();
            usrmodel = (from n in db.GetParentChildsRightSP(userId)
                        join c in db.NewUserRegistrations on n.SponsorId equals c.UserId
                        where n.IsPaidMember.Value == false
                        select new UserModel
                        {
                            UserId = n.UserId.Value,
                            Username = n.Username,
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
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            string UserTypeUser = Common.Enum.UserType.User.ToString();
            UserModel usrmodel = new UserModel();
            List<UserModel> List = new List<UserModel>();
            List = db.NewUserRegistrations.Where(a => a.UserCode.Equals(UserTypeUser)
                && a.SponsorId.Equals(userId))
                .Select(x => new UserModel
                    //List = db.NewUserRegistrations.Select(x => new UserModel
                    {
                    UserId = x.UserId,
                    Username = x.Username,
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




