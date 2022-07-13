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
using babySiter.Utils;
using babySiter.Models;
using System.Net.Mail;

namespace babySiter.Controllers
{
    [RoutePrefix("api/BabySitters")]
    public class BabySittersController : ApiController
    {
        private BabySitterDBEntities db = new BabySitterDBEntities();

        // GET: api/BabySitters
        [HttpGet]
        [Route("")]
        public IQueryable<BabySitter> GetBabySitter()
        {
            return db.BabySitter;
        }


        [HttpGet]
        [Route("GetAllBabySitters")]
        [ResponseType(typeof(IEnumerable<BabySitter>))]
        public IEnumerable<BabySitter> GetAllBabySitters()
        {
            return db.BabySitter.ToList();
        }

        [HttpGet]
        [Route("GetBabySitterOpenToDay/{MotherID}")]
        [ResponseType(typeof(IEnumerable<BabySitter>))]
        public IEnumerable<BabySitter> GetBabySitterOpenToDay(int MotherID)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("\n", ModelState.Select(x => string.Format("error key: {0}, error value: {1}", x.Key, x.Value)));
                    throw new Exception("model is invalid");
                }
                var babySitterList = db.BabySitter.Include(p => p.OpenBabySitter)
                .Where(p => p.OpenBabySitter.Any(o => o.Date.Year == DateTime.Now.Year
                && o.Date.Month == DateTime.Now.Month && o.Date.Day == DateTime.Now.Day)).ToList();
                int day = (int)DateTime.Now.DayOfWeek;
                day++;
                var BSFixed = db.BabySitter.Include(p => p.FixedBabySitter)
                    .Where(p => p.FixedBabySitter.Any(o => o.Day == day)/*&&!babySitterList.Any(a=>a.Id==p.Id)*/).ToList();

                foreach (var item in BSFixed)
                {
                    var babysitter = babySitterList.FirstOrDefault(d => d.Id == item.Id);
                    if (babysitter == null)
                    {
                        babySitterList.Add(item);
                    }
                    else
                    {
                        babysitter.FixedBabySitter = item.FixedBabySitter;
                    }
                }
                babySitterList.ForEach(p =>
                {
                    p.Favorite = db.Favorite.Where(f => f.IdMother == MotherID && f.IdBabySitter == p.Id).ToList();//ללא השורה הזאת

                    p.OpenBabySitter = p.OpenBabySitter.Where(o => o.Date.Year == DateTime.Now.Year
                    && o.Date.Month == DateTime.Now.Month && o.Date.Day == DateTime.Now.Day).ToList();
                    p.Password = null;
                    if (p.ShowMail)
                    {
                        p.Mail = null;
                    }
                    if (p.ShowTell)
                    {
                        p.Phone = null;
                    }
                });
                return babySitterList;
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


        [HttpGet]
        [Route("GetBabySitterFavotite/{IdMother}")]
        [ResponseType(typeof(IEnumerable<BabySitter>))]
        public IEnumerable<BabySitter> GetBabySitterFavotite(int IdMother)
        {
            var q = db.BabySitter.Include(p => p.Favorite).Where(c => c.Favorite.Any(o => o.IdMother == IdMother)).ToList();
            q.ForEach(p =>
            {
                p.Favorite = db.Favorite.Where(f => f.IdMother == IdMother && f.IdBabySitter == p.Id).ToList();//ללא שורה
            });

            return q;
        }


        ////GET: api/BabySitters/5
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(BabySitter))]
        public IHttpActionResult GetBabySitter(int id)
        {
            BabySitter babySitter = db.BabySitter.Find(id);
            if (babySitter == null)
            {
                return NotFound();
            }

            return Ok(babySitter);
        }

        [HttpPost]
        [Route("PostBSBySearch")]
        [ResponseType(typeof(IEnumerable<BabySitter>))]
        public IEnumerable<BabySitter> PostBSMatchedBySearch(ExpandBabySitter babySiter)
        {
            var groups = db.MemberGroup.Where(x => x.IdMother == babySiter.MotherID).Select(x => x.IdGroup).ToList();

            List<BabySitter> q = new List<BabySitter>();

            q = db.BabySitter.Include(x => x.MemberGroup).Where(x => !x.IsShowFromGroups || x.MemberGroup.Any(g => groups.Contains(g.IdGroup))).ToList();

            if (babySiter.FirstName != null && babySiter.FirstName != "")
            {
                q = q.Where(x => x.FirstName == babySiter.FirstName).ToList();
                if (q.Count() == 0)
                    return null;
            }

            if (babySiter.LastName != null && babySiter.LastName != "")
            {

                if (q.Count() > 0)
                {
                    q = q.Where(x => x.LastName == babySiter.LastName).ToList();
                }
                else
                {
                    q = db.BabySitter.Where(x => x.LastName == babySiter.LastName).ToList();
                }
                if (q.Count() == 0)
                    return null;
            }

            if (babySiter.City != null && babySiter.City != "")
            {

                if (q.Count() > 0)
                {
                    q = q.Where(x => x.City == babySiter.City).ToList();
                }
                else
                {
                    q = db.BabySitter.Where(x => x.City == babySiter.City).ToList();
                }
                if (q.Count() == 0)
                    return null;
            }

            if (babySiter.Street != null && babySiter.Street != "")
            {

                if (q.Count() > 0)
                {
                    q = q.Where(x => x.Street == babySiter.Street).ToList();
                }
                else
                {
                    q = db.BabySitter.Where(x => x.Street == babySiter.Street).ToList();
                }
                if (q.Count() == 0)
                    return null;
            }

            if (babySiter.HighAgeChildren != 0)
            {

                if (q.Count() > 0)
                {
                    q = q.Where(x => x.HighAgeChildren <= babySiter.HighAgeChildren).ToList();
                }
                else
                {
                    q = db.BabySitter.Where(x => x.HighAgeChildren <= babySiter.HighAgeChildren).ToList();
                }
                if (q.Count() == 0)
                    return null;
            }

            if (babySiter.date.Day != 1 && babySiter.date.Month != 1 && babySiter.date.Year != 1)
            {
                var babySitterList = db.BabySitter.Include(p => p.OpenBabySitter)
               .Where(p => p.OpenBabySitter.Any(o => o.Date == babySiter.date)).ToList();

                int day = (int)babySiter.date.DayOfWeek;
                day++;
                var BSFixed = db.BabySitter.Include(p => p.FixedBabySitter)
                    .Where(p => p.FixedBabySitter.Any(o => o.Day == day)).ToList();

                if (q.Count() > 0)
                {
                    List<BabySitter> s = new List<BabySitter>();
                    s.AddRange(q);
                    foreach (var item in s)
                    {

                        var babysitter = babySitterList.FirstOrDefault(d => d.Id == item.Id);
                        if (babysitter == null)
                        {
                            q.Remove(item);
                        }
                    }

                    foreach (var item in s)
                    {
                        var babysitter = BSFixed.FirstOrDefault(d => d.Id == item.Id);
                        if (babysitter == null)
                        {
                            q.Remove(item);
                        }
                    }
                }
                else
                {
                    q.AddRange(babySitterList);
                    foreach (var item in BSFixed)
                    {
                        var babysitter = q.FirstOrDefault(d => d.Id == item.Id);
                        if (babysitter == null)
                        {
                            q.Add(item);
                        }
                    }
                }
                if (q.Count() == 0)
                    return null;
            }

            if (babySiter.LowAgeChildren != 0)
            {

                if (q.Count() > 0)
                {
                    q = q.Where(x => x.LowAgeChildren >= babySiter.LowAgeChildren).ToList();
                }
                else
                {
                    q = db.BabySitter.Where(x => x.LowAgeChildren >= babySiter.LowAgeChildren).ToList();
                }
                if (q.Count() == 0)
                    return null;
            }

            if (babySiter.MaxChildren != 0)
            {

                if (q.Count() > 0)
                {
                    q = q.Where(x => x.MaxChildren <= babySiter.MaxChildren).ToList();
                }
                else
                {
                    q = db.BabySitter.Where(x => x.MaxChildren <= babySiter.MaxChildren).ToList();
                }
                if (q.Count() == 0)
                    return null;
            }

            if (babySiter.SalaryHour != null)
            {

                if (q.Count() > 0)
                {
                    q = q.Where(x => x.SalaryHour <= babySiter.SalaryHour).ToList();
                }
                else
                {
                    q = db.BabySitter.Where(x => x.SalaryHour <= babySiter.SalaryHour).ToList();
                }
                if (q.Count() == 0)
                    return null;
            }

            if (babySiter.age != 0)
            {
                if (q.Count() > 0)
                {

                    //q = (from e in q
                    //     let years = DateTime.Now.Year - e.BornDate.Year
                    //     let birthdayThisYear = e.BornDate.AddYears(years)
                    //     where birthdayThisYear.Year >= babySiter.age
                    //     select e).ToList();
                    q = (from e in q
                         let years = DateTime.Now.Year - e.BornDate.Year
                         where years >= babySiter.age
                         select e).ToList();
                }
                else
                {
                    q = (from e in db.BabySitter
                         let years = DateTime.Now.Year - e.BornDate.Year
                         where years >= babySiter.age
                         select e).ToList();
                }
                if (q.Count() == 0)
                    return null;
            }

            if (babySiter.CollectingChildren != false)
            {

                if (q.Count() > 0)
                {
                    q = q.Where(x => x.CollectingChildren == true).ToList();
                }
                else
                {
                    q = db.BabySitter.Where(x => x.CollectingChildren == true).ToList();
                }
                if (q.Count() == 0)
                    return null;
            }

            if (babySiter.license != false)
            {

                if (q.Count() > 0)
                {
                    q = q.Where(x => x.license == true).ToList();
                }
                else
                {
                    q = db.BabySitter.Where(x => x.license == true).ToList();
                }
                if (q.Count() == 0)
                    return null;
            }

            if (babySiter.SpecialChildren != false)
            {

                if (q.Count() > 0)
                {
                    q = q.Where(x => x.SpecialChildren == true).ToList();
                }
                else
                {
                    q = db.BabySitter.Where(x => x.SpecialChildren == true).ToList();
                }
                if (q.Count() == 0)
                    return null;
            }

            if (babySiter.Household != false)
            {

                if (q.Count() > 0)
                {
                    q = q.Where(x => x.Household == true).ToList();
                }
                else
                {
                    q = db.BabySitter.Where(x => x.Household == true).ToList();
                }
                if (q.Count() == 0)
                    return null;
            }

            if (q.Count() == 0)
                q = db.BabySitter.ToList();

            q.ForEach(p =>
            {
                p.Favorite = db.Favorite.Where(f => f.IdMother == babySiter.MotherID && f.IdBabySitter == p.Id).ToList();
                p.FixedBabySitter = db.FixedBabySitter.Where(f => f.IdBabySitter == p.Id).ToList();
                p.OpenBabySitter = db.OpenBabySitter.Where(f => f.IdBabySitter == p.Id).ToList();
                p.Password = null;
                //if(p.ShowMail)
            });

            return q;
        }


        // PUT: api/BabySitters/5
        [HttpPut]
        [Route("{id:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBabySitter(int id, BabySitter babySitter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != babySitter.Id)
            {
                return BadRequest();
            }

            db.Entry(babySitter).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BabySitterExists(id))
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


        // POST: api/BabySitters
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(BabySitter))]
        public BabySitter PostBabySitter(BabySitter babySitter)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("\n", ModelState.Select(x => string.Format("error key: {0}, error value: {1}", x.Key, x.Value)));
                    throw new Exception("model is invalid");
                }
                var fixedBabySitter = babySitter.FixedBabySitter.ToList();/*.Clone().ToList()*/
                babySitter.FixedBabySitter = null;
                babySitter = db.BabySitter.Add(babySitter);
                db.SaveChanges();
                fixedBabySitter.ForEach(x => x.IdBabySitter = babySitter.Id);
                db.FixedBabySitter.AddRange(fixedBabySitter);
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
            return babySitter;
        }


        // DELETE: api/BabySitters/5
        [HttpDelete]
        [Route("{id:int}")]
        [ResponseType(typeof(BabySitter))]
        public IHttpActionResult DeleteBabySitter(int id)
        {
            BabySitter babySitter = db.BabySitter.Find(id);
            if (babySitter == null)
            {
                return NotFound();
            }

            db.BabySitter.Remove(babySitter);
            db.SaveChanges();

            return Ok(babySitter);
        }

        [HttpGet]
        [Route("sendEmail/{IdMother}/{IdBS}/{BodyEmail}/{IdInvite}")]
        [ResponseType(typeof(string))]
        public string sendEmail(int IdMother, int IdBS, string BodyEmail, int IdInvite)
        {
            BabySitter babySitter = db.BabySitter.FirstOrDefault(x => x.Id == IdBS);
            MailAddress emailAddress = new MailAddress(babySitter.Mail);
            SendEmail.send(IdMother, IdBS, BodyEmail, IdInvite, emailAddress, "Babysitter Request");
            return "sucsses";
        }

        [Route("sendEmailCancelDate/{IdMother}/{IdBS}/{BodyEmail}/{IdInvite}")]
        [ResponseType(typeof(string))]
         public  string sendEmailCancelDate(int IdMother, int IdBS, string BodyEmail, int IdInvite)
        {
            BabySitter babySitter = db.BabySitter.FirstOrDefault(x => x.Id == IdBS);
            MailAddress emailAddress = new MailAddress(babySitter.Mail);
            SendEmail.sendCancel(IdMother, IdBS, BodyEmail, IdInvite, emailAddress, "Cancel Reservation");
            return "sucsses";
        }

        //[HttpGet]
        //[Route("sendEmail/{IdGroup}/{IdBS}/{BodyEmail}/{IdInvite}")]
        //[ResponseType(typeof(string))]
        //public string sendEmailConfirmAddToGroup(int IdGroup, int IdBS, string BodyEmail, int IdInvite)
        //{
        //    BabySitter babySitter = db.BabySitter.FirstOrDefault(x => x.Id == IdBS);
        //    MailAddress emailAddress = new MailAddress(babySitter.Mail);
        //    SendEmail.sendEmailConfirmAddToGroup(emailAddress, "בקשת בקשת הצטרפות לקבוצה", BodyEmail, IdInvite);
        //    return "נשלח בהצלחה";
        //}

        [HttpGet]
        [Route("GetverifyPassword/{IdBS}/{password}")]
        [ResponseType(typeof(Boolean))]
        public IHttpActionResult GetVerifyPassword(int IdBS, string password)
        {
            BabySitter babySitter = db.BabySitter.FirstOrDefault(x => x.Id == IdBS);
            if (babySitter.Password == password)
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

        private bool BabySitterExists(int id)
        {
            return db.BabySitter.Count(e => e.Id == id) > 0;
        }



    }
}