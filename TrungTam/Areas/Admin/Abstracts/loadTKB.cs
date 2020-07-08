using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrungTam.Areas.Admin.Abstracts
{
    public class loadTKB
    {
        public Guid malop { get; set; }
        public string tenlop { get; set; }
        public Nullable<short> trangthai { get; set; }
        public bool check { get; set; }
    }
}