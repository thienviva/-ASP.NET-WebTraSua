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
    public class PHANCAsController : Controller
    {
        private QLQuanTraSuaEntities db = new QLQuanTraSuaEntities();

        // GET: PHANCAs
        public ActionResult Index()
        {
            var pHANCAs = db.PHANCAs.Include(p => p.CHINHANH).Include(p => p.NHANVIEN);
            return View(pHANCAs.ToList());
        }

        // GET: PHANCAs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHANCA pHANCA = db.PHANCAs.Find(id);
            if (pHANCA == null)
            {
                return HttpNotFound();
            }
            return View(pHANCA);
        }

        // GET: PHANCAs/Create
        public ActionResult Create()
        {
            ViewBag.MaCN = new SelectList(db.CHINHANHs, "MaCN", "TenCN");
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV");
            return View();
        }

        // POST: PHANCAs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNV,MaCN,NgayLamViec,GioLamViec")] PHANCA pHANCA)
        {
            if (ModelState.IsValid)
            {
                db.PHANCAs.Add(pHANCA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaCN = new SelectList(db.CHINHANHs, "MaCN", "TenCN", pHANCA.MaCN);
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV", pHANCA.MaNV);
            return View(pHANCA);
        }

        // GET: PHANCAs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHANCA pHANCA = db.PHANCAs.Find(id);
            if (pHANCA == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaCN = new SelectList(db.CHINHANHs, "MaCN", "TenCN", pHANCA.MaCN);
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV", pHANCA.MaNV);
            return View(pHANCA);
        }

        // POST: PHANCAs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNV,MaCN,NgayLamViec,GioLamViec")] PHANCA pHANCA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pHANCA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaCN = new SelectList(db.CHINHANHs, "MaCN", "TenCN", pHANCA.MaCN);
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV", pHANCA.MaNV);
            return View(pHANCA);
        }

        // GET: PHANCAs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHANCA pHANCA = db.PHANCAs.Find(id);
            if (pHANCA == null)
            {
                return HttpNotFound();
            }
            return View(pHANCA);
        }

        // POST: PHANCAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PHANCA pHANCA = db.PHANCAs.Find(id);
            db.PHANCAs.Remove(pHANCA);
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
