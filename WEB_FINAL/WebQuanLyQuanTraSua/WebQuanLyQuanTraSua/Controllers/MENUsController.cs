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
using System.ComponentModel;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Web;
using OfficeOpenXml;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Microsoft.Ajax.Utilities;
using EPPlusTest;



namespace WebQuanLyQuanTraSua.Controllers
{
    public class MENUsController : Controller
    {
        private QLQuanTraSuaEntities db = new QLQuanTraSuaEntities();
        [HttpGet]

        //Tạo 2 partial View 1 và 2 để hiện thị
        [ChildActionOnly]
        public ActionResult MENUStyle1Partial()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult MENUStyle2artial()
        {
            return PartialView();
        }


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
        // GET: MENUs
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
            var properties = typeof(MENU).GetProperties();
            List<Tuple<string, bool, int>> list = new List<Tuple<string, bool, int>>();
            foreach (var item in properties)
            {
                int order = 999;
                var isVirtual = item.GetAccessors()[0].IsVirtual;

                if (item.Name == "TenMon") order = 2;
                if (item.Name == "MaMon") order = 1;
                if (item.Name == "Gia") order = 3;
                if (item.Name == "SoLuongBan") order = 4;
                if (item.Name == "MaLoai") continue; // Không hiển thị cột này
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
            var mENUs = from l in db.MENUs
                        select l;

            // 5. Tạo thuộc tính sắp xếp mặc định là "MaMon"
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "MaMon";

            // 5. Sắp xếp tăng/giảm bằng phương thức OrderBy sử dụng trong thư viện Dynamic LINQ
            if (sortOrder == "desc") mENUs = mENUs.OrderBy(sortProperty + " desc");
            else if (sortOrder == "asc") mENUs = mENUs.OrderBy(sortProperty);
            else mENUs = mENUs.OrderBy("MaMon");

            // 5.1. Thêm phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                mENUs = mENUs.Where(s => s.TenMon.Contains(searchString));
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
            int checkTotal = (int)(mENUs.ToList().Count / pageSize) + 1;
            // Nếu trang vượt qua tổng số trang thì thiết lập là 1 hoặc tổng số trang
            if (pageNumber > checkTotal) pageNumber = checkTotal;

            // 7. Trả về các Link được phân trang theo kích thước và số trang.
            return View(mENUs.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost, HttpParamAction]
        public ActionResult Reset()
        {
            ViewBag.searchValue = "";
            Index(null, null, "", "", "");
            return Content("/MENUs/Index");
        }


        // GET: MENUs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MENU mENU = db.MENUs.Find(id);
            if (mENU == null)
            {
                return HttpNotFound();
            }
            return View(mENU);
        }

        // GET: MENUs/Create
        public ActionResult Create()
        {
            ViewBag.MaLoai = new SelectList(db.LOAIMENUs, "MaLoai", "TenLoai");
            return View();
        }

        // POST: MENUs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaMon,MaLoai,TenMon,Gia,SoLuongDaBan,HinhAnh")] MENU mENU)
        {
            if (ModelState.IsValid)
            {
                db.MENUs.Add(mENU);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoai = new SelectList(db.LOAIMENUs, "MaLoai", "TenLoai", mENU.MaLoai);
            return View(mENU);
        }

        // GET: MENUs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MENU mENU = db.MENUs.Find(id);
            if (mENU == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoai = new SelectList(db.LOAIMENUs, "MaLoai", "TenLoai", mENU.MaLoai);
            return View(mENU);
        }

        // POST: MENUs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaMon,MaLoai,TenMon,Gia,SoLuongDaBan,HinhAnh")] MENU mENU)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mENU).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoai = new SelectList(db.LOAIMENUs, "MaLoai", "TenLoai", mENU.MaLoai);
            return View(mENU);
        }

        // GET: MENUs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MENU mENU = db.MENUs.Find(id);
            if (mENU == null)
            {
                return HttpNotFound();
            }
            return View(mENU);
        }

        // POST: MENUs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MENU mENU = db.MENUs.Find(id);
            db.MENUs.Remove(mENU);
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

        public void ExportMENU()
        {

            var list = db.MENUs.OrderByDescending(n => n.MaLoai).ToList();

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "Mã món";
            Sheet.Cells["B1"].Value = "Tên món";
            Sheet.Cells["C1"].Value = "Loại";
            Sheet.Cells["D1"].Value = "Giá tiền(VNĐ)";
            Sheet.Cells["E1"].Value = "Hình ảnh";
            Sheet.Cells["F1"].Value = "Số lượng đã bán";
            int row = 2;

            foreach (var item in list)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = item.MaMon;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.TenMon;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.MaLoai;
                Sheet.Cells[string.Format("D{0}", row)].Value = item.Gia;
                Sheet.Cells[string.Format("E{0}", row)].Value = item.HinhAnh;
                Sheet.Cells[string.Format("F{0}", row)].Value = item.SoLuongDaBan;
                row++;
            }


            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Report.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();
        }

        public ActionResult ImportExport(string DuongDan)
        {
            ViewBag.A = DuongDan;
            return View();
        }

        [HttpPost]
        public ActionResult ImportFromExcel(HttpPostedFileBase postedFile)
        {
            if (ModelState.IsValid)
            {
                if (postedFile != null && postedFile.ContentLength > (1024 * 1024 * 50))  // 50MB limit  
                {
                    ModelState.AddModelError("postedFile", "Your file is to large. Maximum size allowed is 50MB !");
                }

                else
                {
                    string filePath = string.Empty;
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //For Excel 97-03.  
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //For Excel 07 and above.  
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                    }

                    try
                    {
                        DataTable dt = new DataTable();
                        conString = string.Format(conString, filePath);

                        using (OleDbConnection connExcel = new OleDbConnection(conString))
                        {
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    cmdExcel.Connection = connExcel;

                                    //Get the name of First Sheet.  
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.  
                                    connExcel.Open();
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dt);
                                    connExcel.Close();
                                }
                            }
                        }
                        foreach (var item in dt.AsEnumerable())
                        {
                            if (ModelState.IsValid)
                            {
                                MENU luutru = new MENU();

                                luutru.MaMon = item.Field<string>("Mã món");
                                luutru.TenMon = item.Field<string>("Tên món");
                                luutru.HinhAnh = item.Field<string>("Hình ảnh");
                                luutru.Gia = int.Parse(item.Field<string>("Giá tiền(VNĐ)"));
                                luutru.MaLoai = int.Parse(item.Field<string>("Loại"));
                                luutru.SoLuongDaBan = int.Parse(item.Field<string>("Số lượng đã bán"));

                                db.MENUs.Add(luutru);
                                db.SaveChanges();

                            }


                        }
                        return RedirectToAction("Index", "MENUs");
                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("Index", "MENUs");
                    }

                }
            }
            return Json("Bạn chưa chọn File!");
        }

    }
}
