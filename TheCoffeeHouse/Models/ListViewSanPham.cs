using System;
using System.Collections.Generic;
using System.Text;

namespace TheCoffeeHouse.Models
{
    public class ListViewSanPham : List<SanPham>
    {
        public string LoaiSP { get; set; }
        public string TenLoaiSP { get; set; }
    }
}
