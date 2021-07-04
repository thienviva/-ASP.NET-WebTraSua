using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebQuanLyQuanTraSua.Models;
using PagedList;
using System.Reflection;
using System.Linq.Dynamic;

namespace WebQuanLyQuanTraSua.Controllers
{
    public class ThongTinKhachHangController : Controller
    {
        private QLQuanTraSuaEntities db = new QLQuanTraSuaEntities();
        // GET: KHACHHANGs/Details/5
        public ActionResult ThongTinKhachHang()
        {
            if ((string)Session["quyen"] == "3")
            {
                int id = (int)Session["ID"];
                KHACHHANG khachHang = db.KHACHHANGs.Find(id);
                if (khachHang == null)
                {
                    return HttpNotFound();
                }
                return View(khachHang);

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

 

        // GET: KHACHHANGs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG kHACHHANG = db.KHACHHANGs.Find(id);
            if (kHACHHANG == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.TAIKHOANKHs, "MaKH", "UserName", kHACHHANG.MaKH);
            return View(kHACHHANG);
        }

        // POST: KHACHHANGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKH,TenKH,Email,Tuoi,SDT,DiaChi,TongChiTieu,CapBac")] KHACHHANG kHACHHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kHACHHANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ThongTinKhachHang");
            }
            ViewBag.MaKH = new SelectList(db.TAIKHOANKHs, "MaKH", "UserName", kHACHHANG.MaKH);
            return View(kHACHHANG);
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