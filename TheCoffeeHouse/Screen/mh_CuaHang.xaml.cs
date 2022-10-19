using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TheCoffeeHouse.Models;
using Newtonsoft.Json;

namespace TheCoffeeHouse.Screen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class mh_CuaHang : ContentPage
    {
        public mh_CuaHang()
        {
            InitializeComponent();
            InitDSCuaHang();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitDSCuaHang();

        }
        void InitDSCuaHang()
        {
            SQLLiteDatabase database = new SQLLiteDatabase();
            List<CuaHangChiTiet> cuahangList = database.GetCuaHangChiTiets();
            List<CuaHangChiTiet> cuahangYeuThichList = database.GetCuaHangYeuThichs();
            List<CuaHangChiTiet> cuahangKhacList = new List<CuaHangChiTiet>();
            bool check = false;
            if (cuahangYeuThichList != null)
            {
                foreach (var ch in cuahangList)
                {
                    check = true;
                    foreach (var chyt in cuahangYeuThichList)
                    {
                        if (chyt.MaCH == ch.MaCH)
                        {
                            check = false;
                            break;
                        }
                    }
                    if (check)
                    {
                        cuahangKhacList.Add(ch);
                    }
                }
                lstDSCuaHangYeuThich.ItemsSource = cuahangYeuThichList;
                lstDSCuaHang.ItemsSource = cuahangKhacList;
                titleCuaHangYeuThich.IsVisible = false;
                lstDSCuaHangYeuThich.IsVisible = false;
                lstDSCuaHang.IsVisible = false;
                titleCuaHangYeuThich.IsVisible = true;
                lstDSCuaHangYeuThich.IsVisible = true;
                lstDSCuaHang.IsVisible = true;
            }
            else
            {
                titleCuaHangYeuThich.IsVisible = false;
                lstDSCuaHangYeuThich.IsVisible = false;
                lstDSCuaHang.ItemsSource = cuahangList;
            }
        }
        private void lstDSCuaHang_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            CuaHangChiTiet ch = (CuaHangChiTiet)e.Item;
            Shell.Current.Navigation.PushAsync(new mh_ChiTietCuaHang(ch, false));
        }

        private void lstDSCuaHangYeuThich_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            CuaHangChiTiet ch = (CuaHangChiTiet)e.Item;
            Shell.Current.Navigation.PushAsync(new mh_ChiTietCuaHang(ch, true));
        }
    }
}