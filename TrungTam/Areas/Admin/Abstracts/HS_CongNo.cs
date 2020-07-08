using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrungTam.Areas.Admin.Abstracts
{
    public class HS_CongNo
    {
        public Guid macongno { get; set; }
        public string tenlop { get; set; }
        public string mahs { get; set; }
        public string hoten { get; set; }
        public string ngaysinh { get; set; }
        public string gioitinh { get; set; }
        public string sdt { get; set; }
        /*public IEnumerable<Nullable<decimal>> dongia { get; set; }*/
        public Nullable<decimal> dongia { get; set; }
        public bool trangthaihd { get; set; }        
    }
}