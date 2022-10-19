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
    public partial class mh_TrangChu : ContentPage
    {
        List<TinTuc> dsTinTuc;
        public mh_TrangChu()
        {
            InitializeComponent();
            List<ImageItem> images = new List<ImageItem>()
            {
                new ImageItem(){Title="", Url="SlideImage_1.jpg"},
                new ImageItem(){Title="", Url="SlideImage_2.jpg"},
                new ImageItem(){Title="", Url="SlideImage_3.jpg"},
                new ImageItem(){Title="", Url="SlideImage_4.jpg"}
            };
            SlideImage.ItemsSource = images;
            Device.StartTimer(TimeSpan.FromSeconds(3), (Func<bool>)(() => {
                SlideImage.Position = (SlideImage.Position + 1) % (images.Count);
                return true;
            }));
            DateTime _now = DateTime.Now;
            int gio = 23 - Convert.ToInt32(_now.ToString("HH"));
            int phut = 59 - Convert.ToInt32(_now.ToString("mm"));
            int giay = 60 - Convert.ToInt32(_now.ToString("ss"));
            Device.StartTimer(TimeSpan.FromSeconds(1), (Func<bool>)(() =>
            {
                giay--;
                if (giay < 0)
                {
                    giay = 59;
                    phut--;
                    if (phut < 0)
                    {
                        gio--;
                        if (gio<0)
                        {
                            _now = DateTime.Now;
                            gio = 24 - Convert.ToInt32(_now.ToString("HH"));
                            phut = 60 - Convert.ToInt32(_now.ToString("mm"));
                            giay = 60 - Convert.ToInt32(_now.ToString("ss"));

                            GetNewDotKhuyenMai();
                        }    
                        else
                        {
                            remainTime.Text = timeText(gio, phut, giay);
                        }    
                    }
                    else
                    {
                        remainTime.Text = timeText(gio, phut, giay);
                    }    
                }
                else
                {
                    remainTime.Text = timeText(gio, phut, giay);
                }
                return true;
            }));
            SQLLiteDatabase dtb = new SQLLiteDatabase();
            dsTinTuc = dtb.GetTinTucs();
            BindableLayout.SetItemsSource(listTinTuc, dsTinTuc);
        }
        public async void GetNewDotKhuyenMai()
        {
            HttpClient httpClient = new HttpClient();

            var dotkhuyenmaiList_str = await httpClient.GetStringAsync("http://172.17.21.44/WebAPITheCoffeeHouse/api/XuLyController/GetDotKhuyenMai");
            List<DotKhuyenMai> dotkhuyenmaiList = JsonConvert.DeserializeObject<List<DotKhuyenMai>>(dotkhuyenmaiList_str);

            SQLLiteDatabase db = new SQLLiteDatabase();

            db.AddListDotKhuyenMai(dotkhuyenmaiList);
            renderSanPhamUuDai();
        }
        public void renderSanPhamUuDai()
        {
            SQLLiteDatabase db = new SQLLiteDatabase();
            List<SanPham> listSanPham = db.GetSanPham();
            List<DotKhuyenMai> listDotKhuyenMai = db.GetDotKhuyenMai();
            if (listDotKhuyenMai == null || listSanPham == null)
            {
                return;
            } 
                
            ListSanPhamUuDai.Children.Clear();
            for (int i = 0; i < listDotKhuyenMai.Count; i++)
            {
                for (int j = 0; j < listSanPham.Count; j++)
                {
                    if (listDotKhuyenMai[i].MaSP == listSanPham[j].MaSP)
                    {
                        Button button = new Button
                        {
                            BindingContext = listSanPham[j].MaSP,
                            Text = "Chọn",
                            BackgroundColor = Color.FromHex("#fef7e5"),
                            TextColor = Color.FromHex("#d88221"),
                            CornerRadius = 5,
                            Margin = new Thickness(3),
                            HeightRequest = 30,
                            Padding = new Thickness(0, 5)
                        };
                        Label labelGiaChuaGiam = new Label
                        {
                            Text = listSanPham[i].Gia.ToString() + "đ",
                            FontSize = 12,
                            VerticalOptions = LayoutOptions.End,
                            TextDecorations = TextDecorations.Strikethrough
                        };
                        Label labelGiaGiam = new Label
                        {
                            Text = (listSanPham[i].Gia - listDotKhuyenMai[i].GiamGia).ToString() + "đ",
                            TextColor = Color.Black
                        };
                        StackLayout stackLayout1 = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Padding = new Thickness(5, 0, 0, 0)
                        };
                        stackLayout1.Children.Add(labelGiaGiam);
                        stackLayout1.Children.Add(labelGiaChuaGiam);

                        Label labelTenSP = new Label
                        {
                            Text = listSanPham[i].TenSP,
                            FontAttributes = FontAttributes.Bold,
                            Padding = new Thickness(5, 0, 0, 0),
                            TextColor = Color.Black
                        };
                        Image image = new Image
                        {
                            Source = listSanPham[j].Img,
                        };
                        StackLayout stackLayout = new StackLayout();
                        stackLayout.Children.Add(image);
                        stackLayout.Children.Add(labelTenSP);
                        stackLayout.Children.Add(stackLayout1);
                        stackLayout.Children.Add(button);
                        Frame frame = new Frame
                        {
                            WidthRequest = 150,
                            VerticalOptions = LayoutOptions.Start,
                            Padding = new Thickness(0),
                            Margin = new Thickness(10, 0),
                            HasShadow = false,
                            CornerRadius = 5
                        };
                        frame.Content = stackLayout;
                        ListSanPhamUuDai.Children.Add(frame);
                    }
                }
            }
        }
        private string timeText(int hour, int minu, int seco)
        {
            string re = "";
            if (hour < 10)
            {
                re += "0" + hour;
            }
            else
            {
                re += hour;
            }
            re += ":";
            if (minu < 10)
            {
                re += "0" + minu;
            }
            else
            {
                re += minu;
            }
            re += ":";
            if (seco < 10)
            {
                re += "0" + seco;
            }
            else
            {
                re += seco;
            }
            return re;
        }
        //Sua
        protected override void OnAppearing()
        {
            base.OnAppearing();
            string _today = DateTime.Now.ToString("yyyy-MM-dd") + " 17:00:00";
            DateTime date1 = Convert.ToDateTime(_today);
            if ((int)(DateTime.Now - date1).TotalSeconds < 0)
            {
                HelloImg.Source = "sun.png";
                HelloText.Text = "Chào buổi sáng";
            }
            else
            {
                HelloImg.Source = "moon.png";
                HelloText.Text = "Chào buổi tối";
            }
            SQLLiteDatabase db = new SQLLiteDatabase();
            KhachHang kh = db.GetKhachHang();
            if (kh != null)
            {
                if (kh.HoTen!=null && kh.HoTen != "")
                {
                    lblUserName.Text = kh.HoTen.ToUpper();
                }
                else
                {
                    lblUserName.Text = kh.MaKH.ToUpper();
                }
                LoaiKhachHang LKH = db.GetLoaiKhachHangByMa(kh.LoaiKH);
                if (LKH!= null)
                {
                    lblUserAccum.Text = kh.TongDiemBean + " BEAN - " + LKH.TenLoaiKH;
                }
                if (kh.ImgQR == "" || kh.ImgQR == null)
                {
                    imgMaVach.Source = "MaVach.png";
                }
                else
                {
                    imgMaVach.Source = kh.ImgQR;
                }
            }
            renderSanPhamUuDai();
        }
        //Code mới
        private void btnViewChiTietTinTuc_Tapped(object sender, EventArgs e)
        {
            TinTuc TinTucSelectedItem = dsTinTuc.FirstOrDefault(itm => itm.MaTT == Convert.ToInt32(((TappedEventArgs)e).Parameter.ToString()));
            if (TinTucSelectedItem != null)
                Shell.Current.Navigation.PushAsync(new mh_ChiTietTinTuc(TinTucSelectedItem));
        }
    }
}