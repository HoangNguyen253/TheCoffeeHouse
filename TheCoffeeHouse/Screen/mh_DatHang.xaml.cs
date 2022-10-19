using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TheCoffeeHouse.Models;

namespace TheCoffeeHouse.Screen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class mh_DatHang : ContentPage
    {
        SanPhamInGroup sanPhamSave = new SanPhamInGroup();
        string sizeSP = "";
        public mh_DatHang()
        {
            InitializeComponent();
            InitDanhSachSP();
            cartAmount.Text = "0";
            SQLLiteDatabase db = new SQLLiteDatabase();
            db.DeleteCart();
            db.DeleteDiaChiDonHang();

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            SQLLiteDatabase db = new SQLLiteDatabase();
            List<Cart> listCart = db.GetCart();
            if (listCart==null)
            {
                cartAmount.Text = "0";
            }
            else
            {
                int SoLuong = 0;
                foreach(Cart c in listCart)
                {
                    SoLuong += c.SoLuong;
                }
                cartAmount.Text = SoLuong.ToString();
            }
        }
        void InitDanhSachSP()
        {
            SQLLiteDatabase db = new SQLLiteDatabase();
            List<SanPham> listSanPham = db.GetSanPham();
            List<LoaiSanPham> listLoaiSanPham = db.GetLoaiSanPham();
            List<DotKhuyenMai> listDotKhuyenMai = db.GetDotKhuyenMai();

            List<GroupSanPham> listGroupSanPham = new List<GroupSanPham>();
            for (int i=0; i< listLoaiSanPham.Count; i++)
            {
                GroupSanPham gsp = new GroupSanPham();
                gsp.TenLoaiSP = listLoaiSanPham[i].TenLoaiSP;
                gsp.LoaiSP = listLoaiSanPham[i].LoaiSP;
                for (int j=0;j< listSanPham.Count;j++)
                {
                    if (listLoaiSanPham[i].LoaiSP == listSanPham[j].LoaiSP)
                    {
                        SanPhamInGroup spig = new SanPhamInGroup();
                        spig.MaSP = listSanPham[j].MaSP;
                        spig.TenSP = listSanPham[j].TenSP;
                        spig.MoTa = listSanPham[j].MoTa;
                        spig.Img = listSanPham[j].Img;
                        spig.Gia = listSanPham[j].Gia;
                        spig.GiaSauGiam = listSanPham[j].Gia;
                        bool kt = true;
                        for (int k = 0; k < listDotKhuyenMai.Count; k++)
                        {
                            if (listSanPham[j].MaSP == listDotKhuyenMai[k].MaSP)
                            {
                                kt = false;
                                spig.GiaSauGiam = spig.GiaSauGiam - listDotKhuyenMai[k].GiamGia;
                            }
                        }
                        spig.GiaSauGiamString = addDotForCurency(spig.GiaSauGiam.ToString()) + "đ";
                        if (kt)
                        {
                            spig.GiaString = "";
                        }
                        else
                        {
                            
                            spig.GiaString = addDotForCurency(spig.Gia.ToString()) + "đ";
                        }
                        gsp.Add(spig);
                    }
                }
                listGroupSanPham.Add(gsp);
            }
            lstSanPham.ItemsSource = listGroupSanPham;
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
        private async void lstSanPham_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            SQLLiteDatabase db = new SQLLiteDatabase();
            curAmount.Text = "1";
            sizeSP = "";
            SizeRadioGroup.Children.Clear();
            SanPhamInGroup sanPhamTapped = (SanPhamInGroup)e.Item;
            List<SP_SIZE> listSize = db.GetSizeSPbyMaSP(sanPhamTapped.MaSP);
            if (listSize != null)
            {
                for (int i=0;i< listSize.Count; i++)
                {
                    RadioButton rb = new RadioButton();
                    rb.FontSize = 16;
                    rb.CheckedChanged += RadioButton_CheckedChanged;
                    if (listSize[i].Size == "S")
                    {
                        rb.Content = "Nhỏ (" + addDotForCurency(listSize[i].TienThem.ToString())+ "đ)";
                        rb.Value = listSize[i].Size + "#" + listSize[i].TienThem.ToString();
                    }
                    if (listSize[i].Size == "L")
                    {
                        rb.Content = "Vừa (" + addDotForCurency(listSize[i].TienThem.ToString()) + "đ)";
                        rb.Value = listSize[i].Size + "#" + listSize[i].TienThem.ToString();
                    }
                    if (listSize[i].Size == "B")
                    {
                        rb.Content = "Lớn (" + addDotForCurency(listSize[i].TienThem.ToString()) + "đ)";
                        rb.Value = listSize[i].Size + "#" + listSize[i].TienThem.ToString();
                    }
                    if (i==0)
                    {
                        sizeSP = listSize[i].Size + "#" + listSize[i].TienThem.ToString();
                        rb.IsChecked = true;
                    }
                    SizeRadioGroup.Children.Add(rb);
                }
            }
            sanPhamSave = sanPhamTapped;
            BSImgSP.Source = sanPhamTapped.Img;
            BStenSP.Text = sanPhamTapped.TenSP;
            BSmoTa.Text = sanPhamTapped.MoTa;
            addToCart.Text = sanPhamTapped.GiaSauGiamString;
            if (Backdrop.Opacity == 0)
            {
                await OpenSheet();
            }
            else
            {
                await CloseSheet();
            }
        }
        private void subtractBtn_Clicked(object sender, EventArgs e)
        {
            int soluong = Convert.ToInt32(curAmount.Text);
            if (soluong>1)
            {
                soluong--;
                curAmount.Text = soluong.ToString();
            }    
        }
        private void addBtn_Clicked(object sender, EventArgs e)
        {
            int soluong = Convert.ToInt32(curAmount.Text);
            soluong++;
            curAmount.Text = soluong.ToString();
        }
        private async void addToCart_Clicked(object sender, EventArgs e)
        {
            SQLLiteDatabase db = new SQLLiteDatabase();
            Cart cart = new Cart()
            {
                MaSP = sanPhamSave.MaSP,
                TenSP = sanPhamSave.TenSP,
                GiaHienTai = sanPhamSave.GiaSauGiam,
                SoLuong = Convert.ToInt32(curAmount.Text),
                Size = sizeSP
            };
            if (db.AddToCart(cart))
            {
                int curAmount = Convert.ToInt32(cartAmount.Text);
                cartAmount.Text = (curAmount + cart.SoLuong).ToString();
            }
            if (Backdrop.Opacity == 0)
            {
                await OpenSheet();
            }
            else
            {
                await CloseSheet();
            }
        }
        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton rbtn = (RadioButton)sender;
            if (rbtn.IsChecked)
            {
                sizeSP = (string) rbtn.Value;
            }    
        }
        //BottomSheet
        uint duration = 300;
        double openY = (Device.RuntimePlatform == "Android") ? 20 : 60;
        double lastPanY = 0;
        bool isBackdropTapEnabled = true;
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (isBackdropTapEnabled)
            {
                await CloseSheet();
            }
        }
        private async void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Running)
            {
                isBackdropTapEnabled = false;
                lastPanY = e.TotalY;
                if (e.TotalY > 0)
                {
                    BottomSheet.TranslationY = openY + e.TotalY;
                }
            }
            else
            {
                if (lastPanY < 200)
                {
                    await OpenSheet();
                }
                else
                {
                    await CloseSheet();
                }
                isBackdropTapEnabled = true;
            }
        }
        async Task OpenSheet()
        {
            await Task.WhenAll
                (
                Backdrop.FadeTo(0.4, length: duration),
                BottomSheet.TranslateTo(0, openY, length: duration, easing: Easing.SinIn)
                );
            Backdrop.InputTransparent = false;
        }
        async Task CloseSheet()
        {
            await Task.WhenAll
                (
                Backdrop.FadeTo(0, length: duration),
                BottomSheet.TranslateTo(0, 860, length: duration, easing: Easing.SinIn)
                );
            Backdrop.InputTransparent = true;
        }
        private async void CloseBottomSheet_Clicked(object sender, EventArgs e)
        {
            if (Backdrop.Opacity == 0)
            {
                await OpenSheet();
            }
            else
            {
                await CloseSheet();
            }
        }
        private async void GoToCart_Clicked(object sender, EventArgs e)
        {
            var SLsender = (ImageButton)sender;

            SLsender.BackgroundColor = Color.Silver;
            await Task.Delay(30);
            SLsender.BackgroundColor = Color.Transparent;
            int sl = Convert.ToInt32(cartAmount.Text);
            if (sl != 0)
            {
                await Shell.Current.Navigation.PushAsync(new mh_ChiTietDonHang());
            }    
            else
            {
                await DisplayAlert("Lỗi", "Vui lòng chọn sản phẩm", "OK");
            }    
        }
    }
}