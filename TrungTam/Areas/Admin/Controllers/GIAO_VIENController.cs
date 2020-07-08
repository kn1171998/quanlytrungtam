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
using TrungTam.Areas.Admin.Abstracts;
using PagedList;
namespace TrungTam.Areas.Admin.Controllers
{
    public class GIAO_VIENController : Controller
    {
        private QL_TRUNGTAM1Entities db = new QL_TRUNGTAM1Entities();
        private BASE bASE = new BASE();
        // GET: Admin/GIAO_VIEN
        public ActionResult Index()
        {
            if (Session["ID"] == null)
                return Redirect("/Home/Index");
            var id = Session["ID"].ToString();
            if (id.First() != '9' && id.First() != '8')
            {
                return Redirect("/Home/Index");
            }
            var gIAO_VIEN = db.GIAO_VIEN.Where(p => p.TRANG_THAI == true);
            return View(gIAO_VIEN.OrderBy(m => m.HO_TEN).ToList());
        }
        public ActionResult ResetPass(string id)
        {
            TAI_KHOAN tAI_KHOAN = db.TAI_KHOAN.Find(id);
            tAI_KHOAN.MAT_KHAU = "123";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: Admin/GIAO_VIEN/Create
        public ActionResult Create()
        {
           
            return View();
        }
        //====================thay chỗ này==================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection f)
        {
            if (ModelState.IsValid)
            {
                GIAO_VIEN gv = new GIAO_VIEN();
                var ma_gv = db.GIAO_VIEN.Find("1000000001");
                if (ma_gv == null)
                    gv.MA_GV = "1000000001";
                else
                {
                    int ma = int.Parse(db.GIAO_VIEN.Select(m => m.MA_GV).ToList().Last()) + 1;
                    gv.MA_GV = ma.ToString();
                }
                gv.HO_TEN = f["name"];
                gv.SDT = f["SDT"];
                gv.NG_SINH = Convert.ToDateTime(f["ngaysinh"]);
                gv.GIOI_TINH = f["gioitinh"];
                gv.EMAIL = f["email"];
                gv.TRANG_THAI = true;
                db.GIAO_VIEN.Add(gv);
                bASE.create_TAI_KHOAN(gv.MA_GV,f["SDT"]);
                db.SaveChanges();
                return RedirectToAction("Index", "GIAO_VIEN", new { area = "Admin" });
            }
            return View();
        }
        //=======================Kết thúc đoạn thay===============================
        // GET: Admin/GIAO_VIEN/Edit/5
        public ActionResult Edit(string id)
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
        //===============================================
        // POST: Admin/GIAO_VIEN/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection f)
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
                return RedirectToAction("Index");
            }
            return View();
        }
        // POST: Admin/GIAO_VIEN/Delete/5

        public ActionResult Delete(string id)
        {
            //====================thay chỗ này==================
            GIAO_VIEN gIAO_VIEN = db.GIAO_VIEN.Find(id);
            gIAO_VIEN.TRANG_THAI = false;
            //====================kết thúc chỗ thay==================
            TAI_KHOAN tAI_KHOAN = db.TAI_KHOAN.Find(id);
            db.TAI_KHOAN.Remove(tAI_KHOAN);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //=================================================
        [HttpGet]
        public JsonResult checkTaiKhoan(string id)
        {
            string data = "0";
            var check = db.TAI_KHOAN.Where(p => p.TEN.Equals(id)).Count();
            if (check > 0)
            {
                data = "1";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //=================================================
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
