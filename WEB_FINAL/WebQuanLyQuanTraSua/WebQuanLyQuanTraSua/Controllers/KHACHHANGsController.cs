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
using OfficeOpenXml;

namespace WebQuanLyQuanTraSua.Controllers
{
    public class KHACHHANGsController : Controller
    {
        private QLQuanTraSuaEntities db = new QLQuanTraSuaEntities();


        public class HttpParamActionAttribute : ActionNameSelectorAttribute
        {
            public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
            {
                if (actionName.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                    return true;

                var request = controllerContext.RequestContext.HttpContext.Request;
                return request[methodInfo.Name] != null;
            }
        }

        [HttpGet]
        // GET: KHACHHANGs
        public ActionResult Index(int? size, int? page, string sortProperty, string sortOrder, string searchString)
        {
            // 1. Tạo biến ViewBag gồm sortOrder, searchValue, sortProperty và page
            if (sortOrder == "asc") ViewBag.sortOrder = "desc";
            if (sortOrder == "desc") ViewBag.sortOrder = "";
            if (sortOrder == "") ViewBag.sortOrder = "asc";
            ViewBag.searchValue = searchString;
            ViewBag.sortProperty = sortProperty;
            ViewBag.page = page;

            // 2. Tạo danh sách chọn số trang
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });
            items.Add(new SelectListItem { Text = "25", Value = "25" });
            items.Add(new SelectListItem { Text = "50", Value = "50" });
            items.Add(new SelectListItem { Text = "100", Value = "100" });
            items.Add(new SelectListItem { Text = "200", Value = "200" });

            // 2.1. Thiết lập số trang đang chọn vào danh sách List<SelectListItem> items
            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }
            ViewBag.size = items;
            ViewBag.currentSize = size;

            // 3. Lấy tất cả tên thuộc tính của lớp Link (LinkID, LinkName, LinkURL,...)
            var properties = typeof(KHACHHANG).GetProperties();
            List<Tuple<string, bool, int>> list = new List<Tuple<string, bool, int>>();
            foreach (var item in properties)
            {
                int order = 999;
                var isVirtual = item.GetAccessors()[0].IsVirtual;

                if (item.Name == "TenKH") order = 2;
                if (item.Name == "MaKH") order = 1;
                if (item.Name == "Email") order = 3;
                if (item.Name == "Tuoi") order = 4;
                if (item.Name == "SDT") order = 5;
                if (item.Name == "DiaChi") order = 6;
                if (item.Name == "TongChiTieu") order = 7;
                if (item.Name == "TAIKHOANKH") continue; // Không hiển thị cột này
                Tuple<string, bool, int> t = new Tuple<string, bool, int>(item.Name, isVirtual, order);
                list.Add(t);
            }
            list = list.OrderBy(x => x.Item3).ToList();

            // 3.1. Tạo Heading sắp xếp cho các cột
            foreach (var item in list)
            {
                if (!item.Item2)
                {
                    if (sortOrder == "desc" && sortProperty == item.Item1)
                    {
                        ViewBag.Headings += "<th><a href='?page=" + page + "&size=" + ViewBag.currentSize + "&sortProperty=" + item.Item1 + "&sortOrder=" +
                            ViewBag.sortOrder + "&searchString=" + searchString + "'>" + item.Item1 + "<i class='fa fa-fw fa-sort-desc'></i></th></a></th>";
                    }
                    else if (sortOrder == "asc" && sortProperty == item.Item1)
                    {
                        ViewBag.Headings += "<th><a href='?page=" + page + "&size=" + ViewBag.currentSize + "&sortProperty=" + item.Item1 + "&sortOrder=" +
                            ViewBag.sortOrder + "&searchString=" + searchString + "'>" + item.Item1 + "<i class='fa fa-fw fa-sort-asc'></a></th>";
                    }
                    else
                    {
                        ViewBag.Headings += "<th><a href='?page=" + page + "&size=" + ViewBag.currentSize + "&sortProperty=" + item.Item1 + "&sortOrder=" +
                           ViewBag.sortOrder + "&searchString=" + searchString + "'>" + item.Item1 + "<i class='fa fa-fw fa-sort'></a></th>";
                    }

                }
                else ViewBag.Headings += "<th>" + item.Item1 + "</th>";
            }

            // 4. Truy vấn lấy tất cả đường dẫn
            var kHACHHANGs = from l in db.KHACHHANGs
                        select l;

            
            // 5. Tạo thuộc tính sắp xếp mặc định là "LinkID"
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "MaKH";

            // 5. Sắp xếp tăng/giảm bằng phương thức OrderBy sử dụng trong thư viện Dynamic LINQ
            if (sortOrder == "desc") kHACHHANGs = kHACHHANGs.OrderBy(sortProperty + " desc");
            else if (sortOrder == "asc") kHACHHANGs = kHACHHANGs.OrderBy(sortProperty);
            else kHACHHANGs = kHACHHANGs.OrderBy("MaKH");

            // 5.1. Thêm phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                kHACHHANGs = kHACHHANGs.Where(s => s.TenKH.Contains(searchString));
            }

            // 5.2. Nếu page = null thì đặt lại là 1.
            page = page ?? 1; //if (page == null) page = 1;

            // 5.3. Tạo kích thước trang (pageSize), mặc định là 5.
            int pageSize = (size ?? 5);

            ViewBag.pageSize = pageSize;

            // 6. Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber. --- dammio.com
            int pageNumber = (page ?? 1);

            // 6.2 Lấy tổng số record chia cho kích thước để biết bao nhiêu trang
            int checkTotal = (int)(kHACHHANGs.ToList().Count / pageSize) + 1;
            // Nếu trang vượt qua tổng số trang thì thiết lập là 1 hoặc tổng số trang
            if (pageNumber > checkTotal) pageNumber = checkTotal;

            // 7. Trả về các Link được phân trang theo kích thước và số trang.
            return View(kHACHHANGs.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost, HttpParamAction]
        public ActionResult Reset()
        {
            ViewBag.searchValue = "";
            Index(null, null, "", "", "");
            return View();
        }

        // GET: KHACHHANGs/Details/5
        public ActionResult Details(int? id)
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
            return View(kHACHHANG);
        }

        // GET: KHACHHANGs/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.TAIKHOANKHs, "MaKH", "UserName");
            return View();
        }

        // POST: KHACHHANGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKH,TenKH,Email,Tuoi,SDT,DiaChi,TongChiTieu,CapBac")] KHACHHANG kHACHHANG)
        {
            if (ModelState.IsValid)
            {
                db.KHACHHANGs.Add(kHACHHANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.TAIKHOANKHs, "MaKH", "UserName", kHACHHANG.MaKH);
            return View(kHACHHANG);
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
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.TAIKHOANKHs, "MaKH", "UserName", kHACHHANG.MaKH);
            return View(kHACHHANG);
        }

        // GET: KHACHHANGs/Delete/5
        public ActionResult Delete(int? id)
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
            return View(kHACHHANG);
        }

        // POST: KHACHHANGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Xóa tài khoản khách hàng trước khi xóa khách hàng
            TAIKHOANKH tAIKHOANKH = db.TAIKHOANKHs.Find(id);
            db.TAIKHOANKHs.Remove(tAIKHOANKH);

            KHACHHANG kHACHHANG = db.KHACHHANGs.Find(id);
            db.KHACHHANGs.Remove(kHACHHANG);
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


        public void XuatKH()
        {

            var list = db.KHACHHANGs.OrderByDescending(n => n.MaKH).ToList();

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "Mã khách hàng";
            Sheet.Cells["B1"].Value = "Họ và tên";
            Sheet.Cells["C1"].Value = "Email";
            Sheet.Cells["D1"].Value = "Tuổi";
            Sheet.Cells["E1"].Value = "Số điện thoại";
            Sheet.Cells["F1"].Value = "Địa chỉ";
            Sheet.Cells["G1"].Value = "Tổng chi tiêu(VNĐ)";
            Sheet.Cells["H1"].Value = "Cấp bậc:";
            Sheet.Cells["I1"].Value = "Tài khoản:";
            Sheet.Cells["J1"].Value = "Mật khẩu:";
            int row = 2;

            foreach (var item in list)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = item.MaKH;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.TenKH;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.Email;
                Sheet.Cells[string.Format("D{0}", row)].Value = item.Tuoi;
                Sheet.Cells[string.Format("E{0}", row)].Value = item.SDT;
                Sheet.Cells[string.Format("F{0}", row)].Value = item.DiaChi;
                Sheet.Cells[string.Format("G{0}", row)].Value = item.TongChiTieu;
                Sheet.Cells[string.Format("H{0}", row)].Value = item.CapBac;
                Sheet.Cells[string.Format("I{0}", row)].Value = item.TAIKHOANKH.UserName;
                Sheet.Cells[string.Format("J{0}", row)].Value = item.TAIKHOANKH.Pass;
                row++;
            }


            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Report.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();
        }
    }
}
