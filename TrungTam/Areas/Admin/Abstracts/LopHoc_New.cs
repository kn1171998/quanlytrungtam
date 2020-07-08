using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrungTam.Areas.Admin.Abstracts
{
    public class LopHoc_New
    {
        public Guid malop { get; set; }
        public string tenlop { get; set; }
        public int? siso { get; set; }
        public string hoten { get; set; }
        public string tenkhoi { get; set; }
        public string tenloai { get; set; }
        public string tenmon { get; set; }
        public Nullable<short> trangthai { get; set; }
        public Nullable<DateTime> ngayketthuc { get; set; }
        public Nullable<DateTime> ngaymolop { get; set; }
        public Nullable<DateTime> ngayhoc { get; set; }
    }
}