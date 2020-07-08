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
    public class KHOI_LOPController : Controller
    {
        private QL_TRUNGTAM1Entities db = new QL_TRUNGTAM1Entities();

        // GET: Admin/KHOI_LOP
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            if (Session["ID"] == null)
                return Redirect("/Home/Index");
            var id = Session["ID"].ToString();
            if (id.First() != '9')
            {
                return Redirect("/Home/Index");
            }
            return View(db.KHOI_LOP.OrderByDescending(m=> m.TEN_KHOI).ToPagedList(page, pageSize));
        }

        // GET: Admin/KHOI_LOP/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHOI_LOP kHOI_LOP = db.KHOI_LOP.Find(id);
            if (kHOI_LOP == null)
            {
                return HttpNotFound();
            }
            return View(kHOI_LOP);
        }

        // GET: Admin/KHOI_LOP/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/KHOI_LOP/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "MA_KHOI,TEN_KHOI")] KHOI_LOP kHOI_LOP)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        kHOI_LOP.MA_KHOI = Guid.NewGuid();
        //        db.KHOI_LOP.Add(kHOI_LOP);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(kHOI_LOP);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection f)
        {
            if (ModelState.IsValid)
            {
                KHOI_LOP kl = new KHOI_LOP();
                kl.MA_KHOI = Guid.NewGuid();
                kl.TEN_KHOI = f["tenkhoi"];
                db.KHOI_LOP.Add(kl);
                db.SaveChanges();
                return RedirectToAction("Index", "KHOI_LOP", new { area = "Admin" });
            }
            return View();
        }
        // GET: Admin/KHOI_LOP/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHOI_LOP kHOI_LOP = db.KHOI_LOP.Find(id);
            if (kHOI_LOP == null)
            {
                return HttpNotFound();
            }
            return View(kHOI_LOP);
        }

        // POST: Admin/KHOI_LOP/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MA_KHOI,TEN_KHOI")] KHOI_LOP kHOI_LOP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kHOI_LOP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kHOI_LOP);
        }

        // GET: Admin/KHOI_LOP/Delete/5
        [HttpPost]
        public ActionResult Delete(string makhoi)
        {
            Guid ma = Guid.Parse(makhoi);
            var kiemtra = (from l in db.BANG_GIA_HOC_PHI
                           where l.MA_KHOI == ma
                           select l).Count();
            string a = "0";
            if (kiemtra == 0)
            {
                KHOI_LOP kHOI_LOP = db.KHOI_LOP.Find(ma);
                db.KHOI_LOP.Remove(kHOI_LOP);
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
