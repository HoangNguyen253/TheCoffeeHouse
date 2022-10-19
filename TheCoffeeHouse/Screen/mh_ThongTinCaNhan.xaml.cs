using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TheCoffeeHouse.Models;
using System.Net.Http;
using Xamarin.Essentials;

namespace TheCoffeeHouse.Screen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class mh_ThongTinCaNhan : ContentPage
    {
        KhachHang khInPage = new KhachHang();
        public mh_ThongTinCaNhan()
        {
            InitializeComponent();
            List<string> dsGioiTinh = new List<string>();
            dsGioiTinh.Add("Nam");
            dsGioiTinh.Add("Nữ");
            GioiTinhNguoiDung.ItemsSource = dsGioiTinh;
            Read_Data_User();
        }
        public void Read_Data_User()
        {
            SQLLiteDatabase db = new SQLLiteDatabase();
            khInPage = db.GetKhachHang();
            if (khInPage.HoTen != "" && khInPage.HoTen != null)
            {
                HoTenNguoiDung.Text = khInPage.HoTen;
            }
            if (!khInPage.SDT.ToString().Contains(" "))
            {
                SDTNguoiDung.Text = khInPage.SDT;
            }
            NgaySinhNguoiDung.Date = khInPage.NgaySinh;
            if (khInPage.GioiTinh == 1)
            {
                GioiTinhNguoiDung.SelectedIndex = 0;
            }
            else GioiTinhNguoiDung.SelectedIndex = 1;
        }

        private async void capNhatTaiKhoanbtn_Clicked(object sender, EventArgs e)
        {
            string hoTenKhachHang = HoTenNguoiDung.Text;
            string sdtKhachHang = SDTNguoiDung.Text;
            string ngaySinhKhachHang = NgaySinhNguoiDung.Date.ToString("yyyy-MM-dd");
            int gioiTinhKhachHang = -1;
            if (GioiTinhNguoiDung.SelectedIndex >= 0)
            {
                gioiTinhKhachHang = GioiTinhNguoiDung.SelectedItem.ToString() == "Nam" ? 1 : 0;
            }
            if (hoTenKhachHang != "" && sdtKhachHang != "" && gioiTinhKhachHang != -1)
            {
                hoTenKhachHang = hoTenKhachHang.Trim();
                hoTenKhachHang = hoTenKhachHang.Replace(' ', '+');
                HttpClient httpClient = new HttpClient();
                var ketquaCapNhat_str = await httpClient.GetStringAsync("http://172.17.21.44/WebAPITheCoffeeHouse/api/XulyController/Update_KhachHang?" +
                    "makh=" + khInPage.MaKH + "&hoten=" + hoTenKhachHang + "&sdt=" + sdtKhachHang + "&ngaysinh=" + ngaySinhKhachHang + "&gioitinh=" + gioiTinhKhachHang.ToString());
                bool check = Convert.ToBoolean(ketquaCapNhat_str);
                if (check)
                {
                    hoTenKhachHang = hoTenKhachHang.Replace('+', ' ');
                    SQLLiteDatabase dtb = new SQLLiteDatabase();
                    int checklite = dtb.CapNhatThongTinKhachHang(khInPage.MaKH, hoTenKhachHang, sdtKhachHang, ngaySinhKhachHang, gioiTinhKhachHang);
                    if (checklite > 0)
                    {
                        await DisplayAlert("THÔNG BÁO", "Cập nhật thành công!", "OK");
                        Read_Data_User();
                    }
                    else
                    {
                        await DisplayAlert("THÔNG BÁO", "Cập nhật thất bại!", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("THÔNG BÁO", "Cập nhật thất bại!", "OK");
                }
            }
        }

        //private async void userAvatar_Clicked(object sender, EventArgs e)
        //{
        //    var file = await MediaPicker.PickPhotoAsync();
        //    if (file == null)
        //        return;
        //    var content = new MultipartFormDataContent();
        //    content.Add(new StreamContent(await file.OpenReadAsync()), "file", file.FileName);
        //    HttpClient httpClient = new HttpClient();
        //    string url = "http://172.17.21.44/WebAPITheCoffeeHouse/api/XulyController/Upload_Avatar_User";
        //    httpClient.BaseAddress = new Uri(url);

        //    var response = await httpClient.PostAsync("", content);
        //    string a = await response.Content.ReadAsStringAsync();

        //    await DisplayAlert("TB", a, "OK");
        //}
    }
}