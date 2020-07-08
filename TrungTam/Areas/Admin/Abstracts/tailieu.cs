using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrungTam.Areas.Admin.Abstracts
{
    public class tailieu
    {
        public System.Guid MATL { get; set; }
        public string TENTL { get; set; }
        public string LINK { get; set; }
        public string MONHOC { get; set; }
        public Nullable<int> KHOI { get; set; }

    }
}