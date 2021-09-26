using System.Runtime.Serialization;

namespace MapPointsFromGeoservices
{
    [DataContract]
    public struct MapPoint
    {
        public MapPoint(double x, double y)
        {
            this._x = this._y = 0;
            this._x = x;
            this._y = y;
        }

        [DataMember(Name = "X")]
        private double _x;
        public double X { get => _x; }

        [DataMember(Name = "Y")]
        private double _y;
        public double Y { get => _y; }

        public override string ToString() => $"MapPoint({this._x}, {this._x})";
    }
}