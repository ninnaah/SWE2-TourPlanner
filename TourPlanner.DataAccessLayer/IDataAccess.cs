using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public interface IDataAccess
    {
        public List<TourItem> GetTours();
        public bool ImportTour(ref TourItem tour, string fileName);
        public bool DeleteTour(TourItem tour);


        public List<TourLogItem> GetTourLogs();
        public bool ImportTourLog(ref TourLogItem tourLog, string fileName);
        public bool DeleteTourLog(TourLogItem tourLog);
    }
}
