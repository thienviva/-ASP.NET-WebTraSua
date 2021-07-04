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
    public class QUANLiesController : Controller
    {
        private QLQuanTraSuaEntities db = new QLQuanTraSuaEntities();

        // GET: QUANLies
        public ActionResult Index()
        {
            var qUANLies = db.QUANLies.Include(q => q.TAIKHOANQL);
            return View(qUANLies.ToList());
        }

        // GET: QUANLies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QUANLY qUANLY = db.QUANLies.Find(id);
            if (qUANLY == null)
            {
                return HttpNotFound();
            }
            return View(qUANLY);
        }

        // GET: QUANLies/Create
        public ActionResult Create()
        {
            ViewBag.MaQL = new SelectList(db.TAIKHOANQLs, "MaQL", "UserName");
            return View();
        }

        // POST: QUANLies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaQL,TenQL,Email,Tuoi,DiaChi,SDT,Luong")] QUANLY qUANLY)
        {
            if (ModelState.IsValid)
            {
                db.QUANLies.Add(qUANLY);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaQL = new SelectList(db.TAIKHOANQLs, "MaQL", "UserName", qUANLY.MaQL);
            return View(qUANLY);
        }

        // GET: QUANLies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QUANLY qUANLY = db.QUANLies.Find(id);
            if (qUANLY == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaQL = new SelectList(db.TAIKHOANQLs, "MaQL", "UserName", qUANLY.MaQL);
            return View(qUANLY);
        }

        // POST: QUANLies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaQL,TenQL,Email,Tuoi,DiaChi,SDT,Luong")] QUANLY qUANLY)
        {
            if (ModelState.IsValid)
            {
                db.Entry(qUANLY).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaQL = new SelectList(db.TAIKHOANQLs, "MaQL", "UserName", qUANLY.MaQL);
            return View(qUANLY);
        }

        // GET: QUANLies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QUANLY qUANLY = db.QUANLies.Find(id);
            if (qUANLY == null)
            {
                return HttpNotFound();
            }
            return View(qUANLY);
        }

        // POST: QUANLies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Xóa tài khoản quản lý trước khi xóa quản lý
            TAIKHOANQL tAIKHOANQL = db.TAIKHOANQLs.Find(id);
            db.TAIKHOANQLs.Remove(tAIKHOANQL);

            QUANLY qUANLY = db.QUANLies.Find(id);
            db.QUANLies.Remove(qUANLY);
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
