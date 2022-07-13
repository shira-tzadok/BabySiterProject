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
    [RoutePrefix("api/Invaites")]

    public class InvaitesController : ApiController
    {
        private BabySitterDBEntities db = new BabySitterDBEntities();

        // GET: api/Invaites
        [HttpGet]
        [Route("GetInvaitsByIdMember/{IdMember}/{IsMother}")]
        public List<Invaite> GetInvaitsByIdMember(int IdMember, bool IsMother)
        {
            List<Invaite> listInvaite;
            if (IsMother == true)
                listInvaite = db.Invaite.Where(x => x.IdMotherInviting == IdMember).ToList();
            else
                listInvaite = db.Invaite.Where(x => x.IdBabySitterInviting == IdMember).ToList();

            foreach (var item in listInvaite)
            {
                if (item.IsMotherInviting == true)
                {
                    BabySitter bs = db.BabySitter.FirstOrDefault(x => x.Id == item.IdBabySitterAcceptsInvitation);
                    if (bs != null)
                    {
                        item.BabySitter = new BabySitter()
                        {
                            FirstName = bs.FirstName,
                            LastName = bs.LastName
                        };
                    }
                }
                else
                {
                    Mother mother = db.Mother.FirstOrDefault(x => x.Id == item.IdMotherAcceptsInvitation);
                    if (mother != null)
                    {
                        item.Mother = new Mother()
                        {
                            FirstName = mother.FirstName,
                            LastName = mother.LastName
                        };
                        item.Mother1 = null;
                    }
                }
            }
            return listInvaite;
        }


        [HttpGet]
        [Route("")]
        public IQueryable<Invaite> GetInvaite1()
        {
            return db.Invaite;
        }


        [HttpGet]
        [Route("GetInvaiteByIdRequest/{idRequest}")]
        [ResponseType(typeof(Invaite))]
        public Invaite GetInvaiteByIdRequest(int idRequest)
        {
            Invaite n;
            n = db.Invaite.FirstOrDefault(x => x.IdRequestMother == idRequest);
            return n;
        }
        // GET: api/Invaites/5
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(Invaite))]
        public IHttpActionResult GetInvaite(int id)
        {
            Invaite invaite = db.Invaite.Find(id);
            if (invaite == null)
            {
                return NotFound();
            }

            return Ok(invaite);
        }

        // PUT: api/Invaites/5
        [HttpPut]
        [Route("{id:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutInvaite(int id, Invaite invaite)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != invaite.Id)
            {
                return BadRequest();
            }

            db.Entry(invaite).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvaiteExists(id))
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

        // POST: api/Invaites
        //[HttpPost]
        //[Route("")]
        //[ResponseType(typeof(Invaite))]
        //public IHttpActionResult PostInvaite(Invaite invaite)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Invaite.Add(invaite);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = invaite.Id }, invaite);
        //}
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Invaite))]
        public Invaite PostInvaite(Invaite invaite)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            //var q = db.Invaite.FirstOrDefault(x=>x.IdRequestMother== invaite.IdRequestMother&&x.);
            //var q = db.Invaite.Contains(invaite);
            db.Invaite.Add(invaite);
            db.SaveChanges();

            return invaite;
        }

        // DELETE: api/Invaites/5
        [HttpDelete]
        [Route("{id:int}")]
        [ResponseType(typeof(Invaite))]
        public IHttpActionResult DeleteInvaite(int id)
        {
            Invaite invaite = db.Invaite.Find(id);
            if (invaite == null)
            {
                return NotFound();
            }
            BabySitter b1 = new BabySitter();
            Mother m1 = new Mother();
            if (invaite.IsMotherInviting == true)
            {
                BabySittersController bs = new BabySittersController();
                b1 = db.BabySitter.FirstOrDefault(x => x.Id == invaite.IdBabySitterAcceptsInvitation);
                m1 = db.Mother.FirstOrDefault(x => x.Id == invaite.IdMotherInviting);

                bs.sendEmailCancelDate(m1.Id, b1.Id, "", invaite.Id);

            }
            else
            {
                MothersController m = new MothersController();
                b1 = db.BabySitter.FirstOrDefault(x => x.Id == invaite.IdBabySitterInviting);
                m1 = db.Mother.FirstOrDefault(x => x.Id == invaite.IdMotherAcceptsInvitation);

                m.sendCancel(m1.Id, b1.Id, "", invaite.Id);
            }
            db.Invaite.Remove(invaite);

            db.Invaite.Remove(invaite);
            db.SaveChanges();

            return Ok(invaite);
        }
        [HttpDelete]
        [Route("DeleteInviteById/{id}/{idMember}/{isMother}")]
        [ResponseType(typeof(Invaite))]
        public IHttpActionResult DeleteInviteById(int id, int idMember, bool isMother)
        {
            Invaite invaite;
            if (isMother == true)
                invaite = db.Invaite.FirstOrDefault(x => x.IdRequestMother == id);
            else
                invaite = db.Invaite.FirstOrDefault(x => x.IdOpenBabySitter == id);

            if (invaite == null)
            {
                return NotFound();
            }

            db.Invaite.Remove(invaite);
            db.SaveChanges();

            return Ok(invaite);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InvaiteExists(int id)
        {
            return db.Invaite.Count(e => e.Id == id) > 0;
        }
    }
}