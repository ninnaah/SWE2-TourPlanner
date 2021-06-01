using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public class TourItemDataAccessObject
    {
        protected Config Config;
        protected IDataAccess DataAccess;
        protected FileSystem FileSystem;
        public Dictionary<string, string> Direction;

        public TourItemDataAccessObject()
        {
            loadConfig();
            DataAccess = new DBConnection(Config);
            FileSystem = new FileSystem(Config);
        }

        public void loadConfig()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("config.json", true)
                .Build();

            Config = config.Get<Config>();
        }

        public string GetFilePath()
        {
            return Config.FilePath;
        }

        public List<TourItem> GetTours()
        {
            return DataAccess.GetTours();
        }
        public async void GetTourMap(TourItem tour)
        {
            MapQuest mapQuest = new MapQuest(Config.MapQuestKey, Config.FilePath);

            /*float[] routeValues = await mapQuest.GetTourValues(tour);
            tour.Distance = routeValues[0]; //in km
            tour.Duration = routeValues[1]/60; //in min
            tour.FuelUsed = routeValues[2]; //in liter*/


            Direction = new Dictionary<string, string>();

            string responseBody = await mapQuest.GetTourValues(tour);
            JObject obj = JsonConvert.DeserializeObject<JObject>(responseBody);

            tour.Distance = (float)obj["route"]["distance"]; //in km
            tour.Duration = (float)obj["route"]["time"]; //in sec
            tour.FuelUsed = (float)obj["route"]["fuelUsed"]; //in liter

            JObject legs = obj["route"]["legs"][0] as JObject;
            
            List<JObject> maneuvers = new List<JObject>();
            foreach (JObject maneuver in legs["maneuvers"])
            {
                maneuvers.Add(maneuver);
            }

            foreach(JObject maneuver in maneuvers)
            {
                Direction.Add((string)maneuver["narrative"], (string)maneuver["iconUrl"]);
            }

            AddTour(tour);
        }

        public bool AddTour(TourItem tour)
        {
            return DataAccess.AddTour(tour);
        }
        public bool DeleteTour(string tourName)
        {
            if (File.Exists($"{Config.FilePath}/maps/{tourName}.png"))
            {
                File.Move($"{Config.FilePath}/maps/{tourName}.png", $"{Config.FilePath}/maps/{tourName}-tmp.png");
                File.Delete(@$"{Config.FilePath}/maps/{tourName}-tmp.png");
            }
            
            return DataAccess.DeleteTour(tourName);
        }



        public void CreateTourReport(TourItem tour)
        {
            List<TourLogItem> allLogs = DataAccess.GetTourLogs();
            List<TourLogItem> logs = new List<TourLogItem>();

            foreach (TourLogItem log in allLogs)
            {
                if (log.TourName == tour.Name)
                {
                    logs.Add(log);
                }
            }

            FileSystem.CreateTourReportPDF(tour, logs);
        }
        public void CreateSummarizeReport()
        {
            List<TourLogItem> allLogs = DataAccess.GetTourLogs();
            FileSystem.CreateSummarizeReportPDF(allLogs);
        }


        public void ExportTours()
        {
            List<TourItem> tours = DataAccess.GetTours();
            FileSystem.ExportTours(tours);
        }
        public IEnumerable<TourItem> ImportTours(string filePath)
        {
            return FileSystem.ImportTours(filePath);
        }


    }



}
