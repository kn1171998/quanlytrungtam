//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrungTam.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class KHUYEN_MAI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KHUYEN_MAI()
        {
            this.CONG_NO = new HashSet<CONG_NO>();
        }
    
        public System.Guid MA_KM { get; set; }
        public string TEN_KM { get; set; }
        public Nullable<int> SO_MON_DK { get; set; }
        public int TIEN_GIAM { get; set; }
        public Nullable<bool> TRANG_THAI { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONG_NO> CONG_NO { get; set; }
    }
}