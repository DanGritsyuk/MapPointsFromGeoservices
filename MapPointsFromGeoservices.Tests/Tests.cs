using NUnit.Framework;
using System.IO;
using MapPointsFromGeoservices;
using System;

namespace MapPointsFromGeoservices.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Тестируем эмуляцию получения одного географического субъекта с двумя полигонами и фильтром точек с шагом два
        /// </summary>
        [Test]
        public void OSMDemo2GeoPointEquatation()
        {
            MapPoint[] tstGeoPoints = new MapPoint[]
            {
                new MapPoint(37.6607618, 55.730601),
                new MapPoint(37.6627185, 55.7276605),
                new MapPoint(37.9061276, 55.7062585),
                new MapPoint(37.9078057, 55.7046341),
            };

            string demoAddress = "https://onedrive.live.com/download?cid=0A60DB328DAB3E9F&resid=A60DB328DAB3E9F%21698334&authkey=ANb4g8QPFRWQZHo";
            string fileName = "FileUT-OSMDemoGeoPointEquatation.txt";

            OSMDemoClass osmService = new OSMDemoClass(demoAddress, fileName);

            osmService.CreateGeoCoordinatesFile(2);

            TestPointsArray(tstGeoPoints, fileName);
        }

        /// <summary>
        /// Тестируем эмуляцию получения двух географических субъектов (один полигоном, другой с одной географической точкой)
        /// </summary>
        [Test]
        public void OSMDemoGeoPointEquatation()
        {
            MapPoint[] tstGeoPoints = new MapPoint[]
            {
                new MapPoint(38.220892, 47.3053966),
                new MapPoint(38.2925345, 47.3059301),
                new MapPoint(38.3109722, 47.3060674),
                new MapPoint(40.5543755, 49.5903512),
            };

            string demoAddress = "https://onedrive.live.com/download?cid=0A60DB328DAB3E9F&resid=A60DB328DAB3E9F%21698335&authkey=AKJuJlwUiCZBXKk";
            string fileName = "FileUT-OSMDemo2GeoPointEquatation.txt";

            OSMDemoClass osmService = new OSMDemoClass(demoAddress, fileName);

            osmService.CreateGeoCoordinatesFile();

            TestPointsArray(tstGeoPoints, fileName);
        }

        /// <summary>
        /// тестируем получение данных из настоящего сервиса Open Street Map
        /// </summary>
        [Test]
        public void OSMGeoPointEquatation()
        {
            string fileName = "FileUT-OSMGeoPointEquatation.txt";

            OpenStreetMap osmService = new OpenStreetMap("Республика РСО-Алания", fileName);
            osmService.CreateGeoCoordinatesFile();

            var loadFile = new SaveLoadFile();
            bool isSuccess = loadFile.DeSerializeObject<MapPoint[]>(fileName).Length > 0;
            DestroyFile(fileName);

            Assert.AreEqual(true, isSuccess);
        }

        /// <summary>
        /// тестируем сериализацию и запись в файл
        /// </summary>
        [Test]
        public void SerializeObjEquatation()
        {
            MapPoint[] tstGeoPoints = new MapPoint[]
            {
                new MapPoint(1, 2),
                new MapPoint(3, 4),
            };

            string fileName = "FileUT-SerializeObjEquatation.txt";

            var saveloadFaile = new SaveLoadFile();
            saveloadFaile.SerializeObject<MapPoint[]>(tstGeoPoints, fileName);

            TestPointsArray(tstGeoPoints, fileName);
        }

        /// <summary>
        /// Делаем сравнения ожидаемого массива с фактическим
        /// </summary>
        /// <param name="expectedGeoPoints">Ожидаемый массив точек</param>
        /// <param name="fileName">Имя файла с сохраненым массивом точек</param>
        private void TestPointsArray(MapPoint[] expectedGeoPoints, string fileName)
        {
            var loadFile = new SaveLoadFile();
            var actualGeoPoints = loadFile.DeSerializeObject<MapPoint[]>(fileName);

            if (expectedGeoPoints.Length != actualGeoPoints.Length) Assert.Fail();

            for (int i = 0; i < expectedGeoPoints.Length; i++)
            {
                Assert.AreEqual(expectedGeoPoints[i], actualGeoPoints[i]);
            }
            DestroyFile(fileName);
        }

        /// <summary>
        /// Удаляем созданный файл с масссивом точек
        /// </summary>
        /// <param name="path">путь к файлу с именем</param>
        private static void DestroyFile(string path)
        {
            try
            { if (File.Exists(path)) { File.Delete(path); } }
            finally
            {
                if (File.Exists(path))
                { throw new IOException(string.Format($"Failed to delete file: '{path}'")); }
            }
        }
    }
}