using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheCoffeeHouse.Screen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class mh_LienHeGopY : ContentPage
    {
        public mh_LienHeGopY()
        {
            InitializeComponent();
        }

        private void GuiGopYVeUngDung_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GuiGopYVeUngDung());
        }

        private void tongDaiOpt_Tapped(object sender, EventArgs e)
        {
            string sdt = sdtTongDai.Text.ToString();
            Xamarin.Essentials.PhoneDialer.Open(sdt);
        }

        private async void emailOpt_Tapped(object sender, EventArgs e)
        {
            List<string> tore = new List<string>();
            tore.Add(emaillienhe.Text.ToString());
            try
            {
                var message = new EmailMessage
                {
                    Subject = "Góp ý về ứng dụng",
                    To = tore
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device  
            }
            catch (Exception ex)
            {
                // Some other exception occurred  
            }
        }

        private async void websiteOpt_Tapped(object sender, EventArgs e)
        {
            string uri = "https://thecoffeehouse.com";
            try
            {
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
            }
        }
    }
}