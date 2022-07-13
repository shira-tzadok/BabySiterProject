using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DAL;

namespace babySiter.Controllers
{
    public class PartOfDaysController : ApiController
    {
        private BabySitterDBEntities db = new BabySitterDBEntities();

        // GET: api/PartOfDays
        public List<PartOfDay> GetPartOfDay()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("\n", ModelState.Select(x => string.Format("error key: {0}, error value: {1}", x.Key, x.Value)));
                    throw new Exception("model is invalid");
                }
                //listPrat
                //PartOfDay part = new PartOfDay();
                //part.IdPart=
                //List<PartOfDay> listPrat = db.PartOfDay.ToList();
                //listPrat.Add();
                return db.PartOfDay.ToList();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        // GET: api/PartOfDays/5
        [ResponseType(typeof(PartOfDay))]
        public IHttpActionResult GetPartOfDay(int id)
        {
            PartOfDay partOfDay = db.PartOfDay.Find(id);
            if (partOfDay == null)
            {
                return NotFound();
            }

            return Ok(partOfDay);
        }

        // PUT: api/PartOfDays/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPartOfDay(int id, PartOfDay partOfDay)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != partOfDay.IdPart)
            {
                return BadRequest();
            }

            db.Entry(partOfDay).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartOfDayExists(id))
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

        // POST: api/PartOfDays
        [ResponseType(typeof(PartOfDay))]
        public IHttpActionResult PostPartOfDay(PartOfDay partOfDay)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PartOfDay.Add(partOfDay);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = partOfDay.IdPart }, partOfDay);
        }

        // DELETE: api/PartOfDays/5
        [ResponseType(typeof(PartOfDay))]
        public IHttpActionResult DeletePartOfDay(int id)
        {
            PartOfDay partOfDay = db.PartOfDay.Find(id);
            if (partOfDay == null)
            {
                return NotFound();
            }

            db.PartOfDay.Remove(partOfDay);
            db.SaveChanges();

            return Ok(partOfDay);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PartOfDayExists(int id)
        {
            return db.PartOfDay.Count(e => e.IdPart == id) > 0;
        }
    }
}