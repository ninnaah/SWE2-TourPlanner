using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public class TourLogItemDataAccessObject : TourItemDataAccessObject
    {
        public List<TourLogItem> GetTourLogs()
        {
            return DataAccess.GetTourLogs();
        }

        public bool AddTourLog(TourLogItem tourLog)
        {
            return DataAccess.AddTourLog(tourLog);
        }

        public bool DeleteTourLog(TourLogItem tourLog)
        {
            return DataAccess.DeleteTourLogDate(tourLog);
        }
    }
}
