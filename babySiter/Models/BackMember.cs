using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace babySiter.Models
{
    public class BackMember
    {
        public int id { get; set; }
        public string firstName { get; set; }

        public string lastName { get; set; }

        public Boolean IsMother { get; set; }

        public string mail { get; set; }
    }

    public class BackMemberGroup : BackMember
    {
        public int MemberGroupId { get; set; }
    }
}