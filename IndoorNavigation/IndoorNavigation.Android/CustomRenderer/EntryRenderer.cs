using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IndoorNavigation.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(BorderEntryRenderer))]
namespace IndoorNavigation.Droid.CustomRenderer
{
    public class BorderEntryRenderer : EntryRenderer
    {
        public BorderEntryRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                var nativeEditText = (EditText)Control;
                nativeEditText.TextAlignment = Android.Views.TextAlignment.Center;
                var shape = new ShapeDrawable(new Android.Graphics.Drawables.Shapes.OvalShape());
                shape.Paint.Color = Xamarin.Forms.Color.Black.ToAndroid();
                shape.Paint.SetStyle(Paint.Style.Stroke);
                nativeEditText.Background = shape;
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(Android.Graphics.Color.White);
                gd.SetCornerRadius(20);
                gd.SetStroke(2, Android.Graphics.Color.LightGray);
                //nativeEditText.SetPadding(10, 15, 3, 15);
                nativeEditText.SetBackground(gd);
            }
        }
    }
}