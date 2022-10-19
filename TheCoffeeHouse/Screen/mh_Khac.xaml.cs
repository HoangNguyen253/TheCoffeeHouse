using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TheCoffeeHouse.Models;

namespace TheCoffeeHouse.Screen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class mh_Khac : ContentPage
    {
        public mh_Khac()
        {
            InitializeComponent();
        }

        private void CaiDatOption_Tapped(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new mh_CaiDat(), true);
            Shell.Current.Navigation.PushAsync(new mh_CaiDat());
        }
        private async void DangNhapOption_Tapped(object sender, EventArgs e)
        {
            var SLsender = (StackLayout)sender;

            SLsender.BackgroundColor = Color.Gray;
            await Task.Delay(50);
            SLsender.BackgroundColor = Color.White;
            SQLLiteDatabase db = new SQLLiteDatabase();
            db.DangXuat();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private void ThongTinCaNhanOption_Tapped(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PushAsync(new mh_ThongTinCaNhan());
        }
        private void LienHeGopYOption_Tapped(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PushAsync(new mh_LienHeGopY());
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PushAsync(new mh_LichSuDonHang());
        }
    }
}