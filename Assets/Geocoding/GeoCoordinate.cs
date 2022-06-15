/*
 * - Created by Tugay Karapınar (14.06.22)
 *    _________     ______    ____  ____  
 *   |  _   _  |  .' ___  |  |_  _||_  _| 
 *   |_/ | | \_| / .'   \_|    \ \  / /   
 *       | |     | |   ____     \ \/ /    
 *      _| |_    \ `.___]  |    _|  |_    
 *     |_____|    `._____.'    |______|
 * --------------------------------------- 
 */

namespace Geocoding
{
    using System;
    using Utilities;
    using UnityEngine;
    
    [Serializable]
    public class GeoCoordinate
    {
        public double timestamp;
        public double latitude;
        public double longitude;
        public double altitude;
        public double horizontalAccuracy;
        public double verticalAccuracy;
        
        public double this[int index]
        {
            get
            {
                return index switch
                {
                    0 => timestamp,
                    1 => latitude,
                    2 => longitude,
                    3 => altitude,
                    4 => horizontalAccuracy,
                    5 => verticalAccuracy,
                    _ => throw new IndexOutOfRangeException("Invalid GeoCoordinate index!")
                };
            }
            set
            {
                switch (index)
                {
                    case 0:
                        timestamp = value;
                        break;
                    case 1:
                        latitude = value;
                        break;
                    case 2:
                        longitude = value;
                        break;
                    case 3:
                        altitude = value;
                        break;
                    case 4:
                        horizontalAccuracy = value;
                        break;
                    case 5:
                        verticalAccuracy = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid GeoCoordinate index!");
                }
            }
        }
        
        public GeoCoordinate(){}
        
        public GeoCoordinate(double timestamp, double latitude, double longitude, double altitude, double horizontalAccuracy, double verticalAccuracy)
            : this(timestamp, latitude, longitude, altitude,verticalAccuracy)
        {
            this.horizontalAccuracy = horizontalAccuracy;
        }
        
        public GeoCoordinate(double timestamp, double latitude, double longitude, double altitude, double verticalAccuracy)
            : this(latitude,longitude,altitude)
        {
            this.timestamp = timestamp;
            this.verticalAccuracy = verticalAccuracy;
        }
        
        public GeoCoordinate(double timestamp, double latitude, double longitude, double horizontalAccuracy)
            : this(latitude,longitude)
        {
            this.timestamp = timestamp;
            this.horizontalAccuracy = horizontalAccuracy;
        }
        
        public GeoCoordinate(double latitude, double longitude, double altitude)
            : this(latitude,longitude)
        {
            this.altitude = altitude;
        }
        public GeoCoordinate(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }
        
        public GeoCoordinate(float latitude, float longitude, float altitude)
            : this(latitude,longitude)
        {
            this.altitude = altitude;
        }
        
        public GeoCoordinate(float latitude, float longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public GeoCoordinate(LocationInfo locationInfo)
            :this(locationInfo.timestamp,locationInfo.latitude,locationInfo.longitude,locationInfo.altitude,locationInfo.horizontalAccuracy,locationInfo.verticalAccuracy) { }

        public override string ToString()
        {
            return $"Lat: {latitude} Lon: {longitude} Alt: {altitude}";
        }
        
        public string FullToString()
        {
            return $"Timestamp: {timestamp} Lat: {latitude} Lon: {longitude} Alt: {altitude} H-Accuracy: {horizontalAccuracy} V-Accuracy: {verticalAccuracy}";
        }

        /// <summary>
        /// Distance between two geographic coordinates.
        /// Altitude is not taken into calculation.
        /// </summary>
        /// <param name="geoCoordinate1">Geographic coordinate</param>
        /// <param name="geoCoordinate2">Geographic coordinate</param>
        /// <param name="unit"> Unit of distance, default unit is meter</param>
        public static double Distance(GeoCoordinate geoCoordinate1, GeoCoordinate geoCoordinate2, DistanceUnit unit = DistanceUnit.Meter)
        { 
            var lon1 = geoCoordinate1.longitude * Mathd.Deg2Rad;
            var lat1 = geoCoordinate1.latitude * Mathd.Deg2Rad;
            var lon2 = geoCoordinate2.longitude * Mathd.Deg2Rad;
            var lat2 = geoCoordinate2.latitude * Mathd.Deg2Rad;
            var deltaLon = lon2 - lon1;
            var deltaLat = lat2 - lat1;

            var a = Mathd.Sin(0.5 * deltaLat) * Mathd.Sin(0.5 * deltaLat) +
                    Mathd.Cos(lat1) * Mathd.Cos(lat2) * Mathd.Sin(0.5 * deltaLon)
                    * Mathd.Sin(0.5 * deltaLon);

            var c = 2.0 * Mathd.Atan2(Mathd.Sqrt(a), Mathd.Sqrt(1.0 - a));
            var distance = c *  Constants.EARTH_RADIUS;

            return unit switch
            {
                DistanceUnit.Meter => distance,
                DistanceUnit.Kilometer => distance / 1000,
                DistanceUnit.Miles => distance / 1609.344d,
                _ => double.NaN
            };
        }
        
        /// <summary>
        /// Angle between north direction and two geographic coordinates direction.
        /// Altitude is not taken into calculation.
        /// </summary>
        /// <param name="geoCoordinate1">Geographic coordinate</param>
        /// <param name="geoCoordinate2">Geographic coordinate</param>
        /// <param name="unit"> Unit of bearing, default unit is radian</param>
        public static double Bearing(GeoCoordinate geoCoordinate1, GeoCoordinate geoCoordinate2, AngleUnit unit = AngleUnit.Radian)
        {
            var lon1 = geoCoordinate1.longitude * Mathd.Deg2Rad;
            var lat1 = geoCoordinate1.latitude * Mathd.Deg2Rad;
            var lon2 = geoCoordinate2.longitude * Mathd.Deg2Rad;
            var lat2 = geoCoordinate2.latitude * Mathd.Deg2Rad;

            var bearing = Mathd.Atan2((Mathd.Sin(lon2 - lon1) * Mathd.Cos(lat2)), Mathd.Cos(lat1) * Mathd.Sin(lat2) - Mathd.Sin(lat1) * Mathd.Cos(lat2) * Mathd.Cos(lon1 - lon2));

            return unit switch
            {
                AngleUnit.Degree => Mathd.Rad2Deg * bearing,
                AngleUnit.Radian => bearing,
                _ => double.NaN
            };
        }
        
        /// <summary>
        /// Midpoint of two geographic coordinates.
        /// Altitude is not taken into calculation
        /// </summary>
        /// <param name="geoCoordinate1">Geographic coordinate</param>
        /// <param name="geoCoordinate2">Geographic coordinate</param>
        public static GeoCoordinate Midpoint(GeoCoordinate geoCoordinate1, GeoCoordinate geoCoordinate2)
        {
            var lon1 = geoCoordinate1.longitude * Mathd.Deg2Rad;
            var lon2 = geoCoordinate2.longitude * Mathd.Deg2Rad;
            var lat1 = geoCoordinate1.longitude * Mathd.Deg2Rad;
            var lat2 = geoCoordinate2.longitude * Mathd.Deg2Rad;

            var bx = Mathd.Cos(lat2) * Mathd.Cos(lon2 - lon1);
            var by = Mathd.Cos(lat2) * Mathd.Sin(lon2 - lon1);
            var midLat = Mathd.Atan2(Mathd.Sin(lat1) + Mathd.Sin(lat2), Mathd.Sqrt((Mathd.Cos(lat1) + bx) * (Mathd.Cos(lat1) + bx) + (by * by)));
            var midLon = lon1 + Mathd.Atan2(by, Mathd.Cos(lat1) + bx);

            return new GeoCoordinate(midLat * Mathd.Rad2Deg, midLon * Mathd.Rad2Deg);
        }
        
        /// <summary>
        /// A geographic coordinates in the desired bearing and distance from a given point.
        /// Altitude is not taken into calculation.
        /// </summary>
        /// <param name="from">Starting geographic coordinate</param>
        /// <param name="distance">Distance of start point, distance unit is meter</param>
        /// <param name="bearing"> The bearing between the start and the target point, bearing unit is radian</param>
        public static GeoCoordinate MoveTowards(GeoCoordinate from, double distance, double bearing)
        {
            var fromLon = from.longitude * Mathd.Deg2Rad;
            var fromLat = from.latitude * Mathd.Deg2Rad;

            distance *= 1000;
            var toLat = Mathd.Asin(Mathd.Sin(fromLat) * Mathd.Cos(distance / Constants.EARTH_RADIUS) + Mathd.Cos(fromLat) * Mathd.Sin(distance / Constants.EARTH_RADIUS) * Mathd.Cos(bearing));
            var toLon = fromLon + Mathd.Atan2(Mathd.Sin(bearing) * Mathd.Sin(distance / Constants.EARTH_RADIUS) * Mathd.Cos(fromLat), Mathd.Cos(distance / Constants.EARTH_RADIUS) - Mathd.Sin(fromLat) * Mathd.Sin(toLat));

            return new GeoCoordinate(toLat * Mathd.Rad2Deg, toLon * Mathd.Rad2Deg);
        }
        
        /// <summary>
        /// A geographic coordinates at a given distance from the first point between two points.
        /// Altitude is not taken into calculation.
        /// </summary>
        /// <param name="startPoint">Starting geographic coordinate</param>
        /// <param name="endPoint">End geographic coordinate</param>
        /// <param name="distance"> The distance of the desired geographic coordinate to the starting point, bearing unit is radian</param>
        public static GeoCoordinate Lerp(GeoCoordinate startPoint, GeoCoordinate endPoint, double distance)
        {
            var bearing = Bearing(startPoint, endPoint);
            return MoveTowards(startPoint, distance * Distance(startPoint, endPoint), bearing);
        }

        /// <summary>
        /// Compares whether the latitude, longitude, and altitude of two geographic coordinates are the same
        /// </summary>
        public bool IsEqual(GeoCoordinate other)
        {
            return latitude.Equals(other.latitude) && longitude.Equals(other.longitude) && altitude.Equals(other.altitude);
        }
    }
}
