using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace babySiter.Controllers
{
    [RoutePrefix("Search")]
    public class SearchBSController : ApiController
    {
        private BabySitterDBEntities db = new BabySitterDBEntities();

        //[HttpGet]
        //[Route("search/{mail}/{password}")]
        //public IHttpActionResult Search(string mail)
        //{
        //}
    }
}
