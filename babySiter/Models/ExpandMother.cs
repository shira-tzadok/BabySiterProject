using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace babySiter.Models
{
    public class ExpandMother:Mother
    {
        public DateTime date { get; set; }
        public int idPart { get; set; }
        public int BabySitterId { get; set; }
    }
}