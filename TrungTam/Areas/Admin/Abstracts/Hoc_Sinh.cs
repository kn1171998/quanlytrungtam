using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrungTam.Areas.Admin.Abstracts
{
    public class Hoc_Sinh
    {
        public string mahs { get; set; }
        public string hoten { get; set; }
        public DateTime ngaysinh { get; set; }
        public string gioitinh { get; set; }
        public string sdt { get; set; }
        public string phuhuynh { get; set; }
    }
}