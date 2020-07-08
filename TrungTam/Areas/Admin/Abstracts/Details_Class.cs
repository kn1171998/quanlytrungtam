using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrungTam.Areas.Admin.Abstracts
{
    public class Details_Class
    {
        public string tenlop { get; set; }
        public int thu { get; set; }
        public string tgbd { get => tgbd; set => tgbd = value; }
        public string Tgkt { get => tgkt; set => tgkt = value; }        
        public string Mahs { get => mahs; set => mahs = value; }
        public string Tenhs { get => tenhs; set => tenhs = value; }
        public DateTime Ngaysinh { get => ngaysinh; set => ngaysinh = value; }

        string tgkt;
        string mahs;
        string tenhs;
        DateTime ngaysinh;        
    }
}