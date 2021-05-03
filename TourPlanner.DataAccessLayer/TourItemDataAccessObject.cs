using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            string jsonString = File.ReadAllText("../../../../config.json");
            Config = JsonConvert.DeserializeObject<Config>(jsonString);
        }

        public List<TourItem> GetTours()
        {
            return DataAccess.GetTours();
        }

        public bool AddTour(TourItem tour)
        {
            return DataAccess.AddTour(ref tour, null);
        }

        public bool DeleteTour(TourItem tour)
        {
            return DataAccess.DeleteTour(tour);
        }

        public void CreateTourReport(TourItem tour)
        {
            List<TourLogItem> logs = DataAccess.GetTourLogs();

            foreach(TourLogItem log in logs)
            {
                if(log.Name != tour.Name)
                {
                    logs.Remove(log);
                }
            }

            FileSystem.CreateTourReportPDF(tour, logs);
        }

    }
}
