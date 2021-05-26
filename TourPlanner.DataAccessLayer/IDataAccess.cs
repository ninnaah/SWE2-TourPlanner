using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public interface IDataAccess
    {
        //Tours
        public List<TourItem> GetTours();
        public bool AddTour(TourItem tour);
        public bool DeleteTour(string tourName);

        //Logs
        public List<TourLogItem> GetTourLogs();
        public bool AddTourLog(TourLogItem tourLog);
        public bool DeleteTourLogDate(TourLogItem tourLog);
        public bool DeleteTourLog(string tourName);
    }
}
