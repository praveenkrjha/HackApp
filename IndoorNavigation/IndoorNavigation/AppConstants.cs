using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IndoorNavigation
{
    public class AppConstants
    {
        public static string BaseUrl = "http://10.104.15.107:53532/api/";

        public static string RequestToken = "B7A6A1F06D8A48BE826BBD184D0BBE17F224C661DABBD9B581ADAA7F2D56A375";

        public static int FloorLength = 552;
        public static int FloorWidth = 240;

        public static double PositionHeight = 30;
        public static Color PositionColor = Color.FromHex("#106DE9");
    }
}
