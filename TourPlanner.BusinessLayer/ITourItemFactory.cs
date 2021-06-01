using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    //abstract Factory
    public interface ITourItemFactory
    {
        string GetFilePath();
        IEnumerable<TourItem> GetTours();
        IEnumerable<TourItem> Search(string tourName);
        Dictionary<string, string> AddTour(TourItem tour);
        bool DeleteTour(TourItem tour);

        void CreateTourReport(TourItem tour);
        void CreateSummarizeReport();

        void ExportTours();
        IEnumerable<TourItem> ImportTours(string filePath);


        IEnumerable<TourLogItem> GetTourLogsForTour(string name);
        bool AddTourLog(TourLogItem tourLog);
        bool DeleteTourLog(TourLogItem tourLog);
    }
}
