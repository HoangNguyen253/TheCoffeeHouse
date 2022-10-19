using System;
using System.Collections.Generic;
using System.Text;

namespace TheCoffeeHouse.Models
{
    public class GroupSanPham :List<SanPhamInGroup>
    {
        public int LoaiSP { get; set; }
        public string TenLoaiSP { get; set; }
    }
}
