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
    public class MON_HOCController : Controller
    {
        private QL_TRUNGTAM1Entities db = new QL_TRUNGTAM1Entities();

        // GET: Admin/MON_HOC
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            if (Session["ID"] == null)
                return Redirect("/Home/Index");
            var id = Session["ID"].ToString();
            if (id.First() != '9')
            {
                return Redirect("/Home/Index");
            }
            return View(db.MON_HOC.OrderByDescending(m => m.TEN_MON).ToPagedList(page, pageSize));
        }

        // GET: Admin/MON_HOC/Details/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection f)
        {
            if (ModelState.IsValid)
            {
                MON_HOC mh = new MON_HOC();
                mh.MA_MON = Guid.NewGuid();
                mh.TEN_MON = f["tenmon"];
                db.MON_HOC.Add(mh);
                db.SaveChanges();
                return RedirectToAction("Index", "MON_HOC", new { area = "Admin" });
            }
            return View();
        }
        // GET: Admin/MON_HOC/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MON_HOC mON_HOC = db.MON_HOC.Find(id);
            if (mON_HOC == null)
            {
                return HttpNotFound();
            }
            return View(mON_HOC);
        }

        // POST: Admin/MON_HOC/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MA_MON,TEN_MON")] MON_HOC mON_HOC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mON_HOC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mON_HOC);
        }

        // GET: Admin/MON_HOC/Delete/5
        
        public ActionResult Delete(string mamon)
        {
            Guid ma = Guid.Parse(mamon);
            var kiemtra = (from b in db.BANG_GIA_HOC_PHI
                           where b.MA_MON == ma
                           select b).Count();
            string a = "0";
            if (kiemtra == 0)
            {
                MON_HOC mON_HOC = db.MON_HOC.Find(ma);
                db.MON_HOC.Remove(mON_HOC);
                db.SaveChanges();
                a = "1";
                return Json(a, JsonRequestBehavior.AllowGet);
            }
            return Json(a, JsonRequestBehavior.AllowGet);
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
