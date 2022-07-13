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
    [RoutePrefix("api/Favorites")]
    public class FavoritesController : ApiController
    {
        private BabySitterDBEntities db = new BabySitterDBEntities();

        // GET: api/Favorites
        [HttpGet]
        [Route("")]
        public IQueryable<Favorite> GetFavorite()
        {
            return db.Favorite;
        }

        // GET: api/Favorites/5
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(Favorite))]
        public IHttpActionResult GetFavorite(int id)
        {
            Favorite favorite = db.Favorite.Find(id);
            if (favorite == null)
            {
                return NotFound();
            }

            return Ok(favorite);


        }


        // PUT: api/Favorites/5
        [HttpPut]
        [Route("{id:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFavorite(int id, Favorite favorite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != favorite.id)
            {
                return BadRequest();
            }

            db.Entry(favorite).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavoriteExists(id))
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


        // POST: api/Favorites
        [HttpPost]
        //[Route("")]
        [Route("PostBSFavorite", Name = "PostBSFavorite")]
        [ResponseType(typeof(Favorite))]
        public IHttpActionResult PostBSFavorite(Favorite favorite)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Favorite f = db.Favorite.FirstOrDefault(x => x.IdBabySitter == favorite.IdBabySitter && x.IdMother == favorite.IdMother);
            if (f == null)
            {
                db.Favorite.Add(favorite);
                db.SaveChanges();
                return CreatedAtRoute("PostBSFavorite", new { id = favorite.id }, favorite);
            }
            return CreatedAtRoute("PostBSFavorite", new { id = f.id }, f);
        }



        [HttpPost]
        [Route("PostRemoveBSFavorite")]
        [ResponseType(typeof(Favorite))]
        public IHttpActionResult PostRemoveBSFavorite(Favorite favorite)
        {
            Favorite f = db.Favorite.FirstOrDefault(x => x.IdBabySitter == favorite.IdBabySitter && x.IdMother == favorite.IdMother);
            if (f == null)
            {
                return NotFound();
            }

            db.Favorite.Remove(f);
            db.SaveChanges();

            return Ok(f);
        }

        // DELETE: api/Favorites/5
        [HttpDelete]
        //[Route("DeleteBSFavorite")]
        [Route("{id:int}")]
        [ResponseType(typeof(Favorite))]
        public IHttpActionResult DeleteFavorite(int id)
        {
            Favorite favorite = db.Favorite.Find(id);
            if (favorite == null)
            {
                return NotFound();
            }

            db.Favorite.Remove(favorite);
            db.SaveChanges();

            return Ok(favorite);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool FavoriteExists(int id)
        {
            return db.Favorite.Count(e => e.id == id) > 0;
        }
    }
}