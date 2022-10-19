using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace TheCoffeeHouse.Models
{
    public class DiaChiNguoiDung
    {
        [PrimaryKey, AutoIncrement]
        public int MaDCND { get; set; }
        public string tenDC { get; set; }
        public string Tinh { get; set; }
        public string Quan { get; set; }
        public string Phuong { get; set; }
        public string SoNhaDuong { get; set; }
    }
}
