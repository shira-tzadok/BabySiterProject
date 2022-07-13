using System;
using System.Collections;
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
    [RoutePrefix("api/LanguageToMembers")]
    public class LanguageToMembersController : ApiController
    {
        private BabySitterDBEntities db = new BabySitterDBEntities();

        // GET: api/LanguageToMembers
        [HttpGet]
        [Route("")]
        public IQueryable<LanguageToMember> GetLanguageToMember()
        {
            return db.LanguageToMember;
        }

        // GET: api/LanguageToMembers/5
        [HttpGet]
        [Route("GetLanguageToMemberById/{idMember}")]
        [ResponseType(typeof(List<LanguageToMember>))]
        public IHttpActionResult GetLanguageToMemberById(int idMember)
        {
            List<LanguageToMember> languageToMemberList = db.LanguageToMember.Where(x=>x.IdMember==idMember).ToList();
            if (languageToMemberList == null)
            {
                return null;
            }

            return Ok(languageToMemberList);
        }

        [HttpGet]
        [ResponseType(typeof(LanguageToMember))]

        public IHttpActionResult GetLanguageToMember(int id)
        {
            LanguageToMember languageToMember = db.LanguageToMember.Find(id);
            if (languageToMember == null)
            {
                return NotFound();
            }

            return Ok(languageToMember);
        }

        // PUT: api/LanguageToMembers/5


        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult PutLanguageToMember(int id, LanguageToMember languageToMember)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != languageToMember.Id)
            {
                return BadRequest();
            }

            db.Entry(languageToMember).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LanguageToMemberExists(id))
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


        // POST: api/LanguageToMembers
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(List<LanguageToMember>))]
        public IHttpActionResult PostLanguageToMember(List<LanguageToMember> languageToMember)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LanguageToMember.AddRange(languageToMember);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = languageToMember}, languageToMember);
        }

        //[ResponseType(typeof(LanguageToMember))]
        //public IHttpActionResult PostLanguageToMember(LanguageToMember languageToMember)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.LanguageToMember.Add(languageToMember);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = languageToMember.Id }, languageToMember);
        //}



        // DELETE: api/LanguageToMembers/5

        [HttpDelete]
        [Route("{id:int}")]
        [ResponseType(typeof(LanguageToMember))]
        public IHttpActionResult DeleteLanguageToMember(int id)
        {
            LanguageToMember languageToMember = db.LanguageToMember.Find(id);
            if (languageToMember == null)
            {
                return NotFound();
            }

            db.LanguageToMember.Remove(languageToMember);
            db.SaveChanges();

            return Ok(languageToMember);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LanguageToMemberExists(int id)
        {
            return db.LanguageToMember.Count(e => e.Id == id) > 0;
        }
    }
}