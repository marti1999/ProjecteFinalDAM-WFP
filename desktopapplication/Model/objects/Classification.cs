//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace desktopapplication.Model

{
    using System;
    using System.Collections.Generic;
    
    public partial class Classification
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Classification()
        {
            this.Clothes = new HashSet<Cloth>();
        }

        public int Id { get; set; }
        public string classificationType { get; set; }
        public bool active { get; set; }
        public int value { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cloth> Clothes { get; set; }
    }
}
