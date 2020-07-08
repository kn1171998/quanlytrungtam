using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrungTam.Areas.Admin.Abstracts
{
    public class ReportDanhGia
    {
        public string tenlop { get; set; }
        public Nullable<int> buoihoc { get; set; }
        public string diemdanh { get; set; }
        public string tengv { get; set; }
        public string btvn { get; set; }
        public string nhanxet { get; set; }        
        public string tenmon { get; set; }
        public string tenhs { get; set; }
    }
}