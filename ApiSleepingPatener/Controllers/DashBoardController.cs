using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiSleepingPatener.Controllers
{
    public class DashBoardController : ApiController
    {
       [Authorize]
        [Route("dashboard/{userId}")]
       public IHttpActionResult DashBoard(int userId)
        {

        }
        public string GetUserTotalDirectCommission(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            using (SleepingtestEntities dc = new SleepingtestEntities())
            {
              
            
                    var CGP = (from a in dc.EWalletTransactions
                               where a.UserId.Value == userId
                               && a.IsParentBonus.Value == true
                               select a).ToList();
                    var query = CGP.Sum(x => x.Amount);
                return query.ToString();
            }
        
           

        }
    }
}
