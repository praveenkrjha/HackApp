using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using Plugin.Permissions;
using RadiusNetworks.IBeaconAndroid;
using Android.Support.V4.App;
using Android.Content;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;

namespace IndoorNavigation.Droid
{
    [Activity(Label = "IndoorNavigation", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IBeaconConsumer
    {
        readonly string[] PermissionsLocation =
    {
      Manifest.Permission.AccessCoarseLocation,
      Manifest.Permission.AccessFineLocation,Manifest.Permission.UseFingerprint
    };

        const int RequestLocationId = 0;

        int _previousProximity;
        private const string UUID = "c42da800-1afb-450c-9a50-5322f762cc4c";
        private const string beaconApp = "JDA-Hack";

        bool _paused;
        View _view;
        IBeaconManager _iBeaconManager;
        MonitorNotifier _monitorNotifier;
        RangeNotifier _rangeNotifier;
        Region _monitoringRegion;
        Region _rangingRegion;
        public MainActivity()
        {

            _iBeaconManager = IBeaconManager.GetInstanceForApplication(this);

            _monitorNotifier = new MonitorNotifier();
            _rangeNotifier = new RangeNotifier();

            _monitoringRegion = new Region(beaconApp, UUID, null, null);
            _rangingRegion = new Region(beaconApp, UUID, null, null);
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            AndroidEnvironment.UnhandledExceptionRaiser += HandleAndroidException;
            base.OnCreate(savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            RequestPermissions(PermissionsLocation, RequestLocationId);

            _iBeaconManager.Bind(this);

            _monitorNotifier.EnterRegionComplete += EnteredRegion;
            _monitorNotifier.ExitRegionComplete += ExitedRegion;

            _rangeNotifier.DidRangeBeaconsInRegionComplete += RangingBeaconsInRegion;
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        void HandleAndroidException(object sender, RaiseThrowableEventArgs e)
        {
            e.Handled = true;
            Console.Write("HANDLED EXCEPTION");
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        protected override void OnResume()
        {
            base.OnResume();
            _paused = false;
        }

        protected override void OnPause()
        {
            base.OnPause();
            _paused = true;
        }

        void EnteredRegion(object sender, MonitorEventArgs e)
        {
            if (_paused)
            {
                ShowNotification();
            }
        }

        void ExitedRegion(object sender, MonitorEventArgs e)
        {
        }

        void RangingBeaconsInRegion(object sender, RangeEventArgs e)
        {
            Console.WriteLine("Found beacons :" + e.Beacons.Count);
            foreach (var item in e.Beacons)
            {

                Console.WriteLine(" Beacon Major : " + item.Major + " Minor : " + item.Minor);

                //var dist = Math.Pow(10, ((float)(item.Rssi + 65) * -1) / 40) * 100;
                var beaconItem = IndoorLayout.FoundBeacons.FirstOrDefault(x => x.Major == item.Major && x.Minor == item.Minor);
                if (beaconItem != null)
                {
                    if(beaconItem.Rssis == null)
                    {
                        beaconItem.Rssis = new List<int>();
                    }
                    //if (item.Rssi > beaconItem.Rssi)
                    {
                        beaconItem.Rssis.Add(item.Rssi);
                        //beaconItem.Distance = dist;
                    }
                }
                else
                {
                    IndoorLayout.FoundBeacons.Add(new BeaconModel
                    {
                        Major = item.Major,
                        Minor = item.Minor,
                        Rssis = new List<int>() { item.Rssi },
                       // Distance = dist,
                        DetectTime = DateTime.Now
                    });
                }
            }

            //if (e.Beacons.Count < 3)
            //{
            //    Console.WriteLine("Did not find enoough beacons.");
            //}
            //else
            //{
            //    var beacon = e.Beacons.FirstOrDefault();

            //    var message = string.Empty;
            //    var rssi = beacon.Rssi;
            //    var distance1 = Math.Pow(10, ((float)(rssi + 65) * -1) / 20);

            //    LstBeacons lstBeacons = new LstBeacons();
            //    lstBeacons.beaconModels = new List<BeaconModel>();
            //    foreach (var beac in e.Beacons)
            //    {
            //        BeaconModel beaconModel = new BeaconModel();
            //        beaconModel.Minor = beac.Minor;
            //        beaconModel.Major = beac.Major;
            //        beaconModel.Distance = Math.Pow(10, ((float)(beac.Rssi + 65) * -1) / 20) * 100;
            //        lstBeacons.beaconModels.Add(beaconModel);
            //    }
            //    lstBeacons.Flag = 1;
            //    //DistanceModel distance = new DistanceModel();
            //    //foreach (var beac in e.Beacons)
            //    //{
            //    //    if (beac.Major == 829 && beac.Minor == 0)
            //    //    {
            //    //        distance.Dist1 = Math.Pow(10, ((float)(beac.Rssi + 65) * -1) / 20) * 100;
            //    //    }
            //    //    else if (beac.Major == 829 && beac.Minor == 720)
            //    //    {
            //    //        distance.Dist2 = Math.Pow(10, ((float)(beac.Rssi + 65) * -1) / 20) * 100;
            //    //    }
            //    //    else if (beac.Major == 1244 && beac.Minor == 720)
            //    //    {
            //    //        distance.Dist3 = Math.Pow(10, ((float)(beac.Rssi + 65) * -1) / 20) * 100;
            //    //    }

            //    //}
            //    //distance.total = distance.Dist1 + distance.Dist2 + distance.Dist3;


            //    switch ((ProximityType)beacon.Proximity)
            //    {
            //        case ProximityType.Immediate:
            //            UpdateDisplay("You found the monkey!", Color.Green);
            //            break;
            //        case ProximityType.Near:
            //            UpdateDisplay("You're getting warmer", Color.Yellow);
            //            break;
            //        case ProximityType.Far:
            //            UpdateDisplay("You're freezing cold", Color.Blue);
            //            break;
            //        case ProximityType.Unknown:
            //            UpdateDisplay("I'm not sure how close you are to the monkey", Color.Red);
            //            break;
            //    }

            //    _previousProximity = beacon.Proximity;
            //}
        }

        #region IBeaconConsumer impl
        public void OnIBeaconServiceConnect()
        {
            _iBeaconManager.SetMonitorNotifier(_monitorNotifier);
            _iBeaconManager.SetRangeNotifier(_rangeNotifier);

            _iBeaconManager.StartMonitoringBeaconsInRegion(_monitoringRegion);
            _iBeaconManager.StartRangingBeaconsInRegion(_rangingRegion);
        }
        #endregion

        private void UpdateDisplay(string message, Color color)
        {
            RunOnUiThread(() =>
            {

            });
        }

        private void ShowNotification()
        {
            var resultIntent = new Intent(this, typeof(MainActivity));
            resultIntent.AddFlags(ActivityFlags.ReorderToFront);
            var pendingIntent = PendingIntent.GetActivity(this, 0, resultIntent, PendingIntentFlags.UpdateCurrent);
            var notificationId = 1234;

            var builder = new NotificationCompat.Builder(this)
                .SetSmallIcon(Resource.Mipmap.icon)
                .SetContentTitle("Indoor Navigation")
                .SetContentText("Notification")
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(true);

            var notification = builder.Build();

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Notify(notificationId, notification);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _monitorNotifier.EnterRegionComplete -= EnteredRegion;
            _monitorNotifier.ExitRegionComplete -= ExitedRegion;

            _rangeNotifier.DidRangeBeaconsInRegionComplete -= RangingBeaconsInRegion;

            _iBeaconManager.StopMonitoringBeaconsInRegion(_monitoringRegion);
            _iBeaconManager.StopRangingBeaconsInRegion(_rangingRegion);
            _iBeaconManager.UnBind(this);
        }
    }
}