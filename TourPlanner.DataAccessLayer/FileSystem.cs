using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TourPlanner.Models;
using QuestPDF.Fluent;
using Newtonsoft.Json.Linq;
using log4net;

namespace TourPlanner.DataAccessLayer
{
    public class FileSystem
    {
        private string _filePath;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(FileSystem));

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
            }else
            {
                _logger.Info($"Delete tour: map doesn't exist");
            }

            if (File.Exists($"{_filePath}/direction/{tourName}.json"))
            {
                File.Delete(@$"{_filePath}/direction/{tourName}.json");
            }
            else
            {
                _logger.Info($"Delete tour: direction.json doesn't exist");
            }

            _logger.Info($"Deleted tour");
        }

        public void CreateTourReportPDF(TourItem tour, List<TourLogItem> logs)
        {
            string fileName = $"{tour.Name}.pdf";

            var document = new TourReport(tour, logs, _filePath);
            document.GeneratePdf(fileName);

            _logger.Info($"Tour report created");
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

            _logger.Info($"Summarize report created");
            Process.Start("explorer.exe", fileName);
        }


        public void ExportTours(List<TourItem> tours)
        {
            string fileName= "tours.json";
            string jsonString;
            jsonString = JsonConvert.SerializeObject(tours, Formatting.Indented);

            File.WriteAllText(@"tours.json", jsonString);
            _logger.Info($"Exported tours");
            Process.Start("explorer.exe", fileName);
        }
        public IEnumerable<TourItem> ImportTours(string filePath)
        {
            List<TourItem> tours = JsonConvert.DeserializeObject<List<TourItem>>(File.ReadAllText(filePath));
            _logger.Info($"Imported tours");
            return tours;
        }

        public void SaveTourDirection(TourItem tour)
        {
            string fileName = $"{tour.Name}.json";
            string jsonString;
            jsonString = JsonConvert.SerializeObject(tour.Direction, Formatting.Indented);

            File.WriteAllText(@$"{_filePath}/direction/{fileName}", jsonString);
            _logger.Info($"Saved tour directions");
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
            _logger.Info($"Get tour directions");
            return tours;
        }

    }
}