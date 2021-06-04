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
            List <TourItem> tours = DataAccess.GetTours();
            FileSystem.GetTourDirection(tours);

            return tours;
        }
        public async void GetTourMap(TourItem tour)
        {
            MapQuest mapQuest = new MapQuest(Config.MapQuestKey, Config.FilePath);

            string responseBody = await mapQuest.GetTourValues(tour);
            JObject obj = JsonConvert.DeserializeObject<JObject>(responseBody);

            if((float)obj["route"]["distance"] == 0)
            {
                throw new ArgumentException("Starting or end point doesn't exist");
            }

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
                TourItemDirection direction = new TourItemDirection();
                direction.Text = (string)maneuver["narrative"];
                direction.Image = (string)maneuver["iconUrl"];
                tour.Direction.Add(direction);
            }

            AddTour(tour);
        }

        public bool AddTour(TourItem tour)
        {
            FileSystem.ExportTourDirection(tour);
            return DataAccess.AddTour(tour);
        }
        public bool DeleteTour(string tourName)
        {
            FileSystem.DeleteTour(tourName);

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
            FileSystem.GetTourDirection(tours);
            FileSystem.ExportTours(tours);
        }
        public IEnumerable<TourItem> ImportTours(string filePath)
        {
            return FileSystem.ImportTours(filePath);
        }


    }



}
