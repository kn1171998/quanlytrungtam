using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrungTam.Areas.Admin.Models;

namespace TrungTam.Areas.Admin.Controllers
{
    public class KHUYEN_MAIController : Controller
    {
        private QL_TRUNGTAM1Entities db = new QL_TRUNGTAM1Entities();

        // GET: Admin/KHUYEN_MAI
        public ActionResult Index()
        {
            if (Session["ID"] == null)
                return Redirect("/Home/Index");
            var id = Session["ID"].ToString();
            if (id.First() != '9')
            {
                return Redirect("/Home/Index");
            }
            return View(db.KHUYEN_MAI.OrderByDescending(p => p.SO_MON_DK).ToList());
        }
        
        //===================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection f)
        {
            KHUYEN_MAI km = new KHUYEN_MAI();
            km.MA_KM = Guid.NewGuid();
            km.TEN_KM = f["name"];
            km.SO_MON_DK = int.Parse(f["somon"]);
            km.TIEN_GIAM = int.Parse(f["phantramgiam"]);
            km.TRANG_THAI = true;
            db.KHUYEN_MAI.Add(km);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //===================================================
        // GET: Admin/KHUYEN_MAI/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHUYEN_MAI kHUYEN_MAI = db.KHUYEN_MAI.Find(id);
            if (kHUYEN_MAI == null)
            {
                return HttpNotFound();
            }
            return View(kHUYEN_MAI);
        }

        // POST: Admin/KHUYEN_MAI/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MA_KM,TEN_KM,SO_MON_DK,TIEN_GIAM,TRANG_THAI")] KHUYEN_MAI kHUYEN_MAI)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kHUYEN_MAI).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kHUYEN_MAI);
        }
        [HttpPost]
        public ActionResult Delete(string id = "")
        {
            string stt = "1";
            try
            {
                Guid ma = Guid.Parse(id);
                KHUYEN_MAI kHUYEN_MAI = db.KHUYEN_MAI.Find(ma);
                var check = db.CONG_NO.Count(p => p.MA_KM == ma);
                if (check > 0)
                    kHUYEN_MAI.TRANG_THAI = false;
                else
                    db.KHUYEN_MAI.Remove(kHUYEN_MAI);
                db.SaveChanges();
                return Json(stt, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                stt = "-1";
                return Json(stt, JsonRequestBehavior.AllowGet);
            }
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
