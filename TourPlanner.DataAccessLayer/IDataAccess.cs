using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    interface IDataAccess
    {
        public List<TourItem> GetTours();
        public bool ImportTour(ref TourItem tour, string fileName);
        public bool DeleteTour(TourItem tour);
    }
}
