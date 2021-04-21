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
        protected IDataAccess DataImport;
        public TourItemDataAccessObject()
        {
            loadConfig();
            DataAccess = new DBConnection(Config);
            DataImport = new FileSystem(Config);
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

        public void ImportTour(string fileName)
        {
            TourItem newTour = new TourItem();
            DataImport.ImportTour(ref newTour, fileName);
            DataAccess.ImportTour(ref newTour, fileName);

        }

        public bool AddTour(TourItem tour)
        {
            return DataAccess.ImportTour(ref tour, null);
        }

        public bool DeleteTour(TourItem tour)
        {
            return DataAccess.DeleteTour(tour);
        }



    }
}
