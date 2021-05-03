﻿using Newtonsoft.Json;
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
        private static string _sessionId;
        private static JObject _boundingBox;
        private static string _key;
        private static string _dirPath;

        public MapQuest(string key, string dirPath, TourItem tour)
        {
            _tour = tour;
            _dirPath = dirPath;
            _key = key;
        }

        public async Task<float> GetMap()
        {
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
            float distance = (float)obj["route"]["distance"];

            return distance;
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

            /*byte[] byteArray = await _httpClient.GetByteArrayAsync(getRequest);

            Debug.WriteLine(byteArray);

            File.WriteAllBytes(filePath, byteArray);*/

            using WebClient client = new();
            await client.DownloadFileTaskAsync(new Uri(getRequest), filePath);
        }


    }
}