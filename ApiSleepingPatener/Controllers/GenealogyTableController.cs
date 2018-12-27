using ApiSleepingPatener.Models;
using ApiSleepingPatener.Models.EWallet;
using ApiSleepingPatener.Models.GenealogyTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace ApiSleepingPatener.Controllers
{
    public class GenealogyTableController : ApiController
    {
        string SendSMSAccountSid = System.Configuration.ConfigurationManager.AppSettings["SendSMSAccountSid"];
        string SendSMSAuthToken = System.Configuration.ConfigurationManager.AppSettings["SendSMSAuthToken"];
        string SendSMSFromNumber = System.Configuration.ConfigurationManager.AppSettings["SendSMSFromNumber"];
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
                    && a.IsPackageBonus.Value.Equals(false) && a.Debit.Value.Equals(true)).ToList();

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
                            IsWithdrawlPaidByAdmin = item.IsWithdrawlPaidByAdmin.Value,
                            IsWithdrawlRequestApproved = item.IsWithdrawlRequestApproved.Value,
                            isWithdrawalOpen = checkWithDrawalOpen,
                
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
                  && a.IsPackageBonus.Value.Equals(false) && a.Debit.Value.Equals(true))
                      .Select(x => new EWalletTransactionModel
                      {
                          TransactionId = x.TransactionId,
                          TransactionSource = x.TransactionSource,
                          TransactionName = x.TransactionName,
                          AsscociatedMember = x.AsscociatedMember.Value,
                          Amount = x.Amount.Value,
                          TransactionDate = x.TransactionDate.Value,
                          IsWithdrawlRequestByUser = x.IsWithdrawlRequestByUser.Value,
                          IsWithdrawlRequestApproved = x.IsWithdrawlRequestApproved.Value,
                          IsWithdrawlPaidByAdmin = x.IsWithdrawlPaidByAdmin.Value
                      }).ToList();

            return Ok(List);
        }

        [HttpPost]
        [Route("sendmatchingtablecommissionrequest/{transactionId}")]
        public IHttpActionResult SendMatchingTableCommissionRequest(int transactionId) //Umair
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                EWalletTransaction ewallet = dc.EWalletTransactions
                    .Where(a => a.TransactionId.Equals(transactionId)).FirstOrDefault();
                if (ewallet != null)
                {
                    if (ewallet.IsWithdrawlRequestByUser == false)
                    {
                        var userId = Convert.ToInt32(ewallet.UserId.Value);
                        NewUserRegistration user = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userId)).FirstOrDefault();
                        EWalletWithdrawalFund newpckgadd = new EWalletWithdrawalFund();

                        if (user.IsVerify.Value == false)
                        {
                            return Ok(new { info = true, message = " you are not eligible to withdrawal direct commission please complete and verify your profile" });
                        }
                        else
                        {
                            //TwilioClient.Init(SendSMSAccountSid, SendSMSAuthToken);
                            //var message = MessageResource.Create(
                            //    body: "Sleeping partner portal alert. "
                            //    + " You Sent Withdrawal Request To Admin"
                            //    + " Your Withdrawal Amount is : " + ewallet.Amount
                            //    + " and request Date is : " + DateTime.Now + "."
                            //    + " Click on http://sleepingpartnermanagementportalrct.com ",
                            //    from: new Twilio.Types.PhoneNumber(SendSMSFromNumber),
                            //    to: new Twilio.Types.PhoneNumber(user.Phone)
                            //);

                            //System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                            //mail.From = new MailAddress("noreply@sleepingpartnermanagementportalrct.com");
                            //mail.To.Add(user.Email);
                            //mail.Subject = "Sleeping partner management portal";
                            //mail.Body += "Sleeping partner portal alert." +
                            //            " You Sent Withdrawal Request To Admin" +
                            //            " Your Withdrawal Amount is : " + ewallet.Amount +
                            //            " and request Date is : " + DateTime.Now + "."
                            //            + " Click on http://sleepingpartnermanagementportalrct.com ";
                            //mail.IsBodyHtml = true;
                            //SmtpClient smtp = new SmtpClient();
                            //smtp.Host = "sleepingpartnermanagementportalrct.com";
                            //smtp.EnableSsl = true;
                            //smtp.UseDefaultCredentials = false;
                            //smtp.Credentials = new NetworkCredential("noreply@sleepingpartnermanagementportalrct.com", "Yly21#p8");
                            //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            //smtp.Port = 25;
                            //ServicePointManager.ServerCertificateValidationCallback =
                            //delegate (object s, X509Certificate certificate,
                            //         X509Chain chain, SslPolicyErrors sslPolicyErrors)
                            //{ return true; };
                            //smtp.Send(mail);

                            ewallet.IsWithdrawlRequestByUser = true;
                            ewallet.IsWithdrawlPaidByAdmin = false;
                            ewallet.IsWithdrawlRequestApproved = false;
                            dc.SaveChanges();


                            newpckgadd.UserName = user.Username;
                            newpckgadd.AccountNumber = user.AccountNumber;
                            newpckgadd.BankName = user.BankName;
                            newpckgadd.WithdrawalFundMethod = Common.Enum.PaymentSource.BankAccount.ToString();
                            newpckgadd.AmountPayble = ewallet.Amount.Value;
                            newpckgadd.WithdrawalFundDescription = ewallet.TransactionSource;
                            newpckgadd.WithdrawalFundCharge = 0;
                            newpckgadd.Country = user.Country;
                            newpckgadd.RequestedDate = DateTime.Now;
                            newpckgadd.ApprovedDate = null;
                            newpckgadd.PaidDate = null;
                            newpckgadd.RejectedDate = null;
                            newpckgadd.IsActive = true;
                            newpckgadd.IsPending = true;
                            newpckgadd.IsApproved = false;
                            newpckgadd.IsPaid = false;
                            newpckgadd.IsRejected = false;
                            newpckgadd.UserId = user.UserId;
                            newpckgadd.EwalletTransId = ewallet.TransactionId;
                            dc.EWalletWithdrawalFunds.Add(newpckgadd);
                            dc.SaveChanges();

                            ModelState.Clear();
                            return Ok(new { success = true, message = "Successfully Done" });
                        }

                    }
                    else
                    {
                        return Ok(new { info = true, message = "You already sended request" });
                    }

                }
                else
                {
                    return Ok(new { error = true, message = "unsuccessfully" });
                }

            }
        }
        [HttpPost]
        [Route("senddirectsalecommissionrequest/{transactionId}")]
        public IHttpActionResult SendDirectSaleCommissionRequest(int transactionId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                EWalletTransaction ewallet = dc.EWalletTransactions
                    .Where(a => a.TransactionId.Equals(transactionId)).FirstOrDefault();
                if (ewallet != null)
                {
                    if (ewallet.IsWithdrawlRequestByUser == false)
                    {
                        var userId = Convert.ToInt32(ewallet.UserId.Value);
                        NewUserRegistration user = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userId)).FirstOrDefault();
                        EWalletWithdrawalFund newpckgadd = new EWalletWithdrawalFund();

                        if (user.IsVerify.Value == false)
                        {
                            return Ok(new { info = true, message = "you are not eligible to withdrawal direct commission please complete and verify your profile" });
                        }
                        else
                        {
                        //    TwilioClient.Init(SendSMSAccountSid, SendSMSAuthToken);
                        //    var message = MessageResource.Create(
                        //        body: "Sleeping partner portal alert. "
                        //        + " You Sent Withdrawal Request To Admin"
                        //        + " Your Withdrawal Amount is : " + ewallet.Amount
                        //        + " and request Date is : " + DateTime.Now + "."
                        //        + " Click on http://sleepingpartnermanagementportalrct.com ",
                        //        from: new Twilio.Types.PhoneNumber(SendSMSFromNumber),
                        //        to: new Twilio.Types.PhoneNumber(user.Phone)
                        //    );


                        //    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                        //    mail.From = new MailAddress("noreply@sleepingpartnermanagementportalrct.com");
                        //    mail.To.Add(user.Email);
                        //    mail.Subject = "Sleeping partner management portal";
                        //    mail.Body += "Sleeping partner portal alert." +
                        //                " You Sent Withdrawal Request To Admin" +
                        //                " Your Withdrawal Amount is : " + ewallet.Amount +
                        //                " and request Date is : " + DateTime.Now + "."
                        //                + " Click on http://sleepingpartnermanagementportalrct.com ";
                        //    mail.IsBodyHtml = true;
                        //    SmtpClient smtp = new SmtpClient();
                        //    smtp.Host = "sleepingpartnermanagementportalrct.com";
                        //    smtp.EnableSsl = true;
                        //    smtp.UseDefaultCredentials = false;
                        //    smtp.Credentials = new NetworkCredential("noreply@sleepingpartnermanagementportalrct.com", "Yly21#p8");
                        //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        //    smtp.Port = 25;
                        //    ServicePointManager.ServerCertificateValidationCallback =
                        //    delegate (object s, X509Certificate certificate,
                        //             X509Chain chain, SslPolicyErrors sslPolicyErrors)
                        //    { return true; };
                        //    smtp.Send(mail);

                            ewallet.IsWithdrawlRequestByUser = true;
                            ewallet.IsWithdrawlPaidByAdmin = false;
                            ewallet.IsWithdrawlRequestApproved = false;
                            dc.SaveChanges();

                            newpckgadd.UserName = user.Username;
                            newpckgadd.AccountNumber = user.AccountNumber;
                            newpckgadd.BankName = user.BankName;
                            newpckgadd.WithdrawalFundMethod = Common.Enum.PaymentSource.BankAccount.ToString();
                            newpckgadd.AmountPayble = ewallet.Amount.Value;
                            newpckgadd.WithdrawalFundDescription = ewallet.TransactionSource;
                            newpckgadd.WithdrawalFundCharge = 0;
                            newpckgadd.Country = user.Country;
                            newpckgadd.RequestedDate = DateTime.Now;
                            newpckgadd.ApprovedDate = null;
                            newpckgadd.PaidDate = null;
                            newpckgadd.RejectedDate = null;
                            newpckgadd.IsActive = true;
                            newpckgadd.IsPending = true;
                            newpckgadd.IsApproved = false;
                            newpckgadd.IsPaid = false;
                            newpckgadd.IsRejected = false;
                            newpckgadd.UserId = user.UserId;
                            newpckgadd.EwalletTransId = ewallet.TransactionId;
                            dc.EWalletWithdrawalFunds.Add(newpckgadd);
                            dc.SaveChanges();

                            ModelState.Clear();
                            return Ok(new { success = true, message = "Successfully Done" });
                        }
                    }
                    else
                    {
                        return Ok(new { info = true, message = "You already sended request" });
                    }
                }
                else
                {
                    return Ok(new { error = true, message = "unsuccessfully" });
                }

            }
            //return View();
        }
    }
    }

