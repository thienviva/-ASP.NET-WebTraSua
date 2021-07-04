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
    public class XuLyDonHangController : Controller
    {
        private QLQuanTraSuaEntities db = new QLQuanTraSuaEntities();
        // GET: XuLyDonHang
        public ActionResult XuLyDonHang()
        {


            if ((string)Session["quyen"] == "1" || (string)Session["quyen"] == "2")
            {
                //List Hóa đơn 
                var lstHD1 = db.HOADONs.OrderByDescending(n => n.NgayXuatHD).ToList();
                //Gán vào ViewBag
                ViewBag.ListHD1 = lstHD1;


                //Lần lượt tạo các Viewbag để lấy listHD từ cơ sở dữ liệu
                //List Hóa đơn "Chờ xác nhận"
                var lstHD2 = db.HOADONs.Where(n => n.TinhTrangHD == "Chờ xác nhận").OrderByDescending(n => n.NgayXuatHD).ToList();
                //Gán vào ViewBag
                ViewBag.ListHD2 = lstHD2;

                //List Hóa đơn "Chờ lấy hàng"
                var lstHD3 = db.HOADONs.Where(n => n.TinhTrangHD == "Chờ lấy hàng").OrderByDescending(n => n.NgayXuatHD).ToList();
                //Gán vào ViewBag
                ViewBag.ListHD3 = lstHD3;

                //List Hóa đơn "Đang giao"
                var lstHD4 = db.HOADONs.Where(n => n.TinhTrangHD == "Đang giao").OrderByDescending(n => n.NgayXuatHD).ToList();
                //Gán vào ViewBag
                ViewBag.ListHD4 = lstHD4;

                //List Hóa đơn "Đã giao"
                var lstHD5 = db.HOADONs.Where(n => n.TinhTrangHD == "Đã giao").OrderByDescending(n => n.NgayXuatHD).ToList();
                //Gán vào ViewBag
                ViewBag.ListHD5 = lstHD5;

                //List Hóa đơn "Đã hủy"
                var lstHD6 = db.HOADONs.Where(n => n.TinhTrangHD == "Đã hủy").OrderByDescending(n => n.NgayXuatHD).ToList();
                //Gán vào ViewBag
                ViewBag.ListHD6 = lstHD6;

                //List Hóa đơn "Trả hàng/Hoàn tiền"
                var lstHD7 = db.HOADONs.Where(n => n.TinhTrangHD == "Trả hàng/Hoàn tiền").OrderByDescending(n => n.NgayXuatHD).ToList();
                //Gán vào ViewBag
                ViewBag.ListHD7 = lstHD7;

                return View();

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

            
        }


        public ActionResult DonHangPartial()
        {
            return PartialView();
        }


        public ActionResult ChiTietDonHang(int? id)
        {
            //List chi tiết hóa đơn
            var lstCTHD = db.CHITIETHDs.Where(n => n.MaHD == id).ToList();
            //Gán vào ViewBag
            ViewBag.ListCTHD = lstCTHD;

            return View(lstCTHD);
        }

        // GET: HOADONs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOADON hOADON = db.HOADONs.Find(id);
            if (hOADON == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaCN = new SelectList(db.CHINHANHs, "MaCN", "TenCN", hOADON.MaCN);
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH", hOADON.MaKH);
            ViewBag.MaKHLA = new SelectList(db.KHACHHANGLAs, "MaKHLA", "HoTen", hOADON.MaKHLA);
            return View(hOADON);
        }

        // POST: HOADONs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHD,MaKH,MaCN,NgayXuatHD,TongGia,TinhTrangHD,MaKHLA")] HOADON hOADON)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hOADON).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("XuLyDonHang");
            }
            ViewBag.MaCN = new SelectList(db.CHINHANHs, "MaCN", "TenCN", hOADON.MaCN);
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH", hOADON.MaKH);
            ViewBag.MaKHLA = new SelectList(db.KHACHHANGLAs, "MaKHLA", "HoTen", hOADON.MaKHLA);
            return View(hOADON);
        }

        // GET: HOADONs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOADON hOADON = db.HOADONs.Find(id);
            if (hOADON == null)
            {
                return HttpNotFound();
            }
            return View(hOADON);
        }

        // POST: HOADONs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var cHITIETHD = db.CHITIETHDs.Where(n => n.MaHD == id).ToList();
            foreach (CHITIETHD item in cHITIETHD)
            {
                db.CHITIETHDs.Remove(item);
            }

            HOADON hOADON = db.HOADONs.Find(id);
            db.HOADONs.Remove(hOADON);
            db.SaveChanges();
            return RedirectToAction("XuLyDonHang");
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