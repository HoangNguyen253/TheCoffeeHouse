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
    public partial class mh_XemLaiDonHang : ContentPage
    {
        public mh_XemLaiDonHang()
        {
            InitializeComponent();
        }
        public mh_XemLaiDonHang(DonHangInLichSu donHang, List<CTDH> CTDHs)
        {
            InitializeComponent();
            Init(donHang, CTDHs);
                
        }
        private string addDotForCurency(string giaTien)
        {
            string giaTienAfter = "";
            for (int i = 1; i <= giaTien.Length; i++)
            {
                giaTienAfter = giaTien[giaTien.Length - i] + giaTienAfter;
                if (i % 3 == 0 && i != giaTien.Length)
                {
                    giaTienAfter = "." + giaTienAfter;
                }
            }
            return giaTienAfter;
        }
        async void Init(DonHangInLichSu donHang, List<CTDH> CTDHs)
        {
            SQLLiteDatabase db = new SQLLiteDatabase();
            Title = "Chi tiết đơn hàng";
            DateBill.Text = "Đơn hàng ngày: " + donHang.ThoiGianDatString;
            DiaChiDonHang DCDH = new DiaChiDonHang();
            if (donHang.PhuongThucNhan == 2)
            {
                HttpClient httpClient = new HttpClient();
                var DCDH_str = await httpClient.GetStringAsync("http://172.17.21.44/WebAPITheCoffeeHouse/api/XuLyController/GetDiaChiDonHang?MaDC=" + donHang.MaDC);
                DCDH = JsonConvert.DeserializeObject<DiaChiDonHang>(DCDH_str);
                lblTypeGet.Text = "Tự đến lấy";
                lblNameAddress.Text = "Giao hàng tại địa chỉ";
                lblDesAddress.Text = DCDH.SoNhaDuong + ", " + DCDH.Phuong + ", " + DCDH.Quan + ", " + DCDH.Tinh;
            }
            else
            {
                CuaHangChiTiet chct = db.GetCuaHangByMa(donHang.MaCH);
                lblTypeGet.Text = "Đến lấy tại";
                lblNameAddress.Text = chct.TenCH;
                lblDesAddress.Text = chct.SoNhaDuong + ", " + chct.Phuong + ", " + chct.Quan + ", " + chct.Tinh;
            }
            totalBill.Text = addDotForCurency(donHang.TongTien.ToString()) + "đ";
            disCountBill.Text = addDotForCurency(donHang.GiamGiaLoaiKhachHang.ToString()) + "đ";
            totalBillNotDiscount.Text = addDotForCurency((donHang.GiamGiaLoaiKhachHang +donHang.TongTien).ToString()) + "đ";
            if (donHang.PhuongThucThanhToan==2)
            {
                PayImage.Source = "ZaloPay.png";
                PayTypeTitle.Text = "Zalo";
                PayTypeDescription.Text = "Ví điện tử Zalo Pay";
            }  
            else
            {
                PayImage.Source = "MoMo.png";
                PayTypeTitle.Text = "Momo";
                PayTypeDescription.Text = "Ví điện tử Momo";
            }
            if (donHang.TrangThai == 1)
            {
                ImageState.Source = "payDone.png";
                TitleState.Text = "Đã thanh toán";
                DescriptState.Text = "Đơn hàng đã được thanh toán xong, đang chờ nhân viên chế biến.";
            }
            if (donHang.TrangThai == 2)
            {
                ImageState.Source = "shipping.png";
                TitleState.Text = "Đang giao";
                DescriptState.Text = "Đơn hàng đã đang được giao tới địa chỉ của quý khách";
            }
            if (donHang.TrangThai == 3)
            {
                ImageState.Source = "readyBill.png";
                TitleState.Text = "Đã chế biến";
                DescriptState.Text = "Đơn hàng đã được chế biến xong, hãy tới cửa hàng lấy ngay nhé";
            }
            if (donHang.TrangThai == 4)
            {
                ImageState.Source = "approved.png";
                TitleState.Text = "Hoàn thành";
                DescriptState.Text = "Đơn hàng đã được thực hiện.";
            }

            for (int i = 0; i < CTDHs.Count; i++)
            {

                SanPham sanPham = db.GetSanPhamByMa(CTDHs[i].MaSP);
                string size = "";
                if (CTDHs[i].Size != "" && CTDHs[i].Size != null)
                {
                    size = CTDHs[i].Size;
                }

                if (size != "")
                {
                    if (size == "S")
                    {
                        size = "Nhỏ";
                    }
                    if (size == "L")
                    {
                        size = "Vừa";
                    }
                    if (size == "B")
                    {
                        size = "Lớn";
                    }
                }
                Grid grid = new Grid
                {
                    HeightRequest = 60,
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                    }
                };

                StackLayout rowOne = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Padding = new Thickness(4, 0),
                };
                Image imageInRowOne = new Image
                {
                    Source = "coffeeorange.png",
                    HeightRequest = 40,
                    VerticalOptions = LayoutOptions.Center
                };
                rowOne.Children.Add(imageInRowOne);

                StackLayout slInRowOne = new StackLayout();
                Label labelTenSanPham = new Label
                {
                    Text = CTDHs[i].SoLuong.ToString() + "x " + sanPham.TenSP,
                    FontSize = 16,
                    TextColor = Color.Black
                };
                slInRowOne.Children.Add(labelTenSanPham);
                Label labelSizeSP = new Label
                {
                    Text = size,
                };
                slInRowOne.Children.Add(labelSizeSP);

                rowOne.Children.Add(slInRowOne);
                Label rowTwo = new Label
                {
                    Text = addDotForCurency((CTDHs[i].ThanhTien).ToString()) + "đ",
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.Black,
                    Padding = new Thickness(0, 0, 15, 0),
                    BackgroundColor = Color.White,
                    HorizontalTextAlignment = TextAlignment.Center
                };

                grid.Children.Add(rowOne, 0, 0);
                grid.Children.Add(rowTwo, 1, 0);
                dsSanPham.Children.Add(grid);
            }
        }
    }
}