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
    
    public partial class Invaite
    {
        public int Id { get; set; }
        public System.DateTime DateInviting { get; set; }
        public Nullable<int> IdMotherInviting { get; set; }
        public Nullable<int> IdBabySitterInviting { get; set; }
        public bool IsMotherInviting { get; set; }
        public Nullable<int> IdMotherAcceptsInvitation { get; set; }
        public Nullable<int> IdBabySitterAcceptsInvitation { get; set; }
        public System.DateTime Date { get; set; }
        public int IdPart { get; set; }
        public string Details { get; set; }
        public Nullable<bool> IsConfirm { get; set; }
        public Nullable<int> IdRequestMother { get; set; }
        public Nullable<int> IdOpenBabySitter { get; set; }
    
        public virtual BabySitter BabySitter { get; set; }
        public virtual PartOfDay PartOfDay { get; set; }
        public virtual Mother Mother { get; set; }
        public virtual Mother Mother1 { get; set; }
        public virtual OpenBabySitter OpenBabySitter { get; set; }
        public virtual RequestMother RequestMother { get; set; }
    }
}
