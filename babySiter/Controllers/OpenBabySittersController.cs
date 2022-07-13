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
    [RoutePrefix("api/OpenBabySitters")]
    public class OpenBabySittersController : ApiController
    {
        private BabySitterDBEntities db = new BabySitterDBEntities();


        [HttpGet]
        [Route("")]
        // GET: api/OpenBabySitters
        public List<OpenBabySitter> GetopenBabySitter()
        {
            return db.OpenBabySitter.ToList();
        }

        //// GET: api/OpenBabySittersToDay
        //public List<OpenBabySitter> GetopenBabySitterToDay()
        //{
        //    //return db.OpenBabySitter.Where(x=>x.Date=(Date)DateTime.Now);
        //    List<OpenBabySitter> l=(db.OpenBabySitter.Where(x => x.Date.Date.Equals(DateTime.Now.Date))).ToList();
        //    return l;
        //    //return db.OpenBabySitter.Where(x => x.Date.Day.Equals(DateTime.Now.DayOfWeek));
        //}







        // GET: api/OpenBabySitters/5
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(OpenBabySitter))]
        public IHttpActionResult GetopenBabySitter(int id)
        {
            OpenBabySitter openBabySiter = db.OpenBabySitter.Find(id);
            if (openBabySiter == null)
            {
                return NotFound();
            }

            return Ok(openBabySiter);
        }

        [HttpGet]
        [Route("GetOpenBabySitterById/{IdBS}")]
        [ResponseType(typeof(IEnumerable<OpenBabySitter>))]
        public IHttpActionResult GetopenBabySitterById(int IdBS)
        {
            List<OpenBabySitter> openBabySiter = db.OpenBabySitter.Where(x => x.IdBabySitter == IdBS).ToList();

            openBabySiter.ForEach(r =>
            {
                var inviteList = db.Invaite.Where(x => x.IdOpenBabySitter == r.Id).ToList();
                inviteList.ForEach(i =>
                {
                    var mother = db.Mother.FirstOrDefault(b => b.Id == i.IdMotherInviting);
                    if (mother != null)
                        i.Mother = new Mother() { FirstName = mother.FirstName, LastName = mother.LastName };
                });
                r.Invaite = inviteList;
            });
            if (openBabySiter == null)
            {
                return NotFound();
            }

            return Ok(openBabySiter);
        }

        [HttpGet]
        [Route("GetOpenBabySitterByDetails/{IdBS}/{date}/{idPart}")]
        [ResponseType(typeof(OpenBabySitter))]
        public OpenBabySitter GetOpenBabySitterByDetails(int IdBS, DateTime date, int idPart)
        {
            OpenBabySitter openBabySiter = db.OpenBabySitter.FirstOrDefault(x => x.IdBabySitter == IdBS && x.Date == date && x.IdPart == idPart);
            return openBabySiter;
        }

        // PUT: api/OpenBabySitters/5
        [HttpPut]
        [Route("{id:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutopenBabySitter(int id, OpenBabySitter openBabySiter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != openBabySiter.Id)
            {
                return BadRequest();
            }

            db.Entry(openBabySiter).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!openBabySiterExists(id))
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

        //  POST: api/OpenBabySitters
        //[Route("PostopenBabySitter")]
        [HttpPost]
        [Route("PostNewDate")]
        [ResponseType(typeof(OpenBabySitter))]
        public OpenBabySitter PostNewDate(OpenBabySitter openBabySitter)
        {
            db.OpenBabySitter.Add(openBabySitter);
            db.SaveChanges();

            return openBabySitter;
        }



        [HttpPost]
        [Route("")]
        [ResponseType(typeof(List<OpenBabySitter>))]
        public IHttpActionResult PostopenBabySitter(List<OpenBabySitter> openBabySiter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OpenBabySitter.AddRange(openBabySiter);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = openBabySiter }, openBabySiter);
        }

        // DELETE: api/OpenBabySitters/5
        //[HttpDelete]
        //[Route("{id:int}")]
        //[ResponseType(typeof(OpenBabySitter))]
        //public IHttpActionResult DeleteopenBabySitter(int id)
        //{
        //   List<OpenBabySitter> openBabySiter = db.OpenBabySitter.Where(x=>x.IdBabySitter==id).ToList();
        //    if (openBabySiter == null)
        //    {
        //        return NotFound();
        //    }

        //    db.OpenBabySitter.RemoveRange(openBabySiter);
        //    db.SaveChanges();

        //    return Ok(openBabySiter);
        //}

        [HttpDelete]
        [Route("{id:int}")]
        [ResponseType(typeof(OpenBabySitter))]
        public IHttpActionResult DeleteopenBabySitter(int id)
        {
            OpenBabySitter openBabySiter = db.OpenBabySitter.FirstOrDefault(x => x.Id == id);
            if (openBabySiter == null)
            {
                return NotFound();
            }
            Invaite invaite = db.Invaite.FirstOrDefault(x => x.IdOpenBabySitter == id && x.IsConfirm == true);
            invaite.Mother = db.Mother.FirstOrDefault(x => x.Id == invaite.IdMotherInviting);
            MothersController mother = new MothersController();
            mother.sendCancel(openBabySiter.IdBabySitter, invaite.Mother.Id, "", invaite.Id);
            db.OpenBabySitter.Remove(openBabySiter);
            db.SaveChanges();

            return Ok(openBabySiter);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool openBabySiterExists(int id)
        {
            return db.OpenBabySitter.Count(e => e.Id == id) > 0;
        }
    }
}