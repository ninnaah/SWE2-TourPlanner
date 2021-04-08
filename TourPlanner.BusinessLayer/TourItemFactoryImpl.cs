using System.Collections.Generic;
using System.Linq;
using TourPlanner.DataAccessLayer;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    internal class TourItemFactoryImpl : ITourItemFactory
    {
        private TourItemDataAccessObject _tourItemDAO = new TourItemDataAccessObject();
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
    }
}
