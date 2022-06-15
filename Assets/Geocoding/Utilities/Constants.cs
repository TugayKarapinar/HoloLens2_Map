/*
 * - Created by Tugay Karapınar (08.06.22)
 *    _________     ______    ____  ____  
 *   |  _   _  |  .' ___  |  |_  _||_  _| 
 *   |_/ | | \_| / .'   \_|    \ \  / /   
 *       | |     | |   ____     \ \/ /    
 *      _| |_    \ `.___]  |    _|  |_    
 *     |_____|    `._____.'    |______|
 * --------------------------------------- 
 */

using System;
using UnityEngine;

namespace Geocoding.Utilities
{
    public static class Constants
    {
        public const int EARTH_RADIUS = 6378137;
        public const int MAX_RENDER_DISTANCE = 5000;
    }
    
    public enum AngleUnit
    {
        Degree, Radian
    }

    public enum DistanceUnit
    {
        Meter, Kilometer, Miles
    }

    public enum SeeThruPositionType
    {
        Real, Render
    }
}

