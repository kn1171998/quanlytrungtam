using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrungTam.Areas.Admin.Models;
namespace TrungTam.Function_Base
{
    public class BASE
    {
        QL_TRUNGTAM1Entities db = new QL_TRUNGTAM1Entities();
        public void create_TAI_KHOAN(string tk, string ten)
        {
            TAI_KHOAN tAI_KHOAN = new TAI_KHOAN();
            tAI_KHOAN.ID = tk;
            tAI_KHOAN.TEN = ten;
            tAI_KHOAN.MAT_KHAU = "123";
            db.TAI_KHOAN.Add(tAI_KHOAN);
            db.SaveChanges();
        }
        //----------------------------------------
        public int DayOfWeekk (string id)
        {
            DateTime day = DateTime.Parse(id);
            string dayS = day.DayOfWeek.ToString();
            // Sun: 0 - Mon: 1 - Tue: 2 - Web: 3 - Thu: 4 - Fri: 5 - Sat: 6
            return (int)day.DayOfWeek;
        }
        //---------------------------------------
        public int DayOfWeek()
        {
            DateTime day = DateTime.Now;
            string dayS = day.DayOfWeek.ToString();
            // Sun: 0 - Mon: 1 - Tue: 2 - Web: 3 - Thu: 4 - Fri: 5 - Sat: 6
            return (int)day.DayOfWeek;
        }
        //---------------------------------------
       
    }
}