using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrungTam.Areas.Admin.Models;
using TrungTam.Areas.Admin.Abstracts;

namespace TrungTam.Areas.Admin.Controllers
{
    public class THONG_KEController : Controller
    {
        private QL_TRUNGTAM1Entities db = new QL_TRUNGTAM1Entities();
        // GET: Admin/THONG_KE
        public ActionResult Index()
        {
            var thang = DateTime.Now.Month;
            if (Session["ID"] == null)
                return Redirect("/Home/Index");
            var id = Session["ID"].ToString();
            if (id.First() != '9' && id.First() != '8')
            {
                return Redirect("/Home/Index");
            }
            var year = DateTime.Now.Year;
            var hoadon = db.HOA_DON.Where(p => p.NGAY_THANH_TOAN.Year.Equals(year));
            var thongke_luong = (from p in hoadon
                                 group p by p.NGAY_THANH_TOAN.Month into hdgroup
                                 select new ThongKe_Luong
                                 {
                                     thang = hdgroup.Key,
                                     luong = hdgroup.Sum(item => item.TONG_TIEN)
                                 }).OrderByDescending(p => p.thang);
            var chitieungoai = db.CHI_TIEU_NGOAI.Where(p => p.NGAY.Value.Year.Equals(year));
            var thongke_chitieungoai = (from p in chitieungoai
                                        group p by p.NGAY.Value.Month into hdgroup
                                        select new ThongKe_Luong
                                        {
                                            thang = hdgroup.Key,
                                            luong = hdgroup.Sum(item => item.THANH_TIEN)
                                        }).OrderBy(p => p.thang);
            //---------------------------------------------------
            var chitietfull = (from p in db.BUOI_HOC
                               where (p.THOI_GIAN).Month == thang
                                 && (p.THOI_GIAN).Year == year && p.TINH_TRANG == false
                               group p by p.MA_LUONG into hdgroup
                               select new
                               {
                                   MA_LUONG = hdgroup.Key,
                                   SO_BUOI = hdgroup.Count(),
                                   LUONG = hdgroup.Sum(item => item.BANG_LUONG.DON_GIA)
                               }).ToList();
            var chitiet_ngoaigio = (from p in db.NGOAI_GIO
                                    where (p.NGAY_LAM).Month == thang
                                      && (p.NGAY_LAM).Year == year && p.TINH_TRANG == false
                                    group p by p.MA_LUONG into hdgroup
                                    select new
                                    {
                                        MA_LUONG = hdgroup.Key,
                                        SO_BUOI = hdgroup.Sum(item => item.SO_LUONG),
                                        LUONG = hdgroup.Sum(item => item.BANG_LUONG.DON_GIA)
                                    }).ToList();
            chitietfull.AddRange(chitiet_ngoaigio);
            var chitiet = (from p in chitietfull
                           join q in db.BANG_LUONG on p.MA_LUONG equals q.MA_LOAI_LUONG
                           select new TINH_LUONG
                           {
                               TEN_LOAI = q.TEN_LOAI,
                               SO_BUOI = p.SO_BUOI,
                               LUONG = p.LUONG
                           }).OrderBy(p => p.TEN_LOAI);
            ViewBag.chitiet = chitiet.ToList();
            var chitieungoai1 = db.CHI_TIEU_NGOAI.Where(p => p.NGAY.Value.Month == thang
                                     && p.NGAY.Value.Year == year);
            double? chitieungoai_now = 0;
            foreach (var item in chitieungoai1)
            {
                chitieungoai_now += item.THANH_TIEN;
            }
            ViewBag.chitieungoai_now = chitieungoai_now;
            //----------------------------------------------
            var luong1 = db.HOA_DON.Where(p => p.NGAY_THANH_TOAN.Month == thang && p.NGAY_THANH_TOAN.Year == year);
            double? luong_now = 0;
            foreach (var item in luong1)
            {
                luong_now += item.TONG_TIEN;
            }
            ViewBag.luong_now = luong_now;
            //----------------------------------------------
            if (thongke_luong.Count() != 0)
                ViewBag.thongke_luong = thongke_luong.ToList();
            else
            {
                
                ViewBag.thongke_luong = new List<ThongKe_Luong>();
            }
               
            if (thongke_chitieungoai.Count() != 0)
                ViewBag.thongke_chitieungoai = thongke_chitieungoai.ToList();
            else
            {

                ViewBag.thongke_luong = new List<ThongKe_Luong>();
            }
            //----------------------------------------------
            var dem = db.CONG_NO.Count();
            if (dem != 0)
            {
                var congno = db.CONG_NO.Where(p => p.NGAY_THANH_TOAN.Value.Year.Equals(year) && p.TRANG_THAI.Equals(true));
                var thongke_hocphi = (from p in congno
                                      group p by p.NGAY_THANH_TOAN.Value.Month into hdgroup
                                      select new ThongKe_HocPhi
                                      {
                                          thang = hdgroup.Key,
                                          luong = hdgroup.Sum(item => item.TONG_TIEN)
                                      }).OrderByDescending(p => p.thang);
                if (thongke_hocphi.Count() > 0)
                    ViewBag.thongke_hocphi = thongke_hocphi.ToList();
                else
                    ViewBag.thongke_hocphi = null;
            }
            //----------------------------------------------
            int hocvien = db.HOC_SINH.Where(p => p.TINH_TRANG == true).Count();
            ViewBag.tonghocvien = hocvien;
            //----------------------------------------------
            var listhocvien = db.HOC_SINH.Where(p => p.TINH_TRANG == true && p.NG_VAO_HOC.Value.Year.Equals(year));
            var Listhocviennew = (from p in listhocvien
                                  group p by p.NG_VAO_HOC.Value.Month into hdgroup
                                  select new listhocsinhnew
                                  {
                                      thang = hdgroup.Key,
                                      soluong = hdgroup.Count()
                             }).OrderByDescending(p => p.thang);
            ViewBag.Listhocviennew = Listhocviennew.ToList();
            //----------------------------------------------
            var ghidanh = db.GHI_DANH;
            int num = 0;
            foreach (var item in ghidanh)
            {
                if (item.TINH_TRANG == false)
                    num += 1;
            }
            ViewBag.ghidanh = ghidanh.ToList();
            ViewBag.num = num;
            
            return View();
        }
        //-------------------------------------------
        [HttpPost]
        public ActionResult Delete(string id)
        {
            Guid ma = Guid.Parse(id);
            GHI_DANH ghidanh = db.GHI_DANH.Find(ma);
            db.GHI_DANH.Remove(ghidanh);
            db.SaveChanges();
            return RedirectToAction("Index", "THONG_KE", new { area = "Admin" });
        }
        public ActionResult home()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Load_hocvienmoi()
        {
            var hocvienmoi = (from p in db.HOC_SINH
                              group p by p.NG_VAO_HOC.Value.Month into hdgroup
                              select new ThongKe_HocPhi
                              {
                                  thang = hdgroup.Key,
                                  luong = hdgroup.Count()
                              }).OrderBy(p => p.thang);
            return Json(hocvienmoi, JsonRequestBehavior.AllowGet);
        }
        public ActionResult index_vanphong()
        {
            var thang = DateTime.Now.Month;
            if (Session["ID"] == null)
                return Redirect("/Home/Index");
            var id = Session["ID"].ToString();
            if (id.First() != '9' && id.First() != '8')
            {
                return Redirect("/Home/Index");
            }
            var year = DateTime.Now.Year;
           
            //----------------------------------------------
            int hocvien = db.HOC_SINH.Where(p => p.TINH_TRANG == true).Count();
            ViewBag.tonghocvien = hocvien;
            //----------------------------------------------
            var listhocvien = db.HOC_SINH.Where(p => p.TINH_TRANG == true && p.NG_VAO_HOC.Value.Year.Equals(year));
            var Listhocviennew = (from p in listhocvien
                                  group p by p.NG_VAO_HOC.Value.Month into hdgroup
                                  select new listhocsinhnew
                                  {
                                      thang = hdgroup.Key,
                                      soluong = hdgroup.Count()
                                  }).OrderByDescending(p => p.thang);
            ViewBag.Listhocviennew = Listhocviennew.ToList();
            //----------------------------------------------
            var ghidanh = db.GHI_DANH;
            int num = 0;
            foreach (var item in ghidanh)
            {
                if (item.TINH_TRANG == false)
                    num += 1;
            }
            ViewBag.ghidanh = ghidanh.ToList();
            ViewBag.num = num;

            return View();
        }
    }
}