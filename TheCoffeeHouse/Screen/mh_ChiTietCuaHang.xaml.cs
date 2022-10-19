using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TheCoffeeHouse.Models;
using Xamarin.Essentials;
using Plugin.Share;
using Plugin.Share.Abstractions;

namespace TheCoffeeHouse.Screen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class mh_ChiTietCuaHang : ContentPage
    {
        CuaHangChiTiet cuaHangInPage;
        bool checkYeuThich;
        public mh_ChiTietCuaHang()
        {
            InitializeComponent();
        }
        public mh_ChiTietCuaHang(CuaHangChiTiet ch, bool checkYT)
        {
            InitializeComponent();
            detailCuaHang.BindingContext = ch;
            titleNameChiTietCuaHang.Text = ch.TenCH;
            cuaHangInPage = ch;
            checkYeuThich = checkYT;
            if (checkYeuThich)
            {
                heartYeuThich.BackgroundColor = Color.Orange;
                contentYeuThich.Text = "Cửa hàng ưa thích";
                contentYeuThich.TextColor = Color.Orange;
            }
        }

        private void optYeuThich_Tapped(object sender, EventArgs e)
        {
            CuaHangYeuThich chyt = new CuaHangYeuThich
            {
                MaCH = cuaHangInPage.MaCH,
                TenCH = cuaHangInPage.TenCH,
                MaDC = cuaHangInPage.MaDC,
                Img = cuaHangInPage.Img,
                KinhDo = cuaHangInPage.KinhDo,
                ViDo = cuaHangInPage.ViDo,
                Tinh = cuaHangInPage.Tinh,
                Quan = cuaHangInPage.Quan,
                Phuong = cuaHangInPage.Phuong,
                SoNhaDuong = cuaHangInPage.SoNhaDuong
            };
            SQLLiteDatabase dtb = new SQLLiteDatabase();
            if (checkYeuThich == false)
            {
                bool check = dtb.AddCuaHangYeuThich(chyt);
                if (check)
                {
                    Shell.Current.Navigation.PopAsync();
                }
            }
            else
            {
                int check_remove = dtb.RemoveCuaHangYeuThich(chyt);
                if (check_remove > 0)
                {
                    Shell.Current.Navigation.PopAsync();
                }
            }
        }

        private void optDiaChi_Tapped(object sender, EventArgs e)
        {
            var location = new Location(Convert.ToDouble(cuaHangInPage.KinhDo), Convert.ToDouble(cuaHangInPage.ViDo));
            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Default };
            Xamarin.Essentials.Map.OpenAsync(location, options);
        }

        private void optLienHe_Tapped(object sender, EventArgs e)
        {
            string sdt = sdtLienHe.Text.ToString();
            var s = sdt.Split(' ');
            Xamarin.Essentials.PhoneDialer.Open(s[2]);
        }

        private void optShare_Tapped(object sender, EventArgs e)
        {
            CrossShare.Current.Share(new ShareMessage
            {
                Text = "Hẹn bạn tại The Coffee House, https://www.google.com/maps/dir/Current+Location/" + cuaHangInPage.KinhDo.ToString() + ","
                + cuaHangInPage.ViDo.ToString() + " nhé"
            });
        }
    }
}