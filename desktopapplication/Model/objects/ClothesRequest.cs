//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using WSRobaSegonaMa.Models;

namespace desktopapplication.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClothesRequest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClothesRequest()
        {
            this.Clothes = new HashSet<Cloth>();
        }
    
        public int Id { get; set; }
        public System.DateTime dateCreated { get; set; }
        public System.DateTime dateExpire { get; set; }
        public int Requestor_Id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cloth> Clothes { get; set; }
        public virtual Requestor Requestor { get; set; }
    }
}
