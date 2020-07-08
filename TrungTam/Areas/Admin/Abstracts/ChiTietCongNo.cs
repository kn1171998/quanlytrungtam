using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrungTam.Areas.Admin.Abstracts
{
    public class ChiTietCongNo
    {
        public string macongno { get; set; }
        public Guid malop { get; set; }
        public string mahs { get; set; }
        public string tenhs { get; set; }
        public string tenlop { get; set; }
        public string tenmon { get; set; }
        public string tenloai { get; set; }
        public string tenkhoi { get; set; }
        public Nullable<decimal> tongtien { get; set; }
        public Nullable<DateTime> ngaythanhtoan { get; set; }
        public Nullable<decimal> giatien { get; set; }
        public Nullable<decimal> tiengiam { get; set; }
    }
}