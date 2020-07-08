using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrungTam.Areas.Admin.Abstracts
{
    public class THOI_KHOA_BIEU_LOP_HOC
    {
        public System.Guid MA_LOP { get; set; }
        public string TEN_LOP { get; set; }
        public Nullable<int> SI_SO { get; set; }
        public string MA_GV { get; set; }
        public int THU { get; set; }
        public string THOI_GIAN_BD { get; set; }
        public string THOI_GIAN_KT { get; set; }
    }
}