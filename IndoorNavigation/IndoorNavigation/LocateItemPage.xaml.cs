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

        //public static ConcurrentBag<BeaconModel> FoundBeacons = new ConcurrentBag<BeaconModel>();



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
            if (IndoorLayout.FoundBeacons.Count >= 3)
            {
                myTimer.Change(Timeout.Infinite, Timeout.Infinite);
                var e = IndoorLayout.FoundBeacons.ToList();
                foreach (var item in e)
                {
                    item.Rssi = (int)Utilities.CalcMedianRssi(item.Rssis);
                }
                IndoorLayout.FoundBeacons = new ConcurrentBag<BeaconModel>();
                //Add your logic to draw the location
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        if (abs.Children.Count > 2)
                            abs.Children.RemoveAt(2);
                        var point = IndoorLayout.GetMedianPoint(e[0], e[1], (e.Count == 3 ? e[2] : null));
                        if (e.Count == 2)
                            AppConstants.PositionHeight = 50;
                        else if (e.Count >= 3)
                            AppConstants.PositionHeight = 30;
                        //var yPer = ((point.Y / 720) * 100);
                        //var y = (yPer / 100) * (AppConstants.FloorLength - 10);

                        //var xPer = ((point.X / 1650) * 100);
                        //var x = (xPer / 100) * (AppConstants.FloorWidth - 10);

                        var yPer = ((point.Y / 720) * 100);
                        var y = (yPer / 100) * (246);

                        var xPer = ((point.X / 1650) * 100);
                        var x = (xPer / 100) * (570);
                        Frame frmNew = new Frame();
                        frmNew.HeightRequest = AppConstants.PositionHeight;
                        frmNew.WidthRequest = AppConstants.PositionHeight;
                        frmNew.CornerRadius = (float)AppConstants.PositionHeight / 2;
                        frmNew.BackgroundColor = AppConstants.PositionColor;
                        abs.Children.Add(frmNew, new Rectangle(y, x, AppConstants.PositionHeight, AppConstants.PositionHeight));
                    }
                    catch
                    {

                    }
                });
            }

            myTimer.Change(2000, 2000);
        }

        public LocateItemPage (int xCoordinate, int yCoordinate)
		{
            timer = new Timer(TimerCallback, null, 1000, 1000);
            myTimer = new Timer(MyTimerCallback, null, 2000, 2000);

            InitializeComponent ();
            double xPlace = xCoordinate;
            double yPlace = yCoordinate;
            var yPer = ((yPlace / 720) * 100);
            var y = (yPer / 100) * (246);

            var xPer = ((xPlace / 1650) * 100);
            var x = (xPer / 100) * (570);
            var frmNew = new Frame();
            frmNew.HeightRequest = 30;
            frmNew.WidthRequest = 30;
            frmNew.CornerRadius = 15;
            frmNew.BackgroundColor = Color.Gold;
            abs.Children.Add(frmNew, new Rectangle(y, x, 30, 30));
        }

        
    }
}