using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using WebQuanLyQuanTraSua.Models;


namespace WebQuanLyQuanTraSua.Controllers
{
    public class ChartJsController : Controller
    {
        private QLQuanTraSuaEntities db = new QLQuanTraSuaEntities();

        // GET: ChartJs
        public ActionResult ThongKe()
        {
            //Đồ thị 1
            //Lần lượt tạo các Viewbag để lấy listMENU từ cơ sở dữ liệu
            //List Trà Sữa 
            var slbTS = db.MENUs.Where(n => n.MaLoai == 1).Sum(n => n.SoLuongDaBan);
            //Gán vào ViewBag
            ViewBag.SLBTS = slbTS;
            //List Đá Xay
            var slbDX = db.MENUs.Where(n => n.MaLoai == 2).Sum(n => n.SoLuongDaBan);
            //Gán vào ViewBag
            ViewBag.SLBDX = slbDX;
            //List TOPPING 
            var slbTP = db.MENUs.Where(n => n.MaLoai == 3).Sum(n => n.SoLuongDaBan);
            //Gán vào ViewBag
            ViewBag.SLBTP = slbTP;


            //Đồ thị 2
            var list = db.HOADONs.ToList();

            var slHD = list.Select(x => x.CHINHANH).Distinct();

            List<int> cn1 = new List<int>();
            List<int> cn2 = new List<int>();
            List<int> cn3 = new List<int>();
            foreach (var item in slHD)
            {
                cn1.Add(list.Count(x => x.MaCN == 101));
            }
            foreach (var item in slHD)
            {
                cn2.Add(list.Count(x => x.MaCN == 102));
            }
            foreach (var item in slHD)
            {
                cn3.Add(list.Count(x => x.MaCN == 103));
            }

            ViewBag.slhd = slHD;
            ViewBag.cn1 = cn1;
            ViewBag.cn2 = cn2;
            ViewBag.cn3 = cn3;


 


            return View();
        }


        public ActionResult GetData()
        {
            //Đồ thị 3
            var query = db.HOADONs.Select(n => new { name = n.NgayXuatHD.ToString(), count = n.TongGia }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetData2()
        {
            //Đồ thị 4
            var query = db.MENUs.Select(n => new { name = n.TenMon, count = n.SoLuongDaBan, gia = n.Gia }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetData3()
        {
            //Đồ thị 2
            var query = db.CHINHANHs.Select(n => new { name = n.TenCN, count = n.HOADONs.Count}).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

    }


}