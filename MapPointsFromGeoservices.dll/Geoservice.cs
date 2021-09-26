using System;
using System.Collections.Generic;

namespace MapPointsFromGeoservices
{
    public abstract class Geoservice
    {
        private readonly string _address;
        private readonly string _filePath;
        private readonly Lazy<MapPoint[]> _mapPoints;

        public string Address => _address;
        public string FilePath => _filePath;
        public MapPoint[] MapPoints => _mapPoints.Value;

        public Geoservice(string address, string filePath)
        {
            _address = address;
            _filePath = filePath;
            _mapPoints = new Lazy<MapPoint[]>(GetRegionMapPoints);
        }

        /// <summary>
        /// По указанному адресу создает файл с массивом георгафических точек в указанном месте на диске
        /// / Create file with map points array from location address 
        /// </summary>
        /// <param name="step">плотность точек, сохраняемые в массиве файла (не должен быть меньше 1) / Step for point filter (must not be less than 1)</param>
        public void CreateGeoCoordinatesFile(int step = 1)
        {
            if (String.IsNullOrWhiteSpace(Address) || String.IsNullOrWhiteSpace(FilePath) || step < 1) { throw new ArgumentException(); }

            var result = FilterPoints(MapPoints, step);

            var saveFile = new SaveLoadFile();

            saveFile.SerializeObject<MapPoint[]>(result, FilePath);
        }

        internal abstract MapPoint[] GetRegionMapPoints();

        private static MapPoint[] FilterPoints(MapPoint[] MapPoints, int step)
        {
            if (step == 1 || MapPoints.Length < step) return MapPoints;

            var lstFilteredPoints = new List<MapPoint>();
            for (int i = step - 1; i < MapPoints.Length; i += step)
            {
                lstFilteredPoints.Add(MapPoints[i]);
            }
            return lstFilteredPoints.ToArray();
        }
    }
}
