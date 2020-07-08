using System;
using System.Linq;
using System.Web.Mvc;
using TrungTam.Areas.Admin.Models;
using TrungTam.Areas.Admin.Abstracts;
using Rotativa;
using System.Collections.Generic;
using System.Globalization;

namespace TrungTam.Areas.Admin.Controllers
{
    public class DANH_GIA_THEO_THANGController : Controller
    {
        private QL_TRUNGTAM1Entities db = new QL_TRUNGTAM1Entities();
        public static IEnumerable<ReportDanhGia> quanque;
        // GET: Admin/DANH_GIA_THEO_THANG
        public ActionResult Index()
        {
            var lop = db.LOP_HOC.Where(p => p.TRANG_THAI == 0).ToList();
            return View(lop);
        }
        [HttpGet]
        public ActionResult Index1(string id)
        {
            var ma = Guid.Parse(id);
            var ct_lop = from l in db.LOP_HOC
                         join ctl in db.CT_LOP_HOC
                         on l.MA_LOP equals ctl.MA_LOP
                         join hs in db.HOC_SINH
                         on ctl.MA_HS equals hs.MA_HS
                         where l.MA_LOP == ma && l.TRANG_THAI == 0
                         select new
                         {
                             mahs = hs.MA_HS,
                             tenhs = hs.HO_TEN,
                             sdt = hs.SDT_PH,
                         };
            return Json(ct_lop, JsonRequestBehavior.AllowGet);
        }
        public List<ReportDanhGia> reportDanhGia(string mahs, string thang, string nam)
        {
            return (from b in db.BUOI_HOC
                    join p in db.CT_BUOIHOC
                    on b.MA_BUOI equals p.MA_BUOI
                    join g in db.GIAO_VIEN
                    on b.MA_GV equals g.MA_GV
             
                    where p.MA_HS.Equals(mahs) && p.BUOI_HOC.THOI_GIAN.Month.ToString().Equals(thang) && p.BUOI_HOC.THOI_GIAN.Year.ToString().Equals(nam)
                    && p.BUOI_HOC.LOP_HOC.TRANG_THAI == 0
                    select new ReportDanhGia
                    {
                        tenlop = p.BUOI_HOC.LOP_HOC.TEN_LOP,
                        buoihoc = p.BUOI_HOC.STT_BUOI,
                        tengv = g.HO_TEN,
                        tenmon = p.BUOI_HOC.LOP_HOC.BANG_GIA_HOC_PHI.MON_HOC.TEN_MON,
                        diemdanh = p.DIEM_DANH_HS.ToString(),
                        btvn = p.BAI_TAP_VN,
                        nhanxet = p.NHAN_XET_GV,
                        tenhs = p.HOC_SINH.HO_TEN
                    }).Distinct().OrderBy(p => p.buoihoc).ToList();

        }
        public static List<ReportDanhGia> rpDanhGia = null;
        public static string cl = "";
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KetQuaHocTap1(string mahs, string date)
        {
            string[] ma = mahs.Split('_');
            var mhs = ma[0];
            string[] str = date.Split('_');
            string thang = str[0], nam = str[1];
            rpDanhGia = reportDanhGia(mahs, thang, nam);
            cl = ma[1];
            //if (ma[1] == "json")
            //    return Json(kq, JsonRequestBehavior.AllowGet);
            //return View(kq);
            return RedirectToAction("KetQuaHocTap", "DANH_GIA_THEO_THANG");
        }
        public ActionResult KetQuaHocTap()
        {
            if(string.IsNullOrEmpty(cl) || rpDanhGia == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            else if(cl == "json")
            {
                return Json(rpDanhGia, JsonRequestBehavior.AllowGet);
            }
            return View(rpDanhGia);
        }
        public ActionResult PrintReportMonth(string id, string date)
        {
            string[] str = date.Split('_');
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            string mahs = id.Substring(0, 10);
            //string dateloc = id.Substring(10);
            string thang = str[0], nam = str[1];

            var chitiet = reportDanhGia(mahs, thang, nam);
            foreach (var a in chitiet)
            {
                if (a.diemdanh == "True")
                    a.diemdanh = "Có";
                else
                    a.diemdanh = "Vắng";
            }
            if (chitiet.Count() != 0)
                quanque = chitiet;
            else quanque = null;
            return RedirectToAction("Prints", "DANH_GIA_THEO_THANG");
        }

        public ActionResult Prints()
        {
            return new ActionAsPdf("PrintReport", "DANH_GIA_THEO_THANG");
        }

        public ActionResult PrintReport()
        {
            try
            {
                return View(quanque);
                //if (quanque != null)
                //{
                //    return View(quanque);
                //}
                //else
                //{
                //    return View();
                //}
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "DANH_GIA_THEO_THANG");
            }
            //return RedirectToAction("Index", "DANH_GIA_THEO_THANG");
        }
    }
}