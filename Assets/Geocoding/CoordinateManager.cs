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
    using UnityEngine;
    using Utilities;
    
    public class CoordinateManager : Singleton<CoordinateManager>
    {
        public GeoCoordinate userGeoCoordinate;
        public Transform userTransform;
        
        private HeadingInfo _userHeadingInfo;
        
        public float startingHeadingValue;

        private void Start()
        {
            if (Camera.main is { }) userTransform = Camera.main.transform;

            _userHeadingInfo = new HeadingInfo(userTransform.eulerAngles.y, startingHeadingValue);
            userGeoCoordinate = new GeoCoordinate(39, 32);
        }

        public void UpdateUserGeoCoordinate(GeoCoordinate newGeoCoordinate)
        {
            userGeoCoordinate = newGeoCoordinate;
        }
        
        public void UpdateUserGeoCoordinate(LocationInfo locationInfo)
        {
            UpdateUserGeoCoordinate(locationInfo.ToGeoCoordinate());
        }

        public void UpdateHeadingValue(float newHeadingAngle)
        {
            _userHeadingInfo.UpdateReferenceHeadingValue(userTransform.eulerAngles.y,newHeadingAngle);
        }

        public float GetHeadingValue()
        {
            return _userHeadingInfo.GetCurrentHeadingValue(userTransform.eulerAngles.y);
        }
    }
}
