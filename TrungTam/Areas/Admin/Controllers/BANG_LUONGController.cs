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
    public class BANG_LUONGController : Controller
    {
        private QL_TRUNGTAM1Entities db = new QL_TRUNGTAM1Entities();

        // GET: Admin/BANG_LUONG
        public ActionResult Index()
        {
            if (Session["ID"] == null)
                return Redirect("/Home/Index");
            var id = Session["ID"].ToString();
            if (id.First() != '9')
            {
                return Redirect("/Home/Index");
            }
            ViewBag.stt = 0;
            return View(db.BANG_LUONG.Where(p => p.TRANG_THAI == true).OrderBy(l => l.SO_LUONG_MAX).ToList());
        } 
        [HttpPost]
        public ActionResult Create(string tenloai ="", string dongia = "", string min ="", string max ="")
        {
            string a = "-1";
            int slmin = int.Parse(min);
            int slmax = int.Parse(max);
            try
            {
                if (slmin != 0 && slmax != 0)
                {
                    var check = db.BANG_LUONG.Where(p => p.TRANG_THAI == true && (p.SO_LUONG_MIN == slmin || p.SO_LUONG_MAX == slmax)).Count();
                    if (check == 0)
                    {
                        BANG_LUONG bl = new BANG_LUONG();
                        bl.MA_LOAI_LUONG = Guid.NewGuid();
                        bl.TEN_LOAI = tenloai;
                        bl.DON_GIA = int.Parse(dongia);
                        bl.TRANG_THAI = true;
                        bl.SO_LUONG_MIN = slmin;
                        bl.SO_LUONG_MAX = slmax;
                        db.BANG_LUONG.Add(bl);
                        db.SaveChanges();
                        a = "1";
                        return Json(a, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        a = "0";
                        return Json(a, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    BANG_LUONG bl = new BANG_LUONG();
                    bl.MA_LOAI_LUONG = Guid.NewGuid();
                    bl.TEN_LOAI = tenloai;
                    bl.DON_GIA = int.Parse(dongia);
                    bl.TRANG_THAI = true;
                    bl.SO_LUONG_MIN = slmin;
                    bl.SO_LUONG_MAX = slmax;
                    db.BANG_LUONG.Add(bl);
                    db.SaveChanges();
                    a = "1";
                    return Json(a, JsonRequestBehavior.AllowGet);
                }
            }
            catch { return Json(a, JsonRequestBehavior.AllowGet); }
        }
        // GET: Admin/BANG_LUONG/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BANG_LUONG bANG_LUONG = db.BANG_LUONG.Find(id);
            if (bANG_LUONG == null)
            {
                return HttpNotFound();
            }
            return View(bANG_LUONG);
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            string a = "-1";
            Guid ma = Guid.Parse(id);
            try
            {
                var check = db.BUOI_HOC.Where(p => p.MA_LUONG == ma).Count();
                var check2 = db.NGOAI_GIO.Where(p => p.MA_LUONG == ma).Count();
                if (check == 0 && check2 == 0)
                {
                    var luong = db.BANG_LUONG.Find(ma);
                    db.BANG_LUONG.Remove(luong);
                    db.SaveChanges();
                    a = "1";
                    return Json(a, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var luong = db.BANG_LUONG.Find(ma);
                    luong.TRANG_THAI = false;
                    db.SaveChanges();
                    a = "1";
                    return Json(a, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(a, JsonRequestBehavior.AllowGet);
            }
        }
        // POST: Admin/BANG_LUONG/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MA_LOAI_LUONG,SO_LUONG_MIN,SO_LUONG_MAX,TEN_LOAI,DON_GIA,TRANG_THAI")] BANG_LUONG bANG_LUONG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bANG_LUONG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bANG_LUONG);
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
