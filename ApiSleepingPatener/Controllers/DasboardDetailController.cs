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
    public class DasboardDetailController : ApiController
    {
        //[Authorize]
        [HttpGet]
        [Route("dashboard/{userId}")]
        public IHttpActionResult DashBoard(int userId)
        {
            Dashboarddetails dbd = new Dashboarddetails();
            string totalGetUserTotalPackageCommission = GetUserTotalPackageCommission(userId);
            string totaldirectcommission = GetUserTotalDirectCommission(userId);
            string totalGetUserTotalMatchingCommission = GetUserTotalMatchingCommission(userId);
            string totalGetUserCurrentPackage = GetUserCurrentPackage(userId);
            string totalGetUserDownlineMembers = GetUserDownlineMembers(userId);
            //Cash WithDrawn
            string totalGetPayoutHistorySum = GetPayoutHistorySum(userId);
            string totalewalletcredit = GetEWalletCreditSum(userId);
            string totalGetEWalletDebitSum = GetEWalletDebitSum(userId);
            string totalGetPaymentsInProcessSum = GetPaymentsInProcessSum(userId);
            string totalGetEWalletSummarySponsorBonus = GetEWalletSummarySponsorBonus(userId);
            //rewards
            string totalGetAllTotalLeftUserPV = GetAllTotalLeftUserPV(userId);
            string totalGetAllTotalRightUserPV = GetAllTotalRightUserPV(userId);
            EwalletModel ewm = GetAllCurrentRewardInfo(userId);





            //string totalGetAllCurrentRewardInfo = GetAllCurrentRewardInfo(userId);


            ////string totalGetleftamount = GetTotalleftamount(userId);
            //string totalGetremaningleftamount = GetTotalremainingleftamount(userId);
            //string totalGetremaningrightamount = GetTotalremainingrightamount(userId);
            //object
            dbd.GetUserTotalPackageCommission = totalGetUserTotalPackageCommission;
            dbd.totaldirectcommission = totaldirectcommission;
            dbd.GetUserTotalMatchingCommission = totalGetUserTotalMatchingCommission;
            dbd.GetUserCurrentPackage = totalGetUserCurrentPackage;
            dbd.GetUserDownlineMembers = totalGetUserDownlineMembers;
            dbd.GetPayoutHistorySum = totalGetPayoutHistorySum;
            dbd.GetEwalletCredit = totalewalletcredit;
            dbd.GetEWalletDebitSum = totalGetEWalletDebitSum;
            dbd.GetPaymentsInProcessSum = totalGetPaymentsInProcessSum;
            dbd.GetEWalletSummarySponsorBonus = totalGetEWalletSummarySponsorBonus;
            dbd.GetAllTotalRightUserPV = totalGetAllTotalRightUserPV;
            dbd.GetAllTotalLeftUserPV = totalGetAllTotalLeftUserPV;
            dbd.GetTotalremainingleftamount = ewm.bonus;
            dbd.GetTotalremainingrightamount = ewm.witdraw;


            return Ok(dbd);
        }
        public string GetUserTotalDirectCommission(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var CGP = (from a in dc.EWalletTransactions
                           where a.UserId.Value == userId
                           && a.IsParentBonus.Value == true
                           select a).ToList();
                var query = CGP.Sum(x => x.Amount);
                return query.ToString();
            }



        }

        public string GetEWalletCreditSum(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                string UserTypeUser = Common.Enum.UserType.User.ToString();

                var CGP = (from a in dc.EWalletTransactions
                           where a.Credit.Value == true
                           && a.Debit.Value == false
                           && a.UserId.Value == userId
                           select a).ToList();
                var query = CGP.Sum(x => x.Amount);
                //return query.toretu
                return query.ToString();
            }



        }

        public string GetEWalletDebitSum(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {

                var Debit = (from a in dc.EWalletTransactions
                             where a.Credit.Value == false
                             && a.Debit.Value == true
                             && a.UserId.Value == userId
                             select a).ToList();
                var DebitValue = Debit.Sum(x => x.Amount);

                var Credit = (from a in dc.EWalletTransactions
                              where a.Credit.Value == true
                              && a.Debit.Value == false
                              && a.UserId.Value == userId
                              select a).ToList();
                var CreditValue = Credit.Sum(x => x.Amount);

                var Sum = DebitValue - CreditValue;
                return Sum.ToString();

            }
        }

        public string GetPaymentsInProcessSum(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var CGP = (from a in dc.EWalletWithdrawalFunds
                           where a.IsActive.Value == true
                           && a.IsApproved.Value == true
                           && a.UserId.Value == userId
                           select a).ToList();
                var query = CGP.Sum(x => x.AmountPayble);
                return query.ToString();
            }
        }

        public string GetUserTotalPackageCommission(int userId)
        {

            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var CGP = (from a in dc.EWalletTransactions
                           where a.UserId.Value == userId
                           && a.IsPackageBonus.Value == true
                           select a).ToList();
                var query = CGP.Sum(x => x.Amount);
                return query.ToString();
            }


        }

        public string GetUserCurrentPackage(int userId)
        {
            return "";
            //using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            //{
            //    NewUserRegistration newpckg = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userId)).FirstOrDefault();

            //    Package pkge = dc.Packages.Where(a => a.PackageId.Equals(newpckg.UserPackage.Value)).FirstOrDefault();

            //    var PackageName = pkge.PackageName;
            //    var PackagePrice = pkge.PackagePrice.Value;
            //    return PackagePrice.ToString();
            //}
        }

        //[HttpGet]
        //[Route("getalltotalleftuserpv/{userId}")]
        public string GetAllTotalLeftUserPV(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var TotalAmountLeftUsers = dc.GetParentChildsLeftSP(userId).Where(a => a.IsPaidMember.Value.Equals(true)).ToList();
                var TotalAmountLeftUsersShow = TotalAmountLeftUsers.Sum(x => x.PaidAmount.Value);
                return TotalAmountLeftUsersShow.ToString();
                // return Ok(TotalAmountLeftUsers);


            }


        }

        //[HttpGet]
        //[Route("getalltotalrightuserpv/{userId}")]
        public string GetAllTotalRightUserPV(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var TotalAmountRightUsers = dc.GetParentChildsRightSP(userId).Where(a => a.IsPaidMember.Value.Equals(true)).ToList();
                var TotalAmountRightUsersShow = TotalAmountRightUsers.Sum(x => x.PaidAmount.Value);
                return TotalAmountRightUsersShow.ToString();

            }

        }

        public string GetUserDownlineMembers(int userId)
        {
            string UserTypeUser = Common.Enum.UserType.User.ToString();
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var totalLeft = dc.GetParentChildsLeftSP(userId).ToList();
                var totalRight = dc.GetParentChildsRightSP(userId).ToList();
                var query = totalLeft.Count() + totalRight.Count();
                return query.ToString();
            }


        }

        public string GetPayoutHistorySum(int userId)
        {

            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {

                var CGP = (from a in dc.EWalletWithdrawalFunds
                           where a.IsActive.Value == false && a.UserId == userId
                           && a.IsApproved.Value == true
                            && a.IsPaid.Value == true
                           select a).ToList();
                var query = CGP.Sum(x => x.AmountPayble);
                return query.ToString();
            }
        }
        public string GetUserTotalMatchingCommission(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {

                var CGP = (from a in dc.EWalletTransactions
                           where a.UserId.Value == userId
                           && a.IsMatchingBonus.Value == true
                           select a).ToList();
                var query = CGP.Sum(x => x.Amount);

                return query.ToString();
            }

        }
        public string GetEWalletSummarySponsorBonus(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var Debit = (from eWallTr in dc.EWalletTransactions
                             where eWallTr.UserId == userId && eWallTr.Credit == false && eWallTr.Debit == true
                             select eWallTr).ToList();
                var DebitValue = Debit.Sum(x => x.Amount);

                var Credit = (from eWallTr in dc.EWalletTransactions
                              where eWallTr.UserId == userId && eWallTr.Credit == true && eWallTr.Debit == false
                              select eWallTr).ToList();
                var CreditValue = Credit.Sum(x => x.Amount);

                var Sum = DebitValue - CreditValue;

                //if (Sum != null)
                //{
                //    return Json(new { success = true, result = Sum }, JsonRequestBehavior.AllowGet);
                //}
                return Sum.ToString();
            }

        }
        public EwalletModel GetAllCurrentRewardInfo(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {

                UserReward usrReward = dc.UserRewards.Where(a => a.UserId.Value.Equals(userId)).OrderByDescending(o => o.RewardId.Value).FirstOrDefault();

                int MinValueId = 0;

                var TotalAmountLeftUsers = dc.GetParentChildsLeftSP(userId).Where(a => a.IsPaidMember.Value.Equals(true)).ToList();
                var TotalAmountRightUsers = dc.GetParentChildsRightSP(userId).Where(a => a.IsPaidMember.Value.Equals(true)).ToList();

                decimal TotalAmountLeftUsersShow = TotalAmountLeftUsers.Sum(x => x.PaidAmount.Value);
                decimal TotalAmountRightUsersShow = TotalAmountRightUsers.Sum(x => x.PaidAmount.Value);

                Reward rewardRightlimit = (from rwrd in dc.Rewards
                                           where rwrd.Rightlimit >= TotalAmountRightUsersShow
                                           select rwrd).FirstOrDefault();

                Reward rewardLeftlimit = (from rwrd in dc.Rewards
                                          where rwrd.Leftlimit >= TotalAmountLeftUsersShow
                                          select rwrd).FirstOrDefault();

                if (rewardLeftlimit == null && rewardRightlimit == null) //if all rewards complete
                {
                    Reward rewardRightIfComplete = (from rwrd in dc.Rewards
                                                    where rwrd.Rightlimit <= TotalAmountRightUsersShow
                                                    select rwrd).OrderByDescending(o => o.Id).FirstOrDefault();

                    Reward rewardLeftIfComplete = (from rwrd in dc.Rewards
                                                   where rwrd.Leftlimit <= TotalAmountLeftUsersShow
                                                   select rwrd).OrderByDescending(o => o.Id).FirstOrDefault();

                    int MaxValueId = Math.Max((int)rewardRightIfComplete.Id, (int)rewardLeftIfComplete.Id);
                    if (usrReward != null)
                    {
                        if (usrReward.RewardId == MaxValueId)
                        {
                            EwalletModel obj1 = new EwalletModel();
                            obj1.bonus = "Completed";
                            obj1.witdraw = "Completed";
                            return obj1;
                        }
                        else
                        {
                            MinValueId = Math.Min((int)rewardRightIfComplete.Id, (int)rewardLeftIfComplete.Id);
                        }
                    }



                }
                else if (rewardLeftlimit == null)
                {
                    rewardRightlimit = (from rwrd in dc.Rewards
                                        where rwrd.Rightlimit <= TotalAmountRightUsersShow
                                        select rwrd).OrderByDescending(o => o.Id).FirstOrDefault();

                    rewardLeftlimit = (from rwrd in dc.Rewards
                                       where rwrd.Leftlimit <= TotalAmountRightUsersShow
                                       select rwrd).OrderByDescending(o => o.Id).FirstOrDefault();

                    MinValueId = Math.Min((int)rewardRightlimit.Id, (int)rewardLeftlimit.Id);

                }
                else if (rewardRightlimit == null)
                {
                    rewardRightlimit = (from rwrd in dc.Rewards
                                        where rwrd.Rightlimit <= TotalAmountRightUsersShow
                                        select rwrd).OrderByDescending(o => o.Id).FirstOrDefault();

                    rewardLeftlimit = (from rwrd in dc.Rewards
                                       where rwrd.Leftlimit <= TotalAmountRightUsersShow
                                       select rwrd).OrderByDescending(o => o.Id).FirstOrDefault();

                    MinValueId = Math.Min((int)rewardRightlimit.Id, (int)rewardLeftlimit.Id);
                }
                else
                {
                    MinValueId = Math.Min((int)rewardRightlimit.Id, (int)rewardLeftlimit.Id);
                }



                Reward reward = dc.Rewards.Where(a => a.Id.Equals(MinValueId)).FirstOrDefault();

                decimal LeftLimitPV = (decimal)reward.Leftlimit;
                decimal TotalLeftUserPV = TotalAmountLeftUsersShow;
                decimal MaxValueLeft = Math.Max(LeftLimitPV, TotalLeftUserPV);
                decimal RemainingLeftUserPV = MaxValueLeft - TotalLeftUserPV;


                decimal RightLimitPV = (decimal)reward.Rightlimit;
                decimal TotalRightUserPV = TotalAmountRightUsersShow;
                decimal MaxValueRight = Math.Max(RightLimitPV, TotalRightUserPV);
                decimal RemainingRightUserPV = MaxValueRight - TotalRightUserPV;


                if (reward != null)
                {
                    EwalletModel obj1 = new EwalletModel();
                    obj1.bonus = RemainingLeftUserPV.ToString();
                    obj1.witdraw = RemainingRightUserPV.ToString();
                    return obj1;
                }
            }
            EwalletModel obj = new EwalletModel();
            obj.bonus = "0";
            obj.witdraw = "0";

            return obj;

        }



        //get ewallet summery 
        [HttpGet]
        [Route("eWalletSummary/{userId}")]
        public IHttpActionResult eWalletSummary(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var Debit = (from eWallTr in dc.EWalletTransactions
                             where eWallTr.UserId == userId && eWallTr.Credit == false && eWallTr.Debit == true
                             select eWallTr).ToList();
                var DebitValue = Debit.Sum(x => x.Amount);

                var Credit = (from eWallTr in dc.EWalletTransactions
                              where eWallTr.UserId == userId && eWallTr.Credit == true && eWallTr.Debit == false
                              select eWallTr).ToList();
                var CreditValue = Credit.Sum(x => x.Amount);

                var Sum = DebitValue - CreditValue;

                //if (Sum != null)
                //{
                //    return Json(new { success = true, result = Sum }, JsonRequestBehavior.AllowGet);
                //}
                return Ok(new { success = true, message = Sum.ToString() });

            }

        }

        //New user selection post on dashboard page 
        [HttpPost]
        [Route("approvesaleexecutive/{userId}")]
        public IHttpActionResult ApproveSalesExecutiveContinue(int userId)
        {
            SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities();
            NewUserRegistration newuserdata = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userId)).FirstOrDefault();
            if (newuserdata != null)
            {
                newuserdata.IsSalesExecutive = true;
                newuserdata.UserDesignation = Common.Enum.UserAsSPorSM.SalesExecutive;
                dc.SaveChanges();
            }
            return Ok(new { success = true });
        }
        [HttpPost]
        [Route("approvesleepingpartner/{userId}")]
        public IHttpActionResult ApproveSleepingPartnerContinue(int userId)
        {
            SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities();
            NewUserRegistration newuserdata = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userId)).FirstOrDefault();
            if (newuserdata != null)
            {
                newuserdata.IsSleepingPartner = true;
                newuserdata.UserDesignation = Common.Enum.UserAsSPorSM.SleepingPartner;
                dc.SaveChanges();
            }
            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("getdata")]
        public IHttpActionResult getdatauser()
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();

            List<EWalletWithdrawalFundModel> List = new List<EWalletWithdrawalFundModel>();
            List = db.EWalletWithdrawalFunds.Where(a => a.IsActive.Value.Equals(true)
                    && a.IsPending.Value.Equals(false) && a.IsApproved.Value.Equals(true)
                    && a.IsPaid.Value.Equals(false) && a.IsRejected.Value.Equals(false))
                .Select(x => new EWalletWithdrawalFundModel
                {
                    WithdrawalFundId = x.WithdrawalFundId,
                    Username = x.UserName,
                    WithdrawalFundMethod = x.WithdrawalFundMethod,
                    AmountPayble = x.AmountPayble.Value,
                    WithdrawalFundCharge = x.WithdrawalFundCharge.Value,
                    ApprovedDate = x.ApprovedDate.Value
                }).ToList();

            return Ok(List);
           
        }

        [HttpPost]
        [Route("verifyuser/{userId}/{id}")]
        public IHttpActionResult changestatus(int userId , int id)
        {
          
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                EWalletTransaction data = new EWalletTransaction();

                EWalletWithdrawalFund newuserdata = dc.EWalletWithdrawalFunds
                    .Where(a => a.WithdrawalFundId.Equals(id)).FirstOrDefault();

                var ewalletTransId = Convert.ToInt32(newuserdata.EwalletTransId.Value);
                EWalletTransaction ewalletCheck = dc.EWalletTransactions.Where(a => a.TransactionId.Equals(ewalletTransId)).FirstOrDefault();

                if (newuserdata != null)
                {
                    newuserdata.PaidDate = DateTime.Now;
                    newuserdata.IsActive = false;
                    newuserdata.IsPaid = true;

                    data.TransactionSource = newuserdata.UserName + " : "
                        + Common.Enum.AmountSource.Withdrawal.ToString() + " : " + newuserdata.WithdrawalFundDescription;
                    data.TransactionName = Common.Enum.AmountDebitOrCredit.Credit.ToString();
                    data.AsscociatedMember = newuserdata.UserId;
                    data.Amount = newuserdata.AmountPayble;
                    data.TransactionDate = DateTime.Now;
                    data.Credit = true;
                    data.Debit = false;
                    data.UserId = newuserdata.UserId;
                    data.IsPackageBonus = ewalletCheck.IsPackageBonus.Value;
                    data.PackageId = ewalletCheck.PackageId.Value;
                    data.IsMatchingBonus = ewalletCheck.IsMatchingBonus.Value;
                    data.IsParentBonus = ewalletCheck.IsParentBonus.Value;
                    data.IsWithdrawlRequestByUser = false;
                    data.IsWithdrawlPaidByAdmin = false;
                    data.IsWithdrawlRequestApproved = false;
                    data.AdminCredit = false;
                    data.AdminDebit = false;
                    data.AdminTransactionName = Common.Enum.AmountSource.Withdrawal.ToString();
                    dc.EWalletTransactions.Add(data);
                    dc.SaveChanges();
                    
                    ewalletCheck.IsWithdrawlRequestApproved = true;
                    ewalletCheck.IsWithdrawlPaidByAdmin = true;
                    dc.SaveChanges();

                    ModelState.Clear();
                    return Ok(new { success = true, message = "success" });
                }
                else
                {
                    return Json(new { error = true, message = "failed" });
                }

            }
        }

    }


}



