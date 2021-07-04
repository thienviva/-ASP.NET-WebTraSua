using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebQuanLyQuanTraSua.Models;

namespace WebQuanLyQuanTraSua.Controllers
{

    public class HomeController : Controller
    {

        private QLQuanTraSuaEntities db = new QLQuanTraSuaEntities();
        public ActionResult Index()
        {

            //Lần lượt tạo các Viewbag để lấy listMENU từ cơ sở dữ liệu
            //List Trà Sữa bán chạy nhất
            var lstTSBC = db.MENUs.Where(n => n.MaLoai == 1 && n.SoLuongDaBan >= 5).OrderByDescending(n => n.SoLuongDaBan).ToList();
            //Gán vào ViewBag
            ViewBag.ListTSBC = lstTSBC;

            //List Đá Xay bán chạy nhất
            var lstDXBC = db.MENUs.Where(n => n.MaLoai == 2 && n.SoLuongDaBan >= 5).OrderByDescending(n => n.SoLuongDaBan).ToList();
            //Gán vào ViewBag
            ViewBag.ListDXBC = lstDXBC;

            //List TOPPING bán chạy nhất
            var lstTPBC = db.MENUs.Where(n => n.MaLoai == 3 && n.SoLuongDaBan >= 5).OrderByDescending(n => n.SoLuongDaBan).ToList();
            //Gán vào ViewBag
            ViewBag.ListTPBC = lstTPBC;

            return View();
        }

        public ActionResult MENUKH()
        {
            //Lần lượt tạo các Viewbag để lấy listMENU từ cơ sở dữ liệu
            //List Trà Sữa 
            //var lstTS1 = db.MENUs.Where(n => n.MaLoai == 1).OrderByDescending(n => n.TenMon).ToList();
            //var lstTS2 = db.MENUs.Where(n => n.MaLoai == 1).OrderBy(n => n.TenMon).ToList();
            var lstTS = db.MENUs.Where(n => n.MaLoai == 1).OrderByDescending(n => n.TenMon).ToList();
            //Gán vào ViewBag
            //ViewBag.ListTS1 = lstTS1;
            //ViewBag.ListTS2 = lstTS2;
            ViewBag.ListTS = lstTS;

            //List Đá Xay 
            var lstDX = db.MENUs.Where(n => n.MaLoai == 2 ).OrderByDescending(n => n.TenMon).ToList();
            //Gán vào ViewBag
            ViewBag.ListDX = lstDX;

            //List TOPPING 
            var lstTP = db.MENUs.Where(n => n.MaLoai == 3).OrderByDescending(n => n.TenMon).ToList();
            //Gán vào ViewBag
            ViewBag.ListTP = lstTP;
            return View();
        }

        


        public ActionResult Logout()
        {
            Session["quyen"] = null;
            Session["TaiKhoan"] = null;
            Session["GioHang"] = null;
            return Redirect("/Home/Index");
        }

        //Mã hóa MD5
        public string GetMD5(string str)
        {
            str = str + "md5";
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            StringBuilder sbHash = new StringBuilder();

            foreach (byte b in bHash)
            {

                sbHash.Append(String.Format("{0:x2}", b));

            }
            return sbHash.ToString();
        }
        [HttpPost]
        //xử lý đăng nhập
        public ActionResult DangNhap(FormCollection f)
        {

            string taikhoan = f["username"].ToString();
            string matkhau = f["password"].ToString();
            string matkhaumd5 = GetMD5(matkhau);
            using (var _context = new QLQuanTraSuaEntities())
            {
                var kh = from u in _context.TAIKHOANKHs
                         where u.UserName == taikhoan && u.Pass == matkhau
                         select u;
                var ttkh = from u in _context.KHACHHANGs
                           where u.TAIKHOANKH.UserName == taikhoan && u.TAIKHOANKH.Pass == matkhau
                           select u;
                var ql = from u in _context.TAIKHOANQLs
                         where u.UserName == taikhoan && u.Pass == matkhau
                         select u;
                var ttql = from u in _context.QUANLies
                           where u.TAIKHOANQL.UserName == taikhoan && u.TAIKHOANQL.Pass == matkhau
                           select u;
                var nv = from u in _context.TAIKHOANNVs
                         where u.UserName == taikhoan && u.Pass == matkhau
                         select u;
                var ttnv = from u in _context.NHANVIENs
                           where u.TAIKHOANNV.UserName == taikhoan && u.TAIKHOANNV.Pass == matkhau
                           select u;
                if (ql != null || nv != null || kh != null)
                {
                    if (ql.Count() == 1)
                    {
                        Session["quyen"] = "1"; //Gán Session cho quản lý

                        Session["UserName"] = ql.First().QUANLY.TenQL;
                        Session["TaiKhoan"] = ql.First().QUANLY;
                        Session["HinhAnh"] = ql.First().QUANLY.HinhAnh;
                        return Content("/QUANLies/Index");
                    }
                    else
                    {
                        if (nv.Count() == 1)
                        {
                            Session["quyen"] = "2";

                            Session["UserName"] = nv.First().NHANVIEN.TenNV;
                            Session["TaiKhoan"] = nv.First().NHANVIEN;
                            return Content("/Home/Index");
                        }
                        else
                        {
                            if (kh.Count() == 1)
                            {
                                Session["quyen"] = "3";

                                Session["UserName"] = kh.First().KHACHHANG.TenKH;
                                Session["TaiKhoan"] = kh.First().KHACHHANG;
                                return Content("/Home/Index");
                            }
                            else
                            {
                                return Content("false");
                            }
                        }

                    }
                }
                return Content("false");
            }
        }



    }
}