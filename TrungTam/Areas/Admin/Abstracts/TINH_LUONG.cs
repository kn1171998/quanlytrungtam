using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrungTam.Areas.Admin.Abstracts
{
    public class TINH_LUONG
    {
        public string MA_GV { get; set; }
        public string TEN_LOAI { get; set; }
        public int SO_BUOI { get; set; }
        public Nullable<double> SO_LUONG { get; set; }
        public Nullable<System.DateTime> THOI_GIAN { get; set; }
        public Nullable<double> LUONG { get; set; }
    }
}