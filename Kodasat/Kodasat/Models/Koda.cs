//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kodasat.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Koda
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Koda()
        {
            this.Depositeds = new HashSet<Deposited>();
        }
    
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<System.TimeSpan> Time { get; set; }
        public int PeopleCount { get; set; }
        public int PeopleDeposited { get; set; }
        public int ChurchesID { get; set; }
        public int fatherID { get; set; }
    
        public virtual Church Church { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deposited> Depositeds { get; set; }
        public virtual Login Login { get; set; }
    }
}