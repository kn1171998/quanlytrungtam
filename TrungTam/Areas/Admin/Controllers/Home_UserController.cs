using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrungTam.Areas.Admin.Models;
using TrungTam.Areas.Admin.Abstracts;
using System.Net;
using System.Data.Entity;

namespace TrungTam.Areas.Admin.Controllers
{
    public class Home_UserController : Controller
    {
        private QL_TRUNGTAM1Entities db = new QL_TRUNGTAM1Entities();
        // GET: Home_User
        public ActionResult Thoikhoabieu()
        {
            if(Session["ID"] == null)
            {
                return Redirect("/Home/Index");
            }
            var noidung = db.TRANG_CHU.OrderByDescending(p => p.NGAY_AP_DUNG);
            ViewBag.noidung = noidung.ToList();
            return View();
        }
        //=======================================
        [HttpGet]
        public ActionResult nhacnhohocphi(string id)
        {
            string ma = Session["ID"].ToString();
            if (ma.First().Equals('2'))
            {
                //var hocphi = db.CONG_NO.Where(p => p.MA_HS.Equals(ma) && p.TRANG_THAI.Equals(false)).Select(p => new {
                //    tongtien = p.TONG_TIEN
                //});
                var tien = (from a in db.CONG_NO
                                 join b in db.HOC_SINH
                                 on a.MA_HS equals b.MA_HS
                                 join ctcn in db.CT_CONG_NO
                                 on a.MA_CONG_NO equals ctcn.MA_CONG_NO
                                 //join c in db.CT_LOP_HOC
                                 //on b.MA_HS equals c.MA_HS
                                 join d in db.LOP_HOC
                                 on ctcn.MA_LOP equals d.MA_LOP
                                 join e in db.BANG_GIA_HOC_PHI
                                 on d.NGAY_AP_DUNG equals e.NGAY_AP_DUNG
                                 join m in db.MON_HOC
                                 on e.MA_MON equals m.MA_MON
                                 join l in db.LOAI_LOP
                                 on e.MA_LOAI equals l.MA_LOAI
                                 join f in db.KHOI_LOP
                                 on e.MA_KHOI equals f.MA_KHOI
                                 where a.MA_HS == ma &&
                                 a.TRANG_THAI == false
                                 && d.TRANG_THAI != -1
                                 select new ChiTietCongNo
                                 {
                                     macongno = a.MA_CONG_NO.ToString(),
                                     malop = d.MA_LOP,
                                     mahs = a.MA_HS,
                                     tenhs = b.HO_TEN,
                                     tenlop = d.TEN_LOP,
                                     tenmon = m.TEN_MON,
                                     tenloai = l.TEN_LOAI,
                                     tenkhoi = f.TEN_KHOI,
                                     giatien = ctcn.THANH_TIEN
                                 }).Distinct().ToList();
                var hocphi = tien.Sum(p => p.giatien);
                if (hocphi == 0)
                    return Json(0, JsonRequestBehavior.AllowGet);
                return Json(hocphi, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(0, JsonRequestBehavior.AllowGet);
        }
        //=======================================
        [HttpGet]
        public ActionResult lol(string id)
        {
            string ma = Session["ID"].ToString();
            if (ma.First().Equals('2'))
            {
                var chonlop = db.LOP_HOC.Where(p => p.TRANG_THAI == 0).Join(db.THOI_KHOA_BIEU, lp => lp.MA_LOP, tkb => tkb.MA_LOP,
                           (lp, tkb) => new THOI_KHOA_BIEU_LOP_HOC
                           {
                               MA_LOP = lp.MA_LOP,
                               TEN_LOP = lp.TEN_LOP,
                               SI_SO = lp.SI_SO,
                               MA_GV = lp.MA_GV,
                               THU = tkb.THU,
                               THOI_GIAN_BD = tkb.THOI_GIAN_BD.ToString(),
                               THOI_GIAN_KT = tkb.THOI_GIAN_KT.ToString()
                           });
                var thoikhoabieu = (db.CT_LOP_HOC.Where(p => p.MA_HS.Equals(ma) && p.LOP_HOC.TRANG_THAI != -1).Join(chonlop
                    , tkb => tkb.MA_LOP, lp => lp.MA_LOP, (tkb, lp) => new THOI_KHOA_BIEU_LOP_HOC
                    {
                        MA_LOP = lp.MA_LOP,
                        TEN_LOP = lp.TEN_LOP,
                        SI_SO = lp.SI_SO,
                        MA_GV = lp.MA_GV,
                        THU = lp.THU,
                        THOI_GIAN_BD = lp.THOI_GIAN_BD,
                        THOI_GIAN_KT = lp.THOI_GIAN_KT
                    }).OrderBy(p => p.THOI_GIAN_BD)).ToList();
                foreach (var item in thoikhoabieu)
                {
                    string[] str1 = item.THOI_GIAN_BD.Split('.');
                    item.THOI_GIAN_BD = str1[0].ToString();
                    string[] str2 = item.THOI_GIAN_KT.Split('.');
                    item.THOI_GIAN_KT = str2[0].ToString();
                }
                return Json(thoikhoabieu, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var thoikhoabieu = db.LOP_HOC.Where(p => p.MA_GV.Equals(ma) && p.TRANG_THAI != -1).Join(db.THOI_KHOA_BIEU, lp => lp.MA_LOP, tkb => tkb.MA_LOP,
                           (lp, tkb) => new THOI_KHOA_BIEU_LOP_HOC
                           {
                               MA_LOP = lp.MA_LOP,
                               TEN_LOP = lp.TEN_LOP,
                               SI_SO = lp.SI_SO,
                               MA_GV = lp.MA_GV,
                               THU = tkb.THU,
                               THOI_GIAN_BD = tkb.THOI_GIAN_BD.ToString(),
                               THOI_GIAN_KT = tkb.THOI_GIAN_KT.ToString()
                           }).ToList();
                foreach(var item in thoikhoabieu)
                {
                    string[] str1 = item.THOI_GIAN_BD.Split('.');
                    item.THOI_GIAN_BD = str1[0].ToString();
                    string[] str2 = item.THOI_GIAN_KT.Split('.');
                    item.THOI_GIAN_KT = str2[0].ToString();
                }
                return Json(thoikhoabieu, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DoiMatKhau(FormCollection f)
        {
            var id = Session["ID"];
            var user = db.TAI_KHOAN.Find(id);
            user.MAT_KHAU = f["pass"];
            db.SaveChanges();
            return RedirectToAction("Thoikhoabieu","Home_User", new { Area = "Admin"});
        }
        public ActionResult EditHS(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOC_SINH hOC_SINH = db.HOC_SINH.Find(id);
            if (hOC_SINH == null)
            {
                return HttpNotFound();
            }
            return View(hOC_SINH);
        }
        public ActionResult EditGV(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GIAO_VIEN gIAO_VIEN = db.GIAO_VIEN.Find(id);
            if (gIAO_VIEN == null)
            {
                return HttpNotFound();
            }
            ViewBag.MA_GV = new SelectList(db.TAI_KHOAN, "TEN", "MAT_KHAU", gIAO_VIEN.MA_GV);
            return View(gIAO_VIEN);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHS(FormCollection f)
        {
            if (ModelState.IsValid)
            {
                HOC_SINH hOC_SINH = db.HOC_SINH.Find(f["mahs"]);
                hOC_SINH.GIOI_TINH = f["gioitinh"];
                hOC_SINH.HO_TEN = f["name"];
                hOC_SINH.KHOI = int.Parse(f["khoi"]);
                hOC_SINH.NG_SINH = DateTime.Parse(f["ngaysinh"]);
                hOC_SINH.NG_VAO_HOC = DateTime.Parse(f["ngaynhaphoc"]);
                hOC_SINH.SDT = f["sdt"].Trim();
                hOC_SINH.MON_DK = f["mondk"];
                hOC_SINH.TRUONG = f["truong"];
                hOC_SINH.PHU_HUYNH = f["phuhuynh"];
                hOC_SINH.SDT_PH = f["sdt_ph"].Trim();
                hOC_SINH.DIA_CHI = f["diachi"];
                TAI_KHOAN tAI_KHOAN = db.TAI_KHOAN.Find(hOC_SINH.MA_HS);
                tAI_KHOAN.TEN = f["sdt_ph"].Trim();
                db.SaveChanges();
                return RedirectToAction("Thoikhoabieu");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGV(FormCollection f)
        {
            if (ModelState.IsValid)
            {
                GIAO_VIEN gIAO_VIEN = db.GIAO_VIEN.Find(f["magv"]);
                gIAO_VIEN.SDT = f["sdt"].Trim();
                gIAO_VIEN.HO_TEN = f["name"];
                gIAO_VIEN.GIOI_TINH = f["gioitinh"];
                gIAO_VIEN.NG_SINH = DateTime.Parse(f["ngaysinh"]);
                gIAO_VIEN.EMAIL = f["email"];
                TAI_KHOAN tAI_KHOAN = db.TAI_KHOAN.Find(f["magv"]);
                tAI_KHOAN.TEN = f["sdt"].Trim();
                db.SaveChanges();
                return RedirectToAction("Thoikhoabieu");
            }
            return View();
        }
        public ActionResult chitietluong()
        {
            if (Session["ID"] == null || Session["ID"].ToString().First() != '1')
            {
                return Redirect("/Home/Index");
            }
            var magv = Session["ID"].ToString();
            int thang = DateTime.Now.Month;
            int nam = DateTime.Now.Year;
            if (magv == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var chitietfull = (from p in db.BUOI_HOC.Include(q => q.BANG_LUONG)
                               where p.MA_GV.Equals(magv) && (p.THOI_GIAN).Month == thang
                                 && (p.THOI_GIAN).Year == nam && p.TINH_TRANG == false
                               group p by p.MA_LUONG into hdgroup
                               select new
                               {
                                   MA_LUONG = hdgroup.Key,
                                   SO_BUOI = hdgroup.Count(),
                                   LUONG = hdgroup.Sum(item => item.BANG_LUONG.DON_GIA)
                               }).ToList();
            var chitiet_ngoaigio = (from p in db.NGOAI_GIO.Include(q => q.BANG_LUONG)
                                    where p.MA_GV.Equals(magv) && (p.NGAY_LAM).Month == thang
                                      && (p.NGAY_LAM).Year == nam && p.TINH_TRANG == false
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
            return View();
        }
        [HttpGet]
        public ActionResult xemluong(string time)
        {
            if (Session["ID"] == null || Session["ID"].ToString().First() != '1')
            {
                return Redirect("/Home/Index");
            }
            var magv = Session["ID"].ToString();
            int thangg = int.Parse(time.Split('_')[0]);
            int namm = int.Parse(time.Split('_')[1]);
            var chitietfull = (from p in db.BUOI_HOC.Include(q => q.BANG_LUONG)
                               where p.MA_GV.Equals(magv) && (p.THOI_GIAN).Month == thangg
                                 && (p.THOI_GIAN).Year == namm && p.TINH_TRANG == false
                               group p by p.MA_LUONG into hdgroup
                               select new
                               {
                                   MA_LUONG = hdgroup.Key,
                                   SO_BUOI = hdgroup.Count(),
                                   LUONG = hdgroup.Sum(item => item.BANG_LUONG.DON_GIA)
                               }).ToList();
            var chitiet_ngoaigio = (from p in db.NGOAI_GIO.Include(q => q.BANG_LUONG)
                                    where p.MA_GV.Equals(magv) && (p.NGAY_LAM).Month == thangg
                                      && (p.NGAY_LAM).Year == namm && p.TINH_TRANG == false
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
                           select new
                           {
                               tenloai = q.TEN_LOAI,
                               sobuoi = p.SO_BUOI,
                               luong = p.LUONG
                           }).OrderBy(p => p.tenloai);
            return Json(chitiet, JsonRequestBehavior.AllowGet);
        }
    }
}