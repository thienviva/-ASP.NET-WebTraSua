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
    public class TAIKHOANKHsController : Controller
    {
        private QLQuanTraSuaEntities db = new QLQuanTraSuaEntities();

        // GET: TAIKHOANKHs
        public ActionResult Index()
        {
            var tAIKHOANKHs = db.TAIKHOANKHs.Include(t => t.KHACHHANG);
            return View(tAIKHOANKHs.ToList());
        }

        // GET: TAIKHOANKHs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOANKH tAIKHOANKH = db.TAIKHOANKHs.Find(id);
            if (tAIKHOANKH == null)
            {
                return HttpNotFound();
            }
            return View(tAIKHOANKH);
        }

        // GET: TAIKHOANKHs/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH");
            return View();
        }

        // POST: TAIKHOANKHs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserName,Pass,MaKH")] TAIKHOANKH tAIKHOANKH)
        {
            if (ModelState.IsValid)
            {
                db.TAIKHOANKHs.Add(tAIKHOANKH);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH", tAIKHOANKH.MaKH);
            return View(tAIKHOANKH);
        }

        // GET: TAIKHOANKHs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOANKH tAIKHOANKH = db.TAIKHOANKHs.Find(id);
            if (tAIKHOANKH == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH", tAIKHOANKH.MaKH);
            return View(tAIKHOANKH);
        }

        // POST: TAIKHOANKHs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserName,Pass,MaKH")] TAIKHOANKH tAIKHOANKH)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tAIKHOANKH).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH", tAIKHOANKH.MaKH);
            return View(tAIKHOANKH);
        }

        // GET: TAIKHOANKHs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOANKH tAIKHOANKH = db.TAIKHOANKHs.Find(id);
            if (tAIKHOANKH == null)
            {
                return HttpNotFound();
            }
            return View(tAIKHOANKH);
        }

        // POST: TAIKHOANKHs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TAIKHOANKH tAIKHOANKH = db.TAIKHOANKHs.Find(id);
            db.TAIKHOANKHs.Remove(tAIKHOANKH);
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
