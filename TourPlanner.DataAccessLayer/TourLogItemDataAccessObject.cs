using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public class TourLogItemDataAccessObject : TourItemDataAccessObject
    {
        public List<TourLogItem> GetTourLogs()
        {
            return DataAccess.GetTourLogs();
        }

        public async void GetTourLogData(TourLogItem log, TourItem tour)
        {
            MapQuest mapQuest = new MapQuest(Config.MapQuestKey, log, tour);

            float[] routeValues = await mapQuest.GetRouteData();
            log.Distance = routeValues[0]; //in km
            log.Duration = routeValues[1]; //in sec
            log.FuelUsed = routeValues[2]; //in liter

            log.AverageSpeed = (log.Distance / (log.Duration/3600));

            AddTourLog(log);
        }

        public bool AddTourLog(TourLogItem tourLog)
        {
            return DataAccess.AddTourLog(tourLog);
        }

        public bool DeleteTourLog(TourLogItem tourLog)
        {
            return DataAccess.DeleteTourLogDate(tourLog);
        }
    }
}
