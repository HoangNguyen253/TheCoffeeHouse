using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace TheCoffeeHouse.Models
{
    public class KhachHang
    {
        [PrimaryKey]
        public string MaKH { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string SDT { get; set; }
        public string ImgQR { get; set; }
        public string ImgAvt { get; set; }
        public byte GioiTinh { get; set; }
        public string OTP { get; set; }
        public DateTime OTPTime { get; set; }
        public int TongTien { get; set; }
        public string LoaiKH { get; set; }
        public int TongDiemBean{ get; set; }
        public int  DiemBeanConLai{ get; set; }
    }
}
