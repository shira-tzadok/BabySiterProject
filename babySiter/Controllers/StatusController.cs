﻿using System;
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
    public class StatusController : ApiController
    {
        private BabySitterDBEntities db = new BabySitterDBEntities();

        // GET: api/Status
        public List<Status> GetStatus()
        {
            return db.Status.ToList();
        }

        // GET: api/Status/5
        [ResponseType(typeof(Status))]
        public IHttpActionResult GetStatus(int id)
        {
            Status status = db.Status.Find(id);
            if (status == null)
            {
                return NotFound();
            }

            return Ok(status);
        }

        // PUT: api/Status/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStatus(int id, Status status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != status.IdType)
            {
                return BadRequest();
            }

            db.Entry(status).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatusExists(id))
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

        // POST: api/Status
        [ResponseType(typeof(Status))]
        public IHttpActionResult PostStatus(Status status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Status.Add(status);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = status.IdType }, status);
        }

        // DELETE: api/Status/5
        [ResponseType(typeof(Status))]
        public IHttpActionResult DeleteStatus(int id)
        {
            Status status = db.Status.Find(id);
            if (status == null)
            {
                return NotFound();
            }

            db.Status.Remove(status);
            db.SaveChanges();

            return Ok(status);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StatusExists(int id)
        {
            return db.Status.Count(e => e.IdType == id) > 0;
        }
    }
}