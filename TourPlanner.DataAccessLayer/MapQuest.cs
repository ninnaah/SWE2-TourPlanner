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
        public static TourItem _tour;
        public static TourLogItem _tourLog;
        private static string _sessionId;
        private static JObject _boundingBox;
        private static string _key;
        private static string _dirPath;

        private static readonly ILog _logger = LogManager.GetLogger(typeof(MapQuest));

        public MapQuest(string key, string dirPath, TourItem tour)
        {
            _tour = tour;
            _dirPath = dirPath;
            _key = key;
        }

        public MapQuest(string key, TourLogItem log, TourItem tour)
        {
            _tourLog = log;
            _tour = tour;
            _key = key;
        }


        //TOUR
        public async Task<float> GetDistance()
        {
            _logger.Info("Starting MapQuest Request");
            _logger.Error("Starting MapQuest Request");
            Task<float> distanceTask = SendRouteRequest();
            float distance = await distanceTask;

            SendMapRequest();
            return distance;
        }

        public static async Task<float> SendRouteRequest()
        {
            string getRequest = $"http://www.mapquestapi.com/directions/v2/route?key={_key}&from={_tour.From}&to={_tour.To}";
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(getRequest);
            string responseBody = await response.Content.ReadAsStringAsync();

            JObject obj = JsonConvert.DeserializeObject<JObject>(responseBody);
            _boundingBox = obj["route"]["boundingBox"] as JObject;

            _sessionId = (string)obj["route"]["sessionId"];

            return (float)obj["route"]["distance"]; //in km
        }

        public static async void SendMapRequest()
        {
            string lowerRightLng = (string)_boundingBox["lr"]["lng"];
            string lowerRightLat = (string)_boundingBox["lr"]["lat"];
            string upperLeftLng = (string)_boundingBox["ul"]["lng"];
            string upperLeftLat = (string)_boundingBox["ul"]["lat"];


            Directory.CreateDirectory($@"{_dirPath}/maps/");
            string filePath = $@"{_dirPath}/maps/{_tour.Name}.png";

            string getRequest = $"https://www.mapquestapi.com/staticmap/v5/map?key={_key}&size=1240,960&session={_sessionId}&boundingBox={upperLeftLat},{upperLeftLng},{lowerRightLat},{lowerRightLng}&zoom=15";

            using WebClient client = new();
            await client.DownloadFileTaskAsync(new Uri(getRequest), filePath);

            _logger.Info("Downloaded MapQuest File");
        }



        //LOG
        public async Task<float[]> GetRouteData()
        {
            _logger.Info("Starting MapQuest Request");
            _logger.Error("Starting MapQuest Request");
            Task<float[]> routeValuesTask = SendRouteDataRequest();
            float[] routeValues = await routeValuesTask;

            return routeValues;
        }

        public static async Task<float[]> SendRouteDataRequest()
        {
            string mode = _tourLog.TransportMode;
            float[] routeValues = new float[3];

            if (mode == "Car")
                mode = "fastest";

            string getRequest = $"http://www.mapquestapi.com/directions/v2/route?key={_key}&from={_tour.From}&to={_tour.To}&routeType={mode}";
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(getRequest);
            string responseBody = await response.Content.ReadAsStringAsync();

            JObject obj = JsonConvert.DeserializeObject<JObject>(responseBody);

            routeValues[0] = (float)obj["route"]["distance"]; //in km
            routeValues[1] = (float)obj["route"]["time"]; //in sec
            routeValues[2] = (float)obj["route"]["fuelUsed"]; //in liter

            return routeValues;
        }


    }
}
