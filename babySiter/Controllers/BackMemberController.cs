using babySiter.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace babySiter.Controllers
{
    [RoutePrefix("BackMember")]
    public class BackMemberController : ApiController
    {

        private BabySitterDBEntities db = new BabySitterDBEntities();
        private MemberGroupsController MemberGroupsController = new MemberGroupsController();
        [HttpGet]
        [Route("isExist/{mail}/{password}")]
        public IHttpActionResult IsExist(string mail, string password)
        {

            BackMember obj = new BackMember();
            //mother m = new mother();
            //DAL.babySiter b = new DAL.babySiter();
            //bool IsBabySiter = false, IsMother = false;
            foreach (var item in db.Mother)
            {
                if (item.Mail == mail && item.Password == password)
                {
                    obj.id = item.Id;
                    obj.firstName = item.FirstName;
                    obj.lastName = item.LastName;
                    obj.mail = item.Mail;
                    obj.IsMother = true;
                    return Ok(obj);
                }
            }

            foreach (var item in db.BabySitter)
            {
                if (item.Mail == mail && item.Password == password)
                {
                    obj.id = item.Id;
                    obj.firstName = item.FirstName;
                    obj.lastName = item.LastName;
                    obj.mail = item.Mail;
                    obj.IsMother = false;
                    return Ok(obj);
                }
            }
            obj = null;
            return Ok(obj);
            //if (!IsMother && !IsBabySiter)
            //    return BadRequest();
            //if(IsBabySiter)
            //    return  Ok("BabySiter");
            //return Ok("Mother");
            //    var IsBabySiter = db.babySiter.FirstOrDefault(x => x.firstName.Trim() + ' ' + x.lastName.Trim() == name.Trim() && x.password == password);

            //    var IsMother = db.mother.FirstOrDefault(x => x.firstName.Trim() + ' ' + x.lastName.Trim() == name.Trim() && x.passwodr == password);

            //    if (IsMother == null && IsBabySiter == null)
            //        return BadRequest();
            //    if (IsMother != null)
            //        return Ok("Mother");
            //    return Ok("BabySiter");
        }


        [HttpGet]
        [Route("getListMemberGroup/{idGroup}")]
        public List<BackMemberGroup> getListMemberGroup(int idGroup)
        {
            List<MemberGroup> ListMemberGroup = MemberGroupsController.GetMemberByIdGroup(idGroup);
            List<BackMemberGroup> ListBackMember = new List<BackMemberGroup>();
            foreach (var item in ListMemberGroup)
            {
                if (item.IdMother != null)
                {
                    Mother mother = db.Mother.Where(x => x.Id == item.IdMother).Select(a=>a).First();
                    
                    if (mother != null)
                    {
                        BackMemberGroup backMember = new BackMemberGroup();
                        backMember.id = mother.Id;
                        backMember.firstName = mother.FirstName;
                        backMember.lastName = mother.LastName;
                        backMember.mail = mother.Mail;
                        backMember.MemberGroupId = item.Id;
                        backMember.IsMother = true;
                        ListBackMember.Add(backMember);
                    }
                }
                else
                if (item.IdBabysitter != null)
                {

                    BabySitter babySitter = db.BabySitter.Where(x => x.Id == item.IdBabysitter).Select(a => a).First();
                    if (babySitter != null)
                    {
                        BackMemberGroup backMember = new BackMemberGroup();
                        backMember.id = babySitter.Id;
                        backMember.firstName = babySitter.FirstName;
                        backMember.lastName = babySitter.LastName;
                        backMember.mail = babySitter.Mail;
                        backMember.MemberGroupId = item.Id;
                        backMember.IsMother = false;
                        ListBackMember.Add(backMember);
                    }
                }

            }

            return ListBackMember;
        }
            

          
    }
}
