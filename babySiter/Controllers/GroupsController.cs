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
using babySiter;
using DAL;

namespace babySiter.Controllers
{
    [RoutePrefix("api/Groups")]
    public class GroupsController : ApiController
    {
        private BabySitterDBEntities db = new BabySitterDBEntities();

        // GET: api/Groups
        [HttpGet]
        [Route("")]
        public IQueryable<Group> Getgroup()
        {
            return db.Group;
        }

        // GET: api/Groups/5
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(Group))]
        public IHttpActionResult Getgroup(int id)
        {
            Group group = db.Group.Find(id);
            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }
        //[HttpGet]
        //[Route("GetGroupsIAmIn/{idMember}")]
        //[ResponseType(typeof(IEnumerable<Group>))]
        //public IEnumerable<Group> GetGroupsIAmIn(int idMember)
        //{
        //    var q = db.Group.Include(p => p.MemberGroup).Where(x => x.MemberGroup.Any(o => o.IdMember == idMember)).ToList();
        //    return q;
        //}
        [HttpGet]
        [Route("GetGroupsIAmIn/{IdMember}/{IsMother}")]
        [ResponseType(typeof(List<Group>))]
        public List<Group> GetGroupsIAmIn(int IdMember, Boolean IsMother)
        {
            List<Group> q;
            if (IsMother == true)
            {
                q = db.Group.Include(p => p.MemberGroup).Where(x => x.MemberGroup.Any(o => o.IdMother == IdMember)).ToList();
                q = q.Where(x => x.IdMotherManager != IdMember).ToList();

            }
            else
                q = db.Group.Include(p => p.MemberGroup).Where(x => x.MemberGroup.Any(o => o.IdBabysitter == IdMember)).ToList();
            q = q.Where(x => x.IdBabySitterManager != IdMember).ToList();
            return q;
        }

        [HttpGet]
        [Route("GetNameGroup/{ManagerId}/{IsMother}")]
        [ResponseType(typeof(List<Group>))]
        public List<Group> GetNameGroup(int ManagerId, Boolean IsMother)
        {
            if (IsMother == true)
                return db.Group.Where(x => x.IdMotherManager == ManagerId).ToList();
            else
                return db.Group.Where(x => x.IdBabySitterManager == ManagerId).ToList();
        }


        [HttpGet]
        [Route("GetNameGroupById/{ManagerId}",Name = "GetNameGroupById")]
        [ResponseType(typeof(Group))]
        public Group GetNameGroupById(int ManagerId)
        {
            return db.Group.FirstOrDefault(x => x.GroupId == ManagerId);
            //if (IsMother == true)
            //    return db.Group.Where(x => x.IdMotherManager == ManagerId).ToList();
            //else
            //    return db.Group.Where(x => x.IdBabySitterManager == ManagerId).ToList();
        }

        // PUT: api/Groups/5
        [HttpPut]
        [Route("{id:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Putgroup(int id, Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != group.GroupId)
            {
                return BadRequest();
            }

            db.Entry(group).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!groupExists(id))
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

        // POST: api/Groups
        //[HttpPost]
        //[Route("")]
        //[ResponseType(typeof(Group))]
        //public IHttpActionResult Postgroup(Group group)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Group.Add(group);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = group.GroupId }, group);
        //}
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Group))]
        public Group Postgroup(Group group)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            //var d = db.Group.Contains(group);
            var w = db.Group.FirstOrDefault(x => x.IdMotherManager == group.IdMotherManager && x.NameGroup == group.NameGroup && x.Password == group.Password);
            if (w == null)
            {
                if (group != null)
                {
                    db.Group.Add(group);
                    db.SaveChanges();
                }
                return NewMethod(group);
            }
            return new Group { GroupId = -1, IdMotherManager = group.IdMotherManager, NameGroup = group.NameGroup, Password = group.Password };

            //return CreatedAtRoute("DefaultApi", new { id = group.GroupId }, group);
        }

        private static Group NewMethod(Group group)
        {
            return new Group { GroupId = group.GroupId, IdMotherManager = group.IdMotherManager, NameGroup = group.NameGroup, Password = group.Password };
        }

        // DELETE: api/Groups/5
        [HttpDelete]
        [Route("Deletegroup/{GroupId}")]
        //[Route("{id:int}")]
        [ResponseType(typeof(Group))]
        public IHttpActionResult Deletegroup(int GroupId)
        {
            Group group = db.Group.Find(GroupId);
            if (group == null)
            {
                return NotFound();
            }
            var q = db.MemberGroup.Where(x => x.IdGroup == GroupId).ToList();
            if (q.Count() > 0)
                db.MemberGroup.RemoveRange(q);
            db.Group.Remove(group);

            db.SaveChanges();

            return Ok(group);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool groupExists(int id)
        {
            return db.Group.Count(e => e.GroupId == id) > 0;
        }
    }
}