using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQuanLyQuanTraSua.Models
{
    public class ItemGioHang
    {
        public string MaMon { get; set; }
        public string TenMon { get; set; }
        
        public int SoLuong { get; set; }
        public int Gia { get; set; }
        public int ThanhTien { get; set; }
        public string HinhAnh { get; set; }

        public ItemGioHang(string iMaMon)
        {
            using (QLQuanTraSuaEntities db = new QLQuanTraSuaEntities())
            {
                this.MaMon = iMaMon;
                MENU mn = db.MENUs.Single(n => n.MaMon == iMaMon);
                this.TenMon = mn.TenMon;
                this.HinhAnh = mn.HinhAnh;
                this.Gia = mn.Gia;
                this.SoLuong = 1;
                this.ThanhTien = Gia * SoLuong;
            }
        }

        public ItemGioHang(string iMaMon, int sl)
        {
            using (QLQuanTraSuaEntities db = new QLQuanTraSuaEntities())
            {
                this.MaMon = iMaMon;
                MENU mn = db.MENUs.Single(n => n.MaMon == iMaMon);
                this.TenMon = mn.TenMon;
                this.HinhAnh = mn.HinhAnh;
                this.Gia = mn.Gia;
                this.SoLuong = sl;
                this.ThanhTien = Gia * SoLuong;
            }
        }

        public ItemGioHang()
        {

        }
    }
}