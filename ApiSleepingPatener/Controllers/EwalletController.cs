﻿using ApiSleepingPatener.Models;
using ApiSleepingPatener.Models.Account;
using ApiSleepingPatener.Models.GenealogyTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace ApiSleepingPatener.Controllers
{
    public class EwalletController : ApiController
    {
        //summary
        [HttpGet]
        [Route("ewalletsummary/summary/{userId}")]
        public IHttpActionResult getbonusandwithdrawsummary(int userId)
        {
            EwalletModel obj = new EwalletModel();
            obj.bonus = GetEWalletSummarySponsorBonus(userId);
            obj.witdraw = GetEWalletSummaryWithdrawal(userId);
                return Ok(obj);

        }

        //montlhy
        [HttpGet]
        [Route("ewalletsummary/summarymonthly/{userId}")]
        public IHttpActionResult getbonusandwithdrawmontlhy(int userId)
        {
            EwalletModel obj = new EwalletModel();
            obj.bonus = GetEWalletThisMonthSponsorBonus(userId);
            obj.witdraw = GetEWalletThisMonthWithdrawal(userId);
            return Ok(obj);

        }
        //yearly
        [HttpGet]
        [Route("ewalletsummary/summaryyearly/{userId}")]
        public IHttpActionResult getbonusandwithdrawyearly(int userId)
        {
            EwalletModel obj = new EwalletModel();
            obj.bonus = GetEWalletThisYearSponsorBonus(userId);
            obj.witdraw = GetEWalletThisYearWithdrawal(userId);
            return Ok(obj);

        }
        //E wallet Transacion
        //Overall transaction
        [HttpGet]
        [Route("wallet/overalllist/{userId}")] 
        public IHttpActionResult GetEWalletTransactionsOverAllList(int userId)
        {
            sleepingtestEntities db = new sleepingtestEntities();
            List<EWalletTransactionModel> List = new List<EWalletTransactionModel>();           
                List = db.EWalletTransactions.Where(a => a.UserId.Value.Equals(userId))
                    .Select(x => new EWalletTransactionModel
                    {
                        TransactionId = x.TransactionId,
                        TransactionSource = x.TransactionSource,
                        TransactionName = x.TransactionName,
                        AsscociatedMember = x.AsscociatedMember.Value,
                        Amount = x.Amount.Value,
                        TransactionDate = x.TransactionDate.Value
                    }).ToList();

         
            //return Json(new { data = List }, JsonRequestBehavior.AllowGet);
            return Ok(List);

        }

        //This Months transaction
        [HttpGet] 
        [Route("wallet/thismonth/{userId}")]
        public IHttpActionResult GetEWalletTransactionsThisMonthList(int userId)
        {            
            sleepingtestEntities db = new sleepingtestEntities();
            List<EWalletTransactionModel> List = new List<EWalletTransactionModel>();          

                List = db.EWalletTransactions.Where(a => a.UserId.Value.Equals(userId)
                    && a.TransactionDate.Value.Year.Equals(DateTime.Now.Year)
                    && a.TransactionDate.Value.Month.Equals(DateTime.Now.Month))
                    .Select(x => new EWalletTransactionModel
                    {
                        TransactionId = x.TransactionId,
                        TransactionSource = x.TransactionSource,
                        TransactionName = x.TransactionName,
                        AsscociatedMember = x.AsscociatedMember.Value,
                        Amount = x.Amount.Value,
                        TransactionDate = x.TransactionDate.Value
                    }).ToList();
            return Ok(List);
                 

        }
        [HttpGet]
        [Route("ewalletcredit/overall/{userId}")]
        public IHttpActionResult GetEWalletCreditsOverAllList(int userId)
        {
            sleepingtestEntities db = new sleepingtestEntities();
            List<EWalletTransactionModel> List = new List<EWalletTransactionModel>();          
                List = db.EWalletTransactions.Where(a => a.UserId.Value.Equals(userId)
                && a.Credit == true && a.Debit == false)
                    .Select(x => new EWalletTransactionModel
                    {
                        TransactionId = x.TransactionId,
                        TransactionSource = x.TransactionSource,
                        TransactionName = x.TransactionName,
                        AsscociatedMember = x.AsscociatedMember.Value,
                        Amount = x.Amount.Value,
                        TransactionDate = x.TransactionDate.Value
                    }).ToList();
            return Ok(List);

        }
        [HttpGet]
        [Route("ewalletcredit/thismonth/{userId}")]
        public IHttpActionResult GetEWalletCreditsThisMonthList(int userId)
        {
            sleepingtestEntities db = new sleepingtestEntities();
            List<EWalletTransactionModel> List = new List<EWalletTransactionModel>();          
                List = db.EWalletTransactions.Where(a => a.UserId.Value.Equals(userId)
                    && a.TransactionDate.Value.Year.Equals(DateTime.Now.Year)
                    && a.TransactionDate.Value.Month.Equals(DateTime.Now.Month)
                    && a.Credit == true && a.Debit == false)
                    .Select(x => new EWalletTransactionModel
                    {
                        TransactionId = x.TransactionId,
                        TransactionSource = x.TransactionSource,
                        TransactionName = x.TransactionName,
                        AsscociatedMember = x.AsscociatedMember.Value,
                        Amount = x.Amount.Value,
                        TransactionDate = x.TransactionDate.Value
                    }).ToList();
            return Ok(List);

        }
        //debit
        [HttpGet]
        [Route("ewalletdebit/overall/{userId}")]
        public IHttpActionResult GetEWalletDebitsOverAllList(int userId)
        {
            sleepingtestEntities db = new sleepingtestEntities();
            List<EWalletTransactionModel> List = new List<EWalletTransactionModel>();          
                List = db.EWalletTransactions.Where(a => a.UserId.Value.Equals(userId)
                    && a.Credit == false && a.Debit == true)
                    .Select(x => new EWalletTransactionModel
                    {
                        TransactionId = x.TransactionId,
                        TransactionSource = x.TransactionSource,
                        TransactionName = x.TransactionName,
                        AsscociatedMember = x.AsscociatedMember.Value,
                        Amount = x.Amount.Value,
                        TransactionDate = x.TransactionDate.Value
                    }).ToList();

            return Ok(List);

        }
        [HttpGet]
        [Route("ewalletdebit/thismonth/{userId}")]
        public IHttpActionResult GetEWalletDebitsThisMonthList(int userId)
        {
            sleepingtestEntities db = new sleepingtestEntities();
            List<EWalletTransactionModel> List = new List<EWalletTransactionModel>();      
                List = db.EWalletTransactions.Where(a => a.UserId.Value.Equals(userId)
                    && a.TransactionDate.Value.Year.Equals(DateTime.Now.Year)
                    && a.TransactionDate.Value.Month.Equals(DateTime.Now.Month)
                    && a.Credit == false && a.Debit == true)
                    .Select(x => new EWalletTransactionModel
                    {
                        TransactionId = x.TransactionId,
                        TransactionSource = x.TransactionSource,
                        TransactionName = x.TransactionName,
                        AsscociatedMember = x.AsscociatedMember.Value,
                        Amount = x.Amount.Value,
                        TransactionDate = x.TransactionDate.Value
                    }).ToList();
            return Ok(List);

        }

        //

        //Overall transaction
        [HttpPost]
        [Route("addAds")]
        public IHttpActionResult postAdd(Advertisement model)
        {
            sleepingtestEntities db = new sleepingtestEntities();
          
            db.Advertisements.Add(model);
            db.SaveChanges();

            return Ok("success");

        }
        
        public string GetEWalletSummarySponsorBonus(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            using (sleepingtestEntities dc = new sleepingtestEntities())
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

                return Sum.ToString();
            }
           // return View();
        }
        
        public string GetEWalletSummaryWithdrawal(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            using (sleepingtestEntities dc = new sleepingtestEntities())
            {
                var CGP = (from eWallTr in dc.EWalletTransactions
                           where eWallTr.UserId == userId && eWallTr.Credit == true && eWallTr.Debit == false
                           select eWallTr).ToList();
                var query = CGP.Sum(x => x.Amount);

                return query.ToString();
            }
            //return View();
        }

        //ewallet motnhly
        
        public string GetEWalletThisMonthSponsorBonus(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            using (sleepingtestEntities dc = new sleepingtestEntities())
            {
                var Debit = (from eWallTr in dc.EWalletTransactions
                             where eWallTr.UserId == userId && eWallTr.Credit == false && eWallTr.Debit == true
                             && eWallTr.TransactionDate.Value.Year == DateTime.Now.Year
                             && eWallTr.TransactionDate.Value.Month == DateTime.Now.Month
                             select eWallTr).ToList();
                var DebitValue = Debit.Sum(x => x.Amount);

                var Credit = (from eWallTr in dc.EWalletTransactions
                              where eWallTr.UserId == userId && eWallTr.Credit == true && eWallTr.Debit == false
                              && eWallTr.TransactionDate.Value.Year == DateTime.Now.Year
                              && eWallTr.TransactionDate.Value.Month == DateTime.Now.Month
                              select eWallTr).ToList();
                var CreditValue = Credit.Sum(x => x.Amount);

                var Sum = DebitValue - CreditValue;
                return Sum.ToString();
            }
                   }


        public string GetEWalletThisMonthWithdrawal(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            using (sleepingtestEntities dc = new sleepingtestEntities())
            {
                var CGP = (from eWallTr in dc.EWalletTransactions
                           where eWallTr.UserId == userId && eWallTr.Credit == true && eWallTr.Debit == false
                           && eWallTr.TransactionDate.Value.Year == DateTime.Now.Year
                           && eWallTr.TransactionDate.Value.Month == DateTime.Now.Month
                           select eWallTr).ToList();
                var query = CGP.Sum(x => x.Amount);
                return query.ToString();
            }
        }

        //ewallet bonus and withdraw yearly

        
        public string GetEWalletThisYearSponsorBonus(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            using (sleepingtestEntities dc = new sleepingtestEntities())
            {
                var Debit = (from eWallTr in dc.EWalletTransactions
                             where eWallTr.UserId == userId && eWallTr.Credit == false && eWallTr.Debit == true
                             && eWallTr.TransactionDate.Value.Year == DateTime.Now.Year
                             select eWallTr).ToList();
                var DebitValue = Debit.Sum(x => x.Amount);

                var Credit = (from eWallTr in dc.EWalletTransactions
                              where eWallTr.UserId == userId && eWallTr.Credit == true && eWallTr.Debit == false
                              && eWallTr.TransactionDate.Value.Year == DateTime.Now.Year
                              select eWallTr).ToList();
                var CreditValue = Credit.Sum(x => x.Amount);

                var Sum = DebitValue - CreditValue;
                return Sum.ToString();
            }
        }

   
        public string GetEWalletThisYearWithdrawal(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            using (sleepingtestEntities dc = new sleepingtestEntities())
            {
                var CGP = (from eWallTr in dc.EWalletTransactions
                           where eWallTr.UserId == userId && eWallTr.Credit == true && eWallTr.Debit == false
                           && eWallTr.TransactionDate.Value.Year == DateTime.Now.Year
                           select eWallTr).ToList();
                var query = CGP.Sum(x => x.Amount);
               return query.ToString();
            }
        }

        //Ewallet my withdrawl request
        [HttpGet]
        [Route("GetEWalletPendingWithdrawalRequests/{userId}")]
        public IHttpActionResult GetEWalletPendingWithdrawalRequestsList(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            sleepingtestEntities db = new sleepingtestEntities();
            List<EWalletWithdrawalFundModel> List = new List<EWalletWithdrawalFundModel>();  
                List = db.EWalletWithdrawalFunds.Where(a => a.UserId.Value.Equals(userId)
                && a.IsActive.Value == true && a.IsPending.Value == true)
                    .Select(x => new EWalletWithdrawalFundModel
                    {
                        UserName = x.UserName,
                        WithdrawalFundMethod = x.WithdrawalFundMethod,
                        AmountPayble = x.AmountPayble.Value,
                        WithdrawalFundCharge = x.WithdrawalFundCharge.Value,
                        RequestedDate = x.RequestedDate.Value
                    }).ToList();
            return Ok(List);

        }
        [HttpGet]
        [Route("GetEWalletApprovedRequestPendingPayment/{userId}")]
        public IHttpActionResult GetEWalletApprovedRequestPendingPaymentList(int userId)
        {
           // var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            sleepingtestEntities db = new sleepingtestEntities();
            List<EWalletWithdrawalFundModel> List = new List<EWalletWithdrawalFundModel>();  
                List = db.EWalletWithdrawalFunds.Where(a => a.UserId.Value.Equals(userId)
                && a.IsActive.Value == true && a.IsApproved.Value == true)
                    .Select(x => new EWalletWithdrawalFundModel
                    {
                        UserName = x.UserName,
                        WithdrawalFundMethod = x.WithdrawalFundMethod,
                        AmountPayble = x.AmountPayble.Value,
                        WithdrawalFundCharge = x.WithdrawalFundCharge.Value,
                        ApprovedDate = x.ApprovedDate.Value
                    }).ToList();
            return Ok(List);

        }
        [HttpGet]
        [Route("GetEWalletApprovedRequestPaidPayment/{userId}")]
        public IHttpActionResult GetEWalletApprovedRequestPaidPaymentList(int userId)
        {
            sleepingtestEntities db = new sleepingtestEntities();
            List<EWalletWithdrawalFundModel> List = new List<EWalletWithdrawalFundModel>();     
                List = db.EWalletWithdrawalFunds.Where(a => a.UserId.Value.Equals(userId)
                && a.IsActive.Value == false && a.IsApproved.Value == true && a.IsPaid.Value == true)
                    .Select(x => new EWalletWithdrawalFundModel
                    {
                        UserName = x.UserName,
                        WithdrawalFundMethod = x.WithdrawalFundMethod,
                        AmountPayble = x.AmountPayble.Value,
                        WithdrawalFundCharge = x.WithdrawalFundCharge.Value,
                        PaidDate = x.PaidDate.Value
                    }).ToList();
            return Ok(List);

        }
        [HttpGet]
        [Route("GetEWalletRejectedWithdrawalRequests/{userId}")]
        public IHttpActionResult GetEWalletRejectedWithdrawalRequestsList(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            sleepingtestEntities db = new sleepingtestEntities();
            List<EWalletWithdrawalFundModel> List = new List<EWalletWithdrawalFundModel>();
            List = db.EWalletWithdrawalFunds.Where(a => a.UserId.Value.Equals(userId)
                && a.IsActive.Value == false && a.IsRejected.Value == true)
                    .Select(x => new EWalletWithdrawalFundModel
                    {
                        UserName = x.UserName,
                        WithdrawalFundMethod = x.WithdrawalFundMethod,
                        AmountPayble = x.AmountPayble.Value,
                        WithdrawalFundCharge = x.WithdrawalFundCharge.Value,
                        RejectedDate = x.RejectedDate.Value
                    }).ToList();
            return Ok(List);

        }
    }
}
