using ApiSleepingPatener.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiSleepingPatener.Controllers
{
    public class ReportsController : ApiController
    {
        //active payout on report page
        [HttpGet]
        [Route("getactivepayout/{userId}")]
        public IHttpActionResult GetActivePayoutList(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<ActivePayoutModel> List = new List<ActivePayoutModel>();

            List = db.EWalletWithdrawalFunds.Where(a => a.UserId.Value.Equals(userId)
            && a.IsActive.Value == true && a.IsApproved.Value == true)
                .Select(x => new ActivePayoutModel
                {
                    Username = x.Username,
                    WithdrawalFundMethod = x.WithdrawalFundMethod,
                    AccountNumber = x.AccountNumber,
                    BankName = x.BankName,
                    AmountPayble = x.AmountPayble.Value,
                    WithdrawalFundCharge = x.WithdrawalFundCharge.Value,
                    ApprovedDate = x.ApprovedDate.Value
                }).ToList();
            return Ok(List);
        }
        //payout history on report page
        [HttpGet]
        [Route("getpayouthistory/{userId}")]
        public IHttpActionResult GetPayoutHistoryList(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<PayoutHistoryModel> List = new List<PayoutHistoryModel>();
            List = db.EWalletWithdrawalFunds.Where(a => a.UserId.Value.Equals(userId)
            && a.IsActive.Value == false && a.IsApproved.Value == true && a.IsPaid.Value == true)
                .Select(x => new PayoutHistoryModel
                {
                    UserId = x.UserId.Value,
                    Username = x.Username,
                    WithdrawalFundMethod = x.WithdrawalFundMethod,
                    AccountNumber = x.AccountNumber,
                    BankName = x.BankName,
                    AmountPayble = x.AmountPayble.Value,
                    WithdrawalFundCharge = x.WithdrawalFundCharge.Value,
                    ApprovedDate = x.ApprovedDate.Value,
                    PaidDate = x.PaidDate.Value
                }).ToList();
            return Ok(List);

        }
        //payout history on report page
        [HttpGet]
        [Route("getpayoutwithdrawinprocess/{userId}")]
        public IHttpActionResult GetPayoutWithdrawalInProccessList(int userId)
        {

            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<PayoutWithdrawalInProccessModel> List = new List<PayoutWithdrawalInProccessModel>();           
                List = db.EWalletWithdrawalFunds.Where(a => a.UserId.Value.Equals(userId)
                && a.IsActive.Value == true && a.IsApproved.Value == true)
                    .Select(x => new PayoutWithdrawalInProccessModel
                    {
                        Username = x.Username,
                        WithdrawalFundMethod = x.WithdrawalFundMethod,
                        AccountNumber = x.AccountNumber,
                        BankName = x.BankName,
                        AmountPayble = x.AmountPayble.Value,
                        WithdrawalFundCharge = x.WithdrawalFundCharge.Value,
                        ApprovedDate = x.ApprovedDate.Value
                    }).ToList();
            return Ok(List);
        }
           

        }
    }

