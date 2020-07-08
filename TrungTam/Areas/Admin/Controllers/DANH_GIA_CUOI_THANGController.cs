using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrungTam.Areas.Admin.Models;
using TrungTam.Areas.Admin.Abstracts;
using PagedList;
using Rotativa;
namespace TrungTam.Areas.Admin.Controllers
{
    public class DANH_GIA_CUOI_THANGController : Controller
    {
        private static QL_TRUNGTAM1Entities db;
        public static IEnumerable<ReportCuoiThang> quanque;
        //public static int chuyencan, tongbuoi;
        public static QL_TRUNGTAM1Entities getDBInstance()
        {
            if (db == null)
            {
                db = new QL_TRUNGTAM1Entities();
            }
            return db;
        }
        // GET: Admin/DANH_GIA_CUOI_THANG
        public ActionResult Index()
        {
            if (Session["ID"] == null)
                return Redirect("/Home/Index");
            var id = Session["ID"].ToString();
            if (id.First() != '1')
            {
                return Redirect("/Home/Index");
            }
            var chonlop = getDBInstance().LOP_HOC.Where(p => p.MA_GV.Equals(id) && p.TRANG_THAI == 0).OrderBy(p => p.NGAY_BAT_DAU).ToList();
            ViewBag.chonlop = chonlop;
            return View();
        }
        public ActionResult Index1(string id, string ngay)
        {
            if (Session["ID"] == null)
                return Redirect("/Homde/Index");
            var id1 = Session["ID"].ToString();
            if (id1.First() != '1')
            {
                return Redirect("/Home/Index");
            }
            int thang = int.Parse(ngay.Split('_')[0]);
            int nam = int.Parse(ngay.Split('_')[1]);
            if(id == "")
                return RedirectToAction("Index", "DANH_GIA_CUOI_THANG");
            Guid malop = Guid.Parse(id);
            var dsDanhGiaCuoiThang = getDBInstance().DANH_GIA_CUOI_THANG.Where(p => p.NGAY_LAP.Value.Month == thang && p.NGAY_LAP.Value.Year.Equals(nam) && p.MA_LOP == malop);
            var dsHocSinh = (from ct in getDBInstance().CT_LOP_HOC
                             where ct.MA_LOP == malop
                             select new dsDanhGiaCuoiThang
                             {
                                 mahs = ct.MA_HS,
                                 malop = id,
                                 hoten = ct.HOC_SINH.HO_TEN,
                                 dienthoai = ct.HOC_SINH.SDT,
                                 nhanxet = "",
                                 status = false
                             }).ToList();
            if (dsDanhGiaCuoiThang.Count() == 0)
                return Json(dsHocSinh, JsonRequestBehavior.AllowGet);
            
            var lstDanhGia =  (from dgct in getDBInstance().DANH_GIA_CUOI_THANG
                               where dgct.MA_LOP == malop
                               && dgct.NGAY_LAP.Value.Month == thang
                               && dgct.NGAY_LAP.Value.Year == nam
                               select new dsDanhGiaCuoiThang
                               {
                                    mahs = dgct.MA_HS,
                                    malop = id,
                                    hoten = dgct.HOC_SINH.HO_TEN,
                                    dienthoai = dgct.HOC_SINH.SDT,
                                    nhanxet = dgct.NHAN_XET,
                                    status = true
                                }).ToList();
            foreach(var item in dsHocSinh)
            {
               foreach(var meti in lstDanhGia)
                {
                    if(meti.mahs == item.mahs)
                    {
                        item.nhanxet = meti.nhanxet;
                        item.status = meti.status;
                    }    
                }    
            }
            return Json(dsHocSinh, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create1(FormCollection f)
        {
            var malop = Guid.Parse(f["lop"]);
            string str2 = "1";
            string str1 = f["thang"];
            string str0 = f["nam"];
            var chonlop = getDBInstance().CT_LOP_HOC.Where(p => p.MA_LOP.Equals(malop)).ToList();
            var ngaylap = DateTime.Parse(str1 + "-" + str2 + "-" + str0); //tháng trước ngày sau
            var dg = getDBInstance().DANH_GIA_CUOI_THANG.Where(p => p.NGAY_LAP.Value.Month == ngaylap.Month && p.NGAY_LAP.Value.Year.Equals(ngaylap.Year) && p.MA_LOP == malop).ToList();
            if (dg.Count() == 0)
            {
                foreach (var item in chonlop)
                {
                    DANH_GIA_CUOI_THANG dgct = new DANH_GIA_CUOI_THANG();
                    dgct.MA_DG = Guid.NewGuid();
                    dgct.NHAN_XET = f["nhanxet" + item.MA_HS];
                    dgct.NGAY_LAP = ngaylap;
                    dgct.MA_HS = f["mahs" + item.MA_HS];
                    dgct.MA_LOP = malop;
                    getDBInstance().DANH_GIA_CUOI_THANG.Add(dgct);
                }
                getDBInstance().SaveChanges();
                return RedirectToAction("Index", "DANH_GIA_CUOI_THANG");
            }
            foreach (var item in chonlop)
            {
                string a = "nhanxet" + item.MA_HS;
                var lst_dgct = dg.Where(p => p.MA_HS == item.MA_HS);
                if(lst_dgct.Count() > 0)
                {     
                    var dgct = lst_dgct.First();
                    var ca = Request.Form[a];
                    dgct.NHAN_XET = f[a];
                }
                else
                {
                    DANH_GIA_CUOI_THANG dgct = new DANH_GIA_CUOI_THANG();
                    dgct.MA_DG = Guid.NewGuid();
                    dgct.NHAN_XET = f[a];
                    dgct.NGAY_LAP = ngaylap;
                    dgct.MA_HS = f["mahs" + item.MA_HS];
                    dgct.MA_LOP = malop;
                    getDBInstance().DANH_GIA_CUOI_THANG.Add(dgct);
                }    
            }
            getDBInstance().SaveChanges();
            return RedirectToAction("Index", "DANH_GIA_CUOI_THANG");
        }
        public ActionResult IndexAdmin()
        {
            if (Session["ID"] == null)
                return Redirect("/Home/Index");
            var id = Session["ID"].ToString();
            if (id.First() != '9' && id.First() != '8')
            {
                return Redirect("/Home/Index");
            }
            var ds_hocvien = getDBInstance().HOC_SINH.Where(p => p.TINH_TRANG == true);
            return View(ds_hocvien.OrderByDescending(p => p.NG_VAO_HOC).ToList());
        }
        //public ActionResult PrintPayment(string)

        public ActionResult PrintReportMonth(string id, string date)
        {
            string[] str = date.Split('_');
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            //System.Globalization.CultureInfo current = System.Globalization.CultureInfo.CurrentCulture;
            //DateTime dat = Convert.ToDateTime(date, System.Globalization.CultureInfo.GetCultureInfo(current.Name).DateTimeFormat);
            string thang = str[0], nam = str[1];
            //var listmonho = getDBInstance().CT_BUOIHOC.Where(p =>
            //p.BUOI_HOC.THOI_GIAN.Month.Equals(thang) &&
            //p.BUOI_HOC.LOP_HOC.TRANG_THAI.Value.Equals(0) &&
            //p.BUOI_HOC.THOI_GIAN.Year.Equals(nam) &&
            //p.MA_HS.Equals(id)).Select(p => p).Distinct().ToList();

            //--------------------Phần cũ của Nhật--------------
            //var listmonhoc = (from ct in db.CT_BUOIHOC
            //                  join b in db.BUOI_HOC
            //                  on ct.MA_BUOI equals b.MA_BUOI
            //                  join l in db.LOP_HOC
            //                  on b.MA_LOP equals l.MA_LOP
            //                  where b.THOI_GIAN.Month.ToString().Equals(thang) &&
            //                 b.LOP_HOC.TRANG_THAI.Value.Equals(0) &&
            //                 b.THOI_GIAN.Year.ToString().Equals(nam) &&
            //                 ct.MA_HS.Equals(id)
            //                  select ct).Distinct().ToList();
            //---------------------Phần mới Ngọc chỉnh
            var listmonhoc = (from ct in db.CT_BUOIHOC
                              where ct.BUOI_HOC.THOI_GIAN.Month.ToString().Equals(thang) &&
                             ct.BUOI_HOC.LOP_HOC.TRANG_THAI.Value.Equals(0) &&
                             ct.BUOI_HOC.THOI_GIAN.Year.ToString().Equals(nam) &&
                             ct.MA_HS.Equals(id)
                              select new
                              {
                                  MA_LOP = ct.BUOI_HOC.MA_LOP,
                                  MA_BUOI = ct.MA_BUOI,
                                  MA_HS = ct.MA_HS,
                                  DIEM = ct.DIEM,
                                  NHAN_XET_GV = ct.NHAN_XET_GV,
                                  DIEM_DANH_HS = ct.DIEM_DANH_HS
                              }
                              ).Distinct().ToList();
            //-------------------------------------------
            var chuyencan = (from b in listmonhoc
                             group b by b.MA_LOP into xa
                             select new
                             {
                                 malop = xa.Key.ToString(),
                                 tong = xa.Count(),
                                 chuyencan = xa.Where(p => p.DIEM_DANH_HS.Value.Equals(1)).Count(),
                                 diemtb = xa.Average(p => p.DIEM)
                             }).ToList();
            //Array tongbuoi = (from b in listmonho
            //               group b by b.BUOI_HOC.MA_LOP into xa
            //               select new 
            //               {
            //                   malop = xa.Key.ToString(),
            //                   tong = xa.Count()
            //               }).ToArray();            
            var danhgia =
               (from dgct in getDBInstance().DANH_GIA_CUOI_THANG
                where dgct.MA_HS.Equals(id)
                && dgct.NGAY_LAP.Value.Month.ToString().Equals(thang)
                && dgct.NGAY_LAP.Value.Year.ToString().Equals(nam)
                select new ReportCuoiThang
                {
                    //chuyencan = 0,
                    //tongbuoi = 0,
                    malop = dgct.MA_LOP.ToString(),
                    nhanxet = dgct.NHAN_XET,
                    lop = dgct.LOP_HOC.TEN_LOP,
                    tengv = dgct.LOP_HOC.GIAO_VIEN.HO_TEN,
                    tenhs = dgct.HOC_SINH.HO_TEN,
                    tenmon = dgct.LOP_HOC.BANG_GIA_HOC_PHI.MON_HOC.TEN_MON
                }).ToList();
            foreach (var i in danhgia)
            {
                foreach (var a in chuyencan)
                {
                    if (a.malop == i.malop)
                    {
                        if (a.diemtb != null)
                        {
                            i.chuyencan = a.chuyencan;
                            i.tongbuoi = a.tong;
                            i.diemtb = (float)(Math.Floor(double.Parse(a.diemtb.ToString()) * 100) / 100);
                        }
                        else
                        {
                            i.chuyencan = a.chuyencan;
                            i.tongbuoi = a.tong;
                        }
                    }
                }
            }
            if (danhgia.Count() != 0)
            {
                quanque = danhgia;

            }
            else quanque = null;

            //return quanque != null ? RedirectToAction("Prints", "DANH_GIA_CUOI_THANG") : RedirectToAction("Index", "DANH_GIA_CUOI_THANG"); ;
            return RedirectToAction("Prints", "DANH_GIA_CUOI_THANG");
        }

        public ActionResult Prints()
        {
            return new ActionAsPdf("PrintAllReport", "DANH_GIA_CUOI_THANG");
        }

        public ActionResult PrintAllReport()
        {
            try
            {
                return View(quanque);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "DANH_GIA_CUOI_THANG");
            }
            //return RedirectToAction("Index", "DANH_GIA_CUOI_THANG");
        }
    }
}