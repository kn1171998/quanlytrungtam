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
    
    public partial class CT_CONG_NO
    {
        public System.Guid MA_CONG_NO { get; set; }
        public System.Guid MA_LOP { get; set; }
        public Nullable<decimal> THANH_TIEN { get; set; }
    
        public virtual CONG_NO CONG_NO { get; set; }
        public virtual LOP_HOC LOP_HOC { get; set; }
    }
}
