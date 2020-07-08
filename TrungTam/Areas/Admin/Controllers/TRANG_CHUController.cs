using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrungTam.Areas.Admin.Controllers;
using TrungTam.Areas.Admin.Models;
using TrungTam.Function_Base;
using TrungTam.Areas.Admin.Abstracts;
using System.IO;

namespace TrungTam.Areas.Admin.Controllers
{
    public class TRANG_CHUController : Controller
    {
        private QL_TRUNGTAM1Entities db = new QL_TRUNGTAM1Entities();
        public ActionResult Index()
        {
            if (Session["ID"] == null)
                return Redirect("/Home/Index");
            var id = Session["ID"].ToString();
            if (id.First() != '9')
            {
                return Redirect("/Home/Index");
            }
            var noidung = db.TRANG_CHU.OrderByDescending(p => p.NGAY_AP_DUNG);
            ViewBag.noidung = noidung.ToList();
            var khuyenmai = db.KHUYEN_MAI.OrderBy(p => p.SO_MON_DK);
            ViewBag.khuyenmai = khuyenmai.ToList();
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult updateFooter(FormCollection f)
        {
            DateTime time = DateTime.Parse("2019-08-28 00:00:00.000");
            TRANG_CHU trangchu = db.TRANG_CHU.Find(time);
            trangchu.DIA_CHI = f["diachi"];
            trangchu.EMAIL = f["email"];
            trangchu.SDT1 = f["sdt1"];
            trangchu.SDT2 = f["sdt2"];
            db.SaveChanges();
            return RedirectToAction("Index", "TRANG_CHU", new { area = "Admin" });
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult updateMucTieu(FormCollection f)
        {
            DateTime time = DateTime.Parse("2019-08-28 00:00:00.000");
            TRANG_CHU trangchu = db.TRANG_CHU.Find(time);
            trangchu.MUC_TIEU = f["muctieu"];
            db.SaveChanges();
            return RedirectToAction("Index", "TRANG_CHU", new { area = "Admin" });
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult updateGioiThieu(FormCollection f)
        {
            DateTime time = DateTime.Parse("2019-08-28 00:00:00.000");
            TRANG_CHU trangchu = db.TRANG_CHU.Find(time);
            trangchu.GIOI_THIEU = f["gioithieu"];
            db.SaveChanges();
            return RedirectToAction("Index", "TRANG_CHU", new { area = "Admin" });
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ChangeImage(FormCollection f, HttpPostedFileBase p)
        {
            //for(int i = 0; i < Request.Files.Count; i++)
            //{
            //    HttpPostedFileBase x = Request.Files[i];
            //}         
            HttpPostedFileBase tv10 = Request.Files["tv10"];
            if (tv10 != null && tv10.ContentLength > 0)
            {
                //string pathSum = path + tv10.FileName;
                tv10.SaveAs(Server.MapPath("~/Asset/admin/img/" + "tv10.jpg"));
            }
            HttpPostedFileBase tv11 = Request.Files["tv10"];
            if (tv11 != null && tv11.ContentLength > 0)
            {
                //string pathSum = path + tv10.FileName;
                tv11.SaveAs(Server.MapPath("~/Asset/admin/img/" + "tv11.jpg"));
            }
            HttpPostedFileBase tv12 = Request.Files["tv12"];
            if (tv12 != null && tv12.ContentLength > 0)
            {
                //string pathSum = path + tv10.FileName;
                tv12.SaveAs(Server.MapPath("~/Asset/admin/img/" + "tv12.jpg"));
            }
            HttpPostedFileBase tv8 = Request.Files["tv8"];
            if (tv8 != null && tv8.ContentLength > 0)
            {
                //string pathSum = path + tv10.FileName;
                tv8.SaveAs(Server.MapPath("~/Asset/admin/img/" + "tv8.jpg"));
            }
            HttpPostedFileBase tv6 = Request.Files["tv6"];
            if (tv6 != null && tv6.ContentLength > 0)
            {
                //string pathSum = path + tv10.FileName;
                tv6.SaveAs(Server.MapPath("~/Asset/admin/img/" + "tv6.jpg"));
            }
            HttpPostedFileBase that = Request.Files["that"];
            if (that != null && that.ContentLength > 0)
            {
                //string pathSum = path + tv10.FileName;
                that.SaveAs(Server.MapPath("~/Asset/admin/img/" + "that.jpg"));
            }
            return RedirectToAction("Index", "TRANG_CHU", new { area = "Admin" });
        }
    }
}