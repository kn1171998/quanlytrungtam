using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrungTam.Areas.Admin.Models;
using TrungTam.Function_Base;
using PagedList;
using TrungTam.Areas.Admin.Abstracts;

namespace TrungTam.Areas.Admin.Controllers
{
    public class HOA_DONController : Controller
    {
        private QL_TRUNGTAM1Entities db = new QL_TRUNGTAM1Entities();
        private BASE bASE = new BASE();
        // GET: Admin/HOA_DON
        public ActionResult Index()
        {
            if (Session["ID"] == null)
                return Redirect("/Home/Index");
            var id = Session["ID"].ToString();
            if (id.First() != '9')
            {
                return Redirect("/Home/Index");
            }
            var hOA_DON = db.HOA_DON.Include(p => p.GIAO_VIEN).OrderByDescending(p => p.NGAY_THANH_TOAN);
            return View(hOA_DON.ToList());
        }

        // GET: Admin/HOA_DON/Details/5
        public ActionResult Details(string id)
        {
            if (id == null) 
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guid ma = Guid.Parse(id);
            var chitiet = (from p in db.CT_HOADON.Include(p => p.BUOI_HOC)
                           where p.MA_HD.Equals(ma)
                           select new chitietluong
                           {
                               TEN_LOP = p.BUOI_HOC.LOP_HOC.TEN_LOP,
                               TEN_LOAI = p.BUOI_HOC.BANG_LUONG.TEN_LOAI,
                               LUONG = p.LUONG,
                               SO_LUONG = 1,
                               THOI_GIAN = p.BUOI_HOC.THOI_GIAN
                           }).OrderBy(p => p.THOI_GIAN);
            var chitiet_ngoaigio = (from p in db.CT_HOADON_NGOAIGIO.Include(p => p.NGOAI_GIO)
                                    where p.MA_HD.Equals(ma)
                                    select new chitietluong
                                    {
                                        TEN_LOP = "",
                                        TEN_LOAI = p.NGOAI_GIO.BANG_LUONG.TEN_LOAI,
                                        SO_LUONG = p.NGOAI_GIO.SO_LUONG,
                                        LUONG = p.LUONG,
                                        THOI_GIAN = p.NGOAI_GIO.NGAY_LAM
                                    }).OrderBy(p => p.THOI_GIAN);
            ViewBag.chitiet = chitiet.ToList();
            ViewBag.chitiet_ngoaigio = chitiet_ngoaigio.ToList();
            return View();
        }
        //------------------------------------------------
        public ActionResult In_hoa_don(string id)
        {
            string[] str = id.Split('_');
            var magv = str[0];
            int thang = int.Parse(str[1]);
            int nam = int.Parse(str[2]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var chitiet = (db.BUOI_HOC.Include(p => p.GIAO_VIEN).Include(q => q.BANG_LUONG)
                .Where(p => p.MA_GV.Equals(magv) && (p.THOI_GIAN).Month == thang
                && (p.THOI_GIAN).Year == nam && p.TINH_TRANG == false)
                .Select(p => new TINH_LUONG
                {
                    MA_GV = p.MA_GV,
                    THOI_GIAN = p.THOI_GIAN,
                    TEN_LOAI = p.BANG_LUONG.TEN_LOAI,
                    SO_BUOI = 1,
                    LUONG = p.BANG_LUONG.DON_GIA
                })).OrderBy(p => p.THOI_GIAN);
            ViewBag.chitiet = chitiet.ToList();
            var chitiet_ngoaigio = (db.NGOAI_GIO.Include(p => p.GIAO_VIEN).Include(q => q.BANG_LUONG)
                .Where(p => p.MA_GV.Equals(id) && (p.NGAY_LAM).Month == thang
                && (p.NGAY_LAM).Year == nam && p.TINH_TRANG == false)
                .Select(p => new TINH_LUONG
                {
                    MA_GV = p.MA_GV,
                    THOI_GIAN = p.NGAY_LAM,
                    TEN_LOAI = p.BANG_LUONG.TEN_LOAI,
                    SO_LUONG = p.SO_LUONG,
                    LUONG = p.BANG_LUONG.DON_GIA
                })).OrderBy(p => p.THOI_GIAN);
            ViewBag.chitiet_ngoaigio = chitiet_ngoaigio.ToList();
            return View();
        }
        //--------------------KHÔNG XOÁ CODE NÀY (CHI TIẾT LƯƠNG ĐÃ GROUP BY)----------------------------
        //public ActionResult chitiet_tinhluong(string id)
        //{
        //    string[] str = id.Split('_');
        //    var magv = str[0];
        //    int thang = int.Parse(str[1]);
        //    int nam = int.Parse(str[2]);
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var chitietfull = (from p in db.BUOI_HOC.Include(q => q.BANG_LUONG)
        //                      where p.MA_GV.Equals(magv) && (p.THOI_GIAN).Month == thang
        //                        && (p.THOI_GIAN).Year == nam && p.TINH_TRANG == false
        //                      group p by p.MA_LUONG into hdgroup
        //                      select new
        //                      {
        //                          MA_LUONG = hdgroup.Key,
        //                          SO_BUOI = hdgroup.Count(),
        //                          LUONG = hdgroup.Sum(item => item.BANG_LUONG.DON_GIA)
        //                      }).ToList();
        //    var chitiet_ngoaigio = (from p in db.NGOAI_GIO.Include(q => q.BANG_LUONG)
        //                      where p.MA_GV.Equals(magv) && (p.NGAY_LAM).Month == thang
        //                        && (p.NGAY_LAM).Year == nam && p.TINH_TRANG == false
        //                      group p by p.MA_LUONG into hdgroup
        //                      select new
        //                      {
        //                          MA_LUONG = hdgroup.Key,
        //                          SO_BUOI = hdgroup.Sum(item => item.SO_LUONG),
        //                          LUONG = hdgroup.Sum(item => item.BANG_LUONG.DON_GIA)
        //                      }).ToList();
        //    chitietfull.AddRange(chitiet_ngoaigio);
        //    var chitiet = (from p in chitietfull
        //                  join q in db.BANG_LUONG on p.MA_LUONG equals q.MA_LOAI_LUONG
        //                    select new TINH_LUONG
        //                    {
        //                        TEN_LOAI = q.TEN_LOAI,
        //                        SO_BUOI = p.SO_BUOI,
        //                        LUONG = p.LUONG
        //                    }).OrderBy(p => p.TEN_LOAI);
        //    ViewBag.chitiet = chitiet.ToList();
        //    return View();
        //}
        //------------------------------------------------
        public ActionResult chitiet_tinhluong(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string[] str = id.Split('_');
            var magv = str[0];
            int thang = int.Parse(str[1]);
            int nam = int.Parse(str[2]);
            
            var chitietfull = (from p in db.BUOI_HOC.Include(q => q.BANG_LUONG)
                               where p.MA_GV.Equals(magv) && (p.THOI_GIAN).Month == thang
                                 && (p.THOI_GIAN).Year == nam && p.TINH_TRANG == false
                               select new
                               {
                                   TEN_LOP = p.LOP_HOC.TEN_LOP,
                                   TEN_LUONG = p.BANG_LUONG.TEN_LOAI,
                                   THOI_GIAN = p.THOI_GIAN,
                                   LUONG = p.BANG_LUONG.DON_GIA,
                               }).ToList();
            var chitiet_ngoaigio = (from p in db.NGOAI_GIO.Include(q => q.BANG_LUONG)
                                    where p.MA_GV.Equals(magv) && (p.NGAY_LAM).Month == thang
                                      && (p.NGAY_LAM).Year == nam && p.TINH_TRANG == false
                                    select new
                                    {
                                        TEN_LOP = "",
                                        TEN_LUONG = p.BANG_LUONG.TEN_LOAI,
                                        THOI_GIAN = p.NGAY_LAM,
                                        LUONG = p.BANG_LUONG.DON_GIA,
                                    }).ToList();
            chitietfull.AddRange(chitiet_ngoaigio);
            var chitiet = (from p in chitietfull
                           select new chitietluong
                           {
                               TEN_LOP = p.TEN_LOP,
                               TEN_LOAI = p.TEN_LUONG,
                               THOI_GIAN = p.THOI_GIAN,
                               LUONG = p.LUONG,
                           }).OrderBy(p => p.THOI_GIAN);
            ViewBag.chitiet = chitiet.ToList();

            var Giaovien = db.GIAO_VIEN.Find(magv);
            if (Giaovien != null)
                ViewBag.giaovien = Giaovien.HO_TEN.ToString();
            return View();
        }
        // GET: Admin/HOA_DON/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/HOA_DON/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(FormCollection f)
        {
            if (ModelState.IsValid)
            {
                int thang = int.Parse(f["thang"]);
                int nam = int.Parse(f["nam"]);
                var hoadon = (from p in db.BUOI_HOC.Include(q => q.BANG_LUONG)
                              where p.TINH_TRANG == false
                              && (p.THOI_GIAN).Month == thang && (p.THOI_GIAN).Year == nam
                              select new NGOAI_GIO_NEW
                              {
                                  MA_GV = p.MA_GV,
                                  LUONG = p.BANG_LUONG.DON_GIA,
                                  SO_LUONG = 1
                              }).ToList();
                //tinh luong ngoai gio
                var ngoaigio = (from q in db.NGOAI_GIO.Include(p => p.BANG_LUONG)
                                where q.TINH_TRANG == false
                                && (q.NGAY_LAM).Month == thang && (q.NGAY_LAM).Year == nam
                                select new NGOAI_GIO_NEW
                                {
                                    MA_GV = q.MA_GV,
                                    LUONG = q.BANG_LUONG.DON_GIA,
                                    SO_LUONG = q.SO_LUONG
                                }).ToList();
                //Lấy thêm dữ liệu cho đầy đủ
                foreach (var item in ngoaigio)
                {
                    hoadon.Add(item);
                }
                foreach (var item in hoadon)
                {
                    item.LUONG = item.LUONG * item.SO_LUONG;
                }
                var hoadon_new = from p in hoadon
                                 group p by p.MA_GV into hdgroup
                                 select new hoadon1
                                 {
                                     ma = hdgroup.Key,
                                     thanhtien = hdgroup.Sum(item => item.LUONG)
                                 };
                var hoadon_full = (from p in hoadon_new
                                   join c in db.GIAO_VIEN on
                                    p.ma equals c.MA_GV
                                   select new hoadon_full
                                   {
                                       magv = p.ma,
                                       luong = p.thanhtien,
                                       thoigian = thang + "/" + nam,
                                       hoten = c.HO_TEN,
                                       sdt = c.SDT
                                   }).OrderBy(p => p.thoigian).ToList();
                //---- Thêm hóa đơn
                foreach (var item in hoadon_full)
                {
                    HOA_DON Hoadon = new HOA_DON();
                    Hoadon.MA_HD = Guid.NewGuid();
                    Hoadon.MA_GV = item.magv;
                    Hoadon.TEN_HD = "Lương tháng " + thang + "/" + nam ;
                    Hoadon.TONG_TIEN = item.luong;
                    Hoadon.NGAY_THANH_TOAN = DateTime.Now;
                    db.HOA_DON.Add(Hoadon);
                    //------- Thêm ct_hoadon ------------------------
                    var chitiet = db.BUOI_HOC.Include(p => p.GIAO_VIEN).Include(q => q.BANG_LUONG)
                    .Where(p => p.MA_GV.Equals(item.magv) && (p.THOI_GIAN).Month == thang 
                    && (p.THOI_GIAN).Year == nam && p.TINH_TRANG == false)
                    .Select(p => new 
                    {
                        MA_Gv = p.MA_GV,
                        MA_BUOi = p.MA_BUOI,
                        LUONG = p.BANG_LUONG.DON_GIA
                    });
                    foreach (var i in chitiet)
                    {
                        CT_HOADON cT_HOADON = new CT_HOADON();
                        cT_HOADON.MA_HD = Hoadon.MA_HD;
                        cT_HOADON.MA_BUOI = i.MA_BUOi;
                        cT_HOADON.LUONG = i.LUONG;
                        db.CT_HOADON.Add(cT_HOADON);
                        //--------------------------------
                        BUOI_HOC bh = db.BUOI_HOC.Find(i.MA_BUOi);
                        bh.TINH_TRANG = true;
                    }
                    //------ Thêm ct_hoadon_ngoaigio ------------
                    var ngoaigio_new = (from q in db.NGOAI_GIO.Include(p => p.BANG_LUONG)
                                    where q.TINH_TRANG == false
                                    && (q.NGAY_LAM).Month == thang && (q.NGAY_LAM).Year == nam
                                    && q.MA_GV == item.magv
                                    select new NGOAI_GIO_NEW
                                    {
                                        MA_NGOAI_GIO = q.MA_NGOAI_GIO,
                                        MA_GV = q.MA_GV,
                                        LUONG = q.BANG_LUONG.DON_GIA,
                                        SO_LUONG = q.SO_LUONG
                                    }).ToList();
                    foreach (var meti in ngoaigio_new)
                    {
                        CT_HOADON_NGOAIGIO cT_HOADON_NGOAIGIO = new CT_HOADON_NGOAIGIO();
                        cT_HOADON_NGOAIGIO.MA_NGOAI_GIO = meti.MA_NGOAI_GIO;
                        cT_HOADON_NGOAIGIO.LUONG = meti.LUONG * meti.SO_LUONG;
                        cT_HOADON_NGOAIGIO.MA_HD = Hoadon.MA_HD;
                        db.CT_HOADON_NGOAIGIO.Add(cT_HOADON_NGOAIGIO);
                        //-----------------------------------------
                        NGOAI_GIO ng = db.NGOAI_GIO.Find(meti.MA_NGOAI_GIO);
                        ng.TINH_TRANG = true;
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index", "HOA_DON", new { area = "Admin" });
            }
            return View();
        }
        // POST: Admin/HOA_DON/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.   
        // POST: Admin/HOA_DON/Delete/5
        [HttpPost]
        public void Delete()
        {
            var hOA_DON = db.HOA_DON.ToList();
            var ct_hoadon = db.CT_HOADON.ToList();
            var ct_hoadon_ngoaigio = db.CT_HOADON_NGOAIGIO.ToList();
            foreach (var item in hOA_DON)
            {
                if (DateTime.Now.Year - (item.NGAY_THANH_TOAN).Year == 2)
                {
                    foreach (var i in ct_hoadon)
                    {
                        if (i.MA_HD.Equals(item.MA_HD))
                            db.CT_HOADON.Remove(i);
                    }
                    foreach (var j in ct_hoadon_ngoaigio)
                    {
                        if (j.MA_HD.Equals(item.MA_HD))
                            db.CT_HOADON_NGOAIGIO.Remove(j);
                    }
                    db.HOA_DON.Remove(item);
                }
                db.SaveChanges();
            }
            //return RedirectToAction("Index");
        }
        //===========================================
        [HttpGet]
        public ActionResult search_thoigian(string id)
        {

            var date = DateTime.Parse(id);
            var search = (from p in db.HOA_DON
                         join a in db.GIAO_VIEN
                         on p.MA_GV equals a.MA_GV
                         where (p.NGAY_THANH_TOAN).Month == date.Month && (p.NGAY_THANH_TOAN).Year == date.Year
                         select new
                         {
                             mahd = p.MA_HD,
                             tengv = a.HO_TEN,
                             tenhd = p.TEN_HD,
                             tongtien = p.TONG_TIEN,
                             ngay = p.NGAY_THANH_TOAN.ToString()
                         }).OrderBy(p => p.ngay);
            return Json(search, JsonRequestBehavior.AllowGet);
        }
        //=====================================================
        [HttpGet]
        public ActionResult search_hoten(string id)
        {

            var search = (from p in db.HOA_DON
                          join a in db.GIAO_VIEN
                          on p.MA_GV equals a.MA_GV
                          where a.HO_TEN.Contains(id)
                          select new
                          {
                              mahd = p.MA_HD,
                              tengv = a.HO_TEN,
                              tenhd = p.TEN_HD,
                              tongtien = p.TONG_TIEN,
                              ngay = p.NGAY_THANH_TOAN.ToString()
                          }).OrderBy(p => p.ngay);
            return Json(search, JsonRequestBehavior.AllowGet);
        }
        //===========================================
        //[HttpGet]
        //public ActionResult loadluong(string id) // 8/2019
        //{
        //    string[] str = id.Split('/');
        //    var thangnam = DateTime.Parse(id);
        //    var thang = int.Parse(str[0]); //8
        //    var nam = int.Parse(str[1]); //2019
        //    var hoadon = from p in db.BUOI_HOC.Include(q => q.BANG_LUONG)
        //                 where p.TINH_TRANG == false
        //                 && (p.THOI_GIAN).Month == thang && (p.THOI_GIAN).Year == nam
        //                 group p by p.MA_GV into hdgroup
        //                 select new hoadon1
        //                 {
        //                     ma = hdgroup.Key,
        //                     thanhtien = hdgroup.Sum(item => item.BANG_LUONG.DON_GIA)
        //                 };
        //    //tinh luong ngoai gio
        //    var ngoaigio = (from q in db.NGOAI_GIO.Include(p => p.BANG_LUONG)
        //                   where q.TINH_TRANG == false
        //                   && (q.NGAY_LAM).Month == thang && (q.NGAY_LAM).Year == nam
        //                   select new NGOAI_GIO_NEW
        //                   {
        //                       MA_GV = q.MA_GV,
        //                       //LUONG = q.BANG_LUONG.DON_GIA * q.SO_LUONG
        //                       LUONG = q.BANG_LUONG.DON_GIA,
        //                       SO_LUONG = q.SO_LUONG
        //                   }).ToList();
        //    var hoadon_full = (from p in hoadon
        //                      join c in db.GIAO_VIEN on
        //     p.ma equals c.MA_GV
        //                      select new hoadon_full
        //                      {
        //                          magv = p.ma,
        //                          luong = p.thanhtien,
        //                          thoigian = id,
        //                          hoten = c.HO_TEN,
        //                          sdt = c.SDT
        //                      }).OrderBy(p => p.thoigian).ToList();
        //    ////tinh luong buoi hoc +ngoai gio
        //    for (int i = 0; i < ngoaigio.Count(); i++)
        //    {
        //        foreach (var meti in hoadon_full)
        //        {
        //            if (ngoaigio[i].MA_GV.Equals(meti.magv))
        //                meti.luong = (ngoaigio[i].LUONG * Decimal.Parse(ngoaigio[i].SO_LUONG.ToString())) + meti.luong;
        //        }
        //    }
        //    return Json(hoadon_full, JsonRequestBehavior.AllowGet);
        //}
        //==============================================================
        [HttpGet]
        public ActionResult loadluong(string id) // 8/2019
        {
            string[] str = id.Split('_');
            //var thangnam = DateTime.Parse(id);
            var thang = int.Parse(str[0]); //8
            var nam = int.Parse(str[1]); //2019
            //tinh luong buoi hoc
            var hoadon = (from p in db.BUOI_HOC.Include(q => q.BANG_LUONG)
                         where p.TINH_TRANG == false
                         && (p.THOI_GIAN).Month == thang && (p.THOI_GIAN).Year == nam
                         select new NGOAI_GIO_NEW
                         {
                             MA_GV = p.MA_GV,
                             LUONG = p.BANG_LUONG.DON_GIA,
                             SO_LUONG = 1
                         }).ToList();
            //tinh luong ngoai gio
            var ngoaigio = (from q in db.NGOAI_GIO.Include(p => p.BANG_LUONG)
                            where q.TINH_TRANG == false
                            && (q.NGAY_LAM).Month == thang && (q.NGAY_LAM).Year == nam
                            select new NGOAI_GIO_NEW
                            {
                                MA_GV = q.MA_GV,
                                LUONG = q.BANG_LUONG.DON_GIA,
                                SO_LUONG = q.SO_LUONG
                            }).ToList();
            foreach (var item in ngoaigio)
            {
                hoadon.Add(item);
            }
            foreach (var item in hoadon)
            {
                item.LUONG = item.LUONG * item.SO_LUONG;
            }
            var hoadon_new = from p in hoadon
                             group p by p.MA_GV into hdgroup
                             select new hoadon1
                             {
                                 ma = hdgroup.Key,
                                 thanhtien = hdgroup.Sum(item => item.LUONG)
                             };
            var hoadon_full = (from p in hoadon_new
                               join c in db.GIAO_VIEN on
                                p.ma equals c.MA_GV
                               select new hoadon_full
                               {
                                   magv = p.ma,
                                   luong = p.thanhtien,
                                   thoigian = thang +"/"+ nam,
                                   hoten = c.HO_TEN,
                                   sdt = c.SDT
                               }).OrderBy(p => p.thoigian).ToList();
            return Json(hoadon_full, JsonRequestBehavior.AllowGet);
        }
        //===========================================

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
