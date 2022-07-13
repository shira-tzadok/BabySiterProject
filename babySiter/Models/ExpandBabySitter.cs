using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace babySiter.Models
{
    public class ExpandBabySitter:BabySitter
    {
        public int age { get; set; }
        public DateTime date { get; set; }
        public int MotherID { get; set; }
    }
}