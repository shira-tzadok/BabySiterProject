using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace babySiter.Models
{
    public class ExpandRequestMother: RequestMother
    {
        public int BabySitterId { get; set; }
    }
}