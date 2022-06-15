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

namespace Geocoding.Utilities
{
    using System;
    using UnityEngine;
    public static class Conversions
    {
        /// <summary>
        /// Unity position of the geographic coordinate
        /// </summary>
        /// <param name="geoCoordinate">Geographic coordinate of desired unity position</param>
        /// <param name="seeThruPositionType">There are two different types.
        /// Real: returns the unity position directly. Default value is Render.
        /// Render: returns unity position is limited to maximum render distance.</param>
        /// <param name="isAltitudeUsed"> Use altitude difference in unity coordinate to be calculated? Default do not use</param>
        public static Vector3 ToSeeThruPosition(this GeoCoordinate geoCoordinate,SeeThruPositionType seeThruPositionType = SeeThruPositionType.Render, bool isAltitudeUsed = false)
        {
            var distance = GeoCoordinate.Distance(CoordinateManager.Instance.userGeoCoordinate, geoCoordinate);
            var bearing = GeoCoordinate.Bearing(CoordinateManager.Instance.userGeoCoordinate, geoCoordinate,AngleUnit.Degree);

            if (seeThruPositionType == SeeThruPositionType.Render && distance > Constants.MAX_RENDER_DISTANCE)
            {
                distance = Constants.MAX_RENDER_DISTANCE;
            }

            //TODO Double Calculations
            
            var targetDirection = CoordinateManager.Instance.userTransform.forward * (float)distance;
            var targetPosition = Quaternion.Euler(new Vector3(0,(float)bearing - CoordinateManager.Instance.GetHeadingValue(),0)) * targetDirection;

            if (isAltitudeUsed)
            {
                var userAltitude = CoordinateManager.Instance.userGeoCoordinate.altitude;
                var targetAltitude = geoCoordinate.altitude;

                var difference = targetAltitude - userAltitude;
                targetPosition = new Vector3(targetPosition.x, targetPosition.y + (float)difference, targetPosition.z);
            }
            
            return targetPosition;
        }

        public static DateTime ToDateTime(this double unixTimeStamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
        
        public static string ToJsonString(this LocationInfo locationInfo)
        {
            var geoCoordinate = locationInfo.ToGeoCoordinate();
            var json = JsonUtility.ToJson(geoCoordinate);
            return json;
        }
        
        public static GeoCoordinate ToGeoCoordinate(this string jsonGeoCoordinateString)
        {
            var geoCoordinate = new GeoCoordinate();
            try
            {
                geoCoordinate = JsonUtility.FromJson<GeoCoordinate>(jsonGeoCoordinateString);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            return geoCoordinate;
        }
        
        public static string ToJsonString(this GeoCoordinate geoCoordinate)
        {
            return JsonUtility.ToJson(geoCoordinate);
        }
        
        public static GeoCoordinate ToGeoCoordinate(this LocationInfo locationInfo)
        {
            return new GeoCoordinate(locationInfo);
        }
    }
}
