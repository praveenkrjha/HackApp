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
	public partial class IndoorLayout : ContentPage
	{
        private Timer timer;
        public static ConcurrentBag<BeaconModel> FoundBeacons = new ConcurrentBag<BeaconModel>();

        public static double CalculateDistanceFromRssi(int rssi)
        {
            return Math.Pow(10, ((float)(rssi + 65) * -1) / 40) * 100;
        }

       private double CalcMedianRssi(List<int> rssis)
        {
            int numberCount = rssis.Count();
            int halfIndex = rssis.Count() / 2;
            var sortedNumbers = rssis.OrderBy(n => n);
            double median;
            if ((numberCount % 2) == 0)
            {

                median = ((sortedNumbers.ElementAt(halfIndex) + sortedNumbers.ElementAt(halfIndex - 1))/2);
                //median = ((sortedNumbers.ElementAt(halfIndex) + sortedNumbers.ElementAt(2)));
            }
            else
            {
                median = sortedNumbers.ElementAt(halfIndex);
            }
            return median;
        }

        public static PointF GetMedianPoint(BeaconModel a, BeaconModel b, BeaconModel c = null)
        {
            PointF point = null;
            if (c == null)
            {
                var dist1 = CalculateDistanceFromRssi(a.Rssi);
                var dist2 = CalculateDistanceFromRssi(b.Rssi);
                point = Utilities.GetLocationWithCenterOfGravity(new PointF(a.Major, a.Minor), new PointF(b.Major, b.Minor)
                    , dist1, dist2);
            }
            else
            {
                var dist1 = CalculateDistanceFromRssi(a.Rssi);
                var dist2 = CalculateDistanceFromRssi(b.Rssi);
                var dist3 = CalculateDistanceFromRssi(c.Rssi);
                point = Utilities.GetLocationWithCenterOfGravity(new PointF(a.Major, a.Minor), new PointF(b.Major, b.Minor)
                    , new PointF(c.Major, c.Minor), dist1, dist2, dist3);
            }
            return point;
        }

        private void TimerCallback(object state)
        {


            if (FoundBeacons.Count >= 2)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                //timer = new Timer(TimerCallback, null, Timeout.Infinite, Timeout.Infinite);
                var e = FoundBeacons.ToList();
                foreach(var item in e)
                {
                    item.Rssi = (int)CalcMedianRssi(item.Rssis);
                }
                FoundBeacons = new ConcurrentBag<BeaconModel>();
                //Add your logic to draw the location
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        if(abs.Children.Count > 1)
                            abs.Children.RemoveAt(1);

                        var point = GetMedianPoint(e[0], e[1], (e.Count == 3 ? e[2] : null));
                        if (e.Count == 2)
                            AppConstants.PositionHeight = 50;
                        else if(e.Count >= 3)
                            AppConstants.PositionHeight = 30;

                        var yPer = ((point.Y / 720) * 100);
                        var y = (yPer / 100) * (240 - 10);

                        var xPer = ((point.X / 1650) * 100);
                        var x = (xPer / 100) * (552 - 10);
                        //var yPer = ((point.Y / 720) * 100);
                        //var y = (yPer / 100) * (AppConstants.FloorLength - 10);

                        // var xPer = ((point.X / 1650) * 100);
                        //var x = (xPer / 100) * (AppConstants.FloorWidth - 10);
                         Frame frmNew = new Frame();
                        frmNew.HeightRequest = AppConstants.PositionHeight;
                        frmNew.WidthRequest = AppConstants.PositionHeight;
                        frmNew.CornerRadius = (float)AppConstants.PositionHeight/2;
                        frmNew.BackgroundColor = AppConstants.PositionColor;
                        abs.Children.Add(frmNew, new Rectangle(y, x, AppConstants.PositionHeight, AppConstants.PositionHeight));
                    }
                    catch
                    {

                    }
                });
            }
            timer.Change(3000, 3000);
            //timer = new Timer(TimerCallback, null, 2000, 2000);
        }

        public IndoorLayout ()
		{
            timer = new Timer(TimerCallback, null, 3000, 3000);

            InitializeComponent ();
            //PointF point = Utilities.GetLocationWithCenterOfGravity(new PointF(829, 0), new PointF(414, 720), new PointF(1244, 720), 60, 100, 100);
            //var yPer = ((point.Y / 150) * 100);
            //var y = (yPer / 100) * (Application.Current.MainPage.Height - 10);

            //var xPer = ((point.X / 150) * 100);
            //var x = (xPer / 100) * (Application.Current.MainPage.Width - 10);
            //Frame frmNew = new Frame();
            //frmNew.HeightRequest = 10;
            //frmNew.WidthRequest = 10;
            //frmNew.CornerRadius = 5;
            //frmNew.BackgroundColor = Color.Blue;
            //abs.Children.Add(frmNew, new Rectangle(x, y, 10, 10));

            //point = Utilities.GetLocationWithCenterOfGravity(new PointF(829, 0), new PointF(829, 720), new PointF(1244, 720), 600, 400, 150);
            //yPer = ((point.Y / 720) * 100);
            //y = (yPer / 100) * (552 - 10);

            //xPer = ((point.X / 1650) * 100);
            //x = (xPer / 100) * (240 - 10);
            //frmNew = new Frame();
            //frmNew.HeightRequest = 30;
            //frmNew.WidthRequest = 30;
            //frmNew.CornerRadius = 15;
            //frmNew.BackgroundColor = Color.FromHex("#106DE9");
            //abs.Children.Add(frmNew, new Rectangle(x, y, 30, 30));
            //MessagingCenter.Subscribe<LstBeacons, List<BeaconModel>>(this, "Data", (s, e) =>
            // {
            //     Device.BeginInvokeOnMainThread(() =>
            //     {
            //         try
            //         {
            //             abs.Children.RemoveAt(1);
            //             point = Utilities.GetLocationWithCenterOfGravity(new PointF(e[0].Major, e[0].Minor), new PointF(e[1].Major, e[1].Minor)
            //                 , new PointF(e[2].Major, e[2].Minor), e[0].Distance, e[1].Distance, e[2].Distance);
            //             yPer = ((point.Y / 720) * 100);
            //             y = (yPer / 100) * (552 - 10);

            //             xPer = ((point.X / 1650) * 100);
            //             x = (xPer / 100) * (240 - 10);
            //             frmNew = new Frame();
            //             frmNew.HeightRequest = 10;
            //             frmNew.WidthRequest = 10;
            //             frmNew.CornerRadius = 5;
            //             frmNew.BackgroundColor = Color.Blue;
            //             abs.Children.Add(frmNew, new Rectangle(x, y, 10, 10));
            //         }
            //         catch
            //         {

            //         }
            //     });
            //});
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