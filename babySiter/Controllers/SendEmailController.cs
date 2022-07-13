using babySiter.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace babySiter.Controllers
{
    [RoutePrefix("SendEmail")]
    public class SendEmailController : ApiController
    {
        private static BabySitterDBEntities db = new BabySitterDBEntities();

        [HttpGet]
        [Route("sendEmailJoinGroup/{IdManagement}/{IdGroup}/{Email}/{IsMother}",Name = "sendEmailJoinGroup")]
        public IHttpActionResult sendEmailJoinGroup(int IdManagement, int IdGroup, string Email, bool IsMother)
        {
            MailAddress emailAddress = new MailAddress(Email);
            SendEmail.sendEmailConfirmAddToGroup(IdManagement, IsMother, IdGroup, emailAddress, "Confirm Join To Group");
            string mess="נשלח בהצלחה";
            return Ok(mess);
        }
    }
}
