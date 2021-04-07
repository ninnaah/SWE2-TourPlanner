using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    class FileSystem : IDataAccess
    {
        private string _filePath;

        public FileSystem(Config config)
        {
            _filePath = $"../../../../{config.filePath}/import";
            Debug.WriteLine(_filePath);
        }

        //??? what for
        public List<TourItem> GetTours()
        {
            return null;
        }

        public bool ImportTour(ref TourItem tour)
        {
            try{
                string jsonString = File.ReadAllText($"{_filePath}/fahrradtourWien.json");

                Debug.WriteLine(jsonString);

                tour = JsonConvert.DeserializeObject<TourItem>(jsonString);
                return true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error: {0}", ex);
                return false;
            }
            
        }



    }
}
