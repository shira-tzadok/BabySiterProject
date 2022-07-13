using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using babySiter;
using babySiter.Models;
using DAL;

namespace babySiter.Controllers
{
    [RoutePrefix("api/RequestMothers")]
    public class RequestMothersController : ApiController
    {
        private BabySitterDBEntities db = new BabySitterDBEntities();

        // GET: api/RequestMothers
        [HttpGet]
        [Route("")]
        public IQueryable<RequestMother> GetrequestMother()
        {
            return db.RequestMother;
        }

        // GET: api/RequestMothers/5
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(RequestMother))]
        public IHttpActionResult GetrequestMother(int id)
        {
            var requestMotherList = db.RequestMother.Where(x => x.IdMother == id).ToList();
            requestMotherList.ForEach(r =>
            {
                var inviteList = db.Invaite.Where(x => x.IdRequestMother == r.Id).ToList();
                inviteList.ForEach(i =>
                {
                    var babysitter = db.BabySitter.FirstOrDefault(b => b.Id == i.IdBabySitterInviting);
                    if (babysitter != null)
                        i.BabySitter = new BabySitter() { FirstName = babysitter.FirstName, LastName = babysitter.LastName };
                });
                r.Invaite = inviteList;
            });
            if (requestMotherList == null)
            {
                return NotFound();
            }

            return Ok(requestMotherList);
        }

        [HttpGet]
        [Route("GetrequestMotherForList/{babySitterId}")]
        [ResponseType(typeof(RequestMother))]
        public IEnumerable<RequestMother> GetrequestMotherForList(int babySitterId)
        {
            var groups = db.MemberGroup.Where(x => x.IdBabysitter == babySitterId).Select(x => x.IdGroup).ToList();

            var q = db.RequestMother
                .Where(r => DbFunctions.TruncateTime(r.Date) >= DbFunctions.TruncateTime(DateTime.Now)).ToList();

            foreach (var request in q)
            {
                var rMother = db.Mother.FirstOrDefault(m => m.Id == request.IdMother);
                if (rMother != null)
                {
                    request.Mother = new Mother()
                    {
                        IsShowFromGroups = rMother.IsShowFromGroups,
                        FirstName = rMother.FirstName,
                        LastName = rMother.LastName,
                        Id = rMother.Id,
                        City = rMother.City,
                        Street = rMother.Street,
                        More = rMother.More,
                        LowAgeChildren = rMother.LowAgeChildren,
                        HighAgeChildren = rMother.HighAgeChildren,
                        ShowMail = rMother.ShowMail,
                        ShowTell = rMother.ShowTell,
                        NumChildren = rMother.NumChildren,
                        SalaryHour = rMother.SalaryHour,
                        MemberGroup = rMother.MemberGroup
                    };
                    if (rMother.ShowTell == true)
                        request.Mother.Phone = rMother.Phone;
                    if (rMother.ShowMail == true)
                        request.Mother.Mail = rMother.Mail;
                    var memberGroup = db.MemberGroup.Where(m => m.IdMother == rMother.Id);
                    request.Mother.MemberGroup = new List<MemberGroup>();
                    foreach (var mGroup in memberGroup)
                    {
                        request.Mother.MemberGroup.Add(new MemberGroup() { IdGroup = mGroup.IdGroup });
                    }
                }
            }

            q = q.Where(x => !x.Mother.IsShowFromGroups || x.Mother.MemberGroup.Any(g => groups.Contains(g.IdGroup))).ToList();
            
            return q;
        }

        // PUT: api/RequestMothers/5
        [HttpPut]
        [Route("{id:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutrequestMother(int id, RequestMother requestMother)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != requestMother.Id)
            {
                return BadRequest();
            }

            db.Entry(requestMother).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!requestMotherExists(id))
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


        [HttpPost]
        [Route("PostMotherBySearch")]
        [ResponseType(typeof(IEnumerable<RequestMother>))]
        public IEnumerable<RequestMother> PostMotherBySearch(ExpandRequestMother mother)
        {
            var groups = db.MemberGroup.Where(x => x.IdBabysitter == mother.BabySitterId).Select(x => x.IdGroup).ToList();

            var q = db.RequestMother
                .Where(r => DbFunctions.TruncateTime(r.Date) >= DbFunctions.TruncateTime(DateTime.Now)).ToList();

            foreach (var request in q)
            {
                var rMother = db.Mother.FirstOrDefault(m => m.Id == request.IdMother);
                if (rMother != null)
                {
                    request.Mother = new Mother()
                    {
                        IsShowFromGroups = rMother.IsShowFromGroups,
                        FirstName = rMother.FirstName,
                        LastName = rMother.LastName,
                        Id = rMother.Id,
                        City = rMother.City,
                        Street = rMother.Street,
                        More = rMother.More,
                        LowAgeChildren = rMother.LowAgeChildren,
                        HighAgeChildren = rMother.HighAgeChildren,
                        ShowMail = rMother.ShowMail,
                        ShowTell = rMother.ShowTell,
                        NumChildren = rMother.NumChildren,
                        SalaryHour = rMother.SalaryHour,
                        MemberGroup = rMother.MemberGroup
                    };
                    if (rMother.ShowTell == true)
                        request.Mother.Phone = rMother.Phone;
                    if (rMother.ShowMail == true)
                        request.Mother.Mail = rMother.Mail;
                    var memberGroup = db.MemberGroup.Where(m => m.IdMother == rMother.Id);
                    request.Mother.MemberGroup = new List<MemberGroup>();
                    foreach (var mGroup in memberGroup)
                    {
                        request.Mother.MemberGroup.Add(new MemberGroup() { IdGroup = mGroup.IdGroup });
                    }
                }
            }

            q = q.Where(x => !x.Mother.IsShowFromGroups || x.Mother.MemberGroup.Any(g => groups.Contains(g.IdGroup))).ToList();

            if (mother.Mother.FirstName != null && mother.Mother.FirstName != "")
            {
                q = q.Where(x => x.Mother.FirstName == mother.Mother.FirstName).ToList();
            }

            if (mother.Mother.LastName != null && mother.Mother.LastName != "")
            {
                q = q.Where(x => x.Mother.LastName == mother.Mother.LastName).ToList();
            }

            if (mother.Mother.City != null && mother.Mother.City != "")
            {
                q = q.Where(x => x.Mother.City == mother.Mother.City).ToList();
            }

            if (mother.Mother.Street != null && mother.Mother.Street != "")
            {
                q = q.Where(x => x.Mother.Street == mother.Mother.Street).ToList();
            }

            //if (!mother.Date.Equals(DateTime.MinValue))
            //{
            //    q = q.Where(x => DbFunctions.TruncateTime(x.Date) >= DbFunctions.TruncateTime(mother.Date)).ToList();
            //}

            if (mother.Mother.HighAgeChildren != 0)
            {
                q = q.Where(x => x.Mother.HighAgeChildren <= mother.Mother.HighAgeChildren).ToList();
            }

            if (mother.Mother.LowAgeChildren != 0)
            {
                q = q.Where(x => x.Mother.LowAgeChildren >= mother.Mother.LowAgeChildren).ToList();
            }

            if (mother.Mother.NumChildren != 0)
            {
                q = q.Where(x => x.Mother.NumChildren <= mother.Mother.NumChildren).ToList();
            }

            if (mother.IdPart != 0)
            {
                q = q.Where(x => x.IdPart == mother.IdPart).ToList();
            }

            if (mother.Mother.SalaryHour != null)
            {
                q = q.Where(x => x.Mother.SalaryHour <= mother.Mother.SalaryHour).ToList();
            }
            return q;
        }



        // POST: api/RequestMothers
        [HttpPost]
        [Route("PostrequestMother")]
        [ResponseType(typeof(RequestMother))]
        public RequestMother PostrequestMother(RequestMother requestMother)
        {
            //if (!ModelState.IsValid)
            //  return BadRequest(ModelState);

            db.RequestMother.Add(requestMother);
            db.SaveChanges();
            return requestMother;
            //return CreatedAtRoute("DefaultApi", new { id = requestMother }, requestMother);
        }


        // POST: api/RequestMothers
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(List<RequestMother>))]
        public List<RequestMother> PostrequestMother(List<RequestMother> requestMother)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            db.RequestMother.AddRange(requestMother);
            db.SaveChanges();
            return requestMother;
            //return CreatedAtRoute("DefaultApi", new { id = requestMother }, requestMother);
        }
        //[ResponseType(typeof(RequestMother))]
        //public IHttpActionResult PostrequestMother(RequestMother requestMother)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.RequestMother.Add(requestMother);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = requestMother.RequestMotherId }, requestMother);
        //}

        //[ResponseType(typeof(RequestMother))]
        //public IHttpActionResult DeleterequestMother(int id)
        //{

        //    RequestMother requestMother = db.RequestMother.Find(id);
        //    if (requestMother == null)
        //    {
        //        return NotFound();
        //    }

        //    db.RequestMother.Remove(requestMother);
        //    db.SaveChanges();

        //    return Ok(requestMother);
        //}

        // DELETE: api/RequestMothers/5
        [HttpDelete]
        [Route("{id:int}")]
        [ResponseType(typeof(RequestMother))]
        public IHttpActionResult DeleterequestMother(int id)
        {

            RequestMother q = db.RequestMother.FirstOrDefault(x => x.Id == id);

            if (q == null)
            {
                return NotFound();
            }
            Invaite invaite = db.Invaite.FirstOrDefault(x => x.IdRequestMother == id);
            if (invaite != null && invaite.IsConfirm == true)
            {
                invaite.BabySitter = db.BabySitter.FirstOrDefault(x => x.Id == invaite.IdBabySitterInviting);
                BabySittersController bs = new BabySittersController();
                bs.sendEmailCancelDate(q.IdMother, invaite.BabySitter.Id, "", invaite.Id);

            }
          
            //invaite.BabySitter = db.BabySitter.FirstOrDefault(x => x.Id == invaite.IdBabySitterAcceptsInvitation || x.Id == invaite.IdBabySitterInviting);
          

            db.RequestMother.Remove(q);
            db.SaveChanges();

            return Ok(q);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool requestMotherExists(int id)
        {
            return db.RequestMother.Count(e => e.Id == id) > 0;
        }
    }
}