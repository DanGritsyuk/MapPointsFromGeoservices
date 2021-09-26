using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;


namespace MapPointsFromGeoservices
{
    public class OpenStreetMap : Geoservice
    {
        public OpenStreetMap(string address, string filePath) : base(address, filePath)
        {

        }

        internal override MapPoint[] GetRegionMapPoints()
        {
            var query = new Uri("https://nominatim.openstreetmap.org/search?q=" + Address + "&format=json&polygon_geojson=1");

            return GetMapPointArray(query, "Mozilla/4.0 (compatible; MSIE 6.0; " + "Windows NT 5.2; .NET CLR 1.0.3705;)");
        }

        internal MapPoint[] GetMapPointArray(Uri uri, string userAgent = null)
        {
            string json;

            using (WebClient wc = new WebClient())
            {
                wc.UseDefaultCredentials = false;
                wc.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                if (userAgent != null) { wc.Headers.Add("user-agent", userAgent); }

                json = wc.DownloadString(uri);
            }

            return GetOSMDataCoordinates(json);
        }

        private static MapPoint[] GetOSMDataCoordinates(string json)
        {
            List<MapPoint> jsonPoints = new List<MapPoint>();

            Func<JsonElement, MapPoint> JsonToDoubleArray = (jElement) =>
            {
                return new MapPoint(jElement[0].GetDouble(), jElement[1].GetDouble());
            };

            Func<JsonElement, object> RecursionFindPoint = null;
            RecursionFindPoint = (jArray) =>
            {
                int jArrayLength = 0;

                try { jsonPoints.Add(JsonToDoubleArray(jArray)); }
                catch { jArrayLength = jArray.GetArrayLength();  }

                for (int i = 0; i < jArrayLength; i++)
                {
                    try { jsonPoints.Add(JsonToDoubleArray(jArray[i])); }
                    catch { RecursionFindPoint(jArray[i]); }
                }

                return null;
            };

            using (var document = JsonDocument.Parse(json))
            {
                foreach (JsonElement jElement in document.RootElement.EnumerateArray())
                {
                    var jArray = jElement.GetProperty("geojson").GetProperty("coordinates");
                    RecursionFindPoint(jArray);
                }
            }

            if (jsonPoints.Count == 0) { throw new WebException("None coordinates in json file"); }
            return jsonPoints.ToArray();
        }
    }
}
