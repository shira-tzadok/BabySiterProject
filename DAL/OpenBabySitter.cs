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
    
    public partial class OpenBabySitter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OpenBabySitter()
        {
            this.Invaite = new HashSet<Invaite>();
        }
    
        public int Id { get; set; }
        public int IdBabySitter { get; set; }
        public System.DateTime DateOpen { get; set; }
        public System.DateTime Date { get; set; }
        public int IdPart { get; set; }
    
        public virtual BabySitter BabySitter { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invaite> Invaite { get; set; }
        public virtual PartOfDay PartOfDay { get; set; }
    }
}
