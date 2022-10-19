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
using System.Net.Http.Headers;

namespace TheCoffeeHouse.Screen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class mh_ChiTietDonHang : ContentPage
    {
        int TongTienHoaDonChuaGiam = 0;
        int TongTienGiamHoaDon = 0;
        int TongTienHoaDon = 0;
        int SoLuongSanPham = 0;
        int phuongThucThanhToan = 0;
        int phuongThucNhanHang = 1;
        int ChietKhauKhachHang = 0;
        public mh_ChiTietDonHang()
        {
            InitializeComponent();
            
            SQLLiteDatabase db = new SQLLiteDatabase();
            List<Cart> listCart = db.GetCart();
            ChietKhauKhachHang = db.GetChietKhauKhachHang();
            int TongTien = 0;
            int soLuong = 0;
            for (int i = 0; i < listCart.Count; i++)
            {
                int GiaSizeThem = 0;
                string size = "";
                if (listCart[i].Size != "")
                {
                    List<string> ds = listCart[i].Size.Split('#').ToList();
                    GiaSizeThem = Convert.ToInt32(ds[1]);
                    size = ds[0];
                }

                if (size!="")
                {
                    if (size=="S")
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

                SwipeView swipeView = new SwipeView() 
                { 
                    BindingContext = listCart[i]
                };
                SwipeItem deleteSwipeItem = new SwipeItem
                {
                    BindingContext = swipeView,
                    Text = "Delete",
                    IconImageSource = "trash.png",
                    BackgroundColor = Color.Red
                };
                deleteSwipeItem.Invoked += SwipeItem_Invoked;

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
                    Text = listCart[i].SoLuong.ToString() + "x " + listCart[i].TenSP,
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
                    Text = addDotForCurency((listCart[i].GiaHienTai + GiaSizeThem).ToString()) + "đ",
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.Black,
                    Padding = new Thickness(0,0,15,0),
                    BackgroundColor=Color.White,
                    HorizontalTextAlignment = TextAlignment.Center
                };

                grid.Children.Add(rowOne, 0, 0);
                grid.Children.Add(rowTwo, 1, 0);

                swipeView.RightItems = new SwipeItems { deleteSwipeItem };
                swipeView.Content = grid;

                dsSanPham.Children.Add(swipeView);

                soLuong += listCart[i].SoLuong;
                TongTien += (listCart[i].GiaHienTai + GiaSizeThem) * listCart[i].SoLuong;
            }
            SoLuongSanPham = soLuong;
            TongTienHoaDonChuaGiam = TongTien;
            TongTienGiamHoaDon = (int) (TongTien / 100) * ChietKhauKhachHang;
            TongTienHoaDon = TongTien - TongTienGiamHoaDon;

            totalBillNotDiscount.Text = addDotForCurency(TongTienHoaDonChuaGiam.ToString()) + "đ";
            disCountBill.Text = addDotForCurency(TongTienGiamHoaDon.ToString()) + "đ";
            totalBill.Text = addDotForCurency(TongTienHoaDon.ToString()) + "đ";

            footerTotal.Text = addDotForCurency(TongTien.ToString()) + "đ";
            footerAmount.Text = soLuong.ToString() + " sản phẩm";
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            SQLLiteDatabase db = new SQLLiteDatabase();
            if (phuongThucNhanHang == 1)
            {
                DiaChiDonHang DCDH = db.GetCuaHangDonHang();
                if (DCDH != null)
                {
                    string TenCuaHang = DCDH.tenDCDH;
                    string DesCuaHang = DCDH.SoNhaDuong + ", " + DCDH.Phuong + ", " + DCDH.Quan + ", " + DCDH.Tinh;
                    lblNameAddress.Text = TenCuaHang;
                    lblDesAddress.Text = DesCuaHang;
                }
            }
            else
            {
                DiaChiDonHang DCDH = db.GetDiaChiNguoiDungGiaoHang();
                if (DCDH != null)
                {
                    string TenCuaHang = DCDH.tenDCDH;
                    string DesCuaHang = DCDH.SoNhaDuong + ", " + DCDH.Phuong + ", " + DCDH.Quan + ", " + DCDH.Tinh;
                    lblNameAddress.Text = TenCuaHang;
                    lblDesAddress.Text = DesCuaHang;
                }
            }    
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
        //Choose type Get
        private async void ChangeTypeGet_Clicked(object sender, EventArgs e)
        {
            if (phuongThucNhanHang == 1)
            {
                SelfGetChoose.BackgroundColor = Color.FromHex("#fff1cd");
            }
            else
            {
                SelfGetChoose.BackgroundColor = Color.White;
            }

            if (phuongThucNhanHang == 2)
            {
                ShipperChoose.BackgroundColor = Color.FromHex("#fff1cd");
            }
            else
            {
                ShipperChoose.BackgroundColor = Color.White;
            }
            Frame frame = ChooseTypeGet;
            if (Backdrop.Opacity == 0)
            {
                await OpenSheet(frame);
            }
            else
            {
                await CloseSheet(frame);
            }
        }
        private async void TapGestureRecognizer_Tapped_SelfGet(object sender, EventArgs e)
        {
            var SLsender = (StackLayout)sender;
            SLsender.BackgroundColor = Color.FromHex("#fff1cd");
            ShipperChoose.BackgroundColor = Color.White;

            phuongThucNhanHang = 1;

            SQLLiteDatabase db = new SQLLiteDatabase();
            DiaChiDonHang DCDH = db.GetCuaHangDonHang();
            if (DCDH != null)
            {
                string TenCuaHang = DCDH.tenDCDH;
                string DesCuaHang = DCDH.SoNhaDuong + ", " + DCDH.Phuong + ", " + DCDH.Quan + ", " + DCDH.Tinh;
                lblNameAddress.Text = TenCuaHang;
                lblDesAddress.Text = DesCuaHang;
            }
            else
            {
                lblTypeGet.Text = "Tự đến lấy";
                footerTypeGet.Text = "Tự đến lấy";
                lblNameAddress.Text = "Chưa có cửa hàng được chọn";
                lblDesAddress.Text = "Chưa có cửa hàng được chọn";
            }
            await CloseSheet(curFrame);
        }
        private async void TapGestureRecognizer_Tapped_Shipper(object sender, EventArgs e)
        {
            var SLsender = (StackLayout)sender;
            SLsender.BackgroundColor = Color.FromHex("#fff1cd");
            SelfGetChoose.BackgroundColor = Color.White;
            phuongThucNhanHang = 2;

            SQLLiteDatabase db = new SQLLiteDatabase();
            DiaChiDonHang DCDH = db.GetDiaChiNguoiDungGiaoHang();
            if (DCDH != null)
            {
                string TenCuaHang = DCDH.tenDCDH;
                string DesCuaHang = DCDH.SoNhaDuong + ", " + DCDH.Phuong + ", " + DCDH.Quan + ", " + DCDH.Tinh;
                lblNameAddress.Text = TenCuaHang;
                lblDesAddress.Text = DesCuaHang;
            }
            else
            {
                lblTypeGet.Text = "Giao hàng";
                footerTypeGet.Text = "Giao hàng";
                lblNameAddress.Text = "Chưa có địa chỉ giao hàng";
                lblDesAddress.Text = "Chưa có địa chỉ giao hàng";

            }
            await CloseSheet(curFrame);
        }
        //Choose type pay
        private async void TapGestureRecognizer_Tapped_ChoosePay(object sender, EventArgs e)
        {
            var SLsender = (StackLayout)sender;
            Color curColor = SLsender.BackgroundColor;
            SLsender.BackgroundColor = Color.Silver;
            await Task.Delay(100);
            SLsender.BackgroundColor = curColor;

            if (phuongThucThanhToan == 1)
            {
                MomoChoose.BackgroundColor = Color.FromHex("#fff1cd");
            }
            else
            {
                MomoChoose.BackgroundColor = Color.White;
            }

            if (phuongThucThanhToan == 2)
            {
                ZaloChoose.BackgroundColor = Color.FromHex("#fff1cd");
            }
            else
            {
                ZaloChoose.BackgroundColor = Color.White;
            }

            Frame frame = ChooseTypePay;
            if (Backdrop.Opacity == 0)
            {
                await OpenSheet(frame);
            }
            else
            {
                await CloseSheet(frame);
            }
        }
        private async void TapGestureRecognizer_Tapped_Momo(object sender, EventArgs e)
        {
            var SLsender = (StackLayout)sender;
            SLsender.BackgroundColor = Color.FromHex("#fff1cd");
            ZaloChoose.BackgroundColor = Color.White;

            PayImage.Source = "Momo.png";
            PayTypeTitle.Text = "Momo";
            PayTypeDescription.Text = "Ví điện tử Momo";
            phuongThucThanhToan = 1;

            await CloseSheet(curFrame);
        }
        private async void TapGestureRecognizer_Tapped_Zalo(object sender, EventArgs e)
        {
            var SLsender = (StackLayout)sender;
            SLsender.BackgroundColor = Color.FromHex("#fff1cd");
            MomoChoose.BackgroundColor = Color.White;

            PayImage.Source = "ZaloPay.png";
            PayTypeTitle.Text = "Zalo";
            PayTypeDescription.Text = "Ví điện tử Zalo Pay";
            phuongThucThanhToan = 2;

            await CloseSheet(curFrame);
        }

        //Swipe Invoke
        private async void SwipeItem_Invoked(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            SwipeView swipeView = item.BindingContext as SwipeView;
            var cart = swipeView.BindingContext as Cart;

            SQLLiteDatabase db = new SQLLiteDatabase();
            if (db.DeleteACart(cart))
            {
                dsSanPham.Children.Remove(swipeView);
                int GiaSizeThem = 0;
                if (cart.Size != "")
                {
                    List<string> ds = cart.Size.Split('#').ToList();
                    GiaSizeThem = Convert.ToInt32(ds[1]);
                }
                int SoTienGocItem = (cart.GiaHienTai + GiaSizeThem) * cart.SoLuong;
                TongTienHoaDonChuaGiam -= SoTienGocItem;
                TongTienGiamHoaDon -= (int)(SoTienGocItem / 100) * ChietKhauKhachHang;
                TongTienHoaDon -= (int)(SoTienGocItem / 100) * (100 - ChietKhauKhachHang);
                SoLuongSanPham -= cart.SoLuong;

                totalBillNotDiscount.Text = addDotForCurency(TongTienHoaDonChuaGiam.ToString()) + "đ";
                disCountBill.Text = addDotForCurency(TongTienGiamHoaDon.ToString()) + "đ";
                totalBill.Text = addDotForCurency(TongTienHoaDon.ToString()) + "đ";

                footerTotal.Text = addDotForCurency(TongTienHoaDon.ToString()) + "đ";
                footerAmount.Text = SoLuongSanPham.ToString() + " sản phẩm";

                await DisplayAlert("Thành công", "Xóa sản phẩm thành công", "OK");

                if (SoLuongSanPham==0)
                {
                    await Shell.Current.Navigation.PopAsync();
                }    
            }
            else
            {
                await DisplayAlert("Lỗi", "Xóa sản phẩm không thành công.", "OK");
            }
        }
        //BottomSheet
        Frame curFrame = new Frame();
        uint duration = 300;
        double openY = (Device.RuntimePlatform == "Android") ? 20 : 60;
        double lastPanY = 0;
        bool isBackdropTapEnabled = true;
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (isBackdropTapEnabled)
            {
                await CloseSheet(curFrame);
            }
        }
        private async void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            Frame frameSend = (Frame)sender;
            if (e.StatusType == GestureStatus.Running)
            {
                isBackdropTapEnabled = false;
                lastPanY = e.TotalY;
                if (e.TotalY > 0)
                {
                    frameSend.TranslationY = openY + e.TotalY;
                }
            }
            else
            {
                if (lastPanY < 120)
                {
                    await OpenSheet(frameSend);
                }
                else
                {
                    await CloseSheet(frameSend);
                }
                isBackdropTapEnabled = true;
            }
        }
        async Task OpenSheet(Frame frame)
        {
            curFrame = frame;
            await Task.WhenAll
                (
                Backdrop.FadeTo(0.4, length: duration),
                frame.TranslateTo(0, openY, length: duration, easing: Easing.SinIn)
                );
            Backdrop.InputTransparent = false;
        }
        async Task CloseSheet(Frame frame)
        {
            await Task.WhenAll
                (
                Backdrop.FadeTo(0, length: duration),
                frame.TranslateTo(0, 280, length: duration, easing: Easing.SinIn)
                );
            Backdrop.InputTransparent = true;
        }
        private async void CloseBottomSheet_Clicked(object sender, EventArgs e)
        {
            if (Backdrop.Opacity == 0)
            {
                await OpenSheet(curFrame);
            }
            else
            {
                await CloseSheet(curFrame);
            }
        }
        private async void TapGestureRecognizer_Tapped_ChooseAddress(object sender, EventArgs e)
        {
            var SLsender = (StackLayout)sender;
            Color curColor = SLsender.BackgroundColor;
            SLsender.BackgroundColor = Color.Silver;
            await Task.Delay(100);
            SLsender.BackgroundColor = curColor;
            if (phuongThucNhanHang == 1)
            {
                await Shell.Current.Navigation.PushAsync(new ChooseCuaHang());
            } 
            else
            {
                await Shell.Current.Navigation.PushAsync(new ChooseDiaChi());
            }    
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {

            List<CTDH> listCTDH = new List<CTDH>();
            DonHang donHang = new DonHang();
            DiaChiDonHang diaChiGiao = new DiaChiDonHang();

            SQLLiteDatabase db = new SQLLiteDatabase();
            KhachHang khachHang = db.GetKhachHang();
            donHang.MaKH = khachHang.MaKH;
            donHang.PhuongThucNhan = phuongThucNhanHang;
            donHang.GiamGiaLoaiKhachHang = TongTienGiamHoaDon;
            donHang.TongTien = TongTienHoaDon;
            donHang.TrangThai = 1;

            if (phuongThucThanhToan == 0)
            {
                await DisplayAlert("Lỗi", "Vui lòng chọn phương thức thanh toán", "OK");
                return;
            }
            else
            {
                donHang.PhuongThucThanhToan = phuongThucThanhToan;
            }

            List<Cart> listCart = db.GetCart();
            for (int i = 0; i < listCart.Count; i++)
            {
                
                int GiaSizeThem = 0;
                string size = "";
                if (listCart[i].Size != "")
                {
                    List<string> ds = listCart[i].Size.Split('#').ToList();
                    GiaSizeThem = Convert.ToInt32(ds[1]);
                    size = ds[0];
                }
                CTDH ctdh = new CTDH()
                {
                    MaSP = listCart[i].MaSP,
                    Size = size,
                    SoLuong = listCart[i].SoLuong,
                    ThanhTien = (listCart[i].GiaHienTai + GiaSizeThem) * listCart[i].SoLuong
                };
                listCTDH.Add(ctdh);
            }
 
            if (phuongThucNhanHang == 1)
            {
                DiaChiDonHang DCDH = db.GetCuaHangDonHang();
                if (DCDH != null)
                {
                    donHang.MaCH = DCDH.MaCH;
                    donHang.MaDC = DCDH.MaDC;
                }
                else
                {
                    await DisplayAlert("Lỗi", "Vui lòng chọn địa chỉ đến lấy", "OK");
                    return;
                }    
            }
            else
            {
                DiaChiDonHang DCDH = db.GetDiaChiNguoiDungGiaoHang();
                if (DCDH != null)
                {
                    diaChiGiao = DCDH;
                }
                else
                {
                    await DisplayAlert("Lỗi", "Vui lòng chọn địa chỉ giao hàng", "OK");
                    return;
                }
            }
            string donHangString = JsonConvert.SerializeObject(donHang);
            string listCTDHString = JsonConvert.SerializeObject(listCTDH);
            FormDatHang formDatHang = new FormDatHang();
            if (phuongThucNhanHang==1)
            {
                formDatHang.donHangString = donHangString;
                formDatHang.listCTDHString = listCTDHString;
                formDatHang.diaChiGiaoString = "";
            }
            else
            {
                string diaChiGiaoString = JsonConvert.SerializeObject(diaChiGiao);
                formDatHang.donHangString = donHangString;
                formDatHang.listCTDHString = listCTDHString;
                formDatHang.diaChiGiaoString = diaChiGiaoString;
            }
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://172.17.21.44/WebAPITheCoffeeHouse");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var uri = new Uri("http://172.17.21.44/WebAPITheCoffeeHouse/api/XuLyController/DatHang");
                    string serializedObject = JsonConvert.SerializeObject(formDatHang);
                    HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(uri, contentPost);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        if (responseString.ToString() == "1")
                        {
                            await DisplayAlert("Thành công", "Đặt hàng thành công", "OK");
                            db.DeleteCart();
                            db.DeleteDiaChiDonHang();
                            await Shell.Current.Navigation.PopAsync();
                        }
                        else
                        {
                            await DisplayAlert("Thất bại", "Đặt hàng thất bại, vui lòng thử lại", "OK");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void TapGestureRecognizer_Tapped_DeleteDonHang(object sender, EventArgs e)
        {
            SQLLiteDatabase db = new SQLLiteDatabase();
            bool answer = await DisplayAlert("Xóa đơn hàng", "Bạn có chắc muốn xóa đơn này", "OK", "Hủy");
            if (answer)
            {
                db.DeleteCart();
                db.DeleteDiaChiDonHang();
                await Shell.Current.Navigation.PopAsync();
            }
        }
    }
}