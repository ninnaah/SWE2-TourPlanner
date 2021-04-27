using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        //protected IDataAccess DataImport;
        public TourItemDataAccessObject()
        {
            loadConfig();
            DataAccess = new DBConnection(Config);
            //DataImport = new FileSystem(Config);
        }
        public void loadConfig()
        {
            string jsonString = File.ReadAllText(@"../../../../config.json");
            Config = JsonConvert.DeserializeObject<Config>(jsonString);
        }

        public List<TourItem> GetTours()
        {
            return DataAccess.GetTours();
        }

        /*public void ImportTour(string fileName)
        {
            TourItem newTour = new TourItem();
            DataImport.ImportTour(ref newTour, fileName);
            DataAccess.ImportTour(ref newTour, fileName);

        }*/

        public bool AddTour(TourItem tour)
        {
            //call maqquest and save imagepath to tourItem
            MapQuest mapQuest = new MapQuest(Config.mapQuestKey, Config.filePath);
            mapQuest.GetMap(tour);
            tour.Distance = MapQuest.Distance;
            Debug.WriteLine("new Distance: " +tour.Distance);
            return DataAccess.ImportTour(tour);
        }

        public bool DeleteTour(TourItem tour)
        {
            //File.Delete(@$"{Config.filePath}/maps/{tour.Name}.png");
            return DataAccess.DeleteTour(tour);
        }



    }
}
