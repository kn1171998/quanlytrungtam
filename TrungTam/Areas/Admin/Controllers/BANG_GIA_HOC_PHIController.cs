using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrungTam.Areas.Admin.Models;
using PagedList;
namespace TrungTam.Areas.Admin.Controllers
{
    public class BANG_GIA_HOC_PHIController : Controller
    {
        private QL_TRUNGTAM1Entities db = new QL_TRUNGTAM1Entities();

        // GET: Admin/BANG_GIA_HOC_PHI
        public ActionResult Index()
        {
            if (Session["ID"] == null)
                return Redirect("/Home/Index");
            var id = Session["ID"].ToString();
            if (id.First() != '9' && id.First() != '8')
            {
                return Redirect("/Home/Index");
            }
            var bANG_GIA_HOC_PHI = db.BANG_GIA_HOC_PHI.Include(b => b.KHOI_LOP).Include(b => b.LOAI_LOP).Include(b => b.MON_HOC).Where(p => p.TRANG_THAI == true);
            ViewBag.listkhoi = db.KHOI_LOP.OrderByDescending(m => m.TEN_KHOI).ToList();
            ViewBag.listloailop = db.LOAI_LOP.OrderByDescending(m => m.TEN_LOAI).ToList();
            ViewBag.listmonhoc = db.MON_HOC.OrderByDescending(m => m.TEN_MON).ToList();
            return View(bANG_GIA_HOC_PHI.Where(p => p.TRANG_THAI == true).OrderByDescending(m => m.MA_KHOI).ToList());
        }
        [HttpPost]
        public ActionResult Delete(string ngayap)
        {
            var ngay = DateTime.Parse(ngayap);
            var ngay_new = ngay.ToString("yyyy/MM/dd HH:mm:ss");
            var ngay_sudung = DateTime.Parse(ngay_new);
            var lOP_HOC = (from l in db.LOP_HOC
                           where l.NGAY_AP_DUNG == ngay_sudung
                           select l).Count();
            string a = "0";
            if (lOP_HOC == 0)
            {
                BANG_GIA_HOC_PHI bANG_GIA_HOC_PHI = db.BANG_GIA_HOC_PHI.Find(ngay_sudung);
                db.BANG_GIA_HOC_PHI.Remove(bANG_GIA_HOC_PHI);
                db.SaveChanges();
                a = "1";
                return Json(a, JsonRequestBehavior.AllowGet);
            }
            var lOP_HOC2 = (from l in db.LOP_HOC
                           where l.NGAY_AP_DUNG == ngay_sudung && l.TRANG_THAI != -1
                           select l).Count();
            if (lOP_HOC2 != 0)
            {
                BANG_GIA_HOC_PHI bANG_GIA_HOC_PHI = db.BANG_GIA_HOC_PHI.Find(ngay_sudung);
                bANG_GIA_HOC_PHI.TRANG_THAI = false;
                db.SaveChanges();
                a = "1";
                return Json(a, JsonRequestBehavior.AllowGet);
            }
            return Json(a, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult Create(string makhoi = "", string maloai ="", string mamon = "", string dongia ="", string sobuoi = "" )
        {
            string stt = "1";
            try
            {
                Guid khoi = Guid.Parse(makhoi);
                Guid loai = Guid.Parse(maloai);
                Guid mon = Guid.Parse(mamon);
                var check = db.BANG_GIA_HOC_PHI.Where(p => p.MA_KHOI == khoi && p.MA_LOAI == loai && p.MA_MON == mon && p.TRANG_THAI == true).Count();
                if (check == 0)
                {
                    BANG_GIA_HOC_PHI bhp = new BANG_GIA_HOC_PHI();
                    var ngayapdung = (DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");
                    bhp.NGAY_AP_DUNG = DateTime.Parse(ngayapdung);
                    bhp.MA_KHOI = khoi;
                    bhp.MA_LOAI = loai;
                    bhp.MA_MON = mon;
                    bhp.TRANG_THAI = true;
                    bhp.DON_GIA = decimal.Parse(dongia);
                    string a = sobuoi.Replace('.', ',');
                    //if(a.Contains("."))
                    //string kq = a[0] + ',' + a[1];
                    bhp.SO_BUOI = float.Parse(a);
                    db.BANG_GIA_HOC_PHI.Add(bhp);
                    db.SaveChanges();
                    return Json(stt, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    stt = "0";
                    return Json(stt, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                stt = "-1";
                return Json(stt, JsonRequestBehavior.AllowGet);
            }
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(FormCollection f)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        BANG_GIA_HOC_PHI bhp = new BANG_GIA_HOC_PHI();
        //        var ngayapdung = (DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");
        //        bhp.NGAY_AP_DUNG = DateTime.Parse(ngayapdung);
        //        bhp.MA_KHOI = Guid.Parse(f["makhoi"]);
        //        bhp.MA_LOAI = Guid.Parse(f["maloai"]);
        //        bhp.MA_MON = Guid.Parse(f["mamon"]);
        //        bhp.DON_GIA = decimal.Parse(f["dongia"]);
        //        string a = f["sobuoi"].Replace('.', ',');
        //        //if(a.Contains("."))
        //        //string kq = a[0] + ',' + a[1];
        //        bhp.SO_BUOI = float.Parse(a);
        //        db.BANG_GIA_HOC_PHI.Add(bhp);
        //        db.SaveChanges();
        //        return RedirectToAction("Index", "BANG_GIA_HOC_PHI", new { area = "Admin" });
        //    }
        //    return View();
        //}
        //=====================================================================

        public ActionResult Details(string id)
        {
            if (Session["ID"] == null)
                return Redirect("/Home/Index");
            var idd = Session["ID"].ToString();
            if (idd.First() != '9' && idd.First() != '8')
            {
                return Redirect("/Home/Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DateTime a = DateTime.Parse(id);
            var bANG_GIA_HOC_PHI = db.BANG_GIA_HOC_PHI.Where(p => p.NGAY_AP_DUNG.Equals(a));
            ViewBag.listkhoi = db.KHOI_LOP.OrderByDescending(m => m.TEN_KHOI).ToList();
            ViewBag.listloailop = db.LOAI_LOP.OrderByDescending(m => m.TEN_LOAI).ToList();
            ViewBag.listmonhoc = db.MON_HOC.OrderByDescending(m => m.TEN_MON).ToList();
            if (bANG_GIA_HOC_PHI == null)
            {
                return HttpNotFound();
            }
            return View(bANG_GIA_HOC_PHI.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection f)
        {
            DateTime ax = DateTime.Parse(f["ngayad"]);
            BANG_GIA_HOC_PHI bhp = db.BANG_GIA_HOC_PHI.Find(ax);
            //bhp.MA_KHOI = Guid.Parse(f["makhoi"]);
            //bhp.MA_LOAI = Guid.Parse(f["maloai"]);
            //bhp.MA_MON = Guid.Parse(f["mamon"]);
            bhp.DON_GIA = decimal.Parse(f["dongia"]);
            string a = f["sobuoi"].Replace('.', ',');
            bhp.SO_BUOI = float.Parse(a);
            db.SaveChanges();
            return RedirectToAction("Index", "BANG_GIA_HOC_PHI");
        }
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
