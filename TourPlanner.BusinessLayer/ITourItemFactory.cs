using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    //abstract Factory
    public interface ITourItemFactory
    {
        IEnumerable<TourItem> GetTours();
        IEnumerable<TourItem> Search(string tourName);
        bool AddTour(TourItem tour);
        bool DeleteTour(TourItem tour);
        void CreateTourReport(TourItem tour);
        void CreateSummarizeReport();


        IEnumerable<TourLogItem> GetTourLogsForTour(string name);
        bool AddTourLog(TourLogItem tourLog, TourItem tour);
        bool DeleteTourLog(TourLogItem tourLog);
    }
}
