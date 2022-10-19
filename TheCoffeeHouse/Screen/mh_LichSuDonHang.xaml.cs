using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TheCoffeeHouse.Models;
using System.Net.Http;
using Newtonsoft.Json;
namespace TheCoffeeHouse.Screen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class mh_LichSuDonHang : ContentPage
    {
        public mh_LichSuDonHang()
        {
            InitializeComponent();
            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Init();
        }
        async void Init()
        {
            SQLLiteDatabase db = new SQLLiteDatabase();
            KhachHang kh = db.GetKhachHang();
            HttpClient httpClient = new HttpClient();
            var donhangList_str = await httpClient.GetStringAsync("http://172.17.21.44/WebAPITheCoffeeHouse/api/XuLyController/GetListDonHang?MaKH="+kh.MaKH);
            List<DonHang> donHangs = JsonConvert.DeserializeObject<List<DonHang>>(donhangList_str);
            List<DonHangInLichSu> donnHangInLichSus = new List<DonHangInLichSu>();
            for (int i=0;i<donHangs.Count;i++)
            {
                string tenTT = "";
                if (donHangs[i].TrangThai ==1)
                {
                    tenTT = "Đã thanh toán";
                }
                if (donHangs[i].TrangThai == 2)
                {
                    tenTT = "Đang giao";
                }
                if (donHangs[i].TrangThai == 3)
                {
                    tenTT = "Đã chế biến";
                }
                if (donHangs[i].TrangThai == 4)
                {
                    tenTT = "Hoàn thành";
                }
                donnHangInLichSus.Add(new DonHangInLichSu 
                { 
                    MaDH = donHangs[i].MaDH,
                    MaCH = donHangs[i].MaCH,
                    MaDC = donHangs[i].MaDC,
                    MaKH = donHangs[i].MaKH,
                    GiamGiaLoaiKhachHang = donHangs[i].GiamGiaLoaiKhachHang,
                    PhuongThucNhan = donHangs[i].PhuongThucNhan,
                    PhuongThucThanhToan = donHangs[i].PhuongThucThanhToan,
                    TrangThai = donHangs[i].TrangThai,
                    ThoiGianDat = donHangs[i].ThoiGianDat,
                    TongTien = donHangs[i].TongTien,
                    TenTrangThai = tenTT,
                    ThoiGianDatString = donHangs[i].ThoiGianDat.ToString("dd/MM/yyyy")
                });
            }
            lstDSDonHang.ItemsSource = donnHangInLichSus;
        }

        private async void lstDSDonHang_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListView lv = sender as ListView;
            DonHangInLichSu dh = lv.SelectedItem as DonHangInLichSu;
            HttpClient httpClient = new HttpClient();
            var cthdList_str = await httpClient.GetStringAsync("http://172.17.21.44/WebAPITheCoffeeHouse/api/XuLyController/GetCTDH?MaDH=" + dh.MaDH);
            List<CTDH> ChiTietDonHangs = JsonConvert.DeserializeObject<List<CTDH>>(cthdList_str);
            await Shell.Current.Navigation.PushAsync(new mh_XemLaiDonHang(dh, ChiTietDonHangs));
        }
    }
}