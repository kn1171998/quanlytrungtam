using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrungTam.Areas.Admin.Abstracts
{
    public class ThoiKhoaBieu
    {
     
        public Guid matkb { get; set; }
        public int thu { get; set; }
        public TimeSpan tgbt { get; set; }
        public TimeSpan tgkt { get; set; }
    }
}