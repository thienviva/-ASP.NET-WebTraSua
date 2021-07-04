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
    public class TAIKHOANQLsController : Controller
    {
        private QLQuanTraSuaEntities db = new QLQuanTraSuaEntities();

        // GET: TAIKHOANQLs
        public ActionResult Index()
        {
            var tAIKHOANQLs = db.TAIKHOANQLs.Include(t => t.QUANLY);
            return View(tAIKHOANQLs.ToList());
        }

        // GET: TAIKHOANQLs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOANQL tAIKHOANQL = db.TAIKHOANQLs.Find(id);
            if (tAIKHOANQL == null)
            {
                return HttpNotFound();
            }
            return View(tAIKHOANQL);
        }

        // GET: TAIKHOANQLs/Create
        public ActionResult Create()
        {
            ViewBag.MaQL = new SelectList(db.QUANLies, "MaQL", "TenQL");
            return View();
        }

        // POST: TAIKHOANQLs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserName,Pass,MaQL")] TAIKHOANQL tAIKHOANQL)
        {
            if (ModelState.IsValid)
            {
                db.TAIKHOANQLs.Add(tAIKHOANQL);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaQL = new SelectList(db.QUANLies, "MaQL", "TenQL", tAIKHOANQL.MaQL);
            return View(tAIKHOANQL);
        }

        // GET: TAIKHOANQLs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOANQL tAIKHOANQL = db.TAIKHOANQLs.Find(id);
            if (tAIKHOANQL == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaQL = new SelectList(db.QUANLies, "MaQL", "TenQL", tAIKHOANQL.MaQL);
            return View(tAIKHOANQL);
        }

        // POST: TAIKHOANQLs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserName,Pass,MaQL")] TAIKHOANQL tAIKHOANQL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tAIKHOANQL).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaQL = new SelectList(db.QUANLies, "MaQL", "TenQL", tAIKHOANQL.MaQL);
            return View(tAIKHOANQL);
        }

        // GET: TAIKHOANQLs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOANQL tAIKHOANQL = db.TAIKHOANQLs.Find(id);
            if (tAIKHOANQL == null)
            {
                return HttpNotFound();
            }
            return View(tAIKHOANQL);
        }

        // POST: TAIKHOANQLs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TAIKHOANQL tAIKHOANQL = db.TAIKHOANQLs.Find(id);
            db.TAIKHOANQLs.Remove(tAIKHOANQL);
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
