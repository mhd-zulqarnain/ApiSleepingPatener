using ApiSleepingPatener.Models;
using ApiSleepingPatener.Models.EWallet;
using ApiSleepingPatener.Models.GenealogyTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ApiSleepingPatener.Controllers
{
    public class GenealogyTableController : ApiController
    {

        //get user commission on genealogy page
        [HttpGet]
        [Route("GetUserCommission/{userId}")]
        public IHttpActionResult GetUserCommissionList(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<EWalletTransactionModel> List = new List<EWalletTransactionModel>();
            List = db.EWalletTransactions.Where(a => a.UserId.Value.Equals(userId)
                && a.IsParentBonus.Value.Equals(false) && a.IsMatchingBonus.Value.Equals(false)
                && a.IsPackageBonus.Value.Equals(true))
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
      //  get direcet commission
      [HttpGet]
      [Route("GetUserDirectCommission/{userId}")]
        public IHttpActionResult GetUserDirectCommissionList(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<EWalletTransactionModel> ListEwalletModel = new List<EWalletTransactionModel>();

            List<EWalletTransaction> ListEwallet = new List<EWalletTransaction>();

            //List<UserGenealogyTable> ListGeoTbl = new List<UserGenealogyTable>();
            NewUserRegistration newuser = new NewUserRegistration();

            ListEwallet = db.EWalletTransactions.Where(a => a.UserId.Value.Equals(userId)
                && a.IsParentBonus.Value.Equals(true) && a.IsMatchingBonus.Value.Equals(false)
                && a.IsPackageBonus.Value.Equals(false)).ToList();

            foreach (var item in ListEwallet)
            {
                var userIdChild = Convert.ToInt32(item.AsscociatedMember);
                bool checkWithDrawalOpen = false;

                newuser = db.NewUserRegistrations.Where(a => a.UserId.Equals(userIdChild)).FirstOrDefault();

                if (newuser.IsWithdrawalOpen == true)
                {
                    checkWithDrawalOpen = true;
                }
                if (newuser.IsWithdrawalOpen == false)
                {
                    checkWithDrawalOpen = false;
                }

                if (newuser != null)
                {
                    ListEwalletModel.Add(new EWalletTransactionModel()
                    {
                        TransactionId = item.TransactionId,
                        TransactionSource = item.TransactionSource,
                        TransactionName = item.TransactionName,
                        AsscociatedMember = item.AsscociatedMember.Value,
                        Amount = item.Amount.Value,
                        TransactionDate = item.TransactionDate.Value,
                        IsWithdrawlRequestByUser = item.IsWithdrawlRequestByUser.Value,
                        isWithdrawalOpen = checkWithDrawalOpen,
                        Username = ""
                    });

                }
            }

            return Ok(ListEwalletModel);        
        }
        //Get user macthing commission on gnealogy page
        [HttpGet]
        [Route("GetUserMatchingCommission/{userId}")]
        public IHttpActionResult GetUserMatchingCommissionList(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<EWalletTransactionModel> List = new List<EWalletTransactionModel>();  
                List = db.EWalletTransactions.Where(a => a.UserId.Value.Equals(userId)
                    && a.IsParentBonus.Value.Equals(false) && a.IsMatchingBonus.Value.Equals(true)
                    && a.IsPackageBonus.Value.Equals(false))
                        .Select(x => new EWalletTransactionModel
                        {
                            TransactionId = x.TransactionId,
                            TransactionSource = x.TransactionSource,
                            TransactionName = x.TransactionName,
                            AsscociatedMember = x.AsscociatedMember.Value,
                            Amount = x.Amount.Value,
                            TransactionDate = x.TransactionDate.Value,
                            IsWithdrawlRequestByUser = x.IsWithdrawlRequestByUser.Value
                        }).ToList();
            return Ok(List);
        }
           

        }
    }

