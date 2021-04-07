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
        private Config config;
        private IDataAccess _dataAccess;
        private IDataAccess _dataImport;
        public TourItemDataAccessObject()
        {
            loadConfig();
            _dataAccess = new DBConnection(config);
            _dataImport = new FileSystem(config);
        }
        public void loadConfig()
        {
            string jsonString = File.ReadAllText("../../../../config.json");
            config = JsonConvert.DeserializeObject<Config>(jsonString);
        }

        public List<TourItem> GetTours()
        {
            return _dataAccess.GetTours();
        }

        public void ImportTour()
        {
            TourItem newTour = new TourItem();
            _dataImport.ImportTour(ref newTour);
            _dataAccess.ImportTour(ref newTour);

        }
    }
}
