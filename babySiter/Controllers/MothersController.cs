using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Description;
using babySiter;
using babySiter.Models;
using DAL;

namespace babySiter.Controllers
{
    [RoutePrefix("api/Mothers")]
    public class MothersController : ApiController
    {
        private BabySitterDBEntities db = new BabySitterDBEntities();

        // GET: api/Mothers
        [HttpGet]
        [Route("")]
        public IQueryable<Mother> Getmother()
        {
            return db.Mother;
        }

        // GET: api/Mothers/5
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(Mother))]
        public IHttpActionResult Getmother(int id)
        {
            Mother mother = db.Mother.Find(id);
            if (mother == null)
            {
                return NotFound();
            }

            return Ok(mother);
        }


        [HttpGet]
        [Route("GetMotherRequestToDay")]
        [ResponseType(typeof(IEnumerable<Mother>))]
        public IEnumerable<Mother> GetMotherRequestToDay()
        {
            var MotherList = db.Mother.Include(p => p.RequestMother)
                .Where(p => p.RequestMother.Any(o => o.Date.Year == DateTime.Now.Year
                && o.Date.Month == DateTime.Now.Month && o.Date.Day == DateTime.Now.Day)).ToList();


            MotherList.ForEach(p =>
            {
                p.RequestMother = p.RequestMother.Where(o => o.Date.Year == DateTime.Now.Year
                && o.Date.Month == DateTime.Now.Month && o.Date.Day == DateTime.Now.Day).ToList();
                p.Password = null;
               
            });
            return MotherList;
        }


        // PUT: api/Mothers/5
        [HttpPut]
        [Route("{id:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Putmother(int id, Mother mother)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (id != mother.Id)
            //{
            //    return BadRequest();
            //}

            db.Entry(mother).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!motherExists(id))
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

        // POST: api/Mothers
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Mother))]
        public IHttpActionResult Postmother(Mother mother)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Mother.Add(mother);
                db.SaveChanges();
            }
            catch(DbEntityValidationException ex)
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
            return CreatedAtRoute("DefaultApi", new { id = mother.Id }, mother);
        }


        [HttpPost]
        [Route("PostMotherBySearch")]
        [ResponseType(typeof(IEnumerable<Mother>))]
        public IEnumerable<Mother> PostMotherBySearch(ExpandMother mother)
        {
            var groups = db.MemberGroup.Where(x => x.IdBabysitter == mother.BabySitterId).Select(x => x.IdGroup).ToList();

            List<Mother> q = new List<Mother>();

            q = db.Mother.Include(x => x.MemberGroup).Where(x => !x.IsShowFromGroups || x.MemberGroup.Any(g => groups.Contains(g.IdGroup))).ToList();


            if (mother.FirstName != null && mother.FirstName != "")
            {
                q = db.Mother.Where(x => x.FirstName == mother.FirstName).ToList();
                if (q.Count() == 0)
                    return null;
            }

            if (mother.LastName != null && mother.LastName != "")
            {
                if (q.Count() > 0)
                {
                    q = q.Where(x => x.LastName == mother.LastName).ToList();
                }
                else
                {
                    q = db.Mother.Where(x => x.LastName == mother.LastName).ToList();
                }
                if (q.Count() == 0)
                    return null;
            }

            if (mother.City != null && mother.City != "")
            {
                if (q.Count() > 0)
                {
                    q = q.Where(x => x.City == mother.City).ToList();
                }
                else
                {
                    q = db.Mother.Where(x => x.City == mother.City).ToList();
                }
                if (q.Count() == 0)
                    return null;
            }

            if (mother.Street != null && mother.Street != "")
            {
                if (q.Count() > 0)
                {
                    q = q.Where(x => x.Street == mother.Street).ToList();
                }
                else
                {
                    q = db.Mother.Where(x => x.Street == mother.Street).ToList();
                }
                if (q.Count() == 0)
                    return null;
            }

            if (mother.date.Day != 1 && mother.date.Month != 1 && mother.date.Year != 1)
            {
                var motherList = db.Mother.Include(p => p.RequestMother)
               .Where(p => p.RequestMother.Any(o => o.Date == mother.date)).ToList();

                if (q.Count() > 0)
                {
                    List<Mother> s = new List<Mother>();
                    s.AddRange(q);
                    foreach (var item in s)
                    {
                        var Mother = motherList.FirstOrDefault(d => d.Id == item.Id);
                        if (Mother == null)
                        {
                            q.Remove(item);
                        }
                    }
                }
                else
                {
                    q.AddRange(motherList);
                }
                if (q.Count() == 0)
                    return null;
            }

            if (mother.HighAgeChildren != 0)
            {
                if (q.Count() > 0)
                {
                    q = q.Where(x => x.HighAgeChildren <= mother.HighAgeChildren).ToList();
                }
                else
                {
                    q = db.Mother.Where(x => x.HighAgeChildren <= mother.HighAgeChildren).ToList();
                }
                if (q.Count() == 0)
                    return null;
            }

            if (mother.LowAgeChildren != 0)
            {
                if (q.Count() > 0)
                {
                    q = q.Where(x => x.LowAgeChildren >= mother.LowAgeChildren).ToList();
                }
                else
                {
                    q = db.Mother.Where(x => x.LowAgeChildren >= mother.LowAgeChildren).ToList();
                }
                if (q.Count() == 0)
                    return null;
            }

            if (mother.NumChildren != 0)
            {
                if (q.Count() > 0)
                {
                    q = q.Where(x => x.NumChildren <= mother.NumChildren).ToList();
                }
                else
                {
                    q = db.Mother.Where(x => x.NumChildren <= mother.NumChildren).ToList();
                }
                if (q.Count() == 0)
                    return null;
            }

            if (mother.idPart != 0)
            {
                var motherList = db.Mother.Include(p => p.RequestMother)
               .Where(p => p.RequestMother.Any(o => o.IdPart == mother.idPart)).ToList();

                if (q.Count() > 0)
                {
                    List<Mother> s = new List<Mother>();
                    s.AddRange(q);
                    foreach (var item in s)
                    {
                        var Mother = motherList.FirstOrDefault(d => d.Id == item.Id);
                        if (Mother == null)
                        {
                            q.Remove(item);
                        }
                    }
                }
                else
                {
                    q.AddRange(motherList);
                }
                if (q.Count() == 0)
                    return null;
            }

            if (mother.SalaryHour != null)
            {
                if (q.Count() > 0)
                {
                    q = q.Where(x => x.SalaryHour <= mother.SalaryHour).ToList();
                }
                else
                {
                    q = db.Mother.Where(x => x.SalaryHour <= mother.SalaryHour).ToList();
                }
                if (q.Count() == 0)
                    return null;
            }

            if (q.Count() == 0)
                return db.Mother.ToList();

            q.ForEach(p =>
            {
                p.Password = null;
                if (p.ShowMail!=true)
                {
                    p.Mail = null;
                }
                if (p.ShowTell!=true)
                {
                    p.Phone = null;
                }
            });

            return q;
        }

        // DELETE: api/Mothers/5
        [HttpDelete]
        [Route("{id:int}")]
        [ResponseType(typeof(Mother))]
        public IHttpActionResult Deletemother(int id)
        {
            Mother mother = db.Mother.Find(id);
            if (mother == null)
            {
                return NotFound();
            }

            db.Mother.Remove(mother);
            db.SaveChanges();

            return Ok(mother);
        }

        [HttpGet]
        [Route("sendEmail/{IdBS}/{IdMother}/{BodyEmail}/{IdInvite}")]
        [ResponseType(typeof(string))]
        public string sendEmail(int IdBS, int IdMother, string BodyEmail,int IdInvite)
        {
            //BabySitter babySitter = db.BabySitter.FirstOrDefault(x => x.Id == IdBS);
            //Invaite invaite = db.Invaite.FirstOrDefault(x => x.Id == IdInvite);

            Mother mother = db.Mother.FirstOrDefault(x => x.Id == IdMother);
            MailAddress emailAddress = new MailAddress(mother.Mail);
            SendEmail.sendMother(IdMother, IdBS, BodyEmail, IdInvite, emailAddress, "Apply For");
            return "sucsses";
        }

        [HttpGet]
        [Route("sendCancel/{IdBS}/{IdMother}/{BodyEmail}/{IdInvite}")]
        [ResponseType(typeof(string))]
        public string sendCancel(int IdBS, int IdMother, string BodyEmail, int IdInvite)
        {
            //BabySitter babySitter = db.BabySitter.FirstOrDefault(x => x.Id == IdBS);
            //Invaite invaite = db.Invaite.FirstOrDefault(x => x.Id == IdInvite);

            Mother mother = db.Mother.FirstOrDefault(x => x.Id == IdMother);
            MailAddress emailAddress = new MailAddress(mother.Mail);
            SendEmail.sendCancelMother(IdMother, IdBS, BodyEmail, IdInvite, emailAddress, "Cancel Reservation");
            return "sucsses";
        }


        [HttpGet]
        [Route("GetverifyPassword/{IdMother}/{password}")]
        [ResponseType(typeof(Boolean))]
        public IHttpActionResult GetVerifyPassword(int IdMother,string password)
        {
            Mother mother = db.Mother.FirstOrDefault(x => x.Id == IdMother);
            if (mother.Password == password)
                return Ok(true);
            return Ok(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool motherExists(int id)
        {
            return db.Mother.Count(e => e.Id == id) > 0;
        }
    }
}