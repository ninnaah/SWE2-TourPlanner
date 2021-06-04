using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public string GetFilePath()
        {
            return _tourItemDAO.GetFilePath();
        }
        public IEnumerable<TourItem> GetTours()
        {
            return _tourItemDAO.GetTours();
        }
        public IEnumerable<TourItem> Search(string searchText)
        {
            IEnumerable<TourItem> allTours = GetTours();

            IEnumerable<TourItem> foundTours = allTours.Where(tour => tour.Name.ToLower().Contains(searchText.ToLower()) 
                                                                    || tour.Description.ToLower().Contains(searchText.ToLower())
                                                                    || tour.From.ToLower().Contains(searchText.ToLower())
                                                                    || tour.To.ToLower().Contains(searchText.ToLower())
                                                                    || tour.TransportMode.ToLower().Contains(searchText.ToLower()));

            List<TourItem> foundToursList = new List<TourItem>();
            foreach(TourItem tour in foundTours)
            {
                foundToursList.Add(tour);
            }

            IEnumerable<TourLogItem> allTourLogs = _tourLogItemDAO.GetTourLogs();

            IEnumerable<TourLogItem> foundTourLogs = allTourLogs.Where(log => log.Weather.ToLower().Contains(searchText.ToLower())
                                                                    || log.Report.ToLower().Contains(searchText.ToLower()));

            foreach(TourLogItem log in foundTourLogs)
            {
                foreach(TourItem tour in allTours)
                {
                    if(log.TourName == tour.Name)
                    {
                        foundToursList.Add(tour);
                    }
                }
            }

            return foundToursList;
        }
        public bool AddTour(TourItem tour)
        {
            _tourItemDAO.GetTourMap(tour);
            return true;
        }
        public bool DeleteTour(TourItem tour)
        {
            return _tourItemDAO.DeleteTour(tour.Name);
        }



        public void CreateTourReport(TourItem tour)
        {
            _tourItemDAO.CreateTourReport(tour);
        }
        public void CreateSummarizeReport()
        {
            _tourItemDAO.CreateSummarizeReport();
        }



        public void ExportTours()
        {
            _tourItemDAO.ExportTours();
        }
        public IEnumerable<TourItem> ImportTours(string filePath)
        {
            return _tourItemDAO.ImportTours(filePath);
        }



        public IEnumerable<TourLogItem> GetTourLogsForTour(string name)
        {
            List<TourLogItem> allLogs = new List<TourLogItem>();
            List<TourLogItem> currentLogs = new List<TourLogItem>();

            allLogs = _tourLogItemDAO.GetTourLogs();

            foreach(TourLogItem item in allLogs)
            {
                if(item.TourName == name)
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
