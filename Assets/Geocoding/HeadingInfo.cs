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
    
    [Serializable]
    public class HeadingInfo
    {
        public float referenceHeadingAngle;
        public float referenceAngleOfYDirection;
        public float accuracy;
        public double timestamp;

        public HeadingInfo(float referenceAngleOfYDirection, float referenceHeadingAngle, float accuracy = 0)
        {
            this.referenceHeadingAngle = referenceHeadingAngle;
            this.referenceAngleOfYDirection = referenceAngleOfYDirection;
            this.accuracy = accuracy;
            timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }

        public float GetCurrentHeadingValue(float currentUserAngleOfYDirection)
        {
            var angleDirectionDifference = referenceAngleOfYDirection - currentUserAngleOfYDirection;
            var heading = referenceHeadingAngle - angleDirectionDifference;

            if (heading > 180) heading -= 360;
            
            return heading;
        }

        public void UpdateReferenceHeadingValue(float currentUserAngleOfYDirection, float newHeadingAngle)
        {
            referenceAngleOfYDirection = currentUserAngleOfYDirection;
            referenceHeadingAngle = newHeadingAngle;
            timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }
    }
}
