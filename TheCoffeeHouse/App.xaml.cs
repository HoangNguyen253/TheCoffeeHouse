using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TheCoffeeHouse.Models;

namespace TheCoffeeHouse
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SQLLiteDatabase db = new SQLLiteDatabase();
            db.Create();
            MainPage = new mh_Main();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
