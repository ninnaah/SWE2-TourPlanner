using System.Collections.Generic;
using System.Linq;
using TourPlanner.DataAccessLayer;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    //concrete factory implementation
    internal class TourItemFactoryImpl : ITourItemFactory
    {
        private TourItemDataAccessObject _tourItemDAO = new TourItemDataAccessObject();
        private TourLogItemDataAccessObject _tourLogItemDAO = new TourLogItemDataAccessObject();
        public IEnumerable<TourItem> GetTours()
        {
            return _tourItemDAO.GetTours();
        }

        public IEnumerable<TourItem> Search(string tourName)
        {
            IEnumerable<TourItem> tours = GetTours();
            return tours.Where(x => x.Name.Contains(tourName));
        }

        public bool AddTour(TourItem tour)
        {
            return _tourItemDAO.AddTour(tour);
        }

        public bool DeleteTour(TourItem tour)
        {
            return _tourItemDAO.DeleteTour(tour);
        }

        public void CreateTourReport(TourItem tour)
        {
            _tourItemDAO.CreateTourReport(tour);
        }



        public IEnumerable<TourLogItem> GetTourLogsForTour(string name)
        {
            List<TourLogItem> allLogs = new List<TourLogItem>();
            List<TourLogItem> currentLogs = new List<TourLogItem>();

            allLogs = _tourLogItemDAO.GetTourLogs();

            foreach(TourLogItem item in allLogs)
            {
                if(item.Name == name)
                {
                    currentLogs.Add(item);
                }
            }

            return currentLogs;
        }

        public bool AddTourLog(TourLogItem tourLog)
        {
            return _tourLogItemDAO.AddTourLog(tourLog);
        }

        public bool DeleteTourLog(TourLogItem tourLog)
        {
            return _tourLogItemDAO.DeleteTourLog(tourLog);
        }
    }
}
