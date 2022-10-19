using System;
using System.Collections.Generic;
using System.Text;

namespace TheCoffeeHouse.Models
{
    public class GroupCuaHang : List<CuaHangChiTiet>
    {
        public string tenLoaiGroup { get; set; }
        public string colorLove { get; set; }
    }
}
