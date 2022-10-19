using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace TheCoffeeHouse.Models
{
    public class LoaiSanPham
    {
        [PrimaryKey]
        public int LoaiSP { get; set; }
        public string TenLoaiSP { get; set; }
    }
}
