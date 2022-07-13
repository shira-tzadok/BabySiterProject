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
    public class FixedDatesController : ApiController
    {
        private BabySitterDBEntities db = new BabySitterDBEntities();

        // GET: api/FixedDates
        public IQueryable<FixedDate> GetFixedDate()
        {
            return db.FixedDate;
        }

        // GET: api/FixedDates/5
        [ResponseType(typeof(FixedDate))]
        public IHttpActionResult GetFixedDate(int id)
        {
            FixedDate fixedDate = db.FixedDate.Find(id);
            if (fixedDate == null)
            {
                return NotFound();
            }

            return Ok(fixedDate);
        }

        // PUT: api/FixedDates/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFixedDate(int id, FixedDate fixedDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fixedDate.Id)
            {
                return BadRequest();
            }

            db.Entry(fixedDate).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FixedDateExists(id))
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

        // POST: api/FixedDates
        [ResponseType(typeof(FixedDate))]
        public IHttpActionResult PostFixedDate(FixedDate fixedDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FixedDate.Add(fixedDate);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = fixedDate.Id }, fixedDate);
        }

        // DELETE: api/FixedDates/5
        [ResponseType(typeof(FixedDate))]
        public IHttpActionResult DeleteFixedDate(int id)
        {
            FixedDate fixedDate = db.FixedDate.Find(id);
            if (fixedDate == null)
            {
                return NotFound();
            }

            db.FixedDate.Remove(fixedDate);
            db.SaveChanges();

            return Ok(fixedDate);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FixedDateExists(int id)
        {
            return db.FixedDate.Count(e => e.Id == id) > 0;
        }
    }
}