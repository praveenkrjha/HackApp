using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IndoorNavigation
{
    public class BeaconModel
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        // public double Distance { get; set; }

        public List<int> Rssis { get; set; }
        public int Rssi { get; set; }

        public DateTime DetectTime { get; set; }
    }

    public class LstBeacons
    {
        public List<BeaconModel> beaconModels { get; set; }
        public int Flag
        {
            get { return 0; }
            set { MessagingCenter.Send(this, "Data", beaconModels); }
        }
    }
}
