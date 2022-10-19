using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheCoffeeHouse.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using TheCoffeeHouse.CusomRender;
[assembly: ExportRenderer(typeof(NoneBGEntry), typeof(NoneBGEntryRenderer))]
namespace TheCoffeeHouse.Droid
{
    public class NoneBGEntryRenderer : EntryRenderer
    {
        public NoneBGEntryRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);


            if (Control != null)
            {
                this.Control.SetBackground(null);
            }
        }
    }
}