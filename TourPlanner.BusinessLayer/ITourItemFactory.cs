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


        IEnumerable<TourLogItem> GetTourLogsForTour(string name);
        bool AddTourLog(TourLogItem tourLog);
        bool DeleteTourLog(TourLogItem tourLog);
    }
}
