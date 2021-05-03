using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TourPlanner.Models;
using QuestPDF.Fluent;

namespace TourPlanner.DataAccessLayer
{
    public class FileSystem
    {
        private string _filePath;

        public FileSystem(Config config)
        {
            _filePath = config.filePath;
        }


        public void CreateTourReportPDF(TourItem tour, List<TourLogItem> logs)
        {
            string fileName = $"{tour.Name}.pdf";

            var document = new TourReport(tour, logs);
            document.GeneratePdf(fileName);

            Process.Start("explorer.exe", fileName);
        }

    }
}