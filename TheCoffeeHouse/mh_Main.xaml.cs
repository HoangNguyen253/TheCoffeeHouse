using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TheCoffeeHouse.Models;
using Newtonsoft.Json;
using TheCoffeeHouse.Screen;

namespace TheCoffeeHouse
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class mh_Main : Shell
    {
        public mh_Main()
        {
            InitializeComponent();
            InitDanhSachSP();
        }
        async void InitDanhSachSP()
        {
            HttpClient httpClient = new HttpClient();

            var sanphamList_str = await httpClient.GetStringAsync("http://172.17.21.44/WebAPITheCoffeeHouse/api/XuLyController/Get_SanPhams");
            List<SanPham> sanphamList = JsonConvert.DeserializeObject<List<SanPham>>(sanphamList_str);

            var loaisanphamList_str = await httpClient.GetStringAsync("http://172.17.21.44/WebAPITheCoffeeHouse/api/XuLyController/Get_LoaiSanPhams");
            List<LoaiSanPham> loaisanphamList = JsonConvert.DeserializeObject<List<LoaiSanPham>>(loaisanphamList_str);

            var dotkhuyenmaiList_str = await httpClient.GetStringAsync("http://172.17.21.44/WebAPITheCoffeeHouse/api/XuLyController/GetDotKhuyenMai");
            List<DotKhuyenMai> dotkhuyenmaiList = JsonConvert.DeserializeObject<List<DotKhuyenMai>>(dotkhuyenmaiList_str);

            var sizeList_str = await httpClient.GetStringAsync("http://172.17.21.44/WebAPITheCoffeeHouse/api/XuLyController/GetSize");
            List<SP_SIZE> sizeList = JsonConvert.DeserializeObject<List<SP_SIZE>>(sizeList_str);

            var loaikhachhangList_str = await httpClient.GetStringAsync("http://172.17.21.44/WebAPITheCoffeeHouse/api/XuLyController/GetLoaiKhachHang");
            List<LoaiKhachHang> loaikhachhangList = JsonConvert.DeserializeObject<List<LoaiKhachHang>>(loaikhachhangList_str);

            var cuahangList_str = await httpClient.GetStringAsync("http://172.17.21.44/WebAPITheCoffeeHouse/api/XuLyController/Get_CuaHangs");
            List<CuaHangChiTiet> cuahangList = JsonConvert.DeserializeObject<List<CuaHangChiTiet>>(cuahangList_str);

            SQLLiteDatabase db = new SQLLiteDatabase();

            db.AddListLoaiSanPham(loaisanphamList);
            db.AddListSanPham(sanphamList);
            db.AddListDotKhuyenMai(dotkhuyenmaiList);
            db.AddListSize(sizeList);
            db.AddListLoaiKhachHang(loaikhachhangList);
            db.AddListCuaHang(cuahangList);


            //Code lan 2

            var tintucList_str = await httpClient.GetStringAsync("http://172.17.21.44/WebAPITheCoffeeHouse/api/XuLyController/Get_TinTucs");
            List<TinTuc> tintucList = JsonConvert.DeserializeObject<List<TinTuc>>(tintucList_str);
            db.AddListTinTuc(tintucList);
        }
    }
}