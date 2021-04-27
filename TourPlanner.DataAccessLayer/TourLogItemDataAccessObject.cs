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

        /*public void ImportTourLog(string fileName)
        {
            TourLogItem newTourLog = new TourLogItem();
            DataImport.ImportTourLog(ref newTourLog, fileName);
            DataAccess.ImportTourLog(ref newTourLog, fileName);

        }*/

        public bool AddTourLog(TourLogItem tourLog)
        {
            return DataAccess.ImportTourLog(tourLog);
        }

        public bool DeleteTourLog(TourLogItem tourLog)
        {
            return DataAccess.DeleteTourLogDate(tourLog);
        }
    }
}
