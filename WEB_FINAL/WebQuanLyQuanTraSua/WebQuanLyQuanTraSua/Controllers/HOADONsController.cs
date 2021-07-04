using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebQuanLyQuanTraSua.Models;
using System.Linq.Dynamic;
using System.ComponentModel;
using OfficeOpenXml;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Ajax.Utilities;
using EPPlusTest;

namespace WebQuanLyQuanTraSua.Controllers
{
    public class HOADONsController : Controller
    {
        private QLQuanTraSuaEntities db = new QLQuanTraSuaEntities();
        [HttpGet]

        // GET: HOADONs
        public ActionResult Index()
        {
            var hOADONs = db.HOADONs.Include(h => h.CHINHANH).Include(h => h.KHACHHANG).Include(h => h.KHACHHANGLA);
            return View(hOADONs.ToList());
        }

        // GET: HOADONs/Details/5
        public ActionResult Details(int? id)
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

        // GET: HOADONs/Create
        public ActionResult Create()
        {
            ViewBag.MaCN = new SelectList(db.CHINHANHs, "MaCN", "TenCN");
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH");
            ViewBag.MaKHLA = new SelectList(db.KHACHHANGLAs, "MaKHLA", "HoTen");
            return View();
        }

        // POST: HOADONs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaHD,MaKH,MaCN,NgayXuatHD,TongGia,TinhTrangHD,MaKHLA")] HOADON hOADON)
        {
            if (ModelState.IsValid)
            {
                db.HOADONs.Add(hOADON);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaCN = new SelectList(db.CHINHANHs, "MaCN", "TenCN", hOADON.MaCN);
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH", hOADON.MaKH);
            ViewBag.MaKHLA = new SelectList(db.KHACHHANGLAs, "MaKHLA", "HoTen", hOADON.MaKHLA);
            return View(hOADON);
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
                return RedirectToAction("Index");
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
