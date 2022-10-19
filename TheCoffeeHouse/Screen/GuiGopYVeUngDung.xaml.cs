using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TheCoffeeHouse.Models;
using System.Net.Http;

namespace TheCoffeeHouse.Screen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GuiGopYVeUngDung : ContentPage
    {
        KhachHang khInPage = new KhachHang();
        public GuiGopYVeUngDung()
        {
            InitializeComponent();
            SQLLiteDatabase dtb = new SQLLiteDatabase();
            khInPage = dtb.GetKhachHang();
        }

        private void exitLogin_Clicked(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PopAsync();
        }

        private async void btnGuiGopY_Clicked(object sender, EventArgs e)
        {
            string noiDungGopYMoi = noiDungGopY.Text;
            noiDungGopYMoi = noiDungGopYMoi.Trim();
            if (noiDungGopYMoi != "" && noiDungGopYMoi != null)
            {
                string noiDungEncode = HttpUtility.UrlEncode(noiDungGopYMoi);
                HttpClient httpClient = new HttpClient();
                var ketquaGopY_str = await httpClient.GetStringAsync("http://172.17.21.44/WebAPITheCoffeeHouse/api/XulyController/GuiGopY?" +
                    "makh=" + khInPage.MaKH + "&noidung=" + noiDungEncode);
                bool check = Convert.ToBoolean(ketquaGopY_str);
                if (check)
                {
                    await DisplayAlert("THÔNG BÁO", "Gửi góp ý thành công!", "OK");
                    noiDungGopY.Text = "";
                }
                else
                {
                    await DisplayAlert("THÔNG BÁO", "Gửi góp ý thất bại!", "OK");
                }
            }
        }
    }
}