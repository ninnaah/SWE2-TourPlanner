﻿using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public interface IDataAccess
    {
        //Tours
        public List<TourItem> GetTours();
        public bool ImportTour(TourItem tour);
        public bool DeleteTour(TourItem tour);

        //Logs
        public List<TourLogItem> GetTourLogs();
        public bool ImportTourLog(TourLogItem tourLog);
        public bool DeleteTourLogDate(TourLogItem tourLog);
        public bool DeleteTourLog(string tourName);
    }
}
