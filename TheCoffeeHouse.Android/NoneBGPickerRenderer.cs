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
[assembly: ExportRenderer(typeof(NoneBGPicker), typeof(NoneBGPickerRenderer))]
namespace TheCoffeeHouse.Droid
{
    class NoneBGPickerRenderer: PickerRenderer
    {
        public NoneBGPickerRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);


            if (Control != null)
            {
                this.Control.SetBackground(null);
            }
        }
    }
}