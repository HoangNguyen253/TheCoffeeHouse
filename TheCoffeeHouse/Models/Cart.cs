using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace TheCoffeeHouse.Models
{
    public class Cart
    {
        [PrimaryKey, AutoIncrement]
        public int IDCart { get; set; }
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
        public int GiaHienTai { get; set; }
        public string Size { get; set; }
    }
}
