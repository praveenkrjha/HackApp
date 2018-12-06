using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IndoorNavigation
{
    public class Utilities
    {
        public static string GetSha256Hash(string text)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));

                // Get the hashed string.
                var strBuilder = new StringBuilder();
                foreach (byte x in hashedBytes)
                {
                    strBuilder.Append(string.Format("{0:x2}", x));
                }
                return strBuilder.ToString();
            }
        }


        public static PointF GetLocationWithCenterOfGravity(PointF a, PointF b, PointF c, double dA, double dB, double dC)
        {
            var METERS_IN_COORDINATE_UNITS_RATIO = 1.0f;

            //Find Center of Gravity
            var cogX = (a.X + b.X + c.X) / 3;
            var cogY = (a.Y + b.Y + c.Y) / 3;
            var cog = new PointF(cogX, cogY);

            //Nearest Beacon
            PointF nearestBeacon;
            double shortestDistanceInMeters;
            if (dA <= dB && dA <= dC)
            {
                nearestBeacon = a;
                shortestDistanceInMeters = dA;
            }
            else if (dB < dC)
            {
                nearestBeacon = b;
                shortestDistanceInMeters = dB;
            }
            else
            {
                nearestBeacon = c;
                shortestDistanceInMeters = dC;
            }

            //http://www.mathplanet.com/education/algebra-2/conic-sections/distance-between-two-points-and-the-midpoint
            //Distance between nearest beacon and COG
            var distanceToCog = (float)(Math.Sqrt(Math.Pow(cog.X - nearestBeacon.X, 2)
                    + Math.Pow(cog.Y - nearestBeacon.Y, 2)));

            //Convert shortest distance in meters into coordinates units.
            var shortestDistanceInCoordinationUnits = shortestDistanceInMeters * METERS_IN_COORDINATE_UNITS_RATIO;

            //http://math.stackexchange.com/questions/46527/coordinates-of-point-on-a-line-defined-by-two-other-points-with-a-known-distance?rq=1
            //On the line between Nearest Beacon and COG find shortestDistance point apart from Nearest Beacon
            var t = shortestDistanceInCoordinationUnits / distanceToCog;
            var pointsDiff = new PointF(cog.X - nearestBeacon.X, cog.Y - nearestBeacon.Y);
            var tTimesDiff = new PointF(pointsDiff.X * t, pointsDiff.Y * t);

            //Add t times diff with nearestBeacon to find coordinates at a distance from nearest beacon in line to COG.
            var userLocation = new PointF(nearestBeacon.X + tTimesDiff.X, nearestBeacon.Y + tTimesDiff.Y);

            return userLocation;
        }

        public static  double CalculateDistanceFromRssi(int rssi)
        {
            return Math.Pow(10, ((float)(rssi + 65) * -1) / 40) * 100;
        }

        public static double CalcMedianRssi(List<int> rssis)
        {
            int numberCount = rssis.Count();
            int halfIndex = rssis.Count() / 2;
            var sortedNumbers = rssis.OrderBy(n => n);
            double median;
            if ((numberCount % 2) == 0)
            {

                median = ((sortedNumbers.ElementAt(halfIndex) + sortedNumbers.ElementAt(halfIndex - 1)) / 2);
                //median = ((sortedNumbers.ElementAt(halfIndex) + sortedNumbers.ElementAt(2)));
            }
            else
            {
                median = sortedNumbers.ElementAt(halfIndex);
            }
            return median;
        }

        public static PointF GetLocationWithCenterOfGravity(PointF a, PointF b, double dA, double dB)
        {
            var METERS_IN_COORDINATE_UNITS_RATIO = 1.0f;

            //Find Center of Gravity
            var cogX = (a.X + b.X) / 2;
            var cogY = (a.Y + b.Y) / 2;
            var cog = new PointF(cogX, cogY);

            //Nearest Beacon
            PointF nearestBeacon;
            double shortestDistanceInMeters;
            if (dA <= dB)
            {
                nearestBeacon = a;
                shortestDistanceInMeters = dA;
            }
            else
            {
                nearestBeacon = b;
                shortestDistanceInMeters = dB;
            }


            //http://www.mathplanet.com/education/algebra-2/conic-sections/distance-between-two-points-and-the-midpoint
            //Distance between nearest beacon and COG
            var distanceToCog = (float)(Math.Sqrt(Math.Pow(cog.X - nearestBeacon.X, 2)
                    + Math.Pow(cog.Y - nearestBeacon.Y, 2)));

            //Convert shortest distance in meters into coordinates units.
            var shortestDistanceInCoordinationUnits = shortestDistanceInMeters * METERS_IN_COORDINATE_UNITS_RATIO;

            //http://math.stackexchange.com/questions/46527/coordinates-of-point-on-a-line-defined-by-two-other-points-with-a-known-distance?rq=1
            //On the line between Nearest Beacon and COG find shortestDistance point apart from Nearest Beacon
            var t = shortestDistanceInCoordinationUnits / distanceToCog;
            var pointsDiff = new PointF(cog.X - nearestBeacon.X, cog.Y - nearestBeacon.Y);
            var tTimesDiff = new PointF(pointsDiff.X * t, pointsDiff.Y * t);

            //Add t times diff with nearestBeacon to find coordinates at a distance from nearest beacon in line to COG.
            var userLocation = new PointF(nearestBeacon.X + tTimesDiff.X, nearestBeacon.Y + tTimesDiff.Y);

            return userLocation;
        }


    }

    public class PointF
    {
        public double X { get; set; }
        public double Y { get; set; }

        public PointF(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return "X : " + this.X + " ; Y : " + this.Y;
        }
    }

}
