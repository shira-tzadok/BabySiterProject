//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class MemberGroup
    {
        public int Id { get; set; }
        public int IdGroup { get; set; }
        public Nullable<int> IdMother { get; set; }
        public Nullable<int> IdBabysitter { get; set; }
        public Nullable<bool> IsApproved { get; set; }
    
        public virtual BabySitter BabySitter { get; set; }
        public virtual Group Group { get; set; }
        public virtual Mother Mother { get; set; }
    }
}
