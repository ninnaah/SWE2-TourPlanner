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


        public void DeleteTour(string tourName)
        {
            if (File.Exists($"{_filePath}/maps/{tourName}.png"))
            {
                File.Move($"{_filePath}/maps/{tourName}.png", $"{_filePath}/maps/{tourName}-tmp.png");
                File.Delete(@$"{_filePath}/maps/{tourName}-tmp.png");
            }

            if (File.Exists($"{_filePath}/direction/{tourName}.json"))
            {
                File.Delete(@$"{_filePath}/direction/{tourName}.json");
            }
        }

        public void CreateTourReportPDF(TourItem tour, List<TourLogItem> logs)
        {
            string fileName = $"{tour.Name}.pdf";

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

        public void ExportTourDirection(TourItem tour)
        {
            string fileName = $"{tour.Name}.json";
            string jsonString;
            jsonString = JsonConvert.SerializeObject(tour.Direction, Formatting.Indented);

            File.WriteAllText(@$"{_filePath}/direction/{fileName}", jsonString);
        }
        public List<TourItem> GetTourDirection(List<TourItem> tours)
        {
            foreach(TourItem tour in tours)
            {
                if (File.Exists($"{_filePath}/direction/{tour.Name}.json"))
                {
                    tour.Direction = JsonConvert.DeserializeObject<List<TourItemDirection>>(File.ReadAllText($"{_filePath}/direction/{tour.Name}.json"));
                }
                
            }

            return tours;
        }

    }
}