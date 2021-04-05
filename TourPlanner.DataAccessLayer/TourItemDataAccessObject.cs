using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public class TourItemDataAccessObject
    {
        private IDataAccess _dataAccess;
        public TourItemDataAccessObject()
        {
            //from config
            _dataAccess = new DBConnection();
        }

        public List<TourItem> GetTours()
        {
            return _dataAccess.GetTours();
        }


    }
}
