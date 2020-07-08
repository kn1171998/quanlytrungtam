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
namespace TrungTam.Areas.Admin.Controllers
{
    public class HOC_SINHController : Controller
    {
        private QL_TRUNGTAM1Entities db = new QL_TRUNGTAM1Entities();
        private BASE bASE = new BASE();
        // GET: Admin/HOC_SINH
        public ActionResult Index()
        {
            if (Session["ID"] == null)
                return Redirect("/Home/Index");
            var id = Session["ID"].ToString();
            if (id.First() != '9' && id.First() != '8')
            {
                return Redirect("/Home/Index");
            }
            var hOC_SINH = db.HOC_SINH.Where(p => p.TINH_TRANG == true);
            return View(hOC_SINH.OrderByDescending(m=>m.MA_HS).ToList());
        }
        public ActionResult ResetPass(string id)
        {
            TAI_KHOAN tAI_KHOAN = db.TAI_KHOAN.Find(id);
            tAI_KHOAN.MAT_KHAU = "123";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: Admin/HOC_SINH/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOC_SINH hOC_SINH = db.HOC_SINH.Find(id);
            if (hOC_SINH == null)
            {
                return HttpNotFound();
            }
            return View(hOC_SINH);
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
        // GET: Admin/HOC_SINH/Create
        public ActionResult Create()
        {
            return View();
        }
        //=========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection f)
        {
            if (ModelState.IsValid)
            {
                HOC_SINH hs = new HOC_SINH();
                var MA_HS = db.HOC_SINH.Find("2000000001");
                if (MA_HS == null)
                    hs.MA_HS = "2000000001";
                else
                {
                    int ma = int.Parse(db.HOC_SINH.Select(m => m.MA_HS).ToList().Last()) + 1;
                    hs.MA_HS = ma.ToString();
                }
                hs.HO_TEN = f["name"];
                hs.SDT = f["SDT"];
                hs.NG_SINH = Convert.ToDateTime(f["ngaysinh"]);
                hs.GIOI_TINH = f["Gioitinh"];
                hs.MON_DK = f["mondk"];
                hs.NG_VAO_HOC = DateTime.Now;
                hs.TRUONG = f["truong"];
                hs.PHU_HUYNH = f["phuhuynh"];
                hs.DIA_CHI = f["diachi"];
                hs.KHOI = int.Parse(f["khoi"]);
                hs.SDT_PH = f["sdt_ph"];
                hs.TINH_TRANG = true;
                db.HOC_SINH.Add(hs);
                bASE.create_TAI_KHOAN(hs.MA_HS,f["sdt_ph"]);
                db.SaveChanges();
                return RedirectToAction("Index", "HOC_SINH", new { area = "Admin" });
            }
            return View();
        }
        //=========================================================
        // GET: Admin/HOC_SINH/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOC_SINH hOC_SINH = db.HOC_SINH.Find(id);
            if (hOC_SINH == null)
            {
                return HttpNotFound();
            }
            return View(hOC_SINH);
        }

        // POST: Admin/HOC_SINH/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "MA_HS,HO_TEN,NG_SINH,GIOI_TINH,TINH_TRANG,KHOI,TRUONG,MON_DK,NG_VAO_HOC,SDT,DIA_CHI,PHU_HUYNH,SDT_PH")] HOC_SINH hOC_SINH)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(hOC_SINH).State = EntityState.Modified;
        //        TAI_KHOAN tAI_KHOAN = db.TAI_KHOAN.Find(hOC_SINH.MA_HS);
        //        tAI_KHOAN.TEN = hOC_SINH.SDT_PH.Trim();
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.MA_HS = new SelectList(db.TAI_KHOAN, "TEN", "MAT_KHAU", hOC_SINH.MA_HS);
        //    return View(hOC_SINH);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection f)
        {
            if (ModelState.IsValid)
            {
                HOC_SINH hOC_SINH = db.HOC_SINH.Find(f["mahs"]);
                hOC_SINH.GIOI_TINH = f["gioitinh"];
                hOC_SINH.HO_TEN = f["name"];
                hOC_SINH.KHOI = int.Parse(f["khoi"]);
                hOC_SINH.NG_SINH = DateTime.Parse(f["ngaysinh"]);
                hOC_SINH.NG_VAO_HOC = DateTime.Parse(f["ngaynhaphoc"]);
                hOC_SINH.SDT = f["sdt"].Trim();
                hOC_SINH.MON_DK = f["mondk"];
                hOC_SINH.TRUONG = f["truong"];
                hOC_SINH.PHU_HUYNH = f["phuhuynh"];
                hOC_SINH.SDT_PH = f["sdt_ph"].Trim();
                hOC_SINH.DIA_CHI = f["diachi"];
                TAI_KHOAN tAI_KHOAN = db.TAI_KHOAN.Find(hOC_SINH.MA_HS);
                tAI_KHOAN.TEN = f["sdt_ph"].Trim();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }


        // GET: Admin/HOC_SINH/Delete/5
        //[HttpPost]
        public ActionResult Delete(string id)
        {
            HOC_SINH hs = db.HOC_SINH.Find(id);
            hs.TINH_TRANG = false;
            TAI_KHOAN tAI_KHOAN = db.TAI_KHOAN.Find(id);
            db.TAI_KHOAN.Remove(tAI_KHOAN);
            var lophoccuahs = db.CT_LOP_HOC.Where(p => p.MA_HS.Equals(id));
            foreach (var item in lophoccuahs)
                db.CT_LOP_HOC.Remove(item);
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
