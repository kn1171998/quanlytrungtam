using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrungTam.Areas.Admin.Abstracts
{
    public class BangHP_Khoi_Loai_Mon
    {
        public DateTime NGAYAD { get; set; }
        public System.Guid MA_KHOI { get; set; }
        public string TEN_KHOI { get; set; }
        public System.Guid MA_MON { get; set; }
        public string TEN_MON { get; set; }
        public System.Guid MA_LOAI { get; set; }
        public string TEN_LOAI { get; set; }
        public Nullable<decimal> DON_GIA { get; set; }
        public Nullable<double> SO_BUOI { get; set; }
    }
}