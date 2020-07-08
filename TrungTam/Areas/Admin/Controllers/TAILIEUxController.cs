using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrungTam.Areas.Admin.Models;
using TrungTam.Areas.Admin.Abstracts;
using PagedList;
namespace TrungTam.Areas.Admin.Controllers
{
    public class TAILIEUxController : Controller
    {
        private QL_TRUNGTAM1Entities db = new QL_TRUNGTAM1Entities();
        // GET: Admin/TAILIEU
        public ActionResult Index(int page = 1, int pageSize = 7)
        {
            if (Session["ID"] == null)
                return Redirect("/Home/Index");
            var id = Session["ID"].ToString();
            var monhoc = db.MON_HOC;
            ViewBag.monhoc = monhoc.ToList();
            var tAILIEUx = db.TAILIEUx.Include(t => t.MON_HOC).OrderBy(p => p.KHOI);
            return View(tAILIEUx.ToPagedList(page, pageSize));
        }
        public ActionResult Detail(int page = 1, int pageSize = 7)
        {
            if (Session["ID"] == null)
                return Redirect("/Home/Index");
            var id = Session["ID"].ToString();
            var monhoc = db.MON_HOC;
            ViewBag.monhoc = monhoc.ToList();
            var tAILIEUx = db.TAILIEUx.Include(t => t.MON_HOC).OrderBy(p => p.KHOI);
            return View(tAILIEUx.ToPagedList(page, pageSize));
        }
        // POST: Admin/TAILIEU/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(FormCollection f)
        {
            TAILIEU tAILIEU = new TAILIEU();
            tAILIEU.MATL = Guid.NewGuid();
            tAILIEU.TENTL = f["name"];
            tAILIEU.LINK = f["link"];
            tAILIEU.MONHOC = Guid.Parse(f["monhoc"]);
            tAILIEU.KHOI = int.Parse(f["khoi"]);
            db.TAILIEUx.Add(tAILIEU);
            db.SaveChanges();
            return RedirectToAction("Index", "TAILIEUx", new { area = "Admin" });
        }

        // GET: Admin/TAILIEU/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAILIEU tAILIEU = db.TAILIEUx.Find(id);
            if (tAILIEU == null)
            {
                return HttpNotFound();
            }
            ViewBag.MONHOC = new SelectList(db.MON_HOC, "MA_MON", "TEN_MON", tAILIEU.MONHOC);
            return View(tAILIEU);
        }

        // POST: Admin/TAILIEU/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MATL,TENTL,LINK,MONHOC,KHOI")] TAILIEU tAILIEU)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tAILIEU).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MONHOC = new SelectList(db.MON_HOC, "MA_MON", "TEN_MON", tAILIEU.MONHOC);
            return View(tAILIEU);
        }
        // POST: Admin/TAILIEU/Delete/5
        [HttpGet]
        public ActionResult LoadKhoi (string khoi, string mon)
        {
            List<tailieu> tailieu = null;
            if (khoi != "0")
            {
                int khoii = int.Parse(khoi);
                if (mon.Equals("0"))
                {
                    var tailieuz = from p in db.TAILIEUx
                                   where p.KHOI == khoii
                                   select new tailieu
                                   {
                                       MATL = p.MATL,
                                       TENTL = p.TENTL,
                                       MONHOC = p.MON_HOC.TEN_MON,
                                       LINK = p.LINK,
                                       KHOI = p.KHOI
                                   };
                    tailieu = tailieuz.ToList();
                }
                else
                {
                    Guid monn = Guid.Parse(mon);
                    var tailieuz = from p in db.TAILIEUx
                                   where p.MONHOC == monn && p.KHOI == khoii
                                   select new tailieu
                                   {
                                       MATL = p.MATL,
                                       TENTL = p.TENTL,
                                       MONHOC = p.MON_HOC.TEN_MON,
                                       LINK = p.LINK,
                                       KHOI = p.KHOI
                                   };
                    tailieu = tailieuz.ToList();
                }
            }
            else 
            {
                if (mon != "0")
                {
                    Guid monn = Guid.Parse(mon);
                    var tailieuz = from p in db.TAILIEUx
                                   where p.MONHOC == monn
                                   select new tailieu
                                   {
                                       MATL = p.MATL,
                                       TENTL = p.TENTL,
                                       MONHOC = p.MON_HOC.TEN_MON,
                                       LINK = p.LINK,
                                       KHOI = p.KHOI
                                   };
                    tailieu = tailieuz.ToList();
                }
                else
                {
                    var tailieuz = from p in db.TAILIEUx
                                   select new tailieu
                                   {
                                       MATL = p.MATL,
                                       TENTL = p.TENTL,
                                       MONHOC = p.MON_HOC.TEN_MON,
                                       LINK = p.LINK,
                                       KHOI = p.KHOI
                                   };
                    tailieu = tailieuz.ToList();
                }
            }
            
            return Json(tailieu, JsonRequestBehavior.AllowGet);
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
