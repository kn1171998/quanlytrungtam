using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrungTam.Areas.Admin.Abstracts
{
    public class chitietluong
    {
        public string TEN_LOP { get; set; }
        public string TEN_LOAI { get; set; }
        public Nullable<System.DateTime> THOI_GIAN { get; set; }
        public Nullable<double> LUONG { get; set; }
        public Nullable<double> SO_LUONG { get; set; }
    }
}