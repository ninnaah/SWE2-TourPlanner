using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TourPlanner.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TourPlanner.DataAccessLayer
{
    public class MapQuest
    {
        public static float Distance;
        private static string _key;
        private static string _dirPath;

        public MapQuest(string key, string dirPath)
        {
            _dirPath = dirPath;
            _key = key;
        }

        public async void GetMap(TourItem tour)
        {
            await SendRequest(tour);
        }


        public async Task SendRequest(TourItem tour)
        {
            await SendRouteRequest(tour);
        }

        public static async Task SendRouteRequest(TourItem tour)
        {
            string getRequest = $"http://www.mapquestapi.com/directions/v2/route?key={_key}&from={tour.From}&to={tour.To}";
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(getRequest);
            string responseBody = await response.Content.ReadAsStringAsync();

            JObject obj = JsonConvert.DeserializeObject<JObject>(responseBody);
            JObject boundingBox = obj["route"]["boundingBox"] as JObject;

            string sessionId = (string)obj["route"]["sessionId"];
            float distance = (float)obj["route"]["distance"];
            tour.Distance = distance;

            Distance = tour.Distance;
            Debug.WriteLine("Distance: " + Distance);

            /*Debug.WriteLine("boundingBox: "+boundingBox);
            Debug.WriteLine("sessionId: "+sessionId);
            Debug.WriteLine("distance: "+distance);*/

            await SendMapRequest(tour.Name, sessionId, boundingBox);
        }

        public static async Task SendMapRequest(string tourName, string sessionId, JObject boundingBox)
        {
            string lowerRightLng = (string)boundingBox["lr"]["lng"];
            string lowerRightLat = (string)boundingBox["lr"]["lat"];
            string upperLeftLng = (string)boundingBox["ul"]["lng"];
            string upperLeftLat = (string)boundingBox["ul"]["lat"];

            Directory.CreateDirectory($@"{_dirPath}/maps/");
            string filePath = $@"{_dirPath}/maps/{tourName}.png";

            string getRequest = $"https://www.mapquestapi.com/staticmap/v5/map?key={_key}&size=1240,960&session={sessionId}&boundingBox={upperLeftLat},{upperLeftLng},{lowerRightLat},{lowerRightLng}&zoom=15";

            /*byte[] byteArray = await _httpClient.GetByteArrayAsync(getRequest);

            Debug.WriteLine(byteArray);

            File.WriteAllBytes(filePath, byteArray);*/

            using WebClient client = new();
            await client.DownloadFileTaskAsync(new Uri(getRequest), filePath);

        }


    }
}
