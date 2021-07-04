using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebQuanLyQuanTraSua.Models;

namespace WebQuanLyQuanTraSua.Controllers
{
    public class GIOHANGsController : Controller
    {
        private QLQuanTraSuaEntities db = new QLQuanTraSuaEntities();

        //Lấy giỏ hàng
        public List<ItemGioHang> LayGioHang()
        {
            //Giỏ hàng đã tồn tại
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if(lstGioHang == null)
            {
                //Nếu Session giỏ hàng chưa tồn tại thì khởi tạo giỏ hàng
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        


        //Thêm giỏ hàng bằng cách "Load lại trang" --> Mua ngay
        public ActionResult ThemGioHang(string MaMon, string strURL)
        {
            //Kiểm tra sản phẩm có tồn tại trong CSDL hay không
            MENU mn = db.MENUs.SingleOrDefault(n => n.MaMon == MaMon);
            if(mn == null)
            {
                //Trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            //TH1: Nếu sản phẩm đã tồn tại trong giỏ hàng
            ItemGioHang mncheck = lstGioHang.SingleOrDefault(n => n.MaMon == MaMon);
            if(mncheck !=null)
            {
                mncheck.SoLuong++;
                mncheck.ThanhTien = mncheck.SoLuong * mncheck.Gia;
                //return Redirect(strURL);
                return RedirectToAction("GIOHANGKH", "GIOHANGs");
            }
            ItemGioHang itemGH = new ItemGioHang(MaMon);
            lstGioHang.Add(itemGH);
            //return Redirect(strURL);
            return RedirectToAction("GIOHANGKH", "GIOHANGs");

        }

        public int TinhTongSoLuong()
        {
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if(lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }


        public int TinhTongTien()
        {
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }


        // GET: GIOHANGs
        public ActionResult GioHangPartial()
        {
            if(TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoluong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoluong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();

            return PartialView();
        }

        public ActionResult GIOHANGKH()
        {
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();

            return View(lstGioHang);
        }

        //Chỉnh sửa giỏ hàng
        public ActionResult SuaGioHang(string MaMon)
        {
            //Kiểm tra session giỏ hàng tồn tại chưa
            if(Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Kiểm tra sản phẩm có tồn tại trong CSDL hay không
            MENU mn = db.MENUs.SingleOrDefault(n => n.MaMon == MaMon);
            if (mn == null)
            {
                //Trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //Lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Kiểm tra xem sản phẩm đó có tồn tại trong giỏ hàng không
            ItemGioHang mncheck = lstGioHang.SingleOrDefault(n => n.MaMon == MaMon);
            if(mncheck ==null)
            {
                return RedirectToAction("Index", "Home");

            }
            //Lấy list giỏ hàng tạo giao diện
            ViewBag.GioHang = lstGioHang;
            //Nếu tồn tại rồi
            return View(mncheck);
        }
        //Xử lý cập nhật giỏ hàng
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            MENU mncheck = db.MENUs.Single(n => n.MaMon == itemGH.MaMon);
            //Cập nhật số lượng trong session giỏ hàng
            //B1: Lấy List<ItemGioHang> từ session["GioHang"]
            List<ItemGioHang> lstGH = LayGioHang();
            //B2: Lấy sản phẩm cần cập nhật từ List<ItemGioHang>
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaMon == itemGH.MaMon);
            //B3: Tiến hành cập nhật
            itemGHUpdate.SoLuong = itemGH.SoLuong;
            itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong * itemGHUpdate.Gia;
            return RedirectToAction("GIOHANGKH");
        }

        public ActionResult XoaGioHang(string MaMon)
        {
            //Kiểm tra session giỏ hàng tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Kiểm tra sản phẩm có tồn tại trong CSDL hay không
            MENU mn = db.MENUs.SingleOrDefault(n => n.MaMon == MaMon);
            if (mn == null)
            {
                //Trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //Lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Kiểm tra xem sản phẩm đó có tồn tại trong giỏ hàng không
            ItemGioHang mncheck = lstGioHang.SingleOrDefault(n => n.MaMon == MaMon);
            if (mncheck == null)
            {
                return RedirectToAction("Index", "Home");

            }
            //Xóa item trong giỏ hàng
            lstGioHang.Remove(mncheck);

            return RedirectToAction("GIOHANGKH");

        }

        //Xây dựng chức năng đặt hàng
        public ActionResult DatHang(KHACHHANGLA khLA, string chinhanh)
        {
            //Kiểm tra session giỏ hàng tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KHACHHANGLA KhachHangLA = new KHACHHANGLA();
            KHACHHANG kh = Session["TaiKhoan"] as KHACHHANG;
            if (Session["TaiKhoan"] == null) //Kiểm tra xem đây là khách hàng vãng lai hay đã có tài khoản
            {
                //Thêm khách hàng vào bảng khách hàng (Khách hàng chưa có tài khoản)

                var makh = (from makhla in db.KHACHHANGLAs
                            select makhla).ToList();


                int tongkhla = makh.Count();
                int MaKHLA = tongkhla + 203 + 205 + 1 ;
                khLA.MaKHLA = MaKHLA;
                KhachHangLA = khLA;
                db.KHACHHANGLAs.Add(KhachHangLA);
                db.SaveChanges();
            }
            else
            {
                //Xử lý đối với khách hàng đã có tài khoản
                KhachHangLA.MaKHLA = kh.MaKH;
                KhachHangLA.HoTen = kh.TenKH;
                KhachHangLA.DiaChiKH = kh.DiaChi;
                KhachHangLA.EmailKH = kh.Email;
                KhachHangLA.SoDienThoai = kh.SDT;

                //Không lưu là do thông tin khách hàng đã có ở bảng khách hàng 
            }
            //Thêm Hóa Đơn

            var tpQuery = (from mahd in db.HOADONs
                           select mahd).ToList();

            
            int tonghd = tpQuery.Count();
            int MaHD = tonghd + 1;

            HOADON hd = new HOADON();
            hd.MaHD = MaHD;

            if (Session["TaiKhoan"] == null) //Kiểm tra xem đây là khách hàng vãng lai hay đã có tài khoản
            { 
                hd.MaKHLA = KhachHangLA.MaKHLA;
                hd.MaKH = null;
                hd.KHACHHANG = null;
            
            }
            else 
            {
                
                hd.MaKH = KhachHangLA.MaKHLA;
                hd.MaKHLA = null;
                hd.KHACHHANGLA = null;
            }

            hd.NgayXuatHD = DateTime.Now;
            hd.TinhTrangHD = "Chờ xác nhận";
            hd.MaCN = 101;

            hd.TongGia = TinhTongTien();

            if (hd.MaKHLA == null) //Kiểm tra xem đây là khách hàng vãng lai hay đã có tài khoản
            {
                kh.TongChiTieu += TinhTongTien();
                db.Entry(kh).State = EntityState.Modified; //Cập nhật thông tin khách hàng có tài khoản
            }


            db.HOADONs.Add(hd);
            db.SaveChanges();
            //Thêm chi tiết Hóa Đơn
            List<ItemGioHang> lstGH = LayGioHang();
            foreach(var item in lstGH)
            {
                CHITIETHD cthd = new CHITIETHD();
                cthd.MaHD = hd.MaHD;
                cthd.MaMon = item.MaMon;
                cthd.SoLuong = item.SoLuong;
                cthd.Ban = "Online";
                cthd.Gia = item.Gia;
                db.CHITIETHDs.Add(cthd);
                
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("GIOHANGKH");
        }

        //Thêm vào giỏ hàng
        public ActionResult ThemGioHangAjax(string MaMon, string strURL)
        {
            //Kiểm tra sản phẩm có tồn tại trong CSDL hay không
            MENU mn = db.MENUs.SingleOrDefault(n => n.MaMon == MaMon);
            if (mn == null)
            {
                //Trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            //TH1: Nếu sản phẩm đã tồn tại trong giỏ hàng
            ItemGioHang mncheck = lstGioHang.SingleOrDefault(n => n.MaMon == MaMon);
            if (mncheck != null)
            {
                mncheck.SoLuong++;
                mncheck.ThanhTien = mncheck.SoLuong * mncheck.Gia;
                ViewBag.TongSoluong = TinhTongSoLuong();
                ViewBag.TongTien = TinhTongTien();
                return PartialView("GioHangPartial");
            }
            ItemGioHang itemGH = new ItemGioHang(MaMon);
            lstGioHang.Add(itemGH);
            ViewBag.TongSoluong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView("GioHangPartial");

        }
    }
}
