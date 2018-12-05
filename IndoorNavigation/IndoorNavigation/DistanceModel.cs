using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IndoorNavigation
{
    public class DistanceModel
    {
        public double Dist1 { get; set; }
        public double Dist2 { get; set; }
        public double Dist3 { get; set; }

        public double total
        {
            get { return Dist1 + Dist2 + Dist3; }
            set
            {
                MessagingCenter.Send(this, "Data", Dist1 + "," + Dist2 + "," + Dist3);
            }
        }
    }
}
