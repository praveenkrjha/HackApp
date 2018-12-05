using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IndoorNavigation
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LocateItemPage : ContentPage
	{
        private Timer timer;
        private Timer myTimer;

        public static ConcurrentBag<BeaconModel> FoundBeacons = new ConcurrentBag<BeaconModel>();



        bool flag = false;
        void TimerCallback(object obj)
        {
            var frmNew = abs.Children[1] as Frame;
            if (!flag)
                frmNew.ScaleTo(0, 1000, Easing.CubicIn);
            else
                frmNew.ScaleTo(1, 1000, Easing.CubicIn);
            flag = !flag;
        }

        private void MyTimerCallback(object state)
        {
            if (FoundBeacons.Count >= 3)
            {
                timer = new Timer(TimerCallback, null, Timeout.Infinite, Timeout.Infinite);
                var e = FoundBeacons.ToList();
                foreach (var item in e)
                {
                    item.Rssi = (int)Utilities.CalcMedianRssi(item.Rssis);
                }
                FoundBeacons = new ConcurrentBag<BeaconModel>();
                //Add your logic to draw the location
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        if (abs.Children.Count > 2)
                            abs.Children.RemoveAt(2);
                        var dist1 = Utilities.CalculateDistanceFromRssi(e[0].Rssi);
                        var dist2 = Utilities.CalculateDistanceFromRssi(e[1].Rssi);
                        var dist3 = Utilities.CalculateDistanceFromRssi(e[2].Rssi);
                        var point = Utilities.GetLocationWithCenterOfGravity(new PointF(e[0].Major, e[0].Minor), new PointF(e[1].Major, e[1].Minor)
                            , new PointF(e[2].Major, e[2].Minor), dist1, dist2, dist3);
                        var yPer = ((point.Y / 720) * 100);
                        var y = (yPer / 100) * (AppConstants.FloorLength - 10);

                        var xPer = ((point.X / 1650) * 100);
                        var x = (xPer / 100) * (AppConstants.FloorWidth - 10);
                        Frame frmNew = new Frame();
                        frmNew.HeightRequest = AppConstants.PositionHeight;
                        frmNew.WidthRequest = AppConstants.PositionHeight;
                        frmNew.CornerRadius = (float)AppConstants.PositionHeight / 2;
                        frmNew.BackgroundColor = AppConstants.PositionColor;
                        abs.Children.Add(frmNew, new Rectangle(x, y, AppConstants.PositionHeight, AppConstants.PositionHeight));
                    }
                    catch
                    {

                    }
                });
            }

            timer = new Timer(TimerCallback, null, 2000, 2000);
        }

        public LocateItemPage (int xCoordinate, int yCoordinate)
		{
            timer = new Timer(TimerCallback, null, 1000, 1000);
            myTimer = new Timer(TimerCallback, null, 2000, 2000);

            InitializeComponent ();
            int xPlace = xCoordinate;
            int yPlace = yCoordinate;
            var yPer = ((yPlace / 720) * 100);
            var y = (yPer / 100) * (552 - 10);

            var xPer = ((xPlace / 1650) * 100);
            var x = (xPer / 100) * (240 - 10);
            var frmNew = new Frame();
            frmNew.HeightRequest = 30;
            frmNew.WidthRequest = 30;
            frmNew.CornerRadius = 15;
            frmNew.BackgroundColor = Color.Gold;
            abs.Children.Add(frmNew, new Rectangle(x, y, 30, 30));
        }

        
    }
}