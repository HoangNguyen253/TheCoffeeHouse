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
    public partial class ChooseDiaChi : ContentPage
    {
        public ChooseDiaChi()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            SQLLiteDatabase db = new SQLLiteDatabase();
            List<DiaChiNguoiDung> listDCND = db.GetDiaChiNguoiDung();
            listDiaChiNguoiDung.ItemsSource = listDCND;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var SLsender = (StackLayout)sender;
            Color curColor = SLsender.BackgroundColor;
            SLsender.BackgroundColor = Color.Silver;
            await Task.Delay(100);
            SLsender.BackgroundColor = curColor;

            HttpClient httpClient = new HttpClient();
            var tinhThanhList_str = await httpClient.GetStringAsync("http://172.17.21.44/WebAPITheCoffeeHouse/api/XuLyController/GetTinhThanh");
            List<province> tinhThanhList = JsonConvert.DeserializeObject<List<province>>(tinhThanhList_str);

            await Shell.Current.Navigation.PushAsync(new AddDiaChiMoi(tinhThanhList));
        }

        private void listDiaChiNguoiDung_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            DiaChiNguoiDung diaChiNguoiDung = (DiaChiNguoiDung)e.Item;
            DiaChiDonHang diaChiDonHang = new DiaChiDonHang 
            { 
                PhuongThucNhanHang = 2,
                Tinh = diaChiNguoiDung.Tinh,
                Quan = diaChiNguoiDung.Quan,
                Phuong = diaChiNguoiDung.Phuong,
                SoNhaDuong = diaChiNguoiDung.SoNhaDuong,
                tenDCDH = diaChiNguoiDung.tenDC
            };
            SQLLiteDatabase db = new SQLLiteDatabase();
            db.ChooseDiaChiNguoiDung(diaChiDonHang);
            Shell.Current.Navigation.PopAsync();
        }
    }
}