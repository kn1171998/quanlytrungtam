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
    
    public partial class HOA_DON
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HOA_DON()
        {
            this.CT_HOADON = new HashSet<CT_HOADON>();
            this.CT_HOADON_NGOAIGIO = new HashSet<CT_HOADON_NGOAIGIO>();
        }
    
        public System.Guid MA_HD { get; set; }
        public string TEN_HD { get; set; }
        public string MA_GV { get; set; }
        public System.DateTime NGAY_THANH_TOAN { get; set; }
        public Nullable<double> TONG_TIEN { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CT_HOADON> CT_HOADON { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CT_HOADON_NGOAIGIO> CT_HOADON_NGOAIGIO { get; set; }
        public virtual GIAO_VIEN GIAO_VIEN { get; set; }
    }
}