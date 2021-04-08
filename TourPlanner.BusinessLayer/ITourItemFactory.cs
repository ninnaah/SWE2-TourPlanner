using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public interface ITourItemFactory
    {
        IEnumerable<TourItem> GetTours();
        IEnumerable<TourItem> Search(string tourName);

        bool AddTour(TourItem tour);
    }
}
