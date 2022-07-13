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
    public class TrainingsController : ApiController
    {
        private BabySitterDBEntities db = new BabySitterDBEntities();

        // GET: api/Trainings
        public IQueryable<Training> GetTraining()
        {
            return db.Training;
        }

        // GET: api/Trainings/5
        [ResponseType(typeof(Training))]
        public IHttpActionResult GetTraining(int id)
        {
            Training training = db.Training.Find(id);
            if (training == null)
            {
                return NotFound();
            }

            return Ok(training);
        }

        // PUT: api/Trainings/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTraining(int id, Training training)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != training.IdTrainingTable)
            {
                return BadRequest();
            }

            db.Entry(training).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingExists(id))
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

        // POST: api/Trainings
        [ResponseType(typeof(Training))]
        public IHttpActionResult PostTraining(Training training)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Training.Add(training);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = training.IdTrainingTable }, training);
        }

        // DELETE: api/Trainings/5
        [ResponseType(typeof(Training))]
        public IHttpActionResult DeleteTraining(int id)
        {
            Training training = db.Training.Find(id);
            if (training == null)
            {
                return NotFound();
            }

            db.Training.Remove(training);
            db.SaveChanges();

            return Ok(training);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TrainingExists(int id)
        {
            return db.Training.Count(e => e.IdTrainingTable == id) > 0;
        }
    }
}