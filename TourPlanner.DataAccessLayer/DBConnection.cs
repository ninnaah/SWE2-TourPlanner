using System;
using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    class DBConnection : IDataAccess
    {
        private string _connection;

        public DBConnection()
        {
            //from config file!
            _connection = loadConfig("blub");
            //connect to db
        }

        public string loadConfig(string fileName)
        {
            string connection = null;
            //read file
            return connection;
        }

        public List<TourItem> GetTours()
        {
            //select tours statement from db
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
