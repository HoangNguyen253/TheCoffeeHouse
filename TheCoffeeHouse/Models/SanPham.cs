using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace TheCoffeeHouse.Models
{
    public class SanPham
    {
        [PrimaryKey]
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string MoTa { get; set; }
        public string Img { get; set; }
        public int Gia { get; set; }
        public int LoaiSP { get; set; }
    }
}
