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
    public class KHACHHANGLAsController : Controller
    {
        private QLQuanTraSuaEntities db = new QLQuanTraSuaEntities();

        // GET: KHACHHANGLAs
        public ActionResult Index()
        {
            return View(db.KHACHHANGLAs.ToList());
        }

        // GET: KHACHHANGLAs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANGLA kHACHHANGLA = db.KHACHHANGLAs.Find(id);
            if (kHACHHANGLA == null)
            {
                return HttpNotFound();
            }
            return View(kHACHHANGLA);
        }

        // GET: KHACHHANGLAs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KHACHHANGLAs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKHLA,HoTen,EmailKH,SoDienThoai,DiaChiKH")] KHACHHANGLA kHACHHANGLA)
        {
            if (ModelState.IsValid)
            {
                db.KHACHHANGLAs.Add(kHACHHANGLA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kHACHHANGLA);
        }

        // GET: KHACHHANGLAs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANGLA kHACHHANGLA = db.KHACHHANGLAs.Find(id);
            if (kHACHHANGLA == null)
            {
                return HttpNotFound();
            }
            return View(kHACHHANGLA);
        }

        // POST: KHACHHANGLAs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKHLA,HoTen,EmailKH,SoDienThoai,DiaChiKH")] KHACHHANGLA kHACHHANGLA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kHACHHANGLA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kHACHHANGLA);
        }

        // GET: KHACHHANGLAs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANGLA kHACHHANGLA = db.KHACHHANGLAs.Find(id);
            if (kHACHHANGLA == null)
            {
                return HttpNotFound();
            }
            return View(kHACHHANGLA);
        }

        // POST: KHACHHANGLAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KHACHHANGLA kHACHHANGLA = db.KHACHHANGLAs.Find(id);
            db.KHACHHANGLAs.Remove(kHACHHANGLA);
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
