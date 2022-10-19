using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCoffeeHouse.Models;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;

namespace TheCoffeeHouse.Screen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        string GlobaleMail = "";
        public LoginPage()
        {
            InitializeComponent();
            checkLogin();
        }
        async void checkLogin()
        {
            SQLLiteDatabase db = new SQLLiteDatabase();
            if (db.CheckUserLogIn() != null)
            {
                await Shell.Current.GoToAsync($"//{nameof(mh_TrangChu)}");
            }
        }
        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        private int _minute = 3, _second = 0;
        private string timeText(int _minute, int __second)
        {
            string _result = "";
            if (_minute < 10)
            {
                _result += "0" + _minute;
            }
            else
            {
                _result += _minute;
            }
            _result += ":";
            if (_second < 10)
            {
                _result += "0" + _second;
            }
            else
            {
                _result += _second;
            }
            return _result;
        }
        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            btnLogin.IsEnabled = false;
            btnLogin.BackgroundColor = Color.Silver;
            string eMail = entryMail.Text;
            if (eMail == null)
            {
                btnLogin.IsEnabled = true;
                btnLogin.BackgroundColor = Color.FromHex("#ed7919");
                await DisplayAlert("Lỗi", "Địa chỉ email không hợp lệ", "OK");
                return;
            }
            if (IsValidEmail(eMail))
            {
                HttpClient httpClient = new HttpClient();

                var result_str = await httpClient.GetStringAsync("http://172.17.21.44/WebAPITheCoffeeHouse/api/XuLyController/GetOTP?eMail=" + eMail);
                bool result = Convert.ToBoolean(result_str);
                if (result)
                {
                    btnLogin.IsEnabled = true;
                    btnLogin.BackgroundColor = Color.FromHex("#ed7919");
                    _minute = 3;
                    _second = 0;
                    GlobaleMail = eMail;
                    textNoti.Text = "Một mã xác thực gồm 6 số đã được gửi về địa chỉ email " + eMail;
                    Device.StartTimer(TimeSpan.FromSeconds(1), (Func<bool>)(() =>
                    {
                        _second--;
                        if (_second < 0)
                        {
                            _second = 59;
                            _minute--;
                            if (_minute != 0)
                            {
                                remainTime.Text = timeText(_minute, _second);
                            }
                        }
                        else
                        {
                            remainTime.Text = timeText(_minute, _second);
                        }
                        return true;
                    }));
                    if (Backdrop.Opacity == 0)
                    {
                        await OpenSheet();
                    }
                    else
                    {
                        await CloseSheet();
                    }
                }
                else
                {
                    btnLogin.IsEnabled = true;
                    btnLogin.BackgroundColor = Color.FromHex("#ed7919");
                    await DisplayAlert("Lỗi", "Gửi mail không thành công, vui lòng thử lại", "OK");
                    return;
                }
            }
            else
            {

                btnLogin.BackgroundColor = Color.FromHex("#ed7919");
                btnLogin.IsEnabled = true;
                await DisplayAlert("Lỗi", "Địa chỉ email không hợp lệ", "OK");
                return;
            }
            btnLogin.IsEnabled = true;
            btnLogin.BackgroundColor = Color.FromHex("#ed7919");
        }
        private async void btnValidateOTP_Clicked(object sender, EventArgs e)
        {
            btnValidateOTP.IsEnabled = false;
            btnValidateOTP.BackgroundColor = Color.Silver;
            bool kt = false;
            string OTP = OTPValue.Text;
            if (OTP == null)
            {
                await DisplayAlert("Lỗi", "Mã OTP phải bao gồm 6 số", "OK");
                return;
            }
            if (OTP.Length < 6)
            {
                await DisplayAlert("Lỗi", "Mã OTP phải bao gồm 6 số", "OK");
                return;
            }
            HttpClient httpClient = new HttpClient();

            var result_str = await httpClient.GetStringAsync("http://172.17.21.44/WebAPITheCoffeeHouse/api/XuLyController/CheckOTP?eMail=" + GlobaleMail + "&OTP=" + OTP);
            string checkNull = (string)result_str;
            if (checkNull == "null")
            {
                await DisplayAlert("Lỗi", "Mã OTP không hợp lệ hoặc hết hiệu lực", "OK");
                return;
            }
            else
            {
                KhachHang kh = JsonConvert.DeserializeObject<KhachHang>(result_str);
                kh.MaKH = kh.MaKH.Trim();
                SQLLiteDatabase db = new SQLLiteDatabase();
                if (db.InsertKhachHang(kh))
                {
                    kt = true;
                    await Shell.Current.GoToAsync($"//{nameof(mh_TrangChu)}");
                }
                else
                {
                    await DisplayAlert("Lỗi", "Đăng nhập thất bại vui lòng thử lại", "OK");
                    return;
                }

            }
            if (kt==false)
            {
                btnValidateOTP.IsEnabled = true;
                btnValidateOTP.BackgroundColor = Color.FromHex("#ed7919");
            }    
        }
        //Bottom Sheet
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

    }
}