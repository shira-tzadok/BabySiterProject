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
using DAL;

namespace babySiter.Controllers
{
    public class FixedBabySittersController : ApiController
    {
        private BabySitterDBEntities db = new BabySitterDBEntities();

        // GET: api/FixedBabySitters
        public IQueryable<FixedBabySitter> GetFixedBabySitter()
        {
            return db.FixedBabySitter;
        }

        // GET: api/FixedBabySitters/5
        [ResponseType(typeof(FixedBabySitter))]
        public IHttpActionResult GetFixedBabySitter(int id)
        {
            FixedBabySitter fixedBabySitter = db.FixedBabySitter.Find(id);
            if (fixedBabySitter == null)
            {
                return NotFound();
            }

            return Ok(fixedBabySitter);
        }

        // PUT: api/FixedBabySitters/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFixedBabySitter(int id, FixedBabySitter fixedBabySitter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fixedBabySitter.Id)
            {
                return BadRequest();
            }

            db.Entry(fixedBabySitter).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FixedBabySitterExists(id))
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

        //// POST: api/FixedBabySitters
        //[ResponseType(typeof(FixedBabySitter))]
        //public IHttpActionResult PostFixedBabySitter(FixedBabySitter fixedBabySitter)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.FixedBabySitter.Add(fixedBabySitter);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = fixedBabySitter.Id }, fixedBabySitter);
        //}

        // POST: api/FixedBabySitters
        [ResponseType(typeof(FixedBabySitter))]
        public IHttpActionResult PostFixedBabySitter(List<FixedBabySitter> fixedBabySitter)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //foreach (var item in fixedBabySitter)
                //{
                //FixedBabySitter f = new FixedBabySitter();
                //f.Id = item.Id;
                //f.IdBabySitter = item.IdBabySitter;
                //f.Day = item.Day;
                //f.IdPart = item.IdPart;
                db.FixedBabySitter.AddRange(fixedBabySitter);
                //}

                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
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
            return CreatedAtRoute("DefaultApi", new { id = fixedBabySitter }, fixedBabySitter);
        }
        // DELETE: api/FixedBabySitters/5
        [ResponseType(typeof(FixedBabySitter))]
        public IHttpActionResult DeleteFixedBabySitter(int id)
        {
            FixedBabySitter fixedBabySitter = db.FixedBabySitter.Find(id);
            if (fixedBabySitter == null)
            {
                return NotFound();
            }

            db.FixedBabySitter.Remove(fixedBabySitter);
            db.SaveChanges();

            return Ok(fixedBabySitter);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FixedBabySitterExists(int id)
        {
            return db.FixedBabySitter.Count(e => e.Id == id) > 0;
        }




        //[HttpGet]
        //[Route("sendEmail")]

        //public void SendEmail(string name, string Email, int phone, string message)
        //{
        //    MailMessage mail = new MailMessage();
        //    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        //    mail.From = new MailAddress(Email);
        //    mail.To.Add("rinat61950@gmail.com");
        //    mail.Subject = "הודעה מאת   Email: " + Email;
        //    mail.Subject = mail.Subject.Replace("\n", Environment.NewLine);
        //    mail.Body = name + "\n" + Convert.ToString(phone) + "\n" + message;
        //    SmtpServer.Port = 587;
        //    SmtpServer.Credentials = new System.Net.NetworkCredential("rinat61950@gmail.com", "Develop@");
        //    SmtpServer.EnableSsl = true;
        //    SmtpServer.Send(mail);

        //}
    }
    }