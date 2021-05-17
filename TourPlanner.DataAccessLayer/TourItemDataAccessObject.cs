using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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

            /*Debug.WriteLine("Configuration:");
            config
                .AsEnumerable()
                .OrderBy(x => x.Key)
                .ToList()
                .ForEach(x => Debug.WriteLine("|" + x.Key + "|" + x.Value + "|")); // see that it is a key-value store and not a complex structured json (the json is a lie!)

            */
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
            MapQuest mapQuest = new MapQuest(Config.MapQuestKey, Config.FilePath, tour);

            float[] routeValues = await mapQuest.GetTourValues();
            tour.Distance = routeValues[0]; //in km
            tour.Duration = routeValues[1]/60; //in min
            tour.FuelUsed = routeValues[2]; //in liter


            AddTour(tour);
        }

        public bool AddTour(TourItem tour)
        {
            return DataAccess.AddTour(tour);
        }

        public bool DeleteTour(TourItem tour)
        {
            //File.Delete(@$"{Config.filePath}/maps/{tour.Name}.png");
            return DataAccess.DeleteTour(tour);
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
