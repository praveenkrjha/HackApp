using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IndoorNavigation.CustomControls;
using IndoorNavigation.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GradientColorStack), typeof(GradientColorStackRenderer))]
namespace IndoorNavigation.Droid.CustomRenderer
{
    public class GradientColorStackRenderer : VisualElementRenderer<StackLayout>
    {
        private Color StartColor { get; set; }
        private Color EndColor { get; set; }
        public GradientColorStackRenderer(Context context) : base(context)
        {
        }

        protected override void DispatchDraw(global::Android.Graphics.Canvas canvas)
        {
            #region for Horizontal Gradient
            //var gradient = new Android.Graphics.LinearGradient(0, 0, Width, 0,
            #endregion

            #region for Vertical Gradient
            var gradient = new Android.Graphics.LinearGradient(0, 0, 0, Height,
            #endregion

                   this.StartColor.ToAndroid(),
                   this.EndColor.ToAndroid(),
                   Android.Graphics.Shader.TileMode.Mirror);

            var paint = new Android.Graphics.Paint()
            {
                Dither = true,
            };
            paint.SetShader(gradient);
            canvas.DrawPaint(paint);
            base.DispatchDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }
            try
            {
                var stack = e.NewElement as GradientColorStack;
                this.StartColor = stack.StartColor;
                this.EndColor = stack.EndColor;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"ERROR:", ex.Message);
            }
        }
    }
}