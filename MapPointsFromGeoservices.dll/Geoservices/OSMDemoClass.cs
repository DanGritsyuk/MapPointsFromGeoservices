using System;

namespace MapPointsFromGeoservices
{
    class OSMDemoClass : OpenStreetMap
    {
        public OSMDemoClass(string address, string filePath) : base(address, filePath) { }

        internal override MapPoint[] GetRegionMapPoints() => GetMapPointArray(new Uri(Address));
    }
}