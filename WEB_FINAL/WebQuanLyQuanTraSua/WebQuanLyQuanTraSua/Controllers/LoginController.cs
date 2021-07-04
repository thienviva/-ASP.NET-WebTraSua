using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebQuanLyQuanTraSua.Models;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit;

namespace WebQuanLyQuanTraSua.Controllers
{
    
    public class LoginController : Controller
    {

        // GET: Login
        public ActionResult Login(string taikhoan, string Pass)
        {
            using (var _context = new QLQuanTraSuaEntities())
            {
                
                TAIKHOANQL tql = new TAIKHOANQL();
                var kh = from u in _context.TAIKHOANKHs
                           where u.UserName == taikhoan && u.Pass == Pass
                           select u;
                var ql = from u in _context.TAIKHOANQLs
                           where u.UserName == taikhoan && u.Pass == Pass
                           select u;
                var nv = from u in _context.TAIKHOANNVs
                         where u.UserName == taikhoan && u.Pass == Pass
                         select u;

                
                if (ql.Count() == 1)
                {
                    Session["quyen"] = "1"; //Gán Session cho quản lý

                    Session["UserName"] = ql.First().QUANLY.TenQL;
                    Session["TaiKhoan"] = ql.First().QUANLY;
                    Session["HinhAnh"] = ql.First().QUANLY.HinhAnh;
                    Session["ID"] = ql.First().QUANLY.MaQL;
                    return RedirectToAction("Index", "QUANLies");
                }
                else
                {
                    if(nv.Count() == 1)
                    {


                        Session["quyen"] = "2";

                        Session["UserName"] = nv.First().NHANVIEN.TenNV;
                        Session["TaiKhoan"] = nv.First().NHANVIEN;
                        Session["ID"] = nv.First().NHANVIEN.MaNV;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        if (kh.Count() == 1)
                        {
                            Session["quyen"] = "3";

                            Session["UserName"] = kh.First().KHACHHANG.TenKH;
                            Session["TaiKhoan"] = kh.First().KHACHHANG;
                            Session["ID"] = kh.First().KHACHHANG.MaKH;
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            
                            return View();
                           
                            
                        }
                    }
                    
                }


            }
        }

        //Quên mật khẩu
        public ActionResult ForgotPass(string SDT, string Email)
        {
            using (var _context = new QLQuanTraSuaEntities())
            {
                KHACHHANG us = _context.KHACHHANGs.SingleOrDefault(n => n.SDT == SDT && n.Email == Email);
                if (us != null)
                {
                    VerifyEmail(SDT, Email);
                    return Content("true");

                } 
            }
            return Content("false");
        }

        public ActionResult VerifyEmail(string SDT, string Email)
        {
            //string Email = f["Email"].ToString();
            //string SDT = f["SDT"].ToString();
            Random matkhaumoi = new Random();
            int mk;
            if (ModelState.IsValid)
            {
                using (var _context = new QLQuanTraSuaEntities())
                {
                    var user = (from u in _context.KHACHHANGs
                                where u.Email == Email
                                select u).Single();
                    mk = matkhaumoi.Next(1000, 9999);
                    user.TAIKHOANKH.Pass = mk.ToString();
                    _context.SaveChanges();
                }
                var message = new MimeMessage();
                //From Address
                message.From.Add(new MailboxAddress("Nhut Thien", "thienviva@gmail.com"));
                //To Address
                message.To.Add(new MailboxAddress("Dot net", Email));
                //Subject
                message.Subject = "Hello";
                //Body
                message.Body = new TextPart("plain")
                {
                    Text = "Mật khẩu mới của quý khách là: " + mk.ToString()
                };
                //Configure send email
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("thienviva@gmail.com", "thien18110203");
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            return Content("true");
        }



    }
}