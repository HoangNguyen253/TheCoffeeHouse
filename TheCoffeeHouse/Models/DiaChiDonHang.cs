using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace TheCoffeeHouse.Models
{
    public class DiaChiDonHang
    {
        [PrimaryKey]
        public int PhuongThucNhanHang { get; set; }
        public int MaDC { get; set; }
        public int MaCH { get; set; }
        public string tenDCDH { get; set; }
        public string Tinh { get; set; }
        public string Quan { get; set; }
        public string Phuong { get; set; }
        public string SoNhaDuong { get; set; }
    }
}
