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
    public partial class ChooseCuaHang : ContentPage
    {
        public ChooseCuaHang()
        {
            InitializeComponent();
            InitDSCuaHang();
        }
        void InitDSCuaHang()
        {
            SQLLiteDatabase database = new SQLLiteDatabase();
            List<CuaHangChiTiet> cuahangList = database.GetCuaHangChiTiets();
            List<CuaHangChiTiet> cuahangYeuThichList = database.GetCuaHangYeuThichs();

            List<GroupCuaHang> groupCuaHangs = new List<GroupCuaHang>();

            if (cuahangYeuThichList != null)
            {
                //cửa hàng yêu thích
                GroupCuaHang gcp = new GroupCuaHang();
                gcp.tenLoaiGroup = "Cửa hàng yêu thích";
                gcp.colorLove = "Orange";
                gcp.AddRange(cuahangYeuThichList);
                groupCuaHangs.Add(gcp);

                //cửa hàng khác
                GroupCuaHang gcpKhac = new GroupCuaHang();
                gcpKhac.tenLoaiGroup = "Cửa hàng khác";
                gcpKhac.colorLove = "Gray";
                foreach (var ch in cuahangList)
                {
                    bool check = true;
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
                        gcpKhac.Add(ch);
                    }
                }
                groupCuaHangs.Add(gcpKhac);
            }
            else
            {
                GroupCuaHang gcpKhac = new GroupCuaHang();
                gcpKhac.tenLoaiGroup = "Cửa hàng khác";
                gcpKhac.AddRange(cuahangList);
                groupCuaHangs.Add(gcpKhac);
            }
            lstGroupCuaHang.ItemsSource = groupCuaHangs;
        }

        private async void lstGroupCuaHang_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            CuaHangChiTiet chct = e.Item as CuaHangChiTiet;
            SQLLiteDatabase db = new SQLLiteDatabase();
            DiaChiDonHang DCDH = new DiaChiDonHang 
            { 
                PhuongThucNhanHang = 1,
                MaCH=chct.MaCH,
                MaDC=chct.MaDC,
                Tinh=chct.Tinh,
                Quan=chct.Quan,
                Phuong=chct.Phuong,
                SoNhaDuong=chct.SoNhaDuong,
                tenDCDH=chct.TenCH
            };
            if (db.ChooseCuaHang(DCDH))
            {
                await Shell.Current.Navigation.PopAsync();
            } 
            else
            {
                await DisplayAlert("Lỗi", "Chọn cửa hàng thất bại.", "OK");
            }    
        }
    }
}