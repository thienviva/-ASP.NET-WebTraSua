using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQuanLyQuanTraSua.Models;

namespace WebQuanLyQuanTraSua.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult Registration(string name, string diachi, string sdt, string taikhoan, string pass, string passagain, string email)
        {
            using (var _context = new QLQuanTraSuaEntities())
            {
                TAIKHOANKH aKH = new TAIKHOANKH();
                KHACHHANG KH = new KHACHHANG();


                var kh = from u in _context.TAIKHOANKHs
                         where u.UserName == taikhoan && u.Pass == pass
                         select u;
                var ql = from u in _context.TAIKHOANQLs
                         where u.UserName == taikhoan && u.Pass == pass
                         select u;
                var nv = from u in _context.TAIKHOANNVs
                         where u.UserName == taikhoan && u.Pass == pass
                         select u;

                var tpQuery = (from kh1 in _context.KHACHHANGs
                               select kh1).ToList();

                int tongkh = tpQuery.Count();
                int makh = tongkh + 1;


                if (ql.Count() == 1)
                {
                    ViewBag.user = "Tài khoản đã có người sử dụng!";
                    return View();
                }
                else
                {
                    if (nv.Count() == 1)
                    {

                        ViewBag.user = "Tài khoản đã có người sử dụng!";
                        return View();
                    }
                    else
                    {
                        if (kh.Count() == 1)
                        {
                            ViewBag.user = "Tài khoản đã có người sử dụng!";
                            return View();
                        }
                        else if (pass == passagain && pass != "" && pass != null && taikhoan != "" && taikhoan != null)
                        {
                            KH.MaKH = makh;
                            KH.TenKH = name;
                            KH.Email = email;
                            KH.DiaChi = diachi;
                            KH.SDT = sdt;
                            KH.CapBac = "Nước";
                            KH.TongChiTieu = 0;
                            aKH.MaKH = makh;
                            aKH.UserName = taikhoan;
                            aKH.Pass = pass;
                            _context.KHACHHANGs.Add(KH);
                            _context.TAIKHOANKHs.Add(aKH);
                            _context.SaveChanges();
                            return RedirectToAction("Login", "Login");
                        }
                        else
                        {
                            return View();
                        }
                    }

                }

                

                //if (pass == passagain && pass != "" && pass != null && taikhoan != "" && taikhoan != null)
                //{
                //    KH.MaKH = makh;
                //    KH.TenKH = name;
                //    KH.Email = email;
                //    KH.DiaChi = diachi;
                //    KH.SDT = sdt;
                //    KH.CapBac = "Nước";
                //    KH.TongChiTieu = 0;
                //    aKH.MaKH = makh;
                //    aKH.UserName = taikhoan;
                //    aKH.Pass = pass;
                //    _context.KHACHHANGs.Add(KH);
                //    _context.TAIKHOANKHs.Add(aKH);
                //    _context.SaveChanges();
                //    return RedirectToAction("Login", "Login");
                //}
                //else
                //{
                //    return View();
                //}
            }
        }
    }
}