using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DAL;

namespace babySiter.Controllers
{
    [RoutePrefix("api/MemberGroups")]
    public class MemberGroupsController : ApiController
    {
        private BabySitterDBEntities db = new BabySitterDBEntities();

        // GET: api/MemberGroups
        [HttpGet]
        [Route("")]
        public IQueryable<MemberGroup> GetMemberGroup()
        {
            return db.MemberGroup;
        }

        // GET: api/MemberGroups/5
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(MemberGroup))]
        public IHttpActionResult GetMemberGroup(int id)
        {
            MemberGroup memberGroup = db.MemberGroup.Find(id);
            if (memberGroup == null)
            {
                return NotFound();
            }

            return Ok(memberGroup);
        }

        [HttpGet]
        [Route("GetAllMemberIamIn/{MemberId}/{IsMother}")]
        [ResponseType(typeof(List<MemberGroup>))]
        public List<MemberGroup> GetAllMemberIamIn(int MemberId, bool IsMother)
        {
            List<MemberGroup> ListIdGroupIamIn;
            if (IsMother == true)
                ListIdGroupIamIn = db.MemberGroup.Where(x => x.IdMother == MemberId).ToList();
            else
                ListIdGroupIamIn = db.MemberGroup.Where(x => x.IdBabysitter == MemberId).ToList();

            if (ListIdGroupIamIn == null)
            {
                return null;
            }
            return ListIdGroupIamIn;
        }

        [HttpGet]
        [Route("GetMemberByIdGroup/{id}")]
        [ResponseType(typeof(List<MemberGroup>))]
        public List<MemberGroup> GetMemberByIdGroup(int id)
        {
            List<MemberGroup> ListIdGroupIamIn = db.MemberGroup.Where(x => x.IdGroup == id).ToList();
            if (ListIdGroupIamIn == null)
            {
                return null;
            }
            return ListIdGroupIamIn;
        }

        // PUT: api/MemberGroups/5
        [HttpPut]
        [Route("{id:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMemberGroup(int id, MemberGroup memberGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != memberGroup.Id)
            {
                return BadRequest();
            }

            db.Entry(memberGroup).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberGroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        //POST: api/MemberGroups
       [HttpPost]
       [Route("")]
       [ResponseType(typeof(MemberGroup))]
        public IHttpActionResult PostMemberGroup(MemberGroup memberGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MemberGroup.Add(memberGroup);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (MemberGroupExists(memberGroup.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = memberGroup.Id }, memberGroup);
        }


        // POST: api/MemberGroups


        [HttpPost]
        //[Route("PostMemberGroupAfterConfirm",Name = "PostMemberGroupAfterConfirm")]
        [Route("PostMemberGroupAfterConfirm", Name = "PostMemberGroupAfterConfirm")]
        [ResponseType(typeof(MemberGroup))]
        public IHttpActionResult PostMemberGroupAfterConfirm(MemberGroup memberGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            memberGroup.IsApproved = true;//////Todo
            db.MemberGroup.Add(memberGroup);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (MemberGroupExists(memberGroup.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            //return memberGroup;
            //return CreatedAtRoute("DefaultApi", new { id = memberGroup.Id }, memberGroup);
            return CreatedAtRoute("PostMemberGroupAfterConfirm", new { id = memberGroup.Id }, memberGroup);
        }

        // DELETE: api/MemberGroups/5
        [HttpDelete]
        [Route("{id:int}")]
        [ResponseType(typeof(MemberGroup))]
        public IHttpActionResult DeleteMemberGroup(int id)
        {
            MemberGroup memberGroup = db.MemberGroup.Find(id);
            if (memberGroup == null)
            {
                return NotFound();
            }

            db.MemberGroup.Remove(memberGroup);
            db.SaveChanges();

            return Ok(memberGroup);
        }
        [HttpDelete]
        [Route("DeleteMemberGroupByIdGroup/{idGroup}/{idMemberGroup}/{isMother}")]
        [ResponseType(typeof(MemberGroup))]
        public IHttpActionResult DeleteMemberGroupByIdGroup(int idGroup,int idMemberGroup,bool isMother)
        {
            MemberGroup memberGroup;
            if (isMother == true)
            {
                memberGroup = db.MemberGroup.FirstOrDefault(x => x.IdGroup == idGroup && x.IdMother == idMemberGroup);
            }
            else
            {
                memberGroup = db.MemberGroup.FirstOrDefault(x => x.IdGroup == idGroup && x.IdBabysitter == idMemberGroup);
            }
            if (memberGroup == null)
            {
                return NotFound();
            }

            db.MemberGroup.Remove(memberGroup);
            db.SaveChanges();

            return Ok(memberGroup);
        }
        [HttpDelete]
        [Route("DeleteMembersByIdGroup/{idGroup}")]
        [ResponseType(typeof(MemberGroup))]
        public IHttpActionResult DeleteMembersByIdGroup(int idGroup)
        {

            List<MemberGroup> memberGroup;
            memberGroup = db.MemberGroup.Where(x=>x.IdGroup==idGroup).ToList();
            
            //if (isMother == true)
            //{
            //    memberGroup = db.MemberGroup.FirstOrDefault(x => x.IdGroup == idGroup && x.IdMother == idMemberGroup);
            //}
            //else
            //{
            //    memberGroup = db.MemberGroup.FirstOrDefault(x => x.IdGroup == idGroup && x.IdBabysitter == idMemberGroup);
            //}
            if (memberGroup == null)
            {
                return NotFound();
            }

            db.MemberGroup.RemoveRange(memberGroup);
            db.SaveChanges();

            return Ok(memberGroup);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MemberGroupExists(int id)
        {
            return db.MemberGroup.Count(e => e.Id == id) > 0;
        }
    }
}