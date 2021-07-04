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
    public class TAIKHOANNVsController : Controller
    {
        private QLQuanTraSuaEntities db = new QLQuanTraSuaEntities();

        // GET: TAIKHOANNVs
        public ActionResult Index()
        {
            var tAIKHOANNVs = db.TAIKHOANNVs.Include(t => t.NHANVIEN);
            return View(tAIKHOANNVs.ToList());
        }

        // GET: TAIKHOANNVs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOANNV tAIKHOANNV = db.TAIKHOANNVs.Find(id);
            if (tAIKHOANNV == null)
            {
                return HttpNotFound();
            }
            return View(tAIKHOANNV);
        }

        // GET: TAIKHOANNVs/Create
        public ActionResult Create()
        {
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV");
            return View();
        }

        // POST: TAIKHOANNVs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserName,Pass,MaNV")] TAIKHOANNV tAIKHOANNV)
        {
            if (ModelState.IsValid)
            {
                db.TAIKHOANNVs.Add(tAIKHOANNV);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV", tAIKHOANNV.MaNV);
            return View(tAIKHOANNV);
        }

        // GET: TAIKHOANNVs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOANNV tAIKHOANNV = db.TAIKHOANNVs.Find(id);
            if (tAIKHOANNV == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV", tAIKHOANNV.MaNV);
            return View(tAIKHOANNV);
        }

        // POST: TAIKHOANNVs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserName,Pass,MaNV")] TAIKHOANNV tAIKHOANNV)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tAIKHOANNV).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV", tAIKHOANNV.MaNV);
            return View(tAIKHOANNV);
        }

        // GET: TAIKHOANNVs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOANNV tAIKHOANNV = db.TAIKHOANNVs.Find(id);
            if (tAIKHOANNV == null)
            {
                return HttpNotFound();
            }
            return View(tAIKHOANNV);
        }

        // POST: TAIKHOANNVs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TAIKHOANNV tAIKHOANNV = db.TAIKHOANNVs.Find(id);
            db.TAIKHOANNVs.Remove(tAIKHOANNV);
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
