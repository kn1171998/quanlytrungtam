using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrungTam.Areas.Admin.Abstracts
{
    public class NGOAI_GIO_NEW
    {
        public string MA_GV { get; set; }
        public Guid MA_NGOAI_GIO { get; set; }
        public Nullable<double> LUONG { get; set; }
        public Nullable<double> SO_LUONG { get; set; }
    }
}