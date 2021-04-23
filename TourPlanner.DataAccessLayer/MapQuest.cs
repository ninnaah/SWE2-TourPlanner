using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public class MapQuest
    {
        private TourItem _tour;
        private static string _key;
        private static HttpClient _httpClient;

        public MapQuest(string key)
        {
            _key = key;
            _httpClient = new HttpClient();
        }

        public async void SendRequest(TourItem tour)
        {
            await SendRouteRequest(tour);
        }

        public static async Task SendRouteRequest(TourItem tour)
        {
            string getRequest = $"http://www.mapquestapi.com/directions/v2/route?key={_key}&from={tour.From}&to={tour.To}";
            HttpResponseMessage response = await _httpClient.GetAsync(getRequest);
            string responseBody = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(responseBody);

            JObject obj = JsonConvert.DeserializeObject<JObject>(responseBody);
            JObject boundingBox = obj["route"]["boundingBox"] as JObject;
            string sessionId = (string)obj["route"]["sessionId"];
            float distance = (float)obj["route"]["distance"];
            tour.Distance = distance;

            Debug.WriteLine(boundingBox);
            Debug.WriteLine(sessionId);
            Debug.WriteLine(distance);

        }




    }
}
