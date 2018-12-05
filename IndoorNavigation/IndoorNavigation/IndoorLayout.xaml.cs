using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IndoorNavigation
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IndoorLayout : ContentPage
	{
		public IndoorLayout ()
		{
			InitializeComponent ();
            PointF point = Utilities.GetLocationWithCenterOfGravity(new PointF(829, 0), new PointF(414, 720), new PointF(1244, 720), 60, 100, 100);
            var yPer = ((point.Y / 150) * 100);
            var y = (yPer / 100) * (Application.Current.MainPage.Height-10);

            var xPer = ((point.X / 150) * 100);
            var x = (xPer / 100) * (Application.Current.MainPage.Width - 10);
            Frame frmNew = new Frame();
            frmNew.HeightRequest = 10;
            frmNew.WidthRequest = 10;
            frmNew.CornerRadius = 5;
            frmNew.BackgroundColor = Color.Blue;
            abs.Children.Add(frmNew, new Rectangle(x,y,10,10));
            MessagingCenter.Subscribe<DistanceModel, string>(this, "Data", (s, e) =>
             {
                 Device.BeginInvokeOnMainThread(() =>
                 {
                     var list = e.Split(',');
                     point = Utilities.GetLocationWithCenterOfGravity(new PointF(829, 0), new PointF(414, 720), new PointF(1244, 720), double.Parse(list[0]), double.Parse(list[1]), double.Parse(list[2]));
                     yPer = ((point.Y / 720) * 100);
                     y = (yPer / 100) * (552 - 10);

                     xPer = ((point.X / 1650) * 100);
                     x = (xPer / 100) * (240 - 10);
                     frmNew = new Frame();
                     frmNew.HeightRequest = 10;
                     frmNew.WidthRequest = 10;
                     frmNew.CornerRadius = 5;
                     frmNew.BackgroundColor = Color.Blue;
                     abs.Children.Add(frmNew, new Rectangle(y, x, 10, 10));

                 });
            });
		}

        void ChangeLoc(double Dist1, double Dist2, double Dist3)
        {
            var point= Utilities.GetLocationWithCenterOfGravity(new PointF(829, 0), new PointF(414, 720), new PointF(1244, 720),Dist1, Dist2, Dist3);
            var yPer = ((point.Y / 150) * 100);
            var y = (yPer / 100) * (Application.Current.MainPage.Height - 10);

            var xPer = ((point.X / 150) * 100);
            var x = (xPer / 100) * (Application.Current.MainPage.Width - 10);
            Frame frmNew = new Frame();
            frmNew.HeightRequest = 10;
            frmNew.WidthRequest = 10;
            frmNew.CornerRadius = 5;
            frmNew.BackgroundColor = Color.Blue;
            abs.Children.Add(frmNew, new Rectangle(x, y, 10, 10));
        }
	}
}