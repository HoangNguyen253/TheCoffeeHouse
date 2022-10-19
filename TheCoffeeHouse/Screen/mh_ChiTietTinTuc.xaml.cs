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
    public partial class mh_ChiTietTinTuc : ContentPage
    {
        public mh_ChiTietTinTuc()
        {
            InitializeComponent();
        }
        public mh_ChiTietTinTuc(TinTuc tt)
        {
            InitializeComponent();
            BindingContext = tt;
        }
    }
}