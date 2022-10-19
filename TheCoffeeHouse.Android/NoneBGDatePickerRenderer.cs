using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheCoffeeHouse.CusomRender;
using TheCoffeeHouse.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NoneBGDatePicker), typeof(NoneBGDatePickerRenderer))]
namespace TheCoffeeHouse.Droid
{
    public class NoneBGDatePickerRenderer: DatePickerRenderer
    {
        public NoneBGDatePickerRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);


            if (Control != null)
            {
                this.Control.SetBackground(null);
            }
        }
    }
}