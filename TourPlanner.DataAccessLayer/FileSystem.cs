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
            _filePath = config.FilePath;
        }


        public void CreateTourReportPDF(TourItem tour, List<TourLogItem> logs)
        {
            string fileName = $"{tour.Name}.pdf";

            var document = new TourReport(tour, logs, _filePath);
            document.GeneratePdf(fileName);

            Process.Start("explorer.exe", fileName);

            /*string fileName = $"{tour.Name}.pdf";
            string path = $"{_filePath}/report/{fileName}";

            var document = new TourReport(tour, logs, _filePath);
            document.GeneratePdf(path);

            //Process.Start("explorer.exe", fileName);*/
        }

        public void CreateSummarizeReportPDF(List<TourLogItem> logs, List<TourItem> tours)
        {
            string fileName = $"summarizeReport.pdf";
            float totalTime = 0;
            float totalDistance = 0;

            foreach(TourLogItem log in logs)
            {
                totalTime += log.Duration;
            }
            foreach (TourItem tour in tours)
            {
                totalDistance += tour.Distance;
            }

            var document = new SummarizeReport(totalTime, totalDistance);
            document.GeneratePdf(fileName);

            Process.Start("explorer.exe", fileName);

            /*string fileName = $"summarizeReport.pdf";
            string path = $"{_filePath}/report/{fileName}";
            float totalTime = 0;
            float totalDistance = 0;

            foreach(TourLogItem log in logs)
            {
                totalTime += log.Duration;
            }
            foreach (TourItem tour in tours)
            {
                totalDistance += tour.Distance;
            }

            var document = new SummarizeReport(totalTime, totalDistance);
            document.GeneratePdf(path);

            //Process.Start("explorer.exe", fileName);*/
        }

    }
}