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
    public class GIA_SUController : Controller
    {
        private QL_TRUNGTAM1Entities db = new QL_TRUNGTAM1Entities();

        // GET: Admin/GIA_SU
        public ActionResult Index()
        {
            if (Session["ID"] == null)
                return Redirect("/Home/Index");
            var id = Session["ID"].ToString();
            if (id.First() != '9' && id.First() != '8')
            {
                return Redirect("/Home/Index");
            }
            return View(db.GIA_SU.OrderBy(m => m.TEN_LOP).ToList());
        }
        public ActionResult tuyengiasu()
        {
            return View(db.GIA_SU.OrderBy(m => m.TEN_LOP).ToList());
        }
        // POST: Admin/GIA_SU/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(FormCollection f)
        {
            if (ModelState.IsValid)
            {
                GIA_SU gIA_SU = new GIA_SU();
                gIA_SU.MA_GIA_SU = Guid.NewGuid();
                gIA_SU.TEN_LOP = f["tenlop"];
                gIA_SU.MON_DAY = f["monhoc"];
                gIA_SU.THOI_GIAN = f["thoigian"];
                gIA_SU.DIA_CHI = f["diachi"];
                gIA_SU.DON_GIA = f["dongia"];
                gIA_SU.YEU_CAU = f["yeucau"];
                gIA_SU.LIEN_HE = f["lienhe"];
                db.GIA_SU.Add(gIA_SU);
                db.SaveChanges();
                return RedirectToAction("Index", "GIA_SU", new { area = "Admin" });
            }

            return View();
        }

        // GET: Admin/GIA_SU/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GIA_SU gIA_SU = db.GIA_SU.Find(id);
            if (gIA_SU == null)
            {
                return HttpNotFound();
            }
            return View(gIA_SU);
        }

        // POST: Admin/GIA_SU/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MA_GIA_SU,TEN_LOP,DON_GIA,DIA_CHI,MON_DAY,THOI_GIAN,YEU_CAU,LIEN_HE")] GIA_SU gIA_SU)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gIA_SU).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gIA_SU);
        }

        // GET: Admin/GIA_SU/Delete/5
      
        // POST: Admin/GIA_SU/Delete/5
        [HttpPost]
        public ActionResult Delete(string id)
        {
            Guid ma = Guid.Parse(id);
            GIA_SU gIA_SU = db.GIA_SU.Find(ma);
            db.GIA_SU.Remove(gIA_SU);
            db.SaveChanges();
            return RedirectToAction("Index");
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
