using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TrungTam.Areas.Admin.Abstracts;
using TrungTam.Areas.Admin.Models;
using TrungTam.Function_Base;
namespace TrungTam.Areas.Admin.Controllers
{
    public class LOP_HOCController : Controller
    {
        private QL_TRUNGTAM1Entities db = new QL_TRUNGTAM1Entities();
        private BASE bASE = new BASE();
        // GET: Admin/LOP_HOC

        public ActionResult Index()
        {
            if (Session["ID"] == null)
                return Redirect("/Home/Index");
            var id = Session["ID"].ToString();
            if (id.First() != '9' && id.First() != '8')
            {
                return Redirect("/Home/Index");
            }
            var lOP_HOC = from l in db.LOP_HOC.Include(l => l.GIAO_VIEN).Include(n => n.BANG_GIA_HOC_PHI)
                          join k in db.KHOI_LOP
                          on l.BANG_GIA_HOC_PHI.MA_KHOI equals k.MA_KHOI
                          join ll in db.LOAI_LOP
                          on l.BANG_GIA_HOC_PHI.MA_LOAI equals ll.MA_LOAI
                          join m in db.MON_HOC
                          on l.BANG_GIA_HOC_PHI.MA_MON equals m.MA_MON
                          select new LopHoc_New
                          {
                              malop = l.MA_LOP,
                              tenlop = l.TEN_LOP,
                              siso = l.SI_SO,
                              hoten = l.GIAO_VIEN.HO_TEN,
                              tenkhoi = k.TEN_KHOI,
                              tenloai = ll.TEN_LOAI,
                              tenmon = m.TEN_MON,
                              trangthai = l.TRANG_THAI,
                              ngayketthuc = l.NGAY_KET_THUC,
                              ngaymolop = l.NGAY_MO_LOP,
                              ngayhoc = l.NGAY_BAT_DAU
                          };
            return View(lOP_HOC.OrderByDescending(l => l.trangthai).ToList());
        }
        //Chi ti?t h?c sinh trong l?p c?a View Details
        // GET: Admin/LOP_HOC/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOP_HOC lOP_HOC = db.LOP_HOC.Find(id);
            if (lOP_HOC == null)
            {
                return HttpNotFound();
            }
            else
            {
                var hocsinh = (from a in db.LOP_HOC
                               join b in db.CT_LOP_HOC
                               on a.MA_LOP equals b.MA_LOP
                               join c in db.HOC_SINH
                               on b.MA_HS equals c.MA_HS
                               where a.MA_LOP == lOP_HOC.MA_LOP
                               select new Hoc_Sinh()
                               {
                                   mahs = c.MA_HS.ToString(),
                                   hoten = c.HO_TEN,
                                   ngaysinh = c.NG_SINH,
                                   gioitinh = c.GIOI_TINH,
                                   sdt = c.SDT_PH,
                                   phuhuynh = c.PHU_HUYNH
                               }).OrderByDescending(l => l.hoten).ToList();

                if (hocsinh.Count() > 0)
                    ViewBag.list_hocsinh = hocsinh;
                else
                    ViewBag.list_hocsinh = null;
                var thoikhoabieu = (from tkb in db.THOI_KHOA_BIEU
                                    where tkb.MA_LOP == lOP_HOC.MA_LOP
                                    select new ThoiKhoaBieu
                                    {
                                        matkb = tkb.MA_TKB,
                                        thu = tkb.THU,
                                        tgbt = tkb.THOI_GIAN_BD,
                                        tgkt = tkb.THOI_GIAN_KT
                                    }).ToList();

                ViewBag.listTKB = thoikhoabieu;
            }
            return View(lOP_HOC);
        }
        //GET: LOP_HOC/ListName
        //List g?i ý ? View Detail
        public JsonResult ListName(string q)
        {
            var data = (from a in db.HOC_SINH
                        where a.HO_TEN.Contains(q) && a.TINH_TRANG != false
                        select a.HO_TEN).Distinct().ToList();
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        [AllowAnonymous]
        public JsonResult Search(string search)
        {
            var malop = Guid.Parse(search.Substring(0, 36));
            var ten = search.Substring(36, search.Length - 36);
            //var dem = (from a in db.HOC_SINH
            //           join b in db.CT_LOP_HOC
            //           on a.MA_HS equals b.MA_HS
            //           join c in db.LOP_HOC
            //           on b.MA_LOP equals c.MA_LOP
            //           where a.HO_TEN == ten && c.MA_LOP == malop
            //           select a).ToList();
            //if (dem.Count() == 0)
            //{
                var data = (from a in db.HOC_SINH
                            where a.HO_TEN.Contains(ten) && a.TINH_TRANG != false
                            select new
                            {
                                mahs = a.MA_HS,
                                hoten = a.HO_TEN,
                                sdt = a.SDT_PH
                            }).Distinct().OrderByDescending(l => l.hoten).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            //}
            //return Json(null);
        }
        //Edit thời khoá biểu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditThoiKhoaBieu(FormCollection f)
        {
            string malop = f["malop"];
            List<THOI_KHOA_BIEU> tkb = db.THOI_KHOA_BIEU.Where(m => m.MA_LOP.ToString() == malop).ToList();
            foreach (var a in tkb)
            {
                string thu = a.THU.ToString();
                string tgbd = a.THOI_GIAN_BD.ToString();
                //string tgkt =a.THOI_GIAN_KT
                a.THU = int.Parse(f["thu_" + thu + tgbd + a.MA_TKB.ToString()]);
                a.THOI_GIAN_BD = TimeSpan.Parse(f["tgbd_" + thu + tgbd + a.MA_TKB].ToString());
                a.THOI_GIAN_KT = TimeSpan.Parse(f["tgkt_" + thu + tgbd + a.MA_TKB]);
            }
            db.SaveChanges();
            return RedirectToAction("Details/" + malop, "LOP_HOC");
        }
        int GetDays(DateTime startDate,List<int> tkb)
        {
            int count = 0;
            for (DateTime currentDate = startDate; currentDate <= GetEndDate(startDate); currentDate = currentDate.AddDays(1))
            {
                if (tkb.Contains(bASE.DayOfWeekk(currentDate.ToShortDateString())))
                {
                    count++;
                }
            }
            return count;
        }
        DateTime GetEndDate(DateTime date)
        {
            if (date.Month == 12)
            {
                // nếu là tháng 12 thì trả về ngày 31
                return new DateTime(date.Year, date.Month, 31);
            }
            // chuyển tới ngày đầu tiên của tháng kế tiếp
            DateTime tem = new DateTime(date.Year, date.Month + 1, 1);
            // lùi lại 1 ngày là về ngày cuối tháng của tháng hiện tại.
            return tem.AddDays(-1);
        }
        //Thêm h?c sinh vào trong table chi ti?t
        //GET: LOP_HOC/Add     
        [HttpPost]
        [AllowAnonymous]
        public JsonResult Add(string tong)
        {

            var mahs = tong.Substring(0, 10);
            var malop = Guid.Parse(tong.Substring(10, tong.Length - 10));
            DateTime currDay = DateTime.Now;
            //int kiemtracongno = db.CONG_NO.Where(p=>p.NGAY_LAP_CONG_NO.Month.Equals(currDay.Month) && p.NGAY_LAP_CONG_NO.Year.Equals(currDay.Year)).Include(p=>p.CT_CONG_NO.Where(x=>x.MA_LOP.Equals(malop))).Count();
            //int kiemtrano = db.CT_CONG_NO.Where(p => p.CONG_NO.NGAY_LAP_CONG_NO.Month.Equals(currDay.Month) && p.CONG_NO.NGAY_LAP_CONG_NO.Year.Equals(currDay.Year) && p.MA_LOP.Equals(malop)).Count();
            var statusClass = db.LOP_HOC.Find(malop).TRANG_THAI;
            //if (kiemtrano > 0)
            //{
            if (statusClass == 0 || statusClass == 1)
            {
                if(db.CT_LOP_HOC.Where(t=>t.MA_HS.Equals(mahs) && t.MA_LOP.Equals(malop)).Count() > 0)
                {
                    tong = "0";//thông báo đã tồn tại trong lớp
                    return Json(tong, JsonRequestBehavior.AllowGet);
                }
                //=================================
                var chonlop = db.LOP_HOC.Where(p => p.TRANG_THAI != -1).Join(db.THOI_KHOA_BIEU, lp => lp.MA_LOP, tkb => tkb.MA_LOP,
                          (lp, tkb) => new THOI_KHOA_BIEU_LOP_HOC
                          {
                              MA_LOP = lp.MA_LOP,
                              TEN_LOP = lp.TEN_LOP,
                              SI_SO = lp.SI_SO,
                              MA_GV = lp.MA_GV,
                              THU = tkb.THU,
                              THOI_GIAN_BD = tkb.THOI_GIAN_BD.ToString(),
                              THOI_GIAN_KT = tkb.THOI_GIAN_KT.ToString()
                          });
                var thoikhoabieuhs = (db.CT_LOP_HOC.Where(p => p.MA_HS.Equals(mahs)).Join(chonlop
                   , tkb => tkb.MA_LOP, lp => lp.MA_LOP, (tkb, lp) => new THOI_KHOA_BIEU_LOP_HOC
                   {
                       MA_LOP = lp.MA_LOP,
                       TEN_LOP = lp.TEN_LOP,
                       SI_SO = lp.SI_SO,
                       MA_GV = lp.MA_GV,
                       THU = lp.THU,
                       THOI_GIAN_BD = lp.THOI_GIAN_BD,
                       THOI_GIAN_KT = lp.THOI_GIAN_KT
                   }).OrderBy(p => p.THOI_GIAN_BD)).ToList();
                var thoikhoabieulop = db.LOP_HOC.Where(p => p.TRANG_THAI != -1).Where(p => p.MA_LOP.Equals(malop)).Join(chonlop
                    , tkb => tkb.MA_LOP, lp => lp.MA_LOP, (tkb, lp) => new THOI_KHOA_BIEU_LOP_HOC
                    {
                        MA_LOP = lp.MA_LOP,
                        TEN_LOP = lp.TEN_LOP,
                        SI_SO = lp.SI_SO,
                        MA_GV = lp.MA_GV,
                        THU = lp.THU,
                        THOI_GIAN_BD = lp.THOI_GIAN_BD,
                        THOI_GIAN_KT = lp.THOI_GIAN_KT
                    }).OrderBy(p => p.THOI_GIAN_BD);
                foreach (var item in thoikhoabieuhs)
                {
                    foreach (var meti in thoikhoabieulop)
                    {
                        if (item.THU.Equals(meti.THU) && ((TimeSpan.Compare(TimeSpan.Parse(item.THOI_GIAN_BD), TimeSpan.Parse(meti.THOI_GIAN_BD)) >= 0 && TimeSpan.Compare(TimeSpan.Parse(item.THOI_GIAN_BD), TimeSpan.Parse(meti.THOI_GIAN_KT)) <= 0) || (TimeSpan.Compare(TimeSpan.Parse(item.THOI_GIAN_KT), TimeSpan.Parse(meti.THOI_GIAN_KT)) <= 0 && TimeSpan.Compare(TimeSpan.Parse(item.THOI_GIAN_KT), TimeSpan.Parse(meti.THOI_GIAN_BD)) >= 0)))
                        {
                            tong = meti.TEN_LOP;
                            return Json(tong, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                //==================================
                List<int> list_tkb = db.THOI_KHOA_BIEU.Where(p => p.MA_LOP.Equals(malop)).Select(p => p.THU).ToList();
                var ngay = DateTime.Now;
                int sobuoiconlai = GetDays(DateTime.Now, list_tkb);
                int sobuoicathang = GetDays(DateTime.Parse(ngay.Month + "/1/" + ngay.Year), list_tkb);
                //var tongbuoi1 = db.LOP_HOC.Find(malop).BANG_GIA_HOC_PHI.SO_BUOI * 4; //tổng số buổi học lấy sobuoihoc * 4tuan
                                                                                     /*lớp học được bao nhiêu buổi*/
                                                                                     //var sobuoihientai = db.BUOI_HOC.Where(p => p.MA_LOP.Equals(malop) && p.THOI_GIAN.Month.Equals(ngay.Month) && p.THOI_GIAN.Year.Equals(ngay.Year)).Count();
                //var sobuoihientai1 = db.LOP_HOC.Find(malop).BUOI_HOC.Where(p => p.THOI_GIAN.Month.Equals(ngay.Month) && p.THOI_GIAN.Year.Equals(ngay.Year)).Count();
                //===================================
                var dongia = db.LOP_HOC.Find(malop).BANG_GIA_HOC_PHI.DON_GIA;
                /*  (dongia / tongbuoi) * (tongbuoi - sobuoihientai)  */
                var tiendong = Math.Ceiling((double.Parse(dongia.ToString()) / sobuoicathang) * sobuoiconlai);

                var congno = db.CT_CONG_NO.Where(p => p.CONG_NO.NGAY_LAP_CONG_NO.Month.Equals(currDay.Month) && p.CONG_NO.NGAY_LAP_CONG_NO.Year.Equals(currDay.Year) && p.CONG_NO.MA_HS.Equals(mahs) && p.CONG_NO.TRANG_THAI == false).ToList();
                CONG_NO cONG_NO = null;
                if (congno.Count() > 0)
                {
                    cONG_NO = db.CONG_NO.Find(congno[0].MA_CONG_NO);
                }
                else
                {
                    cONG_NO = new CONG_NO();
                    cONG_NO.MA_CONG_NO = Guid.NewGuid();
                    cONG_NO.NGAY_LAP_CONG_NO = DateTime.Now;
                    cONG_NO.MA_HS = mahs;
                    cONG_NO.TRANG_THAI = false;
                    db.CONG_NO.Add(cONG_NO);
                }

                CT_LOP_HOC ct_lop = new CT_LOP_HOC();
                ct_lop.MA_HS = mahs;
                ct_lop.MA_LOP = malop;
                db.CT_LOP_HOC.Add(ct_lop);
                CT_CONG_NO cT_CONG_NO = new CT_CONG_NO();
                cT_CONG_NO.MA_CONG_NO = cONG_NO.MA_CONG_NO;
                cT_CONG_NO.MA_LOP = malop;
                cT_CONG_NO.THANH_TIEN = decimal.Parse(tiendong.ToString());

                db.CT_CONG_NO.Add(cT_CONG_NO);
                tong = "1"; //thống báo thêm thành công
                db.SaveChanges();

            }
            else if (statusClass == -1)
            {
                tong = "-1"; //thông báo lớp kết thúc
            }
            //else
            //{
            //    CT_LOP_HOC ct_lop = new CT_LOP_HOC();
            //    ct_lop.MA_HS = mahs;
            //    ct_lop.MA_LOP = malop;
            //    db.CT_LOP_HOC.Add(ct_lop);
            //    db.SaveChanges();
            //}
            //}
            //else
            //{
            //    CT_LOP_HOC ct_lop = new CT_LOP_HOC();
            //    ct_lop.MA_HS = mahs;
            //    ct_lop.MA_LOP = malop;
            //    db.CT_LOP_HOC.Add(ct_lop);
            //    db.SaveChanges();
            //}

            return Json(tong, JsonRequestBehavior.AllowGet);
        }
        //POST: Admin/LOP_HOC/Create
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.    
        //Truy?n d? li?u qua View Create
        public ActionResult Create()
        {
            var khoi = (from a in db.KHOI_LOP
                        join b in db.BANG_GIA_HOC_PHI
                        on a.MA_KHOI equals b.MA_KHOI
                        select new HocPhi_Khoi()
                        {
                            MA_KHOI = a.MA_KHOI,
                            TEN_KHOI = a.TEN_KHOI
                        }).Distinct().ToList();
            if (khoi.Count() > 0)
                ViewBag.listkhoi = khoi.ToList();
            else
                ViewBag.listkhoi = null;
            var banghp = (from a in db.BANG_GIA_HOC_PHI
                          join b in db.MON_HOC
                          on a.MA_MON equals b.MA_MON
                          join c in db.LOAI_LOP
                          on a.MA_LOAI equals c.MA_LOAI
                          join d in db.KHOI_LOP
                          on a.MA_KHOI equals d.MA_KHOI
                          select new BangHP_Khoi_Loai_Mon
                          {
                              NGAYAD = a.NGAY_AP_DUNG,
                              MA_KHOI = d.MA_KHOI,
                              TEN_KHOI = d.TEN_KHOI,
                              MA_MON = b.MA_MON,
                              TEN_MON = b.TEN_MON,
                              MA_LOAI = c.MA_LOAI,
                              TEN_LOAI = c.TEN_LOAI,
                              DON_GIA = a.DON_GIA,
                              SO_BUOI = a.SO_BUOI
                          }).ToList();
            //ViewBag.listmonhoc = monhoc.ToList();
            var listgiaovien = db.GIAO_VIEN.Where(p => p.TRANG_THAI.Equals(true)).OrderBy(m => m.HO_TEN).ToList();
            if (listgiaovien.Count() > 0)
                ViewBag.listgiaovien = listgiaovien;
            else
                ViewBag.listgiaovien = listgiaovien;
            if (banghp.Count() > 0)
                ViewBag.listbanghp = banghp;
            else
                ViewBag.listbanghp = null;
            return View();
        }
        public class ajax_BangHP_Lop
        {
            public string a { get; set; }
            public string b { get; set; }
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult Create(ajax_BangHP_Lop l)
        {
            //var a = f.makhoi;
            JavaScriptSerializer js = new JavaScriptSerializer();
            var listTKB = js.Deserialize<List<ThoiKhoaBieu>>(l.b);
            listTKB.RemoveAt(0);
            var lop = js.Deserialize<Lop_Hoc>(l.a);
            //var banghp = (Lop_Hoc)js.Deserialize(Json, typeof(banghpJson));
            var ngayapdung = DateTime.Parse(lop.ngayad);
            var dmngayapdung = ngayapdung.ToString("yyyy/MM/dd HH:mm:ss");
            if (ModelState.IsValid)
            {
                LOP_HOC lh = new LOP_HOC();
                lh.MA_LOP = Guid.NewGuid();
                //lh.MA_KHOI = Guid.Parse(lop.makhoi);
                //lh.MA_MON = Guid.Parse(lop.mamon);
                //lh.MA_LOAI = Guid.Parse(lop.maloai);
                lh.TRANG_THAI = 1;
                lh.MA_GV = lop.magiaovien;
                lh.TEN_LOP = lop.tenlop;
                lh.NGAY_AP_DUNG = DateTime.Parse(dmngayapdung);

                lh.NGAY_MO_LOP = DateTime.Now;
                //lh.TRANG_THAI = 1;
                foreach (ThoiKhoaBieu i in listTKB)
                {
                    THOI_KHOA_BIEU tkb = new THOI_KHOA_BIEU();
                    tkb.MA_TKB = Guid.NewGuid();
                    tkb.MA_LOP = lh.MA_LOP;
                    tkb.THU = i.thu;
                    tkb.THOI_GIAN_BD = i.tgbt;
                    tkb.THOI_GIAN_KT = i.tgkt;
                    db.THOI_KHOA_BIEU.Add(tkb);
                }
                db.LOP_HOC.Add(lh);
                db.SaveChanges();
                return Json(l);
            }
            return Json(l);
        }
        //Ki?m tra th?i khóa bi?u có trùng không
        public class KtraTKB
        {
            public string tenlop { get; set; }
            public string magv { get; set; }
            public string tengv { get; set; }
            public string thoigianbd { get; set; }
            public string thoigiankt { get; set; }
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult KIEM_TRA_TKB(int thu)
        {
            //var tgbd_Them = TimeSpan.Parse(tkb.tgbt);
            //var tgkt_Them = TimeSpan.Parse(tkb.tgkt);
            //var khoangtg = tgkt - tgbd;          
            var a = (from lh in db.LOP_HOC
                     join tkb in db.THOI_KHOA_BIEU
                     on lh.MA_LOP equals tkb.MA_LOP
                     where tkb.THU == thu && lh.TRANG_THAI != -1
                     select new KtraTKB
                     {
                         tenlop = lh.TEN_LOP,
                         magv = lh.MA_GV,
                         tengv = lh.GIAO_VIEN.HO_TEN,
                         //thoigianbd = (tkb.THOI_GIAN_BD).ToString(@"hh\:mm"),
                         thoigianbd = tkb.THOI_GIAN_BD.ToString(),
                         thoigiankt = tkb.THOI_GIAN_KT.ToString()
                     }).ToList();
            foreach (KtraTKB x in a)
            {
                TimeSpan time = TimeSpan.Parse(x.thoigianbd);
                x.thoigianbd = time.ToString(@"hh\:mm");
                TimeSpan time1 = TimeSpan.Parse(x.thoigiankt);
                x.thoigiankt = time1.ToString(@"hh\:mm");
            }
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatuClass(string id)
        {
            if (id == null || string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOP_HOC lOP = db.LOP_HOC.Find(Guid.Parse(id));
            if (lOP == null)
            {
                return HttpNotFound();
            }
            lOP.TRANG_THAI = 0;//CHUYỂN TRẠNG THÁI THÀNH ĐANG HỌC
            lOP.NGAY_BAT_DAU = DateTime.Now;
            db.SaveChanges();
            string x = "1";
            return Json(x, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatuTheEnd(string id)
        {
            if (id == null || string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOP_HOC lOP = db.LOP_HOC.Find(Guid.Parse(id));
            if (lOP == null)
            {
                return HttpNotFound();
            }
            lOP.TRANG_THAI = -1;//CHUYỂN TRẠNG THÁI THÀNH KẾT THÚC
            lOP.NGAY_KET_THUC = DateTime.Now;
            db.SaveChanges();
            string x = "1";
            return Json(x, JsonRequestBehavior.AllowGet);
        }
        // GET: Admin/LOP_HOC/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOP_HOC lOP_HOC = db.LOP_HOC.Find(id);
            if (lOP_HOC == null)
            {
                return HttpNotFound();
            }
            ViewBag.MA_GV = new SelectList(db.GIAO_VIEN.Where(p => p.TRANG_THAI.Equals(true)), "MA_GV", "HO_TEN", lOP_HOC.MA_GV);
            //ViewBag.MA_KHOI = new SelectList(db.KHOI_LOP, "MA_KHOI", "TEN_KHOI", lOP_HOC.MA_KHOI);
            //ViewBag.MA_LOAI = new SelectList(db.LOAI_LOP, "MA_LOAI", "TEN_LOAI", lOP_HOC.MA_LOAI);
            //ViewBag.MA_MON = new SelectList(db.MON_HOC, "MA_MON", "TEN_MON", lOP_HOC.MA_MON);
            return View(lOP_HOC);
        }

        // POST: Admin/LOP_HOC/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MA_LOP,TEN_LOP,SI_SO,MA_LOAI,MA_MON,MA_KHOI,NGAY_MO_LOP,NGAY_BAT_DAU, NGAY_KET_THUC,TRANG_THAI,NGAY_AP_DUNG,MA_GV")] LOP_HOC lOP_HOC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lOP_HOC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MA_GV = new SelectList(db.GIAO_VIEN, "MA_GV", "HO_TEN", lOP_HOC.MA_GV);
            return View(lOP_HOC);
        }

        // GET: Admin/LOP_HOC/Delete/5
        public ActionResult Delete(string tong)
        {
            string[] a = tong.Split('_');
            if (a[0] == null || a[1] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var malop = Guid.Parse(a[1]);
            var mahs = a[0];
            //LOP_HOC lOP_HOC = db.LOP_HOC.Find(id);
            List<CT_LOP_HOC> ct = db.CT_LOP_HOC.Where(t => t.MA_HS == mahs && t.MA_LOP == malop).Select(t => t).ToList();
            List<CT_CONG_NO> cT_CONG_NOs = (from ctcn in db.CT_CONG_NO
                                            join cn in db.CONG_NO
                                            on ctcn.MA_CONG_NO equals cn.MA_CONG_NO
                                            where cn.MA_HS == mahs && ctcn.MA_LOP == malop
                                            && cn.TRANG_THAI == false
                                            select ctcn).ToList();
            string xam = "-1";
            if (ct == null || cT_CONG_NOs == null)
            {
               
                return Json(xam, JsonRequestBehavior.AllowGet);//đã thanh toán học phí
            }
            else
            {
                //List<CT_CONG_NO> cT_CONG_KiemTra = from ctcn in db.CT_CONG_NO
                //                                   join cn in db.CONG_NO
                //                                   on ctcn.MA_CONG_NO equals cn.MA_CONG_NO
                //                                   where cn.MA_HS == mahs
                CONG_NO cn = null;
                if (cT_CONG_NOs.Count() > 0)
                {
                    var macn = cT_CONG_NOs.Select(t => t.MA_CONG_NO).ToList();
                    db.CT_CONG_NO.Remove(cT_CONG_NOs[0]);
                    cn = db.CONG_NO.Find(macn[0]);
                    var vcl = macn[0];
                    var soctcn = (from ctcn in db.CT_CONG_NO
                                  join cnnew in db.CONG_NO
                                  on ctcn.MA_CONG_NO equals cn.MA_CONG_NO
                                  where cnnew.MA_CONG_NO == vcl
                                  select ctcn).ToList();
                    if (soctcn.Count() ==1)
                    {
                        if (cn != null)
                            db.CONG_NO.Remove(cn);
                    }
                }
                db.CT_LOP_HOC.Remove(ct[0]);
                db.SaveChanges();
            }
            xam = "1";//thành công
            return Json(xam, JsonRequestBehavior.AllowGet);
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
