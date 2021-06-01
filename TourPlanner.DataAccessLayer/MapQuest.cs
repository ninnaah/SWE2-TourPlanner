using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public class MapQuest
    {
        private static JObject _boundingBox;
        private static string _key;
        private static string _dirPath;

        private static readonly ILog _logger = LogManager.GetLogger(typeof(MapQuest));

        public MapQuest(string key, string dirPath)
        {
            _dirPath = dirPath;
            _key = key;
        }

        //public async Task<float[]> GetTourValues(TourItem tour)
        public async Task<string> GetTourValues(TourItem tour)
        {            
            _logger.Info("Starting MapQuest Request");
            _logger.Error("Starting MapQuest Request");

            /*Task<float[]> routeValuesTask = SendRouteRequest(tour);
            float[] routeValues = await routeValuesTask;

            return routeValues;*/

            Task<string> responseBodyTask = SendRouteRequest(tour);
            string responseBody = await responseBodyTask;

            return responseBody;

        }

        //public static async Task<float[]> SendRouteRequest(TourItem tour)
        public static async Task<string> SendRouteRequest(TourItem tour)
        {
            string mode = tour.TransportMode;

            //float[] routeValues = new float[3];

            if (mode == "Car")
                mode = "fastest";

            string getRequest = $"http://www.mapquestapi.com/directions/v2/route?key={_key}&from={tour.From}&to={tour.To}&routeType={mode}";

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(getRequest);
            string responseBody = await response.Content.ReadAsStringAsync();

            JObject obj = JsonConvert.DeserializeObject<JObject>(responseBody);

            _boundingBox = obj["route"]["boundingBox"] as JObject;
            string sessionId = (string)obj["route"]["sessionId"];

            /*routeValues[0] = (float)obj["route"]["distance"]; //in km
            routeValues[1] = (float)obj["route"]["time"]; //in sec
            routeValues[2] = (float)obj["route"]["fuelUsed"]; //in liter*/

            SendMapRequest(tour, sessionId);

            //return routeValues;
            return responseBody;
        }

        public static async void SendMapRequest(TourItem tour, string sessionId)
        {
            string lowerRightLng = (string)_boundingBox["lr"]["lng"];
            string lowerRightLat = (string)_boundingBox["lr"]["lat"];
            string upperLeftLng = (string)_boundingBox["ul"]["lng"];
            string upperLeftLat = (string)_boundingBox["ul"]["lat"];

            Directory.CreateDirectory($@"{_dirPath}/maps/");
            string filePath = $@"{_dirPath}/maps/{tour.Name}.png";

            string getRequest = $"https://www.mapquestapi.com/staticmap/v5/map?key={_key}&size=1240,960&session={sessionId}&boundingBox={upperLeftLat},{upperLeftLng},{lowerRightLat},{lowerRightLng}&zoom=15";

            using WebClient client = new();
            await client.DownloadFileTaskAsync(new Uri(getRequest), filePath);

            _logger.Info("Downloaded MapQuest File");
        }

        


    }
}
