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
    public class LOAI_LOPController : Controller
    {
        private QL_TRUNGTAM1Entities db = new QL_TRUNGTAM1Entities();

        // GET: Admin/LOAI_LOP
        public ActionResult Index()
        {
            if (Session["ID"] == null)
                return Redirect("/Home/Index");
            var id = Session["ID"].ToString();
            if (id.First() != '9')
            {
                return Redirect("/Home/Index");
            }
            return View(db.LOAI_LOP.ToList());
        }

        // GET: Admin/LOAI_LOP/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAI_LOP lOAI_LOP = db.LOAI_LOP.Find(id);
            if (lOAI_LOP == null)
            {
                return HttpNotFound();
            }
            return View(lOAI_LOP);
        }

        // GET: Admin/LOAI_LOP/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/LOAI_LOP/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "MA_LOAI,TEN_LOAI")] LOAI_LOP lOAI_LOP)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        lOAI_LOP.MA_LOAI = Guid.NewGuid();
        //        db.LOAI_LOP.Add(lOAI_LOP);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(lOAI_LOP);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection f)
        {
            if(ModelState.IsValid)
            {
                LOAI_LOP lOAI_LOP = new LOAI_LOP();
                lOAI_LOP.MA_LOAI = Guid.NewGuid();
                lOAI_LOP.TEN_LOAI = f["tenloai"];
                lOAI_LOP.SI_SO = int.Parse(f["siso"]);
                db.LOAI_LOP.Add(lOAI_LOP);
                db.SaveChanges();
                return RedirectToAction("Index", "LOAI_LOP", new { area = "Admin" });
            }
            return View();
        }
        // GET: Admin/LOAI_LOP/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAI_LOP lOAI_LOP = db.LOAI_LOP.Find(id);
            if (lOAI_LOP == null)
            {
                return HttpNotFound();
            }
            return View(lOAI_LOP);
        }

        // POST: Admin/LOAI_LOP/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MA_LOAI,TEN_LOAI,SI_SO")] LOAI_LOP lOAI_LOP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lOAI_LOP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lOAI_LOP);
        }

        // GET: Admin/LOAI_LOP/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    LOAI_LOP lOAI_LOP = db.LOAI_LOP.Find(id);
        //    if (lOAI_LOP == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(lOAI_LOP);
        //}

        // POST: Admin/LOAI_LOP/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Delete(string maloai)
        {
            Guid ma = Guid.Parse(maloai);
           
            var kiemtra = (from b in db.BANG_GIA_HOC_PHI
                           where b.MA_LOAI == ma
                           select b).Count();
            string a = "0";
            if (kiemtra == 0)
            {
                LOAI_LOP lOAI_LOP = db.LOAI_LOP.Find(ma);
                db.LOAI_LOP.Remove(lOAI_LOP);
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
