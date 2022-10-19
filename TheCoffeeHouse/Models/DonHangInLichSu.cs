﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TheCoffeeHouse.Models
{
    public class DonHangInLichSu
    {
        public string MaDH { get; set; }
        public string MaKH { get; set; }
        public int PhuongThucNhan { get; set; }
        public int PhuongThucThanhToan { get; set; }
        public DateTime ThoiGianDat { get; set; }
        public string ThoiGianDatString { get; set; }
        public int GiamGiaLoaiKhachHang { get; set; }
        public int TongTien { get; set; }
        public int TrangThai { get; set; }
        public int MaCH { get; set; }
        public int DanhGiaSao { get; set; }
        public string DanhGiaText { get; set; }
        public int MaDC { get; set; }
        public string TenTrangThai { get; set; }
    }
}
