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
    public partial class AddDiaChiMoi : ContentPage
    {
        public AddDiaChiMoi()
        {
            InitializeComponent();
        }
        public AddDiaChiMoi(List<province> listProvince)
        {
            InitializeComponent();
            pickerTinh.ItemsSource = listProvince;
        }
        async Task RotateImageContinously(Image image)
        {
            int a = pickerTinh.SelectedIndex;
            while (true)
            {
                for (int i = 1; i < 7; i++)
                {
                    if (image.Rotation >= 360f) image.Rotation = 0;
                    await image.RotateTo(i * (360 / 6), 200, Easing.Linear);
                }
            }
        }
        public async void CallImage(Image image)
        {
            await RotateImageContinously(image);
        }

        private async void pickerTinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            successTinhImage.IsVisible = false;
            failTinhImage.IsVisible = false;
            province Tinh = pickerTinh.ItemsSource[pickerTinh.SelectedIndex] as province;
            string provinceid = Tinh.provinceid;
            HttpClient httpClient = new HttpClient();
            var quanHuyenList_str = await httpClient.GetStringAsync("http://172.17.21.44/WebAPITheCoffeeHouse/api/XuLyController/GetQuanOfTinh?provinceid="+ provinceid);
            sendingTinhImage.IsVisible = true;
            CallImage(sendingTinhImage);
            List<district> quanHuyenList = JsonConvert.DeserializeObject<List<district>>(quanHuyenList_str);
            if (quanHuyenList.Count!=0)
            {
                pickerQuan.ItemsSource = quanHuyenList;
                sendingTinhImage.IsVisible = false;
                successTinhImage.IsVisible = true;
            }    
            else
            {
                sendingTinhImage.IsVisible = false;
                failTinhImage.IsVisible = true;
            }    
            
        }

        private async void pickerQuan_SelectedIndexChanged(object sender, EventArgs e)
        {
            successQuanImage.IsVisible = false;
            failQuanImage.IsVisible = false;

            district Quan = pickerQuan.ItemsSource[pickerQuan.SelectedIndex] as district;
            string districtid = Quan.districtid;

            HttpClient httpClient = new HttpClient();
            var phuongXaList_str = await httpClient.GetStringAsync("http://172.17.21.44/WebAPITheCoffeeHouse/api/XuLyController/GetPhuongOfQuan?districtid=" + districtid);

            sendingQuanImage.IsVisible = true; 
            CallImage(sendingQuanImage);
            List<ward> phuongXaList = JsonConvert.DeserializeObject<List<ward>>(phuongXaList_str);
            if (phuongXaList.Count != 0)
            {
                pickerPhuong.ItemsSource = phuongXaList;
                sendingQuanImage.IsVisible = false;
                successQuanImage.IsVisible = true;
            }
            else
            {
                sendingQuanImage.IsVisible = false;
                failQuanImage.IsVisible = true;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (TenDiaChi.Text == null)
            {
                await DisplayAlert("Lỗi", "Vui lòng nhập tên địa chỉ này", "OK");
                return;
            }
            if (TenDiaChi.Text.Length == 0)
            {
                await DisplayAlert("Lỗi", "Vui lòng nhập tên địa chỉ này", "OK");
                return;
            }
            if (pickerTinh.SelectedIndex==-1)
            {
                await DisplayAlert("Lỗi", "Vui lòng chọn quận", "OK");
                return;
            }
            if (pickerQuan.SelectedIndex == -1)
            {
                await DisplayAlert("Lỗi", "Vui lòng chọn quận", "OK");
                return;
            }
            if (pickerPhuong.SelectedIndex == -1)
            {
                await DisplayAlert("Lỗi", "Vui lòng chọn quận", "OK");
                return;
            }
            if(SoNhaDuong.Text == null)
            {
                await DisplayAlert("Lỗi", "Vui lòng nhập số", "OK");
                return;
            }
            if (SoNhaDuong.Text.Length == 0)
            {
                await DisplayAlert("Lỗi", "Vui lòng nhập số nhà", "OK");
                return;
            }
            province Tinh = pickerTinh.ItemsSource[pickerTinh.SelectedIndex] as province;
            district Quan = pickerQuan.ItemsSource[pickerQuan.SelectedIndex] as district;
            ward Phuong = pickerPhuong.ItemsSource[pickerPhuong.SelectedIndex] as ward;
            DiaChiNguoiDung DCND = new DiaChiNguoiDung
            {

                Tinh = Tinh.name,
                Quan = Quan.name,
                Phuong = Phuong.name,
                SoNhaDuong = SoNhaDuong.Text,
                tenDC = TenDiaChi.Text,
            };
            SQLLiteDatabase db = new SQLLiteDatabase();
            db.InsertDiaChiNguoiDung(DCND);
            await Shell.Current.Navigation.PopAsync();
        }
    }
}