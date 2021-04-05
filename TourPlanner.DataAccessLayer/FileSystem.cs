using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    class FileSystem : IDataAccess
    {
        private string _filePath;

        public FileSystem()
        {
            //from config file!
            _filePath = "...";
        }
        public List<TourItem> GetTours()
        {
            //get items from fileSystem (import/export)
            return new List<TourItem>()
            {
                new Models.TourItem() { Name = "Kahlenberg", Description = "Wien Nussdorf - Kahlenberg", Distance = 10},
                new Models.TourItem() { Name = "Cobenzl", Description = "Wien Nussdorf - Cobenzl", Distance = 7},
                new Models.TourItem() { Name = "Wienerwald", Description = "Wien Neuwaldegg - Rundweg", Distance = 5},
                new Models.TourItem() { Name = "Prater", Description = "Wien Prater - Rundweg", Distance = 5}
            };
        }
    }
}
