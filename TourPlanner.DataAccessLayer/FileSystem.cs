using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TourPlanner.Models;
using QuestPDF.Fluent;
using Newtonsoft.Json.Linq;

namespace TourPlanner.DataAccessLayer
{
    public class FileSystem
    {
        private string _filePath;

        public FileSystem(Config config)
        {
            _filePath = config.FilePath;
        }


        public void CreateTourReportPDF(TourItem tour, List<TourLogItem> logs)
        {
            string fileName = "{tour.Name}.pdf";

            var document = new TourReport(tour, logs, _filePath);
            document.GeneratePdf(fileName);

            Process.Start("explorer.exe", fileName);
        }
        public void CreateSummarizeReportPDF(List<TourLogItem> logs)
        {
            string fileName = "summarizeReport.pdf";
            float totalTime = 0;
            float totalDistance = 0;

            foreach(TourLogItem log in logs)
            {
                totalTime += log.Duration;
                totalDistance += log.Distance;
            }

            var document = new SummarizeReport(totalTime, totalDistance);
            document.GeneratePdf(fileName);

            Process.Start("explorer.exe", fileName);
        }


        public void ExportTours(List<TourItem> tours)
        {
            string fileName= "tours.json";
            string jsonString;
            jsonString = JsonConvert.SerializeObject(tours, Formatting.Indented);

            File.WriteAllText(@"tours.json", jsonString);
            Process.Start("explorer.exe", fileName);
        }
        public IEnumerable<TourItem> ImportTours(string filePath)
        {
            List<TourItem> tours = JsonConvert.DeserializeObject<List<TourItem>>(File.ReadAllText(filePath));
            return tours;
        }

    }
}